﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// 
/// 
/// 
/// StartGameManager will control everything from the start of the game 
/// to the first move of the normal turns. 
/// 
/// Controls the 4 players each putting down their first settlement and road
/// and then the resource at the end. 
/// 
/// MAKES CRAZY ASSUMPTIONS ABOUT PLAYER TURN ORDER. Player ID 0 goes first, playerID 1 goes second
/// 
/// The pipeline will be - CreateGameManager - StartGameManager - TurnStateManager
/// 
/// 
/// 
/// 
/// Maybe picks up RoadBuilt Events?
/// 
/// 
/// 
/// </summary>



public class StartGameManager {

//	public static Player currentPlayer;
	public static List<int> playerTurnCounts;
	
	public static GameObject settlements;
	public static GameObject roads;

	public static bool secondPhase = false; // the second countdown phase
	public static bool builtSettlement = false; // if true, then building road

	public static bool finished = true;
	public static bool startedUp = false;
	

	private static void initializePlayerTurnCountList() {
		playerTurnCounts = new List<int> ();
		for (int i = 0; i < GameManager.Instance.players.Count; i++) {
			playerTurnCounts.Add(0);
		}
	}

	// Called in UI manager startup....
	public static void Startup(){
		if (startedUp) return;
		initializePlayerTurnCountList ();
		

		finished = false;

		// Opens the UI...
		// Setups the first player
		// adds the game objects

		UIManager.ChangeMajorUIState(MajorUIState.start);
		UIManager.UpdateMajorUI();

		settlements = GameObject.FindGameObjectWithTag("Settlement").transform.parent.gameObject;
		roads = GameObject.FindGameObjectWithTag("Road").transform.parent.gameObject;
		TurnState.freeBuild = true;
		if (GameManager.Instance.myTurn()) {
			settlements.BroadcastMessage("showSettlementStartup");
			roads.BroadcastMessage("makeInvisible");
			if (GameManager.Instance.myPlayer.IsAI()) {
				startedUp = true;
				GameManager.Instance.myPlayer.brain.SetupSettlement();
			}
		}

		startedUp = true;
	}

	/// <summary>
	/// For each phase, update the scene to properly accompany it
	/// start by showing the cities
	/// 
	/// builtSettlment = false;
	///  - shows the settlements 
	///  - click the settlement  
	/// 	- build it, give it to player
	///  	- if secondPhase then give resources on build
	///  	- then call NextPhase()
	/// 	
	/// 
	/// builtSettlement = true;
	///  - shows the roads
	///  - click the road
	/// 	- build it, give to player
	/// 	- then call nextPhase
	/// 
	/// 
	/// </summary>
	public static void NextPhase(){
//		Debug.Log ("worked");
//		Debugger.Log ("PlayerHand", finished.ToString ());
		if (finished) return;
		Startup();


		if (!builtSettlement){
			// After the current player has built a settlement
			settlements.BroadcastMessage("hideSettlement");
			roads.BroadcastMessage("toggleRoadStartup");
			builtSettlement = true;
			if (GameManager.Instance.myPlayer.IsAI()) {
				GameManager.Instance.myPlayer.brain.SetupRoad();
			}
			// Hand out resources for that settlement

		} else {
			// After the previous player has built their road
			roads.BroadcastMessage("makeInvisible");
			GameManager.Instance.networkView.RPC("syncTurnCounter", RPCMode.All, TurnState.currentPlayer.playerId);
			NextPlayer();
		}
	}

	public static void NextPlayer(){
		int sum = playerTurnCounts.Sum (x => x);


		if (sum == playerTurnCounts.Count){
			GameManager.Instance.networkView.RPC("syncSetupPhase", RPCMode.All, Convert.ToInt32(true), Convert.ToInt32(false));
		} else if (sum == playerTurnCounts.Count * 2){
			GameManager.Instance.networkView.RPC("syncSetupPhase", RPCMode.All, Convert.ToInt32(true), Convert.ToInt32(true));
			return;
		} else {
			if (!secondPhase){
				int nextPlayerIndex = 0;
				if (TurnState.currentPlayer.playerId < playerTurnCounts.Count - 1)
					nextPlayerIndex = TurnState.currentPlayer.playerId + 1;
				TurnState.currentPlayer = GameManager.Instance.players[nextPlayerIndex];
			} else { 
				int nextPlayerIndex = playerTurnCounts.Count - 1;
				if (TurnState.currentPlayer.playerId > 0)
					nextPlayerIndex = TurnState.currentPlayer.playerId - 1;
				TurnState.currentPlayer = GameManager.Instance.players[nextPlayerIndex];
			}
			GameManager.Instance.networkView.RPC ("syncCurrentPlayer", RPCMode.Others, TurnState.currentPlayer.playerId);
		}
		GameManager.Instance.networkView.RPC ("nextPhaseStartup", RPCMode.All);
	}

	/// <summary>
	///  Hides everything and finishes... hands over control to TurnState.
	/// </summary>
	public static void Finish(){
		finished = true;
		settlements.BroadcastMessage("hideSettlement");
		roads.BroadcastMessage("makeInvisible");

//		Debug.Log ("It is finished");
	}


	public static void StaticReset(){
		initializePlayerTurnCountList (); // tracks number of players have built both a settlement and a thing
		
		secondPhase = false; // the second countdown phase
		builtSettlement = false; // if true, then building road
		
		finished = true;
		startedUp = false;

	}
}