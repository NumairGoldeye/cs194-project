﻿using UnityEngine;
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
		//for now, remove half of each player's resources
	}

	public void stealResources() {
		//make a dialogue box pop up, choose one player. random resource
	}


	public void receiveRobber () {
		getRobber ();
		removeResources ();//Each player has to remove half of their resources. For now, it will be random
		stealResources ();//uses turnState.currentPlayer look it up though
	}


	public void getRobber () {
		GameObject robber = GameObject.Find ("Robber");
		robber.transform.position = transform.position;
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
			case ResourceType.sheep:
				renderer.material.color = Color.green;
				break;
			case ResourceType.wheat:
				renderer.material.color = Color.yellow;
				break;
			case ResourceType.ore:
				renderer.material.color = Color.gray;
				break;
			case ResourceType.brick:
				renderer.material.color = Color.red;
				break;
			case ResourceType.wood:
				renderer.material.color = Color.black;
				break;
			case ResourceType.desert:
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
