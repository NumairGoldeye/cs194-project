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

		Vertex v = new Vertex(tilesToAdd, portsToAdd, new CityClass(), new SettlementClass());
		verticies.Add(v);
		foreach (int i in tileIndicies) {
			tiles[i].addVertex(VertexCount-1);
		}
	}

	// See note for addVerticies.
	private void addEdges() {
		for (int i = 0; i < NUM_EDGES; ++i) {
			addEdgeBetween (i, 0, 3);
			addEdgeBetween (i, 0, 4);
			addEdgeBetween (i, 1, 4);
			addEdgeBetween (i, 1, 5);
			addEdgeBetween (i, 5, 2);
			addEdgeBetween (i, 2, 6);
			addEdgeBetween (i, 3, 7);
			addEdgeBetween (i, 4, 8);
			addEdgeBetween (i, 5, 9);
			addEdgeBetween (i, 6, 10);
			addEdgeBetween (i, 7, 11);
			addEdgeBetween (i, 7, 12);
			addEdgeBetween (i, 8, 13);
			addEdgeBetween (i, 13, 9);
			addEdgeBetween (i, 9, 14);
			addEdgeBetween (i, 14, 10);
			addEdgeBetween (i, 10, 15);
			addEdgeBetween (i, 11, 16);
			addEdgeBetween (i, 12, 17);
			addEdgeBetween (i, 13, 18);
			addEdgeBetween (i, 14, 19);
			addEdgeBetween (i, 15, 20);
			addEdgeBetween (i, 16, 21);
			addEdgeBetween (i, 16, 22);
			addEdgeBetween (i, 22, 17);
			addEdgeBetween (i, 17, 23);
			addEdgeBetween (i, 23, 18);
			addEdgeBetween (i, 18, 24);
			addEdgeBetween (i, 24, 29);
			addEdgeBetween (i, 19, 25);
			addEdgeBetween (i, 25, 20);
			addEdgeBetween (i, 10, 16);
			addEdgeBetween (i, 21, 27);
			addEdgeBetween (i, 22, 28);
			addEdgeBetween (i, 23, 29);
			addEdgeBetween (i, 24, 30);
			addEdgeBetween (i, 25, 31);
			addEdgeBetween (i, 26, 32);
			addEdgeBetween (i, 27, 33);
			addEdgeBetween (i, 33, 28);
			addEdgeBetween (i, 28, 34);
			addEdgeBetween (i, 34, 29);
			addEdgeBetween (i, 29, 35);
			addEdgeBetween (i, 35, 30);
			addEdgeBetween (i, 30, 36);
			addEdgeBetween (i, 36, 31);
			addEdgeBetween (i, 31, 37);
			addEdgeBetween (i, 37, 32);
			addEdgeBetween (i, 33, 38);
			addEdgeBetween (i, 34, 39);
			addEdgeBetween (i, 35, 40);
			addEdgeBetween (i, 36, 41);
			addEdgeBetween (i, 37, 41);
			addEdgeBetween (i, 38, 43);
			addEdgeBetween (i, 43, 39);
			addEdgeBetween (i, 39, 44);
			addEdgeBetween (i, 44, 40);
			addEdgeBetween (i, 40, 45);
			addEdgeBetween (i, 45, 41);
			addEdgeBetween (i, 41, 46);
			addEdgeBetween (i, 46, 42);
			addEdgeBetween (i, 43, 47);
			addEdgeBetween (i, 44, 48);
			addEdgeBetween (i, 45, 49);
			addEdgeBetween (i, 46, 50);
			addEdgeBetween (i, 47, 51);
			addEdgeBetween (i, 51, 48);
			addEdgeBetween (i, 48, 52);
			addEdgeBetween (i, 52, 49);
			addEdgeBetween (i, 49, 53);
			addEdgeBetween (i, 53, 50);
		}
	}

	private void addEdgeBetween(int edgeIndex, int vertex1Index, int vertex2Index) {
		Vertex v1 = verticies[vertex1Index];
		Vertex v2 = verticies[vertex2Index];
		// If too slow, there are more efficient ways to do this.
		Edge e = new Edge(v1, v2, vertex1Index, vertex2Index, getRoadOnEdge(edgeIndex));
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
