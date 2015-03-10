using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettlementClass : MonoBehaviour {

	private bool built;
	private bool visible;
	private bool upgrading;
	public GameObject settlements;
	private GameObject settlement;
	private GameObject city;
	private bool hasCity;
	public int vertexIndex;
	private int ownerId;
	private bool stealing;
	public PortClass port;
	
	void Start () {
		settlement = transform.FindChild("SettlementObject").gameObject;
		city = transform.FindChild("CityObject").gameObject;
		Debugger.Log ("City", city.tag);
		built = false;
		upgrading = false;
		ownerId = -1;
		hideSettlement ();
		hideCity ();
		hasCity = false;
		stealing = false;
	}
	
	private void hideCity() {
		Color temp = city.renderer.material.color;
		temp.a = 0;
		city.renderer.material.color = temp;
	}
	
	private void showCity() {
		city.renderer.material.color = TurnState.currentPlayer.playerColor;
	}

	public void toggleStealing() {
		stealing = !stealing;
	}

	public bool isStealing() {
		return stealing;
	}
	
	public bool isCity() {
		return hasCity;
	}

	public int getPlayer() {
		return ownerId;
	}

	public bool isBuilt() {
		return built;
	}
	
	public void upgradeAbility() {
		if (isBuilt ())
			upgrading = true;
	}

	private bool isSettlementReadyToBeShown(List<SettlementClass> settlementsToBeShown) {
		foreach(SettlementClass settlement in settlementsToBeShown) {
			if (settlement.vertexIndex == vertexIndex)
				return true;
		}
		return false;
	}

	public void toggleSettlements() {
		if (!visible && !built && !StandardBoardGraph.Instance.hasBuiltNeighbooringSettlement(this) && StandardBoardGraph.Instance.hasConnectingRoad(TurnState.currentPlayer, this))
		    showSettlement();
		else if (visible && !built)
			hideSettlement();
	}

	public void showSettlementStartup() {
		if (!StandardBoardGraph.Instance.hasBuiltNeighbooringSettlement (this))
			showSettlement ();
	}

	public void hideSettlement() {
		if (built && !hasCity) return;
		visible = false;
		Color temp = settlement.renderer.material.color;
		temp.a = 0;
		settlement.renderer.material.color = temp;		
	}

	public void showSettlement() {
		visible = true;
		Color temp = settlement.renderer.material.color;
		temp.a = 0.8f;
		settlement.renderer.material.color = temp;
	}

	private void setPlayerSettlement() {
		Player p = TurnState.currentPlayer;
		settlement.renderer.material.color = p.playerColor;
		ownerId = TurnState.currentPlayer.playerId;
		if (!TurnState.freeBuild)
			BuyManager.PurchaseForPlayer(BuyableType.settlement, p);
		TurnState.currentPlayer.AddVictoryPoint();
		p.AddSettlement(this);
	}

	public void setPlayerCity() {
		BuyManager.PurchaseForPlayer(BuyableType.city, TurnState.currentPlayer);
		hasCity = true;
		hideSettlement();
		showCity();
		TurnState.currentPlayer.AddVictoryPoint();
		upgrading = false;
		GameManager.Instance.networkView.RPC ("syncCityUpgrade", RPCMode.Others, this.vertexIndex);
	}

	public void executeStealing() {
		StandardBoardGraph graph = StandardBoardGraph.Instance;
		List<TileClass> tiles = graph.getTilesForSettlement(this);
		TileClass currentTile = tiles[0];
		foreach (TileClass tile in tiles) {
			if (tile.hasRobber) currentTile = tile;
		}
		currentTile.endStealing();
		
		Player owner = GameManager.Instance.players[ownerId];
		ResourceType resource = (ResourceType)(owner.removeRandomResource());
		TurnState.currentPlayer.AddResource(resource, 1);
		GameManager.Instance.networkView.RPC ("syncSteal", RPCMode.Others, owner.playerId, resource); 
	}

	public void buildSettlement() {
		built = true;
		setPlayerSettlement();

		settlements.BroadcastMessage ("hideSettlement");
		if (StartGameManager.secondPhase) 
			GameManager.Instance.distributeResourcesForSettlement(this);
		StartGameManager.NextPhase(); // TODO figure out how to move this out of here... 

		//TODO Sync with everyone else
		GameManager.Instance.networkView.RPC ("syncSettlementBuild", RPCMode.Others, this.vertexIndex);
	}
	
	void OnMouseDown() {
		if (!GameManager.Instance.myTurn()) return;
		if (!built) {
			if (!visible) return;
			buildSettlement();
		} else {
			if (upgrading) {
				setPlayerCity();
			}
			else if (stealing){
				executeStealing();
			}
		}
	}
}
