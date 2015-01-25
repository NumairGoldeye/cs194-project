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

	// Each index corresponds to a the ResourceType by enum
	// resourceCounts[0] should be the number of sheep
	public int[] resourceCounts;

	// resources
	// dev cards 

	// Use this for initialization
	void Start () {
		playerCount = 0;
		resourceCounts = new int[5];

		AddResource(ResourceType.sheep, 200);
		LogResources();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	// Call if you want to add a resource to a player name
	// OPtional second parameter for multiple resources
	// Returns true if it works?
	public bool AddResource(ResourceType resource, int amount = 1){
		// Debug.Log(resource.ToString() + amount);
		resourceCounts[(int)resource] += amount;

		return true;
	}

	// Returns the number of a specific resource that a player has
	// returns -1 on terrible terrible failure
	public int GetResourceCount(ResourceType resource){

		// Debug.Log((int)resource);
		return resourceCounts[(int) resource];

	}

	// Call this if you want to see how many of each Resource a player has
	void LogResources(){
		foreach(ResourceType res in Enum.GetValues(typeof(ResourceType))){
			if (ResourceType.desert != res)
				Debug.Log(res.ToString() + GetResourceCount(res));
		}
	}







}
