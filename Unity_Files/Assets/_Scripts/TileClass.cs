using UnityEngine;
using System.Collections.Generic;


public class TileClass : MonoBehaviour {

	public ResourceType type;
	public int diceValue;
	public int tileNumber;
	public bool hasRobber;

	private List<SettlementClass> settlements = new List<SettlementClass> ();


	// The index of the POSITION of this tile on the board. Tiles are indexed in row-major order.
	public int tileIndex;

	public void removeResources() {
		foreach (Player player in GameManager.Instance.players) {
			if (player.getTotalResources() > 7){ //TODO: make 7 into a constant in a reasonable place
				player.removeHalfResources();
			}
		}
	}

	public void stealResources() {
		StandardBoardGraph graph = StandardBoardGraph.Instance;
		List<SettlementClass> settlements = graph.getSettlementsForTile (this);

		int settlementsAvailable = 0;

		foreach (SettlementClass settlement in settlements){
			if (!settlement.isBuilt()) continue;
			else if (settlement.getPlayer() == TurnState.currentPlayer.playerId) continue;

			settlementsAvailable ++;
			settlement.toggleStealing();
			//TODO Sync Settlement State & Display Text
		}

		if (settlementsAvailable > 0) {
			UnityEngine.UI.Text stealing = (UnityEngine.UI.Text)(GameObject.Find ("StealingInstructions").GetComponent(typeof(UnityEngine.UI.Text)));
			stealing.text = "Click on a settlement to steal a resource from that player!";
		}
	}

	public void endStealing() {
		StandardBoardGraph graph = StandardBoardGraph.Instance;
		List<SettlementClass> settlements = graph.getSettlementsForTile (this);

		foreach (SettlementClass settlement in settlements) {
			settlement.toggleStealing();
		}

		UnityEngine.UI.Text stealing = (UnityEngine.UI.Text)(GameObject.Find ("StealingInstructions").GetComponent(typeof(UnityEngine.UI.Text)));
		stealing.text = "";
	}

	void OnMouseDown() {
		if (hasRobber) return;
		if (GameManager.Instance.getDiceRoll() == 7 || TurnState.getSubStateType() == TurnSubStateType.robbering) {
			if (Network.isClient)
				GameManager.Instance.requestRobberMove(this);
			else 
				GameManager.Instance.handleRobberMove(tileIndex);
		}
	}

	private void removeRobber() {
		TileClass tileWithRobber = GameManager.Instance.getRobberTile ();
		tileWithRobber.hasRobber = false;
	}

	public void receiveRobber () {
		removeRobber ();
		getRobber ();
		DevConfirmButton.clickButton ();
		removeResources ();//Each player has to remove half of their resources. For now, it will be random
		stealResources ();
		/* 
		 * TODO: upon clicking this tile, close the dialogue box
		 */

	}
	
	public void getRobber () {
		GameObject robber = GameObject.Find ("Robber");
		robber.transform.position = transform.position;
		hasRobber = true;
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
	}

	private void getMaterial () {

		switch (type) {
			case ResourceType.Sheep:
				renderer.material.color = Color.green;
				break;
			case ResourceType.Wheat:
				renderer.material.color = Color.yellow;
				break;
			case ResourceType.Ore:
				renderer.material.color = Color.gray;
				break;
			case ResourceType.Brick:
				renderer.material.color = Color.red;
				break;
			case ResourceType.Wood:
				renderer.material.color = new Color(0.4f, 0.2f, 0);
				break;
			case ResourceType.None:
				renderer.material.color = Color.white;
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
