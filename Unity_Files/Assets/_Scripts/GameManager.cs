using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Currently this class is only used to set up the board.
 * */
public class GameManager : MonoBehaviour {

	private static int numTiles = 19; //number of tiles in play
	private int tilesRemaining; 
	public enum tileType {sheep, wood, brick, ore, wheat, desert};
	private static int[] tileCounts = {4, 4, 3, 3, 4, 1};
	List<tileType> tiles;

	private void createTiles () {
		tiles = new List<tileType>();
		tilesRemaining = numTiles;
		
		foreach(tileType tile in Enum.GetValues(typeof(tileType))) {
			for (int i = 0; i < tileCounts[(int)tile]; i++) {
				tiles.Add(tile);
			}
		}
		tiles.Shuffle ();
	}
	                          

	void Awake () {
		createTiles ();
	}

	public tileType assignTileType () {
		if (tilesRemaining <= 0) {
			Debug.LogError("Tried to assign tiles past tile limit");
			return tileType.sheep; //returns sheep to not crash program
		}

		tileType val = tiles[tilesRemaining - 1];

		tilesRemaining--;

		return val;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
