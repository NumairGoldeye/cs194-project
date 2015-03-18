/**
 * Contains the methods used to create and to get information about the board.
 */
using System;
using System.Collections.Generic;

public interface BoardGraph {

	int RoadCount { get; }
	int SettlementCount { get; }
	int TileCount { get; }

	List<SettlementClass> getSettlements();
	List<RoadClass> getRoads();
	List<TileClass> getTiles();

	/** Returns the Settlement corresponding to the given index for that space on the board.
	 *
	 * The Settlements are indexed starting at 0, starting at the leftmost tile in the top
	 * row.  They ascend in row-major order.
	 */
	SettlementClass getSettlement(int index);
	
	/**
	 * Returns all of the roads connected to the given settlement.
	 */
	List<RoadClass> getConnectedRoads(SettlementClass settlement);

	/**
	 * Returns all roads adjacent to the given one.
	 */
	List<RoadClass> getAdjacentRoads(RoadClass road);

	/**
	 * Returns all (6) settlements adjacent to the given tile.
	 */
	List<SettlementClass> getSettlementsForTile(TileClass tile);

	/**
	 * Returns all (i.e. both) settlements connected to a given road.
	 */
	List<SettlementClass> getSettlementsForRoad(RoadClass road);

	/**
	 * Returns all tiles next to a given settlement.
	 */
	List<TileClass> getTilesForSettlement(SettlementClass settlement);

	/**
	 * Returns all settlements next to a given settlement.
	 */
	List<SettlementClass> getSettlementsForSettlement(SettlementClass settlement);

	/** Returns the edge corresponding to the given index for that space on the board.
	 *
	 * The edges are indexed starting at 0, starting at the very top of the board, and increaing
	 * leftwards.
	 */
	RoadClass getRoad(int index);

	/**
	 * Returns the tile corresponding to the given tile.
	 */
	TileClass getTile(int index);


	
	/**
     * Returns the total length of longest road that belongs to a player 
     */
	int longestroad(Player player);

	/**
     * Returns the positions that player can build city   
     */
	List<SettlementClass> BuildableCity(Player player);
	/**
     * Returns the positions that player can build roads   
     */
	List<RoadClass> BuildableRoads(Player player);
	
	
	/**
     * Returns the positions that player can build settlement  
     */
	List<SettlementClass> BuildableSettlements(Player player);

	void AIBrain (Player player);


}