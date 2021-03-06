﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadClass : MonoBehaviour {

	private bool built;
	private bool visible;

	public SettlementClass settlement1;
	public SettlementClass settlement2;
	public AudioSource roadSound;

	public GameObject roadsObject;

	public int edgeIndex; // The index of the edge this road is on.

	public int ownerId;

	private static int MIN_LONGEST_ROAD_LENGTH = 5;
	private static int LONGEST_ROAD_VICTORY_POINTS = 2;

	void Start () {
		built = false;
		makeInvisible();
		ownerId = -1;

		roadSound = (AudioSource)(GameObject.Find ("Music").GetComponents (typeof(AudioSource)) [2]);
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

	public void hideIfPossible() {
		if (!built)
			makeInvisible();
	}

	public void toggleRoad() {
		if (isRoadReadyToBeShown(StandardBoardGraph.Instance.BuildableRoads(GameManager.Instance.myPlayer)) && !built && !visible)
			makeVisible ();
		else if (visible && !built)
			makeInvisible();
	}

	/// <summary>
	/// Used to check the edge case in game startup where you are supposed to place a road next to the settlement you just built
	/// </summary>
	private bool checkStartupCondition() {
		if (settlement1.getPlayer() == TurnState.currentPlayer.playerId) {
			List<RoadClass> connectedRoads1 = StandardBoardGraph.Instance.getConnectedRoads (settlement1);
			foreach(RoadClass road in connectedRoads1)
				if (road.isBuilt()) return false;
			return true;
		} else if (settlement2.getPlayer() == TurnState.currentPlayer.playerId) {
			List<RoadClass> connectedRoads2 = StandardBoardGraph.Instance.getConnectedRoads (settlement2);
			foreach(RoadClass road in connectedRoads2)
				if (road.isBuilt()) return false;
			return true;
		}
		return false;
	}

	public void toggleRoadStartup() {
		if (isRoadReadyToBeShown (StandardBoardGraph.Instance.BuildableRoads (GameManager.Instance.myPlayer)) && !built && !visible && checkStartupCondition ())
			makeVisible ();
	}

	public void makeInvisible() {
		if (built) return;
		visible = false;
		Color temp = renderer.material.color;
		temp.a = 0;
		renderer.material.color = temp;		
	}

	public void makeVisibleWithCheck(){
		if (isRoadReadyToBeShown(StandardBoardGraph.Instance.BuildableRoads(GameManager.Instance.myPlayer)) && !built && !visible)
			makeVisible();
	}

	public void makeVisible() {
		visible = true;
		Color temp = renderer.material.color;
		temp.a = 0.8f;
		renderer.material.color = temp;
	}

	public void SetPlayer(){
		built = true;
		TurnState.currentPlayer.AddRoad (this);
		renderer.material.color = TurnState.currentPlayer.playerColor;
		ownerId = TurnState.currentPlayer.playerId;
		roadSound.Play ();
		UpdateLongestRoad();
	}

	// To be used if somebody changes their mind about a roadbuilding dev card
	public void ClearPlayer(){
		ownerId = -1;
		renderer.material.color = Color.white;
		built = false;
	}

	public void buildRoad(bool toNextPhase=true) {
		if (!GameManager.Instance.myTurn ()) {
			Debug.Log("NOT YOUR TURN");
			return;
		}

		SetPlayer();

		if (TurnState.CheckSecondRoadBuilt(this)){
			if (Network.isServer)
				roadsObject.BroadcastMessage ("makeInvisible");
			else 
				//TODO: send message to client who is building the road to make his roads invisible
				Debugger.Log ("foo", "bar"); 

		}
		if (!TurnState.freeBuild){
			BuyManager.PurchaseForPlayer (BuyableType.road, TurnState.currentPlayer);
		}
		Debugger.Log ("PlayerHand", "Road owner: " + ownerId.ToString ());
		GameManager.Instance.networkView.RPC("syncRoadBuild", RPCMode.Others, this.edgeIndex);
		 StartGameManager.NextPhase(); // TODO figure out how to move this out of here...
	}

	private void UpdateLongestRoad() {
//		KeyValuePair<Player, int> best = new KeyValuePair<Player, int> (null, -1);
//		foreach (Player candidate in GameManager.Instance.players) {
//			int length = GameManager.Instance.graph.GetLongestRoadForPlayer(candidate);
//			if (length > best.Value) {
//				best = new KeyValuePair<Player, int>(candidate, length);
//			}
//		}
//		if (best.Key == null) return;
//		if (best.Value >= MIN_LONGEST_ROAD_LENGTH) {
//			Player oldPlayerWithOldLongestRoad = GameManager.Instance.playerWithLongestRoad;
//			oldPlayerWithOldLongestRoad.RemoveVictoryPoint(LONGEST_ROAD_VICTORY_POINTS);
//			GameManager.Instance.playerWithLongestRoad = best.Key;
//			best.Key.AddVictoryPoint(LONGEST_ROAD_VICTORY_POINTS);
//		}

		Player player = TurnState.currentPlayer;
		int currentPlayerLongestRoad = StandardBoardGraph.Instance.GetLongestRoadForPlayer(player);
		Debug.Log ("LongestRoad For Player: " + player.playerName + ": " + currentPlayerLongestRoad);
		if (currentPlayerLongestRoad < MIN_LONGEST_ROAD_LENGTH) return;

		int oldPlayerWithLongestRoadId = GameManager.Instance.playerWithLongestRoadId;
		Player oldPlayerWithLongestRoad = Player.FindByPlayerId(oldPlayerWithLongestRoadId);
		if (-1 == oldPlayerWithLongestRoadId) {
			player.AddVictoryPoint(LONGEST_ROAD_VICTORY_POINTS);
			GameManager.Instance.playerWithLongestRoadId = player.playerId;
		} else {
			if (player.playerId == oldPlayerWithLongestRoadId) return; // Already has longest road!
			int oldPlayerLongestRoad = StandardBoardGraph.Instance.GetLongestRoadForPlayer(oldPlayerWithLongestRoad);
			if (currentPlayerLongestRoad > oldPlayerLongestRoad) {
				player.AddVictoryPoint(LONGEST_ROAD_VICTORY_POINTS);
				GameManager.Instance.playerWithLongestRoadId = player.playerId;
				oldPlayerWithLongestRoad.RemoveVictoryPoint(LONGEST_ROAD_VICTORY_POINTS);
			}
		}
	}

	void OnMouseDown() {
		if (!visible || built) return;
		if (GameManager.Instance.gameStarted && GameManager.Instance.myTurn()) {
			buildRoad();
		}

	}
}
