﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Currently this class is only used to set up the board.
 * */
public class GameManager : MonoBehaviour {

	private BoardGraph graph;

	public struct TileInfo {
		public int diceNumber;
		public ResourceType type; 

		public TileInfo(ResourceType type, int diceNumber) {
			this.diceNumber = diceNumber;
			this.type = type;
		}
	}

	private static int numTiles = 19; //number of tiles in play

	private static int[] tileCounts = {4, 4, 3, 3, 4, 1};
	// The dice numbers for each tyle, in order
	private static int[] diceNumbers =
		{5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11};

	List<TileInfo> tiles;

	public UnityEngine.UI.RawImage die1Image;
	public UnityEngine.UI.RawImage die2Image;

	/// <summary>
	/// Creates the tiles.
	/// </summary>
	private void createTiles () {
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
			if (type == ResourceType.desert) {
				tiles.Add(new TileInfo(type, 0));
			} else {
				tiles.Add(new TileInfo(type, diceNumbers[numberIndex]));
				numberIndex++;
			}
			resourceIndex++;
		}

		assignTileInfo ();

	}
	                          
	/// <summary>
	/// Assigns the tile info.
	/// </summary>
	public void assignTileInfo () {

		for (int i = 0; i < numTiles; i++) {
			TileClass tile = graph.getTile (i);
			tile.assignType(tiles[tile.tileNumber].diceNumber, tiles[tile.tileNumber].type);
			if (tile.type == ResourceType.desert) {
				tile.getRobber();
			}
			else {
				tile.hasRobber = false;
			}
		}
	}

	/// <summary>
	/// Distributes the resources.
	/// </summary>
	/// <param name="roll">Roll.</param>
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
						Player p = TurnState.players[settlement.getPlayer()];
						p.AddResource(tile.type, 1);
					} else if (settlement.isBuilt() && settlement.isCity()) {
						Player p = TurnState.players[settlement.getPlayer()];
						p.AddResource(tile.type, 2);
					}
				}
			}
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

	/// <summary>
	/// Rolls the dice.
	/// </summary>
	public void rollDice (){

		int die1 = UnityEngine.Random.Range (1, 7);
		int die2 = UnityEngine.Random.Range (1, 7);
		int roll = die1 + die2;

		displayDice (die1, die2);
		
		distributeResources (roll);
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake () {
		graph = StandardBoardGraph.Instance;
		createTiles ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
