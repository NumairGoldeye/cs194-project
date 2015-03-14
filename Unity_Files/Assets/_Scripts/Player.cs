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

	// The amount of a resource it costs to trade for another resource with no ports.
	private const int TRADE_COST = 4;

	// =----- Static variables -----
	public static int playerCount;
	/// <summary>
	/// All players, regardless of active/inactive status
	/// </summary>
	public static List<Player> everyDarnPlayer = new List<Player>();
	/// <summary>
	/// Only the players active in this game
	/// </summary>
	public static List<Player> allPlayers = new List<Player>();
	public static Color[] playerColors = new Color[]{Color.blue, Color.red, Color.cyan, Color.green, Color.yellow, Color.magenta};

	//AI parameter: playerStrategy indicates what strategy AI uses, depending on what the current situation
	//of other players and the current board scenario 
	public int playerStrategy; 
	// 1 indicates the longestroad strategy 
	// 2 indicates largest army 
	// 3 indicates just building settlement and city 



	/// <summary>
	/// Readonly to check playerActive. Modify using setPlayerActive(bool);
	/// </summary>
	public bool playerActive = true;
	public string playerName;

	/// <summary>
	/// This will give the index into Player.everyDarnPlayer
	/// </summary>
	public int playerId = 0;
	public Color playerColor;
	/// <summary>
	/// Use player.AddVictoryPoint to check for victory after adding point.
	/// </summary>
	public int victoryPoints;

	/// <summary>
	/// The hand.
	/// </summary>
	public PlayerHand hand;

	// Each index corresponds to a the ResourceType by enum
	// resourceCounts[0] should be the number of sheep
	public int[] resourceCounts;
	public int[] devCardCounts; 
	public int totalResources;

	public DevCardType lastCardTypeDrawn;
	public int numUsedKnights = 0;
	public bool hasLargestArmy = false;
	public bool hasLongestRoad = false;

	private List<SettlementClass> settlements;
	private List<RoadClass> roads;



	/// <summary>
	/// Needs to reset Static variables to reload scene
	/// </summary>
	public static void StaticReset(){
		playerCount = 0;
		everyDarnPlayer = new List<Player>();
		allPlayers = new List<Player>();
	}

	//resource getter for AI to understand what strategy to use for AI itself 

	public int sheetcount(Player player){
		return resourceCounts[0];
	}

	
	public int woodcount(Player player){
		return resourceCounts[1];
	}

	
	public int brickcount(Player player){
		return resourceCounts[2];
	}

	
	public int orecount(Player player){
		return resourceCounts[3];
	}

	
	public int wheatcount(Player player){
		return resourceCounts[4];
	}

	//total amount of dev cards equal to largest army potential in AI's eyes 
	public int largestarmyForAI(){
		int count = 0;

		foreach(int dev in devCardCounts){
			count =  count + dev; 
		}
		return count;
	}

	/// <summary>
	/// More reliable player retrieval. Returns null on failure
	/// </summary>
	public static Player FindByPlayerId(int id){
		foreach(Player p in everyDarnPlayer){ 
			if (p.playerId == id)
				return p;
		}
		return null;
	}



	public static void StorePlayer(Player player){
		// Player.playerCount++; - make sure this is called somewhere in Start()
		allPlayers.Add(player);
		everyDarnPlayer.Add(player);
	}

	public static Player[] OtherPlayers(Player player){
		Player[] otherPlayers = new Player[ playerCount - 1 ];
		int index = 0;
		foreach(Player p in allPlayers){
			if (p != player && p != null){
				Debugger.Log("Player", p.playerName);
				otherPlayers[index] = p;
				index++;
			}
		}
		return otherPlayers;
	}

	// resources
	// dev cards 

	// Use this for initialization

	void Awake(){
		playerId = Player.playerCount;
		playerName = "Player " + Player.playerCount.ToString();
		Player.playerCount++;
		Player.StorePlayer(this);
	}

	void Start () {
		// They will actually choose this later
		playerColor = Player.playerColors[playerId];
		gameObject.name = playerName;

		// -1 for the desert
		resourceCounts = new int[Enum.GetNames(typeof(ResourceType)).Length - 1];

		devCardCounts = new int[Enum.GetNames(typeof(DevCardType)).Length];

		settlements = new List<SettlementClass>();
		roads = new List<RoadClass>();

//		 AddDevCard(DevCardType.roadBuilding);
//		 AddDevCard(DevCardType.knight);
//		 AddDevCard(DevCardType.monopoly);
//		 AddDevCard(DevCardType.victoryPoint);
//		 AddDevCard(DevCardType.victoryPoint);
		 AddResource(ResourceType.Wood, 2);
//		 AddResource(ResourceType.brick, 2);
		// LogResources();
		// Debugger.Log("Charlie", "Something amazing");
		// BuyManager.Test(this);
		// BuyManager.PurchaseForPlayer(BuyableType.city, this);
		// BuyManager.Test(this);	
	}
	
	// Update is called once per frame
	void Update () {
	}

	//set the initial strategy 
	void Strategyset(){
		playerStrategy = 0;
	}



	/// <summary>
	/// Sets the player active stuatus and modifies the Player.allPlayer array accordingly
	/// </summary>
	public void SetPlayerActive(bool status){
		playerActive = status;
		if (status){
			// if true, then add to allPlayers
			if (Player.allPlayers.FindIndex( FindSelf) == -1){
				Player.allPlayers.Add(this);
				Player.playerCount++;
			}
		} else {
			// if false, remove from allPlayers
			if (Player.allPlayers.FindIndex( FindSelf) > -1){
				Player.allPlayers.Remove(this);
				Player.playerCount--;	
			}
		}
	}

//	private static void UpdateIds(){
//	}

	/// <summary>
	/// weird c# predicate thing that helps finds the player in an array
	/// </summary>
	private bool FindSelf(Player p){
		return (this == p);
	}

	// Call if you want to add a resource to a player name
	// OPtional second parameter for multiple resources
	// Returns true if it works?
	public bool AddResource(ResourceType resource, int amount = 1){
		// Debug.Log(resource.ToString() + amount);
		if (amount < 0) {
			RemoveResource(resource, -amount);
			return true;
		}

		resourceCounts[(int)resource] += amount;
		totalResources += amount;

		if (TurnState.currentPlayer == this){
			hand.AddResourceCard(resource, this, amount);
		}

		return true;
	}

	/// <summary>
	/// Returns the resourectypes as an array
	/// </summary>
	/// <returns>The resource array.</returns>
	public ResourceType[] GetResourceArray(){
		int numResources = 0;
		for(int i = 0; i < resourceCounts.Length; i++){
			if (resourceCounts[i] > 0)
				numResources += resourceCounts[i];
		}

		ResourceType[] result = new ResourceType[numResources];
		int index = 0;

		foreach(ResourceType res in Enum.GetValues(typeof(ResourceType))){
			if (ResourceType.None != res){
				for (int i = 0; i < resourceCounts[(int)res]; i++){
					result[index] = res;
					index++;
				}
			}
		}
		return result;
	}

	/// <summary>
	/// Returns the resourectypes as an array
	/// </summary>
	/// <returns>The resource array.</returns>
	public DevCardType[] GetDevCardArray(){
		int numCards = 0;
		for(int i = 0; i < devCardCounts.Length; i++){
			if (devCardCounts[i] > 0)
				numCards += devCardCounts[i];
		}
		
		DevCardType[] result = new DevCardType[numCards];
		int index = 0;
		
		foreach(DevCardType d in Enum.GetValues(typeof(DevCardType))){
			for (int i = 0; i < devCardCounts[(int) d]; i++){
				result[index] = d;
				index++;
			}
		}
		return result;
	}

	// Call if you want to 
	// Don't know why i want it to return anything. sigh
	public bool RemoveResource(ResourceType resource, int amount = 1){
		//TODO: check if there are enough resources to remove
		resourceCounts[(int)resource] -= amount;
		totalResources -= amount;

		hand.RemoveResourceCard(resource, this, amount);
		return true;
	}

	// CHecks if a player at least has some number of resources
	public bool HasResourceAmount(ResourceType resource, int amount){
		return GetResourceCount(resource) >= amount;
	}

	public int getTotalResources() {
		return totalResources;
	}

	public int removeRandomResource() {
		int removeIndex = UnityEngine.Random.Range (0, totalResources);
		int resourcesSeen = 0;
		int i = 0;
		for (i = 0; i < Enum.GetNames(typeof(ResourceType)).Length - 1; i++) {
			int resourceInBucket = resourceCounts[i];
			if (resourceInBucket + resourcesSeen > removeIndex) {
				RemoveResource((ResourceType)i, 1);
				return i;
			}
			else {
				resourcesSeen += resourceCounts[i];
			}
		}
		return i;
	}


	public void removeHalfResources() {
		//TODO let the players choose
		int resourcesToRemove = totalResources / 2;
		for (int i = 0; i < resourcesToRemove; i++) {
			removeRandomResource();
		}
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
			if (ResourceType.None != res)
				Debugger.Log(flag, res.ToString() + GetResourceCount(res));
		}
	}

	public int DevCardCount(DevCardType devCard){
		return devCardCounts[(int) devCard];
	}

	public bool HasCard(DevCardType devCard){
		return devCardCounts[(int) devCard] > 0;
	}

	/// <summary>
	/// Draws a random card. Should be called by UI
	/// </summary>
	/// <returns>The random card.</returns>
	public DevCardType DrawRandomCard(){
		DevCardType newCardType = DevCard.RandomCard();
		AddDevCard(newCardType);
		return newCardType;
	}


	public bool AddDevCard(DevCardType devCard){
		devCardCounts[(int)devCard] += 1;
		lastCardTypeDrawn = devCard;
		if (devCard == DevCardType.victoryPoint){
			AddVictoryPoint();
		}

		if (this == TurnState.currentPlayer){
//			Debug.Log("foo");
			hand.AddDevCard(devCard, this);
		} else {
//			Debug.Log ("bar");
		}

		return true;
	}

	public bool RemoveDevCard(DevCardType devCard){
		devCardCounts[(int)devCard] -= 1;
		if (devCard == DevCardType.victoryPoint){
			victoryPoints--;
		}
		if (this == TurnState.currentPlayer){
			hand.RemoveDevCard(devCard, this);
		}
		return true;
	}

	public void AddRoad(RoadClass newRoad){
		roads.Add(newRoad);
	}

	public void AddSettlement(SettlementClass newSettlement){
		settlements.Add(newSettlement);
	}
	//  Will remove settlements from the settlements list
	// public void AddCity(CityClass newCity){
	// 	settlements.Remove(newCity.settlement)

	// 	cities.Add(newCity);
	// }

	// Returns an typed array
	public RoadClass[] GetRoads(){
		return roads.ToArray();
	}

	public SettlementClass[] GetSettlements(){
		return settlements.ToArray();
	}


	public void AddVictoryPoint(int amount = 1){
		victoryPoints +=  amount;
		TurnState.CheckVictory();
	}

	public void RemoveVictoryPoint(){
		victoryPoints--;
//		TurnState.CheckVictory();
	}

	public bool HasResources(TradeCounter counter) {
		foreach (KeyValuePair<ResourceType, int> pair in counter) {
			if (!HasResourceAmount(pair.Key, pair.Value)) {
				return false;
			}
		}
		return true;
	}


	/// <summary>
	/// Call this whenever the cards or resouces are updated
	/// </summary>
	public void UpdateHand(){
		hand.UpdateHand();
	}

	// public CityClass[] GetCities(){
	// 	return cities.ToArray();
	// }

	public int getTradeRatioFor(ResourceType resource) {
		int result = TRADE_COST;
		foreach (SettlementClass settlement in settlements) {
			PortClass port = settlement.port;
			if (port != null && port.ratio < result && (resource == port.type || ResourceType.None == port.type)) {
				result = port.ratio;
			}
		}
		return result;
	}



}
