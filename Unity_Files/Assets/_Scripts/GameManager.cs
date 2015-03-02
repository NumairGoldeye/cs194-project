using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Currently this class is only used to set up the board.
 * */
public class GameManager : MonoBehaviour {

	private static BoardGraph graph;

	private static GameManager instance;

	//Keep track of my player ID
	public int myPlayer;

	public Color[] playerColors = new Color[]{Color.blue, Color.red, Color.cyan, Color.green, Color.yellow, Color.magenta};
	public List<Player> players = new List<Player>();

	public static GameManager Instance {
		get {
			if (instance == null) {
				instance = new GameManager();
				graph = StandardBoardGraph.Instance;
			}
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


	/* --------------------------------------------------------------------
	 * Server Functions
	 * --------------------------------------------------------------------*/

	public void respondToPlayerJoin(Player p, NetworkPlayer player)
	{
		networkView.RPC ("associateWithPlayer", player, p.playerId); 
		syncPlayersWithClients ();
	}


	public void syncPlayersWithClients()
	{
		for (int index = 0; index < GameManager.Instance.players.Count; index++) {
			Player p = GameManager.Instance.players[index];
			networkView.RPC("syncPlayerInfo", RPCMode.Others, p.networkPlayer, p.playerId, p.playerName);
		}
	}

	public void syncTurnStateWithClients()
	{
		networkView.RPC ("syncTurnStateInfo", RPCMode.Others, TurnState.currentPlayer.playerId, TurnState.winningPlayer.playerId, (int)TurnState.stateType, 
		                 (int)TurnState.subStateType, Convert.ToInt32(TurnState.gameOver), TurnState.numTurns, (int)TurnState.chosenResource, 
		                 Convert.ToInt32(TurnState.cardPlayedThisTurn), Convert.ToInt32(TurnState.freeBuild));
	}

	public void syncStartStateWithClients()
	{
		//Sync the tiles
		for (int index = 0; index < numTiles; index++) {
			TileClass tile = graph.getTile(index);
			networkView.RPC("syncTileInfo", RPCMode.Others, index, tile.diceValue, Convert.ToInt32(tile.hasRobber), (int)tile.type);
        }
	}

	public Player createPlayer(NetworkPlayer p)
	{
		string name = "Player " + GameManager.Instance.players.Count.ToString ();
		Player player = new Player(GameManager.Instance.players.Count, GameManager.Instance.playerColors[GameManager.Instance.players.Count], p, name);
		GameManager.Instance.players.Add (player);
		if (p == Network.player) {
			GameManager.Instance.myPlayer = player.playerId;
			TurnState.currentPlayer = GameManager.Instance.players[GameManager.Instance.myPlayer];
		} else 
			GameManager.Instance.respondToPlayerJoin (player, p);
		return player;
	}

	public void removePlayer(NetworkPlayer player)
	{
		for (int i = 0; i < players.Count; i++) {
			if (players[i].networkPlayer == player)
				players.RemoveAt(i);
		}
		Debugger.Log ("Network", "Count: " + players.Count.ToString ());
	}

	public void syncResourcesWithClients()
	{
//		for (int index = 0; index < players.Count; index++) {
//			networkView.RPC ("syncResources", RPCMode.Others, );
//		}

	}

	public void handleRobberMove(int tileIndex)
	{
		TileClass tile = graph.getTiles()[tileIndex];
		tile.receiveRobber ();
		GameObject robber = GameObject.Find ("Robber");
		networkView.RPC ("syncRobber", RPCMode.Others, robber.transform.position); 

	}

	/* --------------------------------------------------------------------*/

	/* ---------------------------------------------------------------------
	 * Client Requests
	 * ---------------------------------------------------------------------*/

	public void requestRobberMove(TileClass tile)
	{
		networkView.RPC ("handleRobberMove", RPCMode.Server, tile.tileIndex);
	}

	/* ---------------------------------------------------------------------*/


	public void setRobberTile(TileClass tile) {
		Debugger.Log ("Robber", tile.diceValue.ToString ());
		tileWithRobber = tile;
	}

	public TileClass getRobberTile() {
		return tileWithRobber;
	}
	
	public void createTiles () {
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
	
	void distributeResources (int roll) {
		BoardGraph graph = StandardBoardGraph.Instance;
		//Loop through the tiles and give out resources for ones with the corresponding die roll.
		for (int index = 0; index < graph.TileCount; index++) {
			TileClass tile = graph.getTile(index);
			if (roll == tile.diceValue && !tile.hasRobber) {
				//This is assuming that each tile keeps track of its vertices
				List<SettlementClass> settlements = tile.getSettlements();
				foreach (SettlementClass settlement in settlements) {
					if (settlement.isBuilt() && !settlement.isCity()) {
						//this is assuming that the settlements and cities are storing the playerID
						Player p = GameManager.Instance.players[settlement.getPlayer()];
						p.AddResource(tile.type, 1);
					} else if (settlement.isBuilt() && settlement.isCity()) {
						Player p = GameManager.Instance.players[settlement.getPlayer()];
						p.AddResource(tile.type, 2);
					}
				}
			}
		}
		//TODO sync player resources
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
		if (Network.isClient) 
			networkView.RPC ("rollDice", RPCMode.Server);
		else {
			die1 = UnityEngine.Random.Range (1, 7);
			die2 = UnityEngine.Random.Range (1, 7);
			diceRoll = die1 + die2;

			displayDice (die1, die2);
		
			distributeResources (diceRoll);
			networkView.RPC ("syncDiceRoll", RPCMode.Others, die1, die2);
		}
	}
	
	void Awake () {
		instance = this;
		graph = StandardBoardGraph.Instance;
	}

	public static void StaticReset(){
		instance = null;
	}


	/* ---------------------------------------------------------
	 * RPC Calls
	 * ---------------------------------------------------------*/

	[RPC]
	void syncTileInfo(int tileIndex, int diceValue, int hasRobber, int resourceType)
	{
		TileClass tile = graph.getTile(tileIndex);
		tile.hasRobber = Convert.ToBoolean(hasRobber);
		if (tile.hasRobber) tile.getRobber();
		tile.assignType(diceValue, (ResourceType)resourceType);
	}

	[RPC]
	void associateWithPlayer(int playerID)
	{
		Debugger.Log ("Network", playerID.ToString ());
		GameManager.Instance.myPlayer = playerID;
	}

	[RPC]
	void syncPlayerInfo(NetworkPlayer player, int playerID, string playerName)
	{
		Player p = new Player (playerID, playerColors[playerID], player, playerName);
		players.Add (p);
	}
	[RPC]
	void syncTurnStateInfo(/*TODO fill in arguments*/)
	{
		//TODO assign EVERYTHING
	}
	[RPC]
	void syncDiceRoll(int die1, int die2)
	{
		GameManager.Instance.die1 = die1;
		GameManager.Instance.die2 = die2;
		Debugger.Log ("Network", "Dice Roll: " + (die1 + die2).ToString ());
		displayDice (die1, die2);
	}
	[RPC]
	void syncRobberMove(Vector3 position)
	{
		GameObject robber = GameObject.Find ("Robber");
		robber.transform.position = position;

	}
}
