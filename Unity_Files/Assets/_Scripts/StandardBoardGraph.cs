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
	private const int NUM_EDGES = 72;
	private const int NUM_VERTEXES = 54;

	private int i;

	private StandardBoardGraph() {
		// Must maintain this order; edges depend on verticies, and verticies depend on tiles.
		addTiles();
		addVerticies();
		addEdges();
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
			tileObject.GetComponent<TileClass>().init();
		}
		tiles.Sort(new tileIndexComparer());
	}

	/** Compares tiles by their index. */
	private class tileIndexComparer : IComparer<TileClass> {
		public int Compare(TileClass a, TileClass b) {
			return a.tileIndex.CompareTo(b.tileIndex);
		}
	}

	// There are cleaner ways to add these than to hardcode then umbers, but
	// it probably isn't worth the effort.  Some options, however:
	//		1) Use text files (like csvs) that map list the edges and verticies.
	//		2) Use some sort of math to figure it out.
	private void addVerticies() {
		addVertexBetween (0);
		addVertexBetween (1);
		addVertexBetween (2);
		addVertexBetween (0);
		addVertexBetween (0, 1);
		addVertexBetween (1, 2);
		addVertexBetween (2);
		addVertexBetween (0, 3);
		addVertexBetween (0, 1, 4);
		addVertexBetween (1, 2, 5);
		addVertexBetween (2, 6);
		addVertexBetween (3);
		addVertexBetween (0, 3, 4);
		addVertexBetween (1, 4, 5);
		addVertexBetween (2, 5, 6);
		addVertexBetween (6);
		addVertexBetween (3, 7);
		addVertexBetween (3, 4, 8);
		addVertexBetween (4, 5, 9);
		addVertexBetween (5, 6, 10);
		addVertexBetween (6, 11);
		addVertexBetween (7);
		addVertexBetween (3, 7, 8);
		addVertexBetween (4, 8, 9);
		addVertexBetween (5, 9, 10);
		addVertexBetween (6, 10, 11);
		addVertexBetween (11);
		addVertexBetween (7);
		addVertexBetween (7, 8, 12);
		addVertexBetween (8, 9, 13);
		addVertexBetween (9, 10, 14);
		addVertexBetween (10, 11, 15);
		addVertexBetween (11);
		addVertexBetween (7, 12);
		addVertexBetween (8, 12, 13);
		addVertexBetween (9, 13, 14);
		addVertexBetween (10, 14, 15);
		addVertexBetween (11, 15);
		addVertexBetween (12);
		addVertexBetween (12, 13, 16);
		addVertexBetween (13, 14, 17);
		addVertexBetween (14, 15, 18);
		addVertexBetween (15);
		addVertexBetween (12, 16);
		addVertexBetween (13, 16, 17);
		addVertexBetween (14, 17, 18);
		addVertexBetween (15, 18);
		addVertexBetween (16);
		addVertexBetween (16, 17);
		addVertexBetween (17, 18);
		addVertexBetween (18);
		addVertexBetween (16);
		addVertexBetween (17);
		addVertexBetween (18);
	}
	
	// TODO: Currently ignores ports.
	private void addVertexBetween(params int[] tileIndicies) {
		List<TileClass> tilesToAdd = new List<TileClass>();
		foreach (int i in tileIndicies) {
			tilesToAdd.Add(tiles[i]);
		}
		List<PortClass> portsToAdd = new List<PortClass>();

		Vertex v = new Vertex(tilesToAdd, portsToAdd, new SettlementClass());
		verticies.Add(v);
		foreach (int i in tileIndicies) {
			tiles[i].addVertex(VertexCount-1);
		}
	}

	// See note for addVerticies.
	private void addEdges() {
			addEdgeBetween (0, 3);
			addEdgeBetween (0, 4);
			addEdgeBetween (1, 4);
			addEdgeBetween (1, 5);
			addEdgeBetween (5, 2);
			addEdgeBetween (2, 6);
			addEdgeBetween (3, 7);
			addEdgeBetween (4, 8);
			addEdgeBetween (5, 9);
			addEdgeBetween (6, 10);
			addEdgeBetween (7, 11);
			addEdgeBetween (7, 12);
			addEdgeBetween (8, 13);
			addEdgeBetween (13, 9);
			addEdgeBetween (9, 14);
			addEdgeBetween (14, 10);
			addEdgeBetween (10, 15);
			addEdgeBetween (11, 16);
			addEdgeBetween (12, 17);
			addEdgeBetween (13, 18);
			addEdgeBetween (14, 19);
			addEdgeBetween (15, 20);
			addEdgeBetween (16, 21);
			addEdgeBetween (16, 22);
			addEdgeBetween (22, 17);
			addEdgeBetween (17, 23);
			addEdgeBetween (23, 18);
			addEdgeBetween (18, 24);
			addEdgeBetween (24, 29);
			addEdgeBetween (19, 25);
			addEdgeBetween (25, 20);
			addEdgeBetween (10, 16);
			addEdgeBetween (21, 27);
			addEdgeBetween (22, 28);
			addEdgeBetween (23, 29);
			addEdgeBetween (24, 30);
			addEdgeBetween (25, 31);
			addEdgeBetween (26, 32);
			addEdgeBetween (27, 33);
			addEdgeBetween (33, 28);
			addEdgeBetween (28, 34);
			addEdgeBetween (34, 29);
			addEdgeBetween (29, 35);
			addEdgeBetween (35, 30);
			addEdgeBetween (30, 36);
			addEdgeBetween (36, 31);
			addEdgeBetween (31, 37);
			addEdgeBetween (37, 32);
			addEdgeBetween (33, 38);
			addEdgeBetween (34, 39);
			addEdgeBetween (35, 40);
			addEdgeBetween (36, 41);
			addEdgeBetween (37, 41);
			addEdgeBetween (38, 43);
			addEdgeBetween (43, 39);
			addEdgeBetween (39, 44);
			addEdgeBetween (44, 40);
			addEdgeBetween (40, 45);
			addEdgeBetween (45, 41);
			addEdgeBetween (41, 46);
			addEdgeBetween (46, 42);
			addEdgeBetween (43, 47);
			addEdgeBetween (44, 48);
			addEdgeBetween (45, 49);
			addEdgeBetween (46, 50);
			addEdgeBetween (47, 51);
			addEdgeBetween (51, 48);
			addEdgeBetween (48, 52);
			addEdgeBetween (52, 49);
			addEdgeBetween (49, 53);
			addEdgeBetween (53, 50);
	}

	private void addEdgeBetween(int vertex1Index, int vertex2Index) {
		Vertex v1 = verticies[vertex1Index];
		Vertex v2 = verticies[vertex2Index];
		// If too slow, there are more efficient ways to do this.
		Edge e = new Edge(v1, v2, vertex1Index, vertex2Index, getRoadOnEdge(edges.Count));
		edges.Add(e);
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
