using UnityEngine;
using System.Collections;
using UnityEditor;


/*

This class is very very important. 

It has 2 functions

to provide booleans for an internal state machine

TurnState.instance.isPlayerTurn(1);
TurnState.instance.isPlacingRoad();
TurnState.instance.isPlacingCity();



Turns have the following order: 

Roll Dice 
Trade
Build

Needed UI for this proto:

Roll Dice button

Trade button
Dev Button
Build Button

End turn button


*/




public class TurnState : MonoBehaviour {


    void Awake()
    {

    }

 
    void Start()
    {
    	// Debug.Log("TurnState start");
    	currentPlayerIndex = ArrayUtility.IndexOf(playerOrder, PlayerEnum.P1);
        TurnState.currentPlayer = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update(){

    }

    int numPlayers = 4;
    int currentPlayerIndex = 0;
    int numTurns = 0;


    // Replace this with actual players later.
    enum PlayerEnum{ P1, P2, P3, P4};   
    PlayerEnum[] playerOrder = {PlayerEnum.P1, PlayerEnum.P2, PlayerEnum.P3, PlayerEnum.P4};
    // static Player[] players = new Player[]{};
    
    public static Player currentPlayer;

    enum TurnPart {roll, trade, build};
    enum UIState { 
        // onTurnNormal, offTurnNormal, 
        // showBuy, showBuyRoad, showBuyCity, showBuySettlement, showBuyDev,
        // showDevCard, showKnight, showYearPlenty, showMonopoly, showRoadLaying,
        //showTrade?  argh


    };
    

    void StartTurn(){

    }

    void EndTurn(){

    }


    void NextPlayerTurn(){
    	numTurns++;
    	currentPlayerIndex = numTurns % numPlayers;
    }

 	void LogCurrentPlayerTurn(){
 		NextPlayerTurn();
 		Debugger.Log("TurnState", currentPlayerIndex.ToString());
 	} 







}
