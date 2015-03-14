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

	public void removeResources() {
		Player[] players = (Player[])(GameObject.FindObjectsOfType (typeof(Player))); 
//		Player[] players = Player.allPlayers;
		foreach (Player player in players) {
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
		if (GameManager.getDiceRoll() == 7 || TurnState.getSubStateType() == TurnSubStateType.robbering) 
			receiveRobber ();
	}

	private void removeRobber() {
		TileClass tileWithRobber = GameManager.getRobberTile ();
		tileWithRobber.hasRobber = false;
	}

	public void receiveRobber () {
		removeRobber ();
		getRobber ();
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
		GameManager.setRobberTile (this);
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
				renderer.material.color = Color.green;
				break;
			case ResourceType.Wheat:
				renderer.material.color = Color.yellow;
				break;
			case ResourceType.Ore:
				renderer.material.color = Color.gray;
				GameObject Mountain = GameObject.FindGameObjectsWithTag ("Mountain")[0];
				Mountain.transform.position = transform.position;
				Mountain.tag = "AssignedMountain";
				break;
			case ResourceType.Brick:
				renderer.material.color = Color.red;
				break;
			case ResourceType.Wood:
				renderer.material.color = new Color(0.4f, 0.2f, 0);
				GameObject forest = GameObject.FindGameObjectsWithTag ("Forest")[0];
				forest.transform.position = transform.position;
				forest.tag = "AssignedForest";
				break;
			case ResourceType.None:
				renderer.material.color = Color.white;
				break;
		}
	}


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addSettlement(SettlementClass settlement) {
//		Debugger.Log ("Vertex", v.settlement.isBuilt ().ToString());
		settlements.Add (settlement);
	}

	public List<SettlementClass> getSettlements() {
		return settlements;
	}
}
