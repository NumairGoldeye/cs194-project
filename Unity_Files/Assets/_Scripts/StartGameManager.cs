﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

	public static Player currentPlayer;
	public static int currentPlayerIndex = 0;
	
	public static GameObject settlements;
	public static GameObject roads;

	public static bool secondPhase = false; // the second countdown phase
	public static bool builtSettlement = false; // if true, then building road

	public static bool finished = false;


	// Called in UI manager startup....
	public static void Startup(){
		// Opens the UI...
		// Setups the first player
		// adds the game objects

		UIManager.ChangeMajorUIState(MajorUIState.start);
		UIManager.UpdateMajorUI();

		settlements = GameObject.FindGameObjectWithTag("Settlement").transform.parent.gameObject;
		roads = GameObject.FindGameObjectWithTag("Road").transform.parent.gameObject;

		currentPlayer = TurnState.players[0];
		settlements.BroadcastMessage("showSettlements");
		roads.BroadcastMessage("makeInvisible");
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
		if (!builtSettlement){
			// After the current player has built a settlement
			settlements.BroadcastMessage("hideSettlements");
			roads.BroadcastMessage("makeVisible");
			builtSettlement = true;

			// Hand out resources for that settlement

		} else {
			// After the previous player has built their road
			settlements.BroadcastMessage("showSettlements");
			roads.BroadcastMessage("makeInvisible");
			builtSettlement = false;
			NextPlayer();
		}
	}

	public static void NextPlayer(){

	}

	/// <summary>
	///  Hides everything and finishes... hands over control to TurnState.
	/// </summary>
	public static void Finish(){

	}











}























