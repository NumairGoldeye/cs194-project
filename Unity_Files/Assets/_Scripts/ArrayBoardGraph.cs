using System.Collections;
using System.Collections.Generic;

public class ArrayBoardGraph : BoardGraph {

	protected List<Vertex> verticies = new List<Vertex>();
	protected List<Edge> edges = new List<Edge>();
	protected List<TileClass> tiles = new List<TileClass>();

	public Vertex getVertex(int index) {
		return verticies[index];
	}

	public List<Edge> getEdges(int vertexIndex) {
		Vertex v = getVertex(vertexIndex);
		return getEdges(v);
	}

	public List<Edge> getEdges(Vertex vertex) {
		List<Edge> result = new List<Edge>();
		foreach (Edge e in edges) {
			if (e.vertex1 == vertex || e.vertex2 == vertex) {
				result.Add(e);
			}
		}
		return result;
	}

	public Edge getEdge(int index) {
		return edges[index];
	}

	public TileClass getTile(int index) {
		return tiles[index];
	}
}
