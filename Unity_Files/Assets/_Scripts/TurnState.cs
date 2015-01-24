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



*/
public class TurnState : MonoBehaviour {

	// This is a Singleton

    //Here is a private reference only this class can access
    private static TurnState _instance;
 
    //This is the public reference that other classes will use
    public static TurnState instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.
            if(_instance == null)
                _instance = GameObject.FindObjectOfType<TurnState>();
            return _instance;
        }
    }


    void Awake()
    {
        if(_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if(this != _instance)
                Destroy(this.gameObject);
        }
    }
 
    void Start()
    {
    	Debug.Log("TurnState start");
    	currentPlayerIndex = ArrayUtility.IndexOf(playerOrder, PlayerEnum.P1);
    }


    void Update(){

    }



    


    int numPlayers = 4;
    int currentPlayerIndex;
    int numTurns = 0;


    // Replace this with actual players later.
    enum PlayerEnum{ P1, P2, P3, P4};   
    PlayerEnum[] playerOrder = {PlayerEnum.P1, PlayerEnum.P2, PlayerEnum.P3, PlayerEnum.P4};


    void nextPlayerTurn(){
    	numTurns++;
    	currentPlayerIndex = numTurns % numPlayers;
    }

 	void LogCurrentPlayerTurn(){
 		nextPlayerTurn();
 		Debug.Log(currentPlayerIndex);
 	} 







}
