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

	
	public string playerName; 
	public int playerId;
	public Color playerColor;

	// Each index corresponds to a the ResourceType by enum
	// resourceCounts[0] should be the number of sheep
	public int[] resourceCounts;
	public int[] devCardCounts; 

	// resources
	// dev cards 

	// Use this for initialization
	void Start () {
		playerCount = 0;
		resourceCounts = new int[5];
		devCardCounts = new int[4];

		// AddDevCard(DevCardType.knight);
		// AddDevCard(DevCardType.knight);
		// AddDevCard(DevCardType.knight);
		// AddDevCard(DevCardType.knight);
		// AddResource(ResourceType.ore, 3);
		// AddResource(ResourceType.wheat, 2);
		// LogResources();
		// Debugger.Log("Charlie", "Something amazing");
		// BuyManager.Test(this);
		// BuyManager.PurchaseForPlayer(BuyableType.city, this);
		// BuyManager.Test(this);	
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

	// Call if you want to 
	// Don't know why i want it to return anything. sigh
	public bool RemoveResource(ResourceType resource, int amount = 1){
		resourceCounts[(int)resource] -= amount;
		return true;
	}

	// CHecks if a player at least has some number of resources
	public bool HasResourceAmount(ResourceType resource, int amount){
		return GetResourceCount(resource) >= amount;
	}

	// Returns the number of a specific resource that a player has
	// returns -1 on terrible terrible failure
	public int GetResourceCount(ResourceType resource){
		// Debug.Log((int)resource);
		return resourceCounts[(int) resource];
	}

	// Call this if you want to see how many of each Resource a player has
	// you need it to pass it the Debugger flag
	void LogResources(string flag){
		foreach(ResourceType res in Enum.GetValues(typeof(ResourceType))){
			if (ResourceType.desert != res)
				Debugger.Log(flag, res.ToString() + GetResourceCount(res));
		}
	}

	public int DevCardCount(DevCardType devCard){
		return devCardCounts[(int) devCard];
	}

	public bool HasCard(DevCardType devCard){
		return devCardCounts[(int) devCard] > 0;
	}

	public bool AddDevCard(DevCardType devCard){
		devCardCounts[(int)devCard] += 1;
		return true;
	}

	public bool RemoveDevCard(DevCardType devCard){
		devCardCounts[(int)devCard] -= 1;
		return true;
	}








}
