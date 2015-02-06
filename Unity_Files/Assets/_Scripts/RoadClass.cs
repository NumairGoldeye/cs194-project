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
		if (!visible || built) return;
		built = true;
//		Color temp = renderer.material.color;
//		temp.a = 1;
//		renderer.material.color = temp;
		renderer.material.color = TurnState.currentPlayer.playerColor;

		// If roadbuilding, allow a second road.. 
		// TODO - undo buttons?
		if (TurnState.CheckSecondRoadBuilt()){
			roads.BroadcastMessage ("toggleRoad");
		}
		

		BuyManager.PurchaseForPlayer (BuyableType.road, TurnState.currentPlayer);
	}
}
