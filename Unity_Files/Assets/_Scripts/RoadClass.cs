using UnityEngine;
using System.Collections;

public class RoadClass : MonoBehaviour {

	private bool built;
	private bool visible;

	public SettlementClass settlement1;
	public SettlementClass settlement2;

	public GameObject roads;

	public int edgeIndex; // The index of the edge this road is on.

	public int ownerId;

	// Use this for initialization
	void Start () {
		built = false;
		makeInvisible();
		ownerId = -1;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool isBuilt() {
		return built;
	}

	public void toggleRoad() {
		if (!built) {
			if (!visible)
				makeVisible();
			else
				makeInvisible();
		}
	}

	public void hideIfPossible(){
		if (!built) {
			makeInvisible();
		}
	}


	public void makeInvisible() {
		visible = false;
		Color temp = renderer.material.color;
		temp.a = 0;
		renderer.material.color = temp;		
	}

	public void makeVisible() {
		visible = true;
		Color temp = renderer.material.color;
		temp.a = 0.8f;
		renderer.material.color = temp;
	}

	void SetPlayer(Player p){
		//TODO add to player.roads in a meaningful way
		renderer.material.color = p.playerColor;
		ownerId = p.playerId;
	}

	// To be used if somebody changes their mind about a roadbuilding dev card
	public void ClearPlayer(){
		ownerId = -1;
		renderer.material.color = Color.white;
		built = false;
	}


	void OnMouseDown() {
		if (!visible || built) return;
		built = true;
		SetPlayer(TurnState.currentPlayer);

		// If roadbuilding, allow a second road.. 
		// TODO - undo buttons?
		//I don't think undo buttons are necessary, they can just cancel and redo it
		if (TurnState.CheckSecondRoadBuilt(this)){
			roads.BroadcastMessage ("toggleRoad");
		}
		
		if (!TurnState.freeBuild){
			BuyManager.PurchaseForPlayer (BuyableType.road, TurnState.currentPlayer);
		}
	}
}
