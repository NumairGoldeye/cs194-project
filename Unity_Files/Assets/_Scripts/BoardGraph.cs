/**
 * Contains the methods used to create and to get information about the board.
 */
using System;
using System.Collections.Generic;

public interface BoardGraph {

	int EdgeCount { get; }
	int VertexCount { get; }
	int TileCount { get; }

	/** Returns the vertex corresponding to the given index for that space on the board.
	 *
	 * The verticies are indexed starting at 0, starting at the leftmost tile in the top
	 * row.  They ascend in row-major order.
	 */
	Vertex getVertex(int index);
	
	/**
	 * Returns all of the edges connected to the vertex with the given index.
	 */
	List<Edge> getEdges(int vertexIndex);
	
	/**
	 * Returns all of the edges connected to the given vertex.
	 */
	List<Edge> getEdges(Vertex vertex);
	
	/** Returns the edge corresponding to the given index for that space on the board.
	 *
	 * The edges are indexed starting at 0, starting at the very top of the board, and increaing
	 * leftwards.
	 */
	Edge getEdge(int index);

	/**
	 * Returns the tile corresponding to the given tile.
	 */
	TileClass getTile(int index);
	
}