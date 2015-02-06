using UnityEngine;
// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public enum DevCardType { knight, roadBuilding, yearOfPlenty, monopoly, victoryPoint};

public class DevCard : MonoBehaviour {

	// chance out of 100 for [knight, road, year, monopoly]
	static int[] cardChances;
	static DevCardType knight = DevCardType.knight;
	static DevCardType roadBuilding = DevCardType.roadBuilding;
	static DevCardType yearOfPlenty = DevCardType.yearOfPlenty;
	static DevCardType monopoly = DevCardType.monopoly;
	static DevCardType victoryPoint = DevCardType.victoryPoint;


	static DevCardType[] devCardsByChance = new DevCardType[] { knight, roadBuilding, yearOfPlenty, monopoly, victoryPoint };


	private static Dictionary<DevCardType, string> devCardNames = new Dictionary<DevCardType, string>();
	private static Dictionary<DevCardType, string> devCardDescriptions = new Dictionary<DevCardType, string>();
	

	// Todo Gives a random card
	public static DevCardType RandomCard(){
		return devCardsByChance[ Random.Range(0, devCardsByChance.Length)];
	}

	public static string NameForCardType(DevCardType type){
		string name = null;
		if (devCardNames.TryGetValue(type, out name)){
			return name;
		} else {
			return "No name for that type";
		}
	}

	public static string DescForCardType(DevCardType type){
		string desc = null;
		if (devCardDescriptions.TryGetValue(type, out desc)){
			return desc;
		} else {
			return "No desc for that type";
		}
	}

	public static string ExecutedCardDesc(DevCardType type, int info){
		switch (type){
		case DevCardType.knight:
			return "You have played a knight and robbed so and so";
		case DevCardType.monopoly:
			return "You have played a monopoly and gained " + info.ToString() + " " + TurnState.chosenResource.ToString();
		case DevCardType.roadBuilding:
			return "You have built two roads? Do i even need this?";
		case DevCardType.victoryPoint:
			return " Buhhh";
		case DevCardType.yearOfPlenty:
			return "You have gained " + info.ToString() + " " + TurnState.chosenResource.ToString();
				
		}

		return "you've messed up - executedCardDesc in DevCard.cs";
	}

	// Carries out the effects of the monopoly on the players...
	// returns number of resources gained
	// 
	public static int EnactMonopoly(Player mainPlayer, ResourceType resource){
		Player[] otherPlayers = Player.OtherPlayers(mainPlayer);

		int totalResourceCount = 0;
		int thisCount = 0;

		foreach(Player p in otherPlayers){
			thisCount = p.GetResourceCount(resource);
			p.RemoveResource(resource, thisCount);
			
			totalResourceCount += thisCount;
		}
		// Todo - log which player loses what...
		mainPlayer.AddResource(resource, totalResourceCount);
		return totalResourceCount;
	}

	public static int EnactYearOfPlenty(Player mainPlayer, ResourceType resource){
		mainPlayer.AddResource(resource, 2);
		return 2;
	}

	// To be called by a UI element
	// Doesn't remove the card - the UI element does that.
	public static int ExecuteCard(DevCardType cardType){
		Player p = TurnState.currentPlayer;
		ResourceType r = TurnState.chosenResource;
		// probably could just do this with a function map
		switch (cardType){
		case DevCardType.monopoly: 
			return EnactMonopoly(p, r);
		case DevCardType.yearOfPlenty:
			return EnactYearOfPlenty(p, r);
		}
		return -1;
	}

	void Awake(){
		DevCard.devCardNames.Add(knight, "Knight");
		DevCard.devCardNames.Add(roadBuilding, "Road Building");
		DevCard.devCardNames.Add(yearOfPlenty, "Year of Plenty");
		DevCard.devCardNames.Add(monopoly, "Monopoly!");
		DevCard.devCardNames.Add(victoryPoint, "Library");
		
		DevCard.devCardDescriptions.Add(knight, "Move the Robber and take a random resource from a neighboring player");
		DevCard.devCardDescriptions.Add(roadBuilding, "Build two roads");
		DevCard.devCardDescriptions.Add(yearOfPlenty, "Take two of any resource");
		DevCard.devCardDescriptions.Add(monopoly, "Name a resource type. All players must give all resouces of that type to you");
		DevCard.devCardDescriptions.Add(victoryPoint, "+1 Victory Point");
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
