using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/*
 * Currently this class is only used to set up the board.
 * */
public class GameManager : MonoBehaviour {

	public BoardGraph graph;

	private static GameManager instance;

	public GameObject settlements;

	//Keep track of my player ID
	public Player myPlayer;
	public string myPlayerName = "";

	public Button RollButton;


	public Color[] playerColors = new Color[]{Color.blue, Color.red, Color.cyan, Color.green, Color.yellow, Color.magenta};
	public List<Player> players = new List<Player>();

	public bool gameStarted = false;

	public static GameManager Instance {
		get {
//			if (instance == null) {
//				instance = new GameManager();
//				graph = StandardBoardGraph.Instance;
//			}
			return instance;
		}
	}

	public struct TileInfo {
		public int diceNumber;
		public ResourceType type; 

		public TileInfo(ResourceType type, int diceNumber) {
			this.diceNumber = diceNumber;
			this.type = type;	
		}
	}

	private int numTiles = 19; //number of tiles in play
	private int diceRoll;
	private int die1;
	private int die2;
	private TileClass tileWithRobber;
	private Player playerWithLargestArmy = null;

	private int[] tileCounts = {4, 4, 3, 3, 4, 1};
	// The dice numbers for each tyle, in order
	private int[] diceNumbers =
		{5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11};

	static List<TileInfo> tiles;

	public UnityEngine.UI.RawImage die1Image;
	public UnityEngine.UI.RawImage die2Image;


//	void Update() {
//		Debugger.Log ("PlayerHand", "Current Player ID: " + TurnState.currentPlayer.playerId.ToString () + ", Name: " + 
//		              TurnState.currentPlayer.playerName + ", Color: " + TurnState.currentPlayer.playerColor.ToString ());
//	}

	/* --------------------------------------------------------------------
	 * Server Functions
	 * --------------------------------------------------------------------*/

	public void respondToPlayerJoin(NetworkPlayer player, int playerID)
	{
		for (int i = 0; i < GameManager.Instance.players.Count; i++) {
			Debugger.Log ("PlayerHand", "Syncing Player... " + players[i].playerId.ToString());
			Player p = GameManager.Instance.players[i];
			networkView.RPC("syncPlayerInfo", RPCMode.Others, p.networkPlayer, p.playerId, p.playerName);
		}
		networkView.RPC ("associateWithPlayer", player, playerID); 
	}

	public void syncStartStateWithClients()
	{
		for (int index = 0; index < numTiles; index++) {
			TileClass tile = GameManager.Instance.graph.getTile(index);
			networkView.RPC("syncTileInfo", RPCMode.Others, index, tile.diceValue, Convert.ToInt32(tile.hasRobber), (int)tile.type);
        }
	}

	public Player createPlayer(NetworkPlayer p, string playerName)
	{
		Player player = new Player(players.Count, p, playerName);
		GameManager.Instance.players.Add (player);
		if (p == Network.player) {
			myPlayer = player;
		}
		return player;
	}

	public void removeResources() {
		foreach (Player player in GameManager.Instance.players) {
			if (player.getTotalResources() > 7){ //TODO: make 7 into a constant in a reasonable place
				player.removeHalfResources();
			}
		}
	}

	void distributeResources (int roll) {
		BoardGraph g = GameManager.Instance.graph;
		//Loop through the tiles and give out resources for ones with the corresponding die roll.
		for (int index = 0; index < g.TileCount; index++) {
			TileClass tile = g.getTile(index);
			if (roll == tile.diceValue && !tile.hasRobber) {
				//This is assuming that each tile keeps track of its vertices
				List<SettlementClass> settlements = tile.getSettlements();
				foreach (SettlementClass settlement in settlements) {
					if (settlement.isBuilt() && !settlement.isCity()) {
						//this is assuming that the settlements and cities are storing the playerID
						Player p = GameManager.Instance.players[settlement.getPlayer()];
						p.AddResource(tile.type, 1);
						networkView.RPC("syncResources", RPCMode.Others, p.playerId, (int)tile.type, 1);
					} else if (settlement.isBuilt() && settlement.isCity()) {
						Player p = GameManager.Instance.players[settlement.getPlayer()];
						p.AddResource(tile.type, 2);
						networkView.RPC("syncResources", RPCMode.Others, p.playerId, (int)tile.type, 2);
					}
				}
			}
		}
	}


	/* --------------------------------------------------------------------*/


	/* ---------------------------------------------------------
	 * RPC Calls
	 * ---------------------------------------------------------*/

	[RPC]
	 void syncCurrentPlayer(int playerID) {
		TurnState.currentPlayer = GameManager.Instance.players[playerID];
		Debugger.Log ("PlayerHand", "Changing current player to " + TurnState.currentPlayer.playerName);
		if (GameManager.Instance.myTurn()) {
			RollButton.interactable = true;
		}
//		Debugger.Log ("PlayerHand", "Changing current player to " + TurnState.currentPlayer.playerName);
	}

	[RPC]
	void syncSteal(int ownerID, int resource) {
		ResourceType type = (ResourceType)resource;
		GameManager.Instance.players [ownerID].RemoveResource (type, 1);
		TurnState.currentPlayer.AddResource (type, 1);
	}

	[RPC]
	void syncSettlementBuild(int index) {
		SettlementClass settlement = graph.getSettlement (index);
		settlement.setPlayerSettlement ();
	}
	
	[RPC]
	void syncCityUpgrade(int index) {
		SettlementClass settlement = graph.getSettlement (index);
		settlement.setPlayerCity ();
	}

	[RPC]
	void syncRoadBuild(int index) {
		RoadClass road = graph.getRoad (index);
		road.SetPlayer ();
	}

	[RPC]
	void syncTileInfo(int tileIndex, int diceValue, int hasRobber, int resourceType) {
		GameManager.Instance.gameStarted = true;
		TileClass tile = graph.getTile(tileIndex);
		tile.hasRobber = Convert.ToBoolean(hasRobber);
		if (tile.hasRobber) tile.getRobber();
		tile.assignType(diceValue, (ResourceType)resourceType);
	}

	[RPC]
	void removePlayer(NetworkPlayer player) {
		for (int i = 0; i < GameManager.Instance.players.Count; i++) {
			if (GameManager.Instance.players[i].networkPlayer == player) {
				GameManager.Instance.players.RemoveAt(i);
				for (int j = i+1; j < players.Count; j++) {
					GameManager.Instance.players[i].playerId--;
				}
			}
		}
		//		Debugger.Log ("Network", "Count: " + players.Count.ToString ());
	}

	[RPC]
	void updatePlayerName(string playerName, int playerID) {
		for (int i = 0; i < GameManager.Instance.players.Count; i++) {
			if (GameManager.Instance.players[i].playerId == playerID) 
				GameManager.Instance.players[i].playerName = playerName;
		}
	}

	[RPC]
	void associateWithPlayer(int playerID) {
//		Debugger.Log ("Network", playerID.ToString ());
		for (int i = 0; i < GameManager.Instance.players.Count; i++) {
			if (GameManager.Instance.players[i].playerId == playerID)
				myPlayer = GameManager.Instance.players[i];
		}
		networkView.RPC ("updatePlayerName", RPCMode.All, myPlayerName, playerID);
//		Debugger.Log ("PlayerHand", "Associating player..." + myPlayer.playerId.ToString());
	}
	
	[RPC]
	void syncPlayerInfo(NetworkPlayer player, int playerID, string playerName) {
//		Debugger.Log ("PlayerHand", "Syncing Player..." + playerID.ToString());
		Player p = new Player (playerID, player, playerName);
		if (!GameManager.Instance.players.Exists(x => x.playerId == p.playerId)) {
		 	GameManager.Instance.players.Add (p);
		}
//		Debugger.Log ("PlayerHand", "Players in game: " + GameManager.Instance.players.Count.ToString ());
	}
	
	[RPC]
	void syncDiceRoll(int die1, int die2) {
		GameManager.Instance.die1 = die1;
		GameManager.Instance.die2 = die2;
		diceRoll = die1 + die2;
		if (diceRoll == 7 && myTurn()) {
			TurnState.SetSubStateType(TurnSubStateType.robbering);
		}
		displayDice (die1, die2);
	}

	[RPC]
	void syncChatMessage(string playerName, string message) {
		ChatLog.Instance.addMessage (playerName, message);
	}

	[RPC]
	void syncRobberMove(int index) {
		TileClass tile = graph.getTile (index);
		GameManager.Instance.removeRobber ();
		tile.getRobber ();
	}

	[RPC]
	void syncResources(int playerID, int resourceType, int count) {
		GameManager.Instance.players [playerID].AddResource ((ResourceType)resourceType, count);
		Debugger.Log ("PlayerHand", "Resource Stolen: " + resourceType.ToString ());
		Debugger.Log ("PlayerHand", "Player: " + playerID.ToString() + " has: " + 
		              string.Join(",", Array.ConvertAll<int, string>(GameManager.Instance.players[playerID].resourceCounts, Convert.ToString)));
	}

	[RPC]
	void makeDiceRoll() {
		die1 = UnityEngine.Random.Range (1, 7);
		die2 = UnityEngine.Random.Range (1, 7);
		diceRoll = die1 + die2;
		
		displayDice (die1, die2);
		if (diceRoll == 7) {
			removeResources();
		} else {
			distributeResources (diceRoll);
		}
		networkView.RPC ("syncDiceRoll", RPCMode.Others, die1, die2);
	}

	[RPC]
	void startupGame() {
		StartGameManager.Startup();
	}

	[RPC]
	void nextPhaseStartup() {
		if (myTurn()) {
			StartGameManager.settlements.BroadcastMessage("showSettlementStartup");
			StartGameManager.roads.BroadcastMessage("makeInvisible");
			StartGameManager.builtSettlement = false;
		}
	}

	[RPC]
	void syncSetupPhase(int secondPhase, int finished) {
		StartGameManager.secondPhase = Convert.ToBoolean (secondPhase);
		if (Convert.ToBoolean(finished) == true)
			StartGameManager.Finish();
	}

	[RPC]
	void syncTurnCounter(int playerID) {
		StartGameManager.playerTurnCounts[playerID] += 1;
	}

	[RPC]
	void syncNextTurn(int turnCount) {
		TurnState.numTurns = turnCount;
	}

	/* --------------------------------------------------------------------------------*/

	public bool myTurn() {
		return (GameManager.Instance.gameStarted && TurnState.currentPlayer.playerId == myPlayer.playerId);
	}

	public void removeRobber() {
		getRobberTile().hasRobber = false;
	}

	public void setRobberTile(TileClass tile) {
		Debugger.Log ("Robber", tile.diceValue.ToString ());
		GameManager.Instance.tileWithRobber = tile;
		GameManager.Instance.tileWithRobber.hasRobber = true;
	}

	public TileClass getRobberTile() {
		return GameManager.Instance.tileWithRobber;
	}
	
	public void createTiles () {
		if (Network.isClient) return;
		tiles = new List<TileInfo>();

		List<ResourceType> tileResources = new List<ResourceType> ();

		foreach(ResourceType tile in Enum.GetValues(typeof(ResourceType))) {
			for (int i = 0; i < tileCounts[(int)tile]; i++) {
				tileResources.Add(tile);
			}
		}
		tileResources.Shuffle ();

		int resourceIndex = 0;
		int numberIndex = 0;
		while (resourceIndex < tileResources.Count) {
			ResourceType type = tileResources[resourceIndex];
			if (type == ResourceType.None) {
				tiles.Add(new TileInfo(type, 0));
			} else {
				tiles.Add(new TileInfo(type, diceNumbers[numberIndex]));
				numberIndex++;
			}
			resourceIndex++;
		}
		assignTileInfo ();
	}
	                         
	public void assignTileInfo () {
		for (int i = 0; i < numTiles; i++) {
			TileClass tile = graph.getTile (i);
			tile.assignType(tiles[tile.tileNumber].diceNumber, tiles[tile.tileNumber].type);
			if (tile.type == ResourceType.None) {
				tile.getRobber();
			}
			else {
				tile.hasRobber = false;
			}
		}
	}

	public void distributeResourcesForSettlement(SettlementClass settlement) {
		List<TileClass> tilesForSettlement = StandardBoardGraph.Instance.getTilesForSettlement (settlement);
		foreach(TileClass tile in tilesForSettlement) {
			if (tile.type != ResourceType.None)
				TurnState.currentPlayer.AddResource(tile.type, 1);
		}
	}

	/// <summary>
	/// Displaies the dice.
	/// </summary>
	/// <param name="die1">Die1.</param>
	/// <param name="die2">Die2.</param>
	void displayDice(int die1, int die2) {
		DiceImages images = GetComponent<DiceImages> ();

		switch (die1) {
			case 1:
				die1Image.texture = images.dice1;
				break;
			case 2:
				die1Image.texture = images.dice2;
				break;
			case 3:
				die1Image.texture = images.dice3;
				break;
			case 4:
				die1Image.texture = images.dice4;
				break;
			case 5:
				die1Image.texture = images.dice5;
				break;
			case 6:
				die1Image.texture = images.dice6;
				break;
		}

		switch (die2) {
			case 1:
				die2Image.texture = images.dice1;
				break;
			case 2:
				die2Image.texture = images.dice2;
				break;
			case 3:
				die2Image.texture = images.dice3;
				break;
			case 4:
				die2Image.texture = images.dice4;
				break;
			case 5:
				die2Image.texture = images.dice5;
				break;
			case 6:
				die2Image.texture = images.dice6;
				break;
		}
	}

	public int getDiceRoll() {
		return diceRoll;
	}

	/// <summary>
	/// Rolls the dice.
	/// </summary>
	public void rollDice (){
		if (!myTurn()) return;
		if (Network.isClient) 
			networkView.RPC ("makeDiceRoll", RPCMode.Server);
		else {
			makeDiceRoll();
		}
		TurnState.instance.EnterTradePhase ();
	}
	
	void Awake () {
		instance = this;
		graph = StandardBoardGraph.Instance;
	}

	public static void StaticReset(){
		instance = null;
	}
}