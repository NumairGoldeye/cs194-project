#pragma warning disable 0219
#pragma warning disable 0414

using UnityEngine;
using UnityEngine.UI;
// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public enum DevCardType { knight, roadBuilding, yearOfPlenty, monopoly, victoryPoint};

/// <summary>
/// Like ResourceClass, will be attached to a DevCard in the PlayerHand
/// 
/// The static functions execute DevCard behaviors.
/// </summary>
public class DevCard : PlayerCard {

	// chance out of 100 for [knight, road, year, monopoly]
	static int[] cardChances;
	static DevCardType knight = DevCardType.knight;
	static DevCardType roadBuilding = DevCardType.roadBuilding;
	static DevCardType yearOfPlenty = DevCardType.yearOfPlenty;
	static DevCardType monopoly = DevCardType.monopoly;
	static DevCardType victoryPoint = DevCardType.victoryPoint;

	// variables for creating the deck
	static DevCardType[] devCardsByChance = new DevCardType[] { knight, roadBuilding, yearOfPlenty, monopoly, victoryPoint };
	static int deckIndex = 0;
	static int deckKnightCount = 14;
	static int deckRoadBuildingCount = 2;
	static int deckYearCount = 2;
	static int deckMonopolyCount = 2;
	static int deckVictoryPointCount = 5;

	private static Dictionary<DevCardType, string> devCardNames;
	private static Dictionary<DevCardType, string> devCardDescriptions = new Dictionary<DevCardType, string>();

	private static bool setup = false;

	//----- Instance things ---//

	public TurnSubStateType stateType;
	
	// Todo Gives a random card
	public static DevCardType RandomCard(){
		return devCardsByChance[ Random.Range(0, devCardsByChance.Length)];
	}



	// Creates the deck to draw from
	public static void CreateDeck(){
		int totalCards = deckKnightCount + deckRoadBuildingCount + deckYearCount + deckMonopolyCount + deckVictoryPointCount;
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
		case DevCardType.knight:
			p.numUsedKnights++;
			//TODO check if player has largest army
			return 1;
			
		}
		return -1;
	}

	// Called in TurnState static
	public static void SetupStatic(){
		if (setup) return;
		setup = true;

		devCardNames = new Dictionary<DevCardType, string>();;
		devCardDescriptions = new Dictionary<DevCardType, string>();
		
		devCardNames.Add(knight, "Knight");
		devCardNames.Add(roadBuilding, "Road Building");
		devCardNames.Add(yearOfPlenty, "Year of Plenty");
		devCardNames.Add(monopoly, "Monopoly!");
		devCardNames.Add(victoryPoint, "Library");
		
		devCardDescriptions.Add(knight, "Move the Robber and take a random resource from a neighboring player");
		devCardDescriptions.Add(roadBuilding, "Build two roads");
		devCardDescriptions.Add(yearOfPlenty, "Take two of any resource");
		devCardDescriptions.Add(monopoly, "Name a resource type. All players must give all resouces of that type to you");
		devCardDescriptions.Add(victoryPoint, "+1 Victory Point");
		
		CreateDeck();
	}



	// Use this for initialization
	public override void Start () {
		base.Start();
		isDev = true;

		DevCard.SetupStatic();


		btn.onClick.AddListener(UseCard);
		

//		Debug.Log ("Card start");
	}
	
	// Update is called once per frame
	void Update () {
//		btn.interactable = TurnState.currentPlayer.HasCard(type) && !TurnState.cardPlayedThisTurn;		
		btn.interactable =  !TurnState.cardPlayedThisTurn;		
	}


	
	void UseCard(){
	

		// This is volatile...
		hand.SelectCard(this);
		hand.popupPanel.SetActive(true);
		hand.playingDevCardPanel.SetActive(true);

		switch (dType){
		case DevCardType.knight:
//			Debug.Log()"Knights not implemented. Hah!");
			break;
		case DevCardType.monopoly:
			hand.resourceButtons.SetActive(true);
			break;
		case DevCardType.roadBuilding:
			break;
		case DevCardType.victoryPoint:
			break;
		case DevCardType.yearOfPlenty:
			hand.resourceButtons.SetActive(true);
			break;
				
		}


		TurnState.SetSubStateType(stateType);
		TurnState.chosenResource = ResourceType.none;
	}

}
