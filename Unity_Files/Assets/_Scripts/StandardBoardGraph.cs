using UnityEngine;
using System;
using System.Collections.Generic;

// The graph used for a standard game of Catan.
// It is a Singleton, in order to get the instance, use
// <code>BoardGraph graph = StandardBoardGraph.Instance;</code>
using System.Collections;


public class StandardBoardGraph : ArrayBoardGraph {

	private static StandardBoardGraph instance;

	private const int NUM_TILES = 19;
	private const int NUM_ROADS = 72;
	private const int NUM_SETTLEMENTS = 54;
	
	private StandardBoardGraph() {
		// Must maintain this order; edges depend on verticies, and verticies depend on tiles.
		addTiles();
		addSettlements();
		addRoads();
	}

	public static StandardBoardGraph Instance {
		get {
			if (instance == null) {
				instance = new StandardBoardGraph();
			}
			return instance;
		}
	}

	private void addTiles() {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Tile");
		foreach (GameObject tileObject in gameObjects) {
			tiles.Add(tileObject.GetComponent<TileClass>());
		}
		tiles.Sort(new tileIndexComparer());
	}

	/** Compares tiles by their index. */
	private class tileIndexComparer : IComparer<TileClass> {
		public int Compare(TileClass a, TileClass b) {
			return a.tileIndex.CompareTo(b.tileIndex);
		}
	}

	private void addSettlements() {

		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Settlement");
		// Uses array to construct to correct order for settlements, then adds them to the list in order.
		SettlementClass[] settlementArray = new SettlementClass[NUM_SETTLEMENTS];
		foreach (GameObject gameObject in gameObjects) {
			SettlementClass settlement = gameObject.GetComponentInChildren<SettlementClass>();
			settlementArray[settlement.vertexIndex] = settlement;
		}
		foreach (SettlementClass s in settlementArray) {
			settlements.Add(s);
		}
		addSettlementsToTile ();
	}

	private void addSettlementsToTile() {
		addSettlementToTile(0, 0);
		addSettlementToTile(3, 0);
		addSettlementToTile(4, 0);
		addSettlementToTile(7, 0);
		addSettlementToTile(8, 0);
		addSettlementToTile(12, 0);
		addSettlementToTile(1, 1);
		addSettlementToTile(4, 1);
		addSettlementToTile(5, 1);
		addSettlementToTile(8, 1);
		addSettlementToTile(9, 1);
		addSettlementToTile(13, 1);
		addSettlementToTile(2, 2);
		addSettlementToTile(5, 2);
		addSettlementToTile(6, 2);
		addSettlementToTile(9, 2);
		addSettlementToTile(10, 2);
		addSettlementToTile(14, 2);
		addSettlementToTile(7, 3);
		addSettlementToTile(11, 3);
		addSettlementToTile(12, 3);
		addSettlementToTile(16, 3);
		addSettlementToTile(17, 3);
		addSettlementToTile(22, 3);
		addSettlementToTile(8, 4);
		addSettlementToTile(12, 4);
		addSettlementToTile(13, 4);
		addSettlementToTile(17, 4);
		addSettlementToTile(18, 4);
		addSettlementToTile(23, 4);
		addSettlementToTile(9, 5);
		addSettlementToTile(13, 5);
		addSettlementToTile(14, 5);
		addSettlementToTile(18, 5);
		addSettlementToTile(19, 5);
		addSettlementToTile(24, 5);
		addSettlementToTile(10, 6);
		addSettlementToTile(14, 6);
		addSettlementToTile(15, 6);
		addSettlementToTile(19, 6);
		addSettlementToTile(20, 6);
		addSettlementToTile(25, 6);
		addSettlementToTile(16, 7);
		addSettlementToTile(21, 7);
		addSettlementToTile(22, 7);
		addSettlementToTile(27, 7);
		addSettlementToTile(28, 7);
		addSettlementToTile(33, 7);
		addSettlementToTile(17, 8);
		addSettlementToTile(22, 8);
		addSettlementToTile(23, 8);
		addSettlementToTile(28, 8);
		addSettlementToTile(29, 8);
		addSettlementToTile(34, 8);
		addSettlementToTile(18, 9);
		addSettlementToTile(23, 9);
		addSettlementToTile(24, 9);
		addSettlementToTile(29, 9);
		addSettlementToTile(30, 9);
		addSettlementToTile(35, 9);
		addSettlementToTile(19, 10);
		addSettlementToTile(24, 10);
		addSettlementToTile(25, 10);
		addSettlementToTile(30, 10);
		addSettlementToTile(31, 10);
		addSettlementToTile(36, 10);
		addSettlementToTile(20, 11);
		addSettlementToTile(25, 11);
		addSettlementToTile(26, 11);
		addSettlementToTile(31, 11);
		addSettlementToTile(32, 11);
		addSettlementToTile(37, 11);
		addSettlementToTile(28, 12);
		addSettlementToTile(33, 12);
		addSettlementToTile(34, 12);
		addSettlementToTile(38, 12);
		addSettlementToTile(39, 12);
		addSettlementToTile(43, 12);
		addSettlementToTile(29, 13);
		addSettlementToTile(34, 13);
		addSettlementToTile(35, 13);
		addSettlementToTile(39, 13);
		addSettlementToTile(40, 13);
		addSettlementToTile(44, 13);
		addSettlementToTile(30, 14);
		addSettlementToTile(35, 14);
		addSettlementToTile(36, 14);
		addSettlementToTile(40, 14);
		addSettlementToTile(41, 14);
		addSettlementToTile(45, 14);
		addSettlementToTile(31, 15);
		addSettlementToTile(36, 15);
		addSettlementToTile(37, 15);
		addSettlementToTile(41, 15);
		addSettlementToTile(42, 15);
		addSettlementToTile(46, 15);
		addSettlementToTile(39, 16);
		addSettlementToTile(43, 16);
		addSettlementToTile(44, 16);
		addSettlementToTile(47, 16);
		addSettlementToTile(48, 16);
		addSettlementToTile(51, 16);
		addSettlementToTile(40, 17);
		addSettlementToTile(44, 17);
		addSettlementToTile(45, 17);
		addSettlementToTile(48, 17);
		addSettlementToTile(49, 17);
		addSettlementToTile(52, 17);
		addSettlementToTile(41, 18);
		addSettlementToTile(45, 18);
		addSettlementToTile(46, 18);
		addSettlementToTile(49, 18);
		addSettlementToTile(50, 18);
		addSettlementToTile(53, 18);
	}

	private void addSettlementToTile(int settlementId, int tileId) {
		tiles[tileId].addSettlement (settlements[settlementId]);
	}

	private SettlementClass getSettlementOnIndex(int vertexIndex) {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Settlement");
		foreach (GameObject obj in gameObjects) {
			SettlementClass settlement = obj.GetComponentInChildren<SettlementClass>();
			if (settlement.vertexIndex == vertexIndex) {
				return settlement;
			}
		}
		return null;
	}

	// See note for addVerticies.
	private void addRoads() {
			addRoadBetween (0, 3);
			addRoadBetween (0, 4);
			addRoadBetween (1, 4);
			addRoadBetween (1, 5);
			addRoadBetween (5, 2);
			addRoadBetween (2, 6);
			addRoadBetween (3, 7);
			addRoadBetween (4, 8);
			addRoadBetween (5, 9);
			addRoadBetween (6, 10);
			addRoadBetween (7, 11);
			addRoadBetween (7, 12);
			addRoadBetween (8, 13);
			addRoadBetween (13, 9);
			addRoadBetween (9, 14);
			addRoadBetween (14, 10);
			addRoadBetween (10, 15);
			addRoadBetween (11, 16);
			addRoadBetween (12, 17);
			addRoadBetween (13, 18);
			addRoadBetween (14, 19);
			addRoadBetween (15, 20);
			addRoadBetween (16, 21);
			addRoadBetween (16, 22);
			addRoadBetween (22, 17);
			addRoadBetween (17, 23);
			addRoadBetween (23, 18);
			addRoadBetween (18, 24);
			addRoadBetween (24, 29);
			addRoadBetween (19, 25);
			addRoadBetween (25, 20);
			addRoadBetween (10, 16);
			addRoadBetween (21, 27);
			addRoadBetween (22, 28);
			addRoadBetween (23, 29);
			addRoadBetween (24, 30);
			addRoadBetween (25, 31);
			addRoadBetween (26, 32);
			addRoadBetween (27, 33);
			addRoadBetween (33, 28);
			addRoadBetween (28, 34);
			addRoadBetween (34, 29);
			addRoadBetween (29, 35);
			addRoadBetween (35, 30);
			addRoadBetween (30, 36);
			addRoadBetween (36, 31);
			addRoadBetween (31, 37);
			addRoadBetween (37, 32);
			addRoadBetween (33, 38);
			addRoadBetween (34, 39);
			addRoadBetween (35, 40);
			addRoadBetween (36, 41);
			addRoadBetween (37, 41);
			addRoadBetween (38, 43);
			addRoadBetween (43, 39);
			addRoadBetween (39, 44);
			addRoadBetween (44, 40);
			addRoadBetween (40, 45);
			addRoadBetween (45, 41);
			addRoadBetween (41, 46);
			addRoadBetween (46, 42);
			addRoadBetween (43, 47);
			addRoadBetween (44, 48);
			addRoadBetween (45, 49);
			addRoadBetween (46, 50);
			addRoadBetween (47, 51);
			addRoadBetween (51, 48);
			addRoadBetween (48, 52);
			addRoadBetween (52, 49);
			addRoadBetween (49, 53);
			addRoadBetween (53, 50);
	}

	private void addRoadBetween(int settlement1index, int settlement2index) {
		SettlementClass s1 = settlements[settlement1index];
		SettlementClass s2 = settlements[settlement2index];
		// If too slow, there are more efficient ways to do this.
		RoadClass rd = getRoadOnEdge(roads.Count);
		roads.Add(rd);
		rd.settlement1 = s1;
		rd.settlement2 = s2;
	}

	private RoadClass getRoadOnEdge(int roadIndex) {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Road");
		foreach (GameObject obj in gameObjects) {
			RoadClass road = obj.GetComponent<RoadClass>();
			if (road.edgeIndex == roadIndex) {
				return road;
			}
		}
		return null;
	}

}
