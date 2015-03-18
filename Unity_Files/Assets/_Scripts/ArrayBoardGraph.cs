using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class ArrayBoardGraph : BoardGraph {

	protected List<SettlementClass> settlements = new List<SettlementClass>();
	protected List<RoadClass> roads = new List<RoadClass>();
	protected List<TileClass> tiles = new List<TileClass>();

	public int RoadCount {
		get {
			return roads.Count;
		}
	}

	public int SettlementCount {
		get {
			return settlements.Count;
		}
	}

	public int TileCount {
		get {
			return tiles.Count;
		}
	}

	public List<SettlementClass> getSettlements()
	{
		return settlements;
	}

	public List<RoadClass> getRoads()
	{
		return roads;
	}

	public List<TileClass> getTiles()
	{
		return tiles;
	}

	public SettlementClass getSettlement(int index) {
		return settlements[index];
	}

	public List<RoadClass> getConnectedRoads(SettlementClass settlement) {
		List<RoadClass> result = new List<RoadClass>();
		foreach (RoadClass rd in roads) {
			if (rd.settlement1 == settlement || rd.settlement2 == settlement) {
				result.Add(rd);
			}
		}
		return result;
	}

	public RoadClass getRoad(int index) {
		return roads[index];
	}

	public TileClass getTile(int index) {
		return tiles[index];
	}

	public List<RoadClass> getAdjacentRoads(RoadClass road) {
		List<RoadClass> result = new List<RoadClass> ();
		foreach (SettlementClass settlement in getSettlementsForRoad(road)) {
			foreach (RoadClass road2 in getConnectedRoads(settlement)) {
				if (road2 != road) {
					result.Add(road2);
				}
			}
		}
		return result;
	}

	public List<SettlementClass> getSettlementsForTile(TileClass tile) {
		return tile.getSettlements();
	}

	public List<SettlementClass> getSettlementsForRoad(RoadClass road) {
		List<SettlementClass> result = new List<SettlementClass> ();
		result.Add (road.settlement1);
		result.Add (road.settlement2);
		return result;
	}

	public List<TileClass> getTilesForSettlement(SettlementClass settlement) {
		List<TileClass> result = new List<TileClass> ();
		foreach (TileClass t in tiles) {
			if (t.getSettlements().Contains(settlement) && !result.Contains(t)) {
				result.Add(t);
			}
		}
		return result;
	}
	
	public List<SettlementClass> getSettlementsForSettlement(SettlementClass settlement){
		List<SettlementClass> result = new List<SettlementClass> ();
		List<RoadClass> connectedRoads = getConnectedRoads (settlement);
		foreach (RoadClass road in connectedRoads) {
			List<SettlementClass> set = getSettlementsForRoad(road);
			foreach(SettlementClass s in set){
				if(s != settlement){
					result.Add(s);
				}
			}
		}
		return result; 
	}

	public bool HasNeighboringSettlement(SettlementClass settlement) {
		List<RoadClass> connectedRoads = getConnectedRoads (settlement);
		foreach (RoadClass road in connectedRoads) {
			List<SettlementClass> set = getSettlementsForRoad(road);
			foreach(SettlementClass s in set) {
				if (s != settlement && s.isBuilt())
					return true;
			}
		}
		return false;
	}

	public bool hasConnectingRoad(Player player, SettlementClass settlement) {
		List<RoadClass> connectedRoads = getConnectedRoads (settlement);
		foreach(RoadClass road in connectedRoads) {
			if (road.ownerId != player.playerId) continue;
			return true;
		}
		return false;
	}

	//This function will help the longestroad function keeps track of validness
	public SettlementClass intersectofroads(RoadClass r1, RoadClass r2){
		SettlementClass result = new SettlementClass ();

		if (r1.settlement1 == r2.settlement1) {
			result = r1.settlement1;		
				} else if (r1.settlement1 == r2.settlement2) {
			result= r1.settlement1;		
				} else if (r1.settlement2 == r2.settlement1) {
			result = r1.settlement2;		
				} else if (r1.settlement2 == r2.settlement2) {
			result= r1.settlement2;		
		}
		return result; 
	}
	
	public int GetLongestRoadForPlayer(Player player) {
		List<int> possibilities = new List<int> ();
		foreach (RoadClass road in player.GetRoads()) {
			possibilities.Add(LongestRoadHelper(player, road.settlement1, new HashSet<RoadClass>()));
			possibilities.Add(LongestRoadHelper(player, road.settlement2, new HashSet<RoadClass>()));
		}
		return Max(possibilities);
	}

	private int LongestRoadHelper(Player player, SettlementClass current, HashSet<RoadClass> visited) {
		List<int> possibilities = new List<int> ();
		foreach (RoadClass road in getConnectedRoads(current)) {
			if (visited.Contains(road)) continue;
			if (road.ownerId != TurnState.currentPlayer.playerId) continue;
			SettlementClass otherEnd = getOtherSettlement(road, current);
			// If there is another player's settlement in the way, don't count it.
			if (otherEnd.isBuilt() && otherEnd.getPlayer() != TurnState.currentPlayer.playerId) continue;
			HashSet<RoadClass> newVisited = new HashSet<RoadClass>(visited);
			newVisited.Add(road);
			possibilities.Add(1 + LongestRoadHelper(player, otherEnd, newVisited));
		}
		return Max(possibilities);
	}

	private SettlementClass getOtherSettlement(RoadClass road, SettlementClass settlement) {
		if (road.settlement1 != settlement) {
			return road.settlement1;
		}
		return road.settlement2;
	}

	// May be better way to do this, but encountered some problems using Linq,
	// faster to just write own.
	private int Max(List<int> list) {
		int result = 0;
		foreach (int elem in list) {
			if (elem > result) result = elem;
		}
		return result;
	}
	
	//buildable city: positions where settlements are there 
	public List<SettlementClass> BuildableCity(Player player){
		SettlementClass[] settlements = player.GetSettlements();
		List<SettlementClass> set = settlements.ToList<SettlementClass>();

		return set;
	}

	public List<RoadClass> BuildableRoads(Player player){

		//case 1: new postions next to built roads
		RoadClass[] playerRoads = player.GetRoads();
		List<RoadClass> result = new List<RoadClass> ();
		foreach (RoadClass r in playerRoads) {
			List<RoadClass> adjacent = getAdjacentRoads(r);
			foreach(RoadClass road in adjacent){
				if(!road.isBuilt() && !result.Contains(road)){
					result.Add(road);
				}
			}
		}

		//case 2: new positions next to built settlements
		SettlementClass[] settlements = player.GetSettlements();
		foreach (SettlementClass set in settlements) {
			List<RoadClass> adjacent2 = getConnectedRoads(set);
			foreach(RoadClass road in adjacent2){
				if(!road.isBuilt() && !result.Contains(road)){
					result.Add(road);
				}
			}
		}
		return result; 
	}
	
	public List<SettlementClass> BuildableSettlements(Player player){
		RoadClass[] playerRoads = player.GetRoads();
		List<SettlementClass> result = new List<SettlementClass> ();

        foreach (RoadClass road in playerRoads) {
			List<SettlementClass> adjacent = getSettlementsForRoad(road);

			foreach(SettlementClass settlement in adjacent){
			 	if(!settlement.isBuilt()){
					List<SettlementClass> adjset = getSettlementsForSettlement(settlement);
					bool judge = true; 

					foreach(SettlementClass settlement2 in adjset){
						if(settlement2.isBuilt()){
								judge = false; 
						}
					}
					if(judge){
						result.Add(settlement);
					}
			    }
			}
		}
		return result; 
	}
}
