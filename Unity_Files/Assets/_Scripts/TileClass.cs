using UnityEngine;
using System.Collections.Generic;


public class TileClass : MonoBehaviour {

	public ResourceType type;
	public int diceValue;
	public int tileNumber;
	public bool hasRobber;
	private int index;

	private List<SettlementClass> settlements = new List<SettlementClass> ();


	// The index of the POSITION of this tile on the board. Tiles are indexed in row-major order.
	public int tileIndex;

	public void stealResources() {
//		Debugger.Log ("PlayerHand", TurnState.currentPlayer.playerName + " is stealing...");
		StandardBoardGraph graph = StandardBoardGraph.Instance;
		List<SettlementClass> settlements = graph.getSettlementsForTile (this);

		int settlementsAvailable = 0;

		foreach (SettlementClass settlement in settlements){
			if (!settlement.isBuilt()) continue;
			else if (settlement.getPlayer() == TurnState.currentPlayer.playerId) continue;

			settlementsAvailable ++;
			settlement.toggleStealing();
		}

		if (settlementsAvailable > 0) {
			// TODO: fix this
			UnityEngine.UI.Text stealing = (UnityEngine.UI.Text)(GameObject.Find ("StealingInstructions").GetComponent(typeof(UnityEngine.UI.Text)));
			stealing.text = "Click on a settlement to steal a resource from that player!";
		} else {
			// If there is no one to steal from, we exit the robber state.
			TurnState.SetSubStateType(TurnSubStateType.none);
		}
	}

	public void endStealing() {
		StandardBoardGraph graph = StandardBoardGraph.Instance;
		List<SettlementClass> settlements = graph.getSettlementsForTile (this);

		foreach (SettlementClass settlement in settlements) {
			settlement.toggleStealing();
		}
//		UnityEngine.UI.Text stealing = (UnityEngine.UI.Text)(GameObject.Find ("StealingInstructions").GetComponent(typeof(UnityEngine.UI.Text)));
//		stealing.text = "";
	}

	void OnMouseDown() {
		if (!GameManager.Instance.gameStarted && !GameManager.Instance.myTurn()) return;
		if (hasRobber) return;
		if (GameManager.Instance.getDiceRoll() == 7 || TurnState.getSubStateType() == TurnSubStateType.robbering) {
			receiveRobber ();

			GameManager.Instance.networkView.RPC ("syncRobberMove", RPCMode.Others, this.tileIndex);
		}
	}



	public void receiveRobber () {
		//TODO Network this function
		GameManager.Instance.removeRobber ();
		getRobber ();
		DevConfirmButton.clickButton ();
		stealResources ();
		/* 
		 * TODO: upon clicking this tile, close the dialogue box
		 */

	}
	
	public void getRobber () {
		GameObject robber = GameObject.Find ("Robber");
		robber.transform.position = transform.position;
		GameManager.Instance.setRobberTile (this);
	}

	private void displayDiceNumber (){
		if (diceValue > 0) {
			GetComponentInChildren <TextMesh>().text = diceValue.ToString ();
		}
		else {//Don't display a number if it is desert
			GetComponentInChildren <TextMesh>().text = "";
		}
	}

	public void assignType(int diceNumber, ResourceType assignedType){
		type = assignedType;
		diceValue = diceNumber;
		displayDiceNumber ();
		getMaterial ();
		this.index = index;
	}

	private void getMaterial () {

		switch (type) {
			case ResourceType.Sheep:
				string name = "tile_images/sheep00" + Random.Range(1, 4).ToString();
				renderer.material.mainTexture = Resources.Load(name) as Texture;
				GameObject sheep = GameObject.FindGameObjectsWithTag ("Sheep")[0];					
				sheep.transform.position = transform.position;
				sheep.tag = "Assigned";
				break;
			case ResourceType.Wheat:
				name = "tile_images/wheat00" + Random.Range(1, 3).ToString();
				renderer.material.mainTexture = Resources.Load(name) as Texture;
				GameObject Wheat = GameObject.FindGameObjectsWithTag ("Hay")[0];
				Wheat.transform.position = transform.position;
				Wheat.tag = "Assigned";	
				break;
			case ResourceType.Ore:
				name = "tile_images/ore00" + Random.Range(1, 2).ToString();
				renderer.material.mainTexture = Resources.Load(name) as Texture;
				GameObject Mountain = GameObject.FindGameObjectsWithTag ("Mountain")[0];
				Mountain.transform.position = transform.position;
				Mountain.tag = "Assigned";
				break;
			case ResourceType.Brick:
				name = "tile_images/brick00" + Random.Range(1, 3).ToString();
				renderer.material.mainTexture = Resources.Load(name) as Texture;
				GameObject Brick = GameObject.FindGameObjectsWithTag ("Brick")[0];
				Brick.transform.position = transform.position;
				Brick.tag = "Assigned";	
				break;
			case ResourceType.Wood:
				name = "tile_images/wood00" + Random.Range(1, 3).ToString();
				renderer.material.mainTexture = Resources.Load(name) as Texture;
				GameObject forest = GameObject.FindGameObjectsWithTag ("Forest")[0];					
				forest.transform.position = transform.position;
				forest.tag = "Assigned";
				break;
			case ResourceType.None:
				name = "tile_images/desert";
				renderer.material.mainTexture = Resources.Load(name) as Texture;
				break;
		}
	}

	public void addSettlement(SettlementClass settlement) {
//		Debugger.Log ("Vertex", v.settlement.isBuilt ().ToString());
		settlements.Add (settlement);
	}

	public List<SettlementClass> getSettlements() {
		return settlements;
	}
}
