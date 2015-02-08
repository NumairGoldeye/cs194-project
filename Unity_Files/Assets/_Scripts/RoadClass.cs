﻿using UnityEngine;
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

		if (TurnState.CheckSecondRoadBuilt(this)){
			roads.BroadcastMessage ("toggleRoad");
		}
		
		if (!TurnState.freeBuild){
			BuyManager.PurchaseForPlayer (BuyableType.road, TurnState.currentPlayer);
		}
	}
}
