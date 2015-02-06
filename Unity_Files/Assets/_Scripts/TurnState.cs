using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;


/*

This class is very very important. 
Tracks lots of global things, like TurnState.currentPlayer

*/


/*

 */

public enum TurnStateType {roll, trade, build};


/*

TurnSubstate is mostly managed in the DevPanel gameobject inside the BuyPanel

The DevUseButtons all open the DevCard Panel, and the UI elements rely on TurnState.substatetype
in order to tell which card to use

DevConfirmButton is responsible for actually executing the cards

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
	public static bool firstRoadBuilt = false;
	public static bool secondRoadBuilt = false;

    // Just for public inspector stuff
    public Player thisCurrentPlayer; 
    public Player[] thisPlayers;
    

    public static void NextTurnState(){
        Array tsTypes  = Enum.GetValues(typeof(TurnStateType));
        int numTurnStates = tsTypes.Length;
        stateType++;
        if (numTurnStates == (int)stateType){
            stateType = (TurnStateType)tsTypes.GetValue(0);
            EndTurn();
        }
    }

    // Chantes the current Player...
    public static void EndTurn(){
        numTurns++;
        // Debugger.Log("TurnState", "EndTurn");
        int index = numTurns % Player.playerCount;
        currentPlayer = players[index];

		// Some simple resets
		cardPlayedThisTurn = false;
		firstRoadBuilt = false;
		secondRoadBuilt = false;
    }

	// Returns true if the second time a road is built for roadbuilding
	public static bool CheckSecondRoadBuilt(){
		if (!firstRoadBuilt){
			firstRoadBuilt = true;
			return false;
		} else {
			secondRoadBuilt = true;
			return true;
		}
	}

	// Card is played when the confirmbutton is clicked, not through other things
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

	public static void SetSubStateType(TurnSubStateType state){
		subStateType = state;
	}

	public static void ResetSubStateType2(){
		TurnState.subStateType = TurnSubStateType.none;
	}

	public void ResetSubStateType(){
		TurnState.subStateType = TurnSubStateType.none;
	}

	
	void Awake(){
        _instance = this;

    }

 
    void Start(){
    	// Debug.Log("TurnState start");
    	// currentPlayerIndex = ArrayUtility.IndexOf(playerOrder, PlayerEnum.P1);


        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        // TurnState.players = playerObjects.Select(p => p.GetComponent<Player>());
        for( int i = 0; i < playerObjects.Length; i ++){
            TurnState.players[i] = playerObjects[i].GetComponent<Player>();
        }

        TurnState.currentPlayer = TurnState.players[0];
        thisCurrentPlayer = TurnState.currentPlayer;

        thisPlayers = TurnState.players;

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
    }



}
