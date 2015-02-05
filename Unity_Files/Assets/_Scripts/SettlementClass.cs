using UnityEngine;
using System.Collections;

public class SettlementClass : MonoBehaviour {

	private bool built;
	private bool visible;
	private bool upgrading;
	public GameObject settlements;
	public GameObject city;
	public bool hasCity;
	public int vertexIndex;
	public int ownerId;

	// Use this for initialization
	void Start () {
		built = false;
		upgrading = false;
		ownerId = -1;
		makeInvisible();
		hasCity = false;
	}
	
	// Update is called once per frame
	void Update () {
	
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

	private void upgradeToCity() {
		built = false;
		makeInvisible();
		city.SendMessage("makeVisible");
	}
	
	public void toggleSettlements() {
		if (!built) {
			if (!visible)
				makeVisible();
			else
				makeInvisible();
		}
	}
	void makeInvisible() {
		visible = false;
		Color temp = renderer.material.color;
		temp.a = 0;
		renderer.material.color = temp;		
	}
	
	void makeVisible() {
		visible = true;
		Color temp = renderer.material.color;
		temp.a = 0.8f;
		renderer.material.color = temp;
	}
	
	void OnMouseDown() {
		if (!built) {
			if (!visible) return;
			built = true;
//			Color temp = renderer.material.color;
//			temp.a = 1;
			renderer.material.color = TurnState.currentPlayer.playerColor;


			ownerId = TurnState.currentPlayer.playerId;
			settlements.BroadcastMessage ("toggleSettlements");
			BuyManager.PurchaseForPlayer(BuyableType.settlement, TurnState.currentPlayer);
		} else {
			if (upgrading) {
				BuyManager.PurchaseForPlayer(BuyableType.city, TurnState.currentPlayer);
				upgradeToCity();
			}
		}
	}
}
