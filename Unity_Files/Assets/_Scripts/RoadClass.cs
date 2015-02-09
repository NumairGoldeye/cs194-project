using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadClass : MonoBehaviour {

	private bool built;
	private bool visible;

	public SettlementClass settlement1;
	public SettlementClass settlement2;

	public GameObject roadsObject;

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

	private bool isRoadReadyToBeShown(List<RoadClass> roadsToBeShown) {
		foreach(RoadClass road in roadsToBeShown) {
			if (road.settlement1.vertexIndex == settlement1.vertexIndex && road.settlement2.vertexIndex == settlement2.vertexIndex)
				return true;
		}
		return false;
	}

	public void toggleRoad() {
		if (isRoadReadyToBeShown(StandardBoardGraph.Instance.BuildableRoads(TurnState.currentPlayer)) && !built && !visible)
			makeVisible ();
		else if (visible && !built)
			makeInvisible();
	}

	public void hideIfPossible(){
		if (!built) {
			makeInvisible();
		}
	}


	public void makeInvisible() {
		if (built) return;
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
		p.AddRoad (this);
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

		StartGameManager.NextPhase(); // TODO figure out how to move this out of here...

		if (TurnState.CheckSecondRoadBuilt(this)){
			roadsObject.BroadcastMessage ("makeInvisible");
		}
		
		if (!TurnState.freeBuild){
			BuyManager.PurchaseForPlayer (BuyableType.road, TurnState.currentPlayer);
		}
	}
}
