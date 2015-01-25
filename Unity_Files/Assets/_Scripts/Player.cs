using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/*

The Player class encapsulates all the information that allows you to differentiate
between the players.

Also manages some other public members







*/
public class Player : MonoBehaviour {

	public static int playerCount{
		get; set;
	}

	public static Player tempPlayer = null;

	public static void CreatePlayer(string name, Color color){
		// adds a player logic
	}


	public static Player GetPlayerForTurn(int turnNum){
		return null;
	}



	
	public string playerName; 
	public int playerId;
	public Color playerColor;

	public ResourceType[] resources;

	// resources
	// dev cards 

	// Use this for initialization
	void Start () {
		playerCount = 0;

		LogResources();
	}
	
	// Update is called once per frame
	void Update () {
	
	}







	// Call if you want to add a resource to a player name
	public bool AddResource(ResourceType resource){
		return true;
	}

	void LogResources(){
		foreach(string resName in Enum.GetNames(typeof(ResourceType))){
			Debug.Log(resName + " woooo");
		}
	}







}
