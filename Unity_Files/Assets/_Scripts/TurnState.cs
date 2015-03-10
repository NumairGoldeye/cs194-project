using UnityEngine;
//using UnityEditor;
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

	public static Player currentPlayer;
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

	
	public static int pointsToWin = 10;
	public static Player winningPlayer;
	public static bool gameOver = false;

    public static TurnStateType stateType = TurnStateType.roll;
	public static TurnSubStateType subStateType = TurnSubStateType.none;
    
	public static int numTurns = 0;
	public static ResourceType chosenResource = ResourceType.None;
	public static bool cardPlayedThisTurn = false;

	// Gosh i want to know how to compare objects to null.
	public static bool firstRoadBuilt = false;
	public static RoadClass firstRoad = null;
	public static bool secondRoadBuilt = false;
	public static RoadClass secondRoad = null;

	public static bool freeBuild;
	
	static GameObject victoryPanel;


	// ----- Instance things ----- //

    // Just for public inspector stuff
    public Player thisCurrentPlayer; 
    public Player[] thisPlayers;
	public int _pointsToWin = 10;
	public GameObject _victoryPanel;
	public TradeConsole tradeConsole;

	public static void Startup(){
		DevCard.SetupStatic();

		UIManager.ChangeMajorUIState(MajorUIState.play);
		UIManager.UpdateMajorUI();
		freeBuild = false;
//		TurnState.players = Player.allPlayers.ToArray();
		ResetTurn();
	}

    public static void NextTurnState(){
        Array tsTypes  = Enum.GetValues(typeof(TurnStateType));
        int numTurnStates = tsTypes.Length;
        stateType++;
		if (stateType != TurnStateType.trade) {
			instance.tradeConsole.Disable(); // Should only be on during trade phase
		}
        if (numTurnStates == (int)stateType){
            stateType = (TurnStateType)tsTypes.GetValue(0);
            EndTurn();
        }
    }

    // Changes the current Player...
    public static void EndTurn(){
        numTurns++;
        // Debugger.Log("TurnState", "EndTurn");
		int index = numTurns % GameManager.Instance.players.Count;
        currentPlayer = GameManager.Instance.players[index];

		Debugger.Log ("PlayerHand", "Changing current player to " + TurnState.currentPlayer.playerName);
		GameManager.Instance.networkView.RPC ("syncCurrentPlayer", RPCMode.Others, TurnState.currentPlayer.playerId);
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
		currentPlayer.UpdateHand();
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
			firstRoad.makeInvisible();
			firstRoadBuilt = false;
			firstRoad = null;
		}
		if (secondRoadBuilt){
			secondRoad.ClearPlayer();
			secondRoad.makeInvisible();
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

//		Players.players = Player.allPlayers.ToArray();

//        TurnState.currentPlayer = GameManager.Instance.players[0];
//        thisCurrentPlayer = TurnState.currentPlayer;

//        thisPlayers = Players.players;
//		TurnState.pointsToWin = _pointsToWin;
//		TurnState.victoryPanel = _victoryPanel;
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

    public void EnterTradePhase(){
        TurnState.stateType = TurnStateType.trade;
		instance.tradeConsole.DisplayTradeOptionConsole ();
    }

	/// <summary>
	/// Wrapper function for stati
	/// </summary>
	public void CheckVictoryWrapper(){
		TurnState.CheckVictory();
	}


	/// <summary>
	/// Call this after victory points are awarded
	/// This will update the UI
	/// </summary>
	public static void CheckVictory(){
		foreach (Player p in GameManager.Instance.players){
			Debugger.Log("PlayerHand", "Player: " + p.playerId.ToString() + ", Points: " + p.victoryPoints.ToString());
			if (p.victoryPoints >= pointsToWin){
				// MainUI hide
//				UIManager.MainUI.SetActive(false);
				UIManager.DisableObjs();
				winningPlayer = p;
//				victoryPanel.SetActive(true);
				gameOver = true;
			}
		}
	}

	public void RestartEverythingWrapper(){
		TurnState.RestartEverything();
	}
	/// <summary>
	/// Restarts everthing -> puts it back on the main UI screen.
	/// </summary>
	public static void RestartEverything(){
		// works because there is only one scene

		//Reset all the static variables
		StaticReset();
		UIManager.StaticReset();
		StartGameManager.StaticReset();
		StandardBoardGraph.StaticReset();
		ChatLog.StaticReset();


		UIManager.ChangeMajorUIState(MajorUIState.create);


		Application.LoadLevel(0);
	}

	public static void StaticReset(){
		gameOver = false;
		
		stateType = TurnStateType.roll;
		subStateType = TurnSubStateType.none;
		
		numTurns = 0;
		chosenResource = ResourceType.None;
		cardPlayedThisTurn = false;
		
		// Gosh i want to know how to compare objects to null.
		firstRoadBuilt = false;
		firstRoad = null;
		secondRoadBuilt = false;
		secondRoad = null;
	}
}
