using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;


/*
The pipeline will be - CreateGameManager - StartGameManager - TurnStateManager
*/

public enum TurnStateType {roll, trade, build};


/*

TurnSubstate is mostly managed in the DevPanel gameobject inside the BuyPanel

The DevUseButtons all open the DevCard Panel, and the UI elements rely on TurnState.substatetype
in order to tell which card to use

DevConfirmButton is responsible for actually executing the cards


Undoing RoadBuilding
There are 3 ways you can quit from a screen that requires you to undo the roadbuilding
- ending the turn
- pressing the close panel button
- pressing the cancel button

bool middleOfRoadBuilding
- if a road is built and a card has not been played 

resetRoads 
- make each road clear its player
- then make it invisible again?


 */
public enum TurnSubStateType {none, monopolyChoose, yearOfPlentyChoose, roadBuilding, robbering};

public class TurnState : MonoBehaviour {

    public static Player currentPlayer {
        get; set;
    }
    
    
    private static TurnState _instance;
    public static TurnState instance {
        get {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.
            if(_instance == null)
                _instance = GameObject.FindObjectOfType<TurnState>();
            return _instance;
        }
    }


    public static Player[] players = new Player[4];
//    public static int numPlayers = 2;
    public static TurnStateType stateType = TurnStateType.roll;
	public static TurnSubStateType subStateType = TurnSubStateType.none;
    
	public static int numTurns = 0;
	public static ResourceType chosenResource = ResourceType.desert;
	public static bool cardPlayedThisTurn = false;

	// Gosh i want to know how to compare objects to null.
	public static bool firstRoadBuilt = false;
	public static RoadClass firstRoad = null;
	public static bool secondRoadBuilt = false;
	public static RoadClass secondRoad = null;

	public static bool freeBuild;

	static GameObject tradeConsole;


	// ----- Instance things ----- //

    // Just for public inspector stuff
    public Player thisCurrentPlayer; 
    public Player[] thisPlayers;

	public static void Startup(){
		UIManager.ChangeMajorUIState(MajorUIState.play);
		UIManager.UpdateMajorUI();
		freeBuild = false;
		TurnState.players = Player.allPlayers.ToArray();
	}


    public static void NextTurnState(){
        Array tsTypes  = Enum.GetValues(typeof(TurnStateType));
        int numTurnStates = tsTypes.Length;
        stateType++;
		if (stateType != TurnStateType.trade) {
			tradeConsole.SetActive (false); // Should only be on during trade phase
		}
        if (numTurnStates == (int)stateType){
            stateType = (TurnStateType)tsTypes.GetValue(0);
            EndTurn();
        }
    }

    // Chantes the current Player...
    public static void EndTurn(){
        numTurns++;
        // Debugger.Log("TurnState", "EndTurn");
        int index = numTurns % Player.allPlayers.Count;
        currentPlayer = players[index];

		// Some simple resets
		ResetTurn();
    }

	public static void ResetTurn(){
		cardPlayedThisTurn = false;
		freeBuild = false;
		// Also need to reset the UI...
		ClearRoadBuilding();
		ResetSubStateType2();
		UIManager.DisableObjs();
	}

	static void ClearRoadBuilding(){
		firstRoadBuilt = false;
		firstRoad = null;
		secondRoadBuilt = false;
		secondRoad = null;
	}

	// Resets the scene once the panel ends 
	public static void ResetScene(){
		// clear the roads
	}



	// Returns true if the second time a road is built for roadbuilding
	// Stores it just in case
	public static bool CheckSecondRoadBuilt(RoadClass road){
		if (TurnState.subStateType != TurnSubStateType.roadBuilding){
			return true;
		}

		if (!firstRoadBuilt){
			firstRoadBuilt = true;
			firstRoad = road;
			return false;
		} else {
			secondRoad = road;
			secondRoadBuilt = true;
			return true;
		}
	}

	// Card effects are executed when the confirm button is clicked, not through other things
	public static bool PlayCardOnConfirm(){
		return subStateType == TurnSubStateType.monopolyChoose || subStateType == TurnSubStateType.yearOfPlentyChoose;
	}



	// Returns the DevCardType for each particular TurnSubStateType
	public static DevCardType DevTypeForSubstate(){
//		Debug.Log (subStateType.ToString());

		switch (subStateType){
		case TurnSubStateType.monopolyChoose: 
			return DevCardType.monopoly;
		case TurnSubStateType.roadBuilding:
			return DevCardType.roadBuilding;
		case TurnSubStateType.yearOfPlentyChoose:
			return DevCardType.yearOfPlenty;
		case TurnSubStateType.robbering:
			return DevCardType.knight;
		}
		// Some default...
		return (DevCardType) (-1);
	}

	public static TurnSubStateType getSubStateType() {
		return subStateType;
	}

	public static void SetSubStateType(TurnSubStateType state){
		subStateType = state;
		switch (subStateType){
		case TurnSubStateType.roadBuilding:
			freeBuild = true;
			break;
		}
	}

	public static void ResetSubStateType2(){
//		Debugger.Log("TurnState", "foo");
		TurnSubStateType tempState = subStateType;
		subStateType = TurnSubStateType.none;
		switch (tempState){
		case TurnSubStateType.roadBuilding:
			freeBuild = false;
			break;
		}

		// IF the player changes their mind about roadBuilding..
		if (StillRoadBuilding()){
			ResetRoadBuilding();
		}
	}

	// If there 
	static bool StillRoadBuilding(){
		return firstRoadBuilt && !cardPlayedThisTurn;
	}

	static void ResetRoadBuilding(){
		if (firstRoadBuilt){
			firstRoad.ClearPlayer();
			firstRoad.hideIfPossible();
			firstRoadBuilt = false;
			firstRoad = null;
		}
		if (secondRoadBuilt){
			secondRoad.ClearPlayer();
			secondRoad.hideIfPossible();
			secondRoad = null;
			secondRoadBuilt = false;
		}
		freeBuild = false;
	}








	public void ResetSubStateType(){
		TurnState.ResetSubStateType2();
	}

	
	void Awake(){
        _instance = this;

    }

 
    void Start(){
    	// Debug.Log("TurnState start");
    	// currentPlayerIndex = ArrayUtility.IndexOf(playerOrder, PlayerEnum.P1);


//        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
//
//        // TurnState.players = playerObjects.Select(p => p.GetComponent<Player>());
//        for( int i = 0; i < playerObjects.Length; i ++){
//            TurnState.players[i] = playerObjects[i].GetComponent<Player>();
//        }

		TurnState.players = Player.allPlayers.ToArray();

        TurnState.currentPlayer = TurnState.players[0];
        thisCurrentPlayer = TurnState.currentPlayer;

        thisPlayers = TurnState.players;
		// This line is REALLY asking for trouble...is there a smarter way to do this?
		tradeConsole = UIManager.instance.disableOnStart[8];
    }

    void Update(){
    }

    void StartTurn(){
    }


    /*

    How the buttons will work

    Roll Dice phase
        roll dice and dev buttons
            click roll dice to 
                roll dice 
                show the end phase button
                show trade button
    trade phase
        trade, dev, end phase
            click trade dice to
                trade!
            click end phase to
    build phase
        build, dev, end turn

    

    */

    void EnterTradePhase(){
        TurnState.stateType = TurnStateType.trade;
		tradeConsole.SetActive (true);
    }



}
