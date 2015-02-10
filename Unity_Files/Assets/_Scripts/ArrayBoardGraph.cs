using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

	public bool hasBuiltNeighbooringSettlement(SettlementClass settlement) {
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


    public int longestroad(Player player){
		int result = 0;
		RoadClass[] playerRoads = player.GetRoads();
		List<RoadClass> roads = playerRoads.ToList<RoadClass>();
				///can add an "if" statement here checking if playerRoads is null meaning no road built 
		foreach (RoadClass r in roads) {
			int subresult = longestroadfromone(r, roads);		
		if(subresult > result) {
				result = subresult;
			}
		}
		return result; 
	}

	public int longestroadfromone(RoadClass rd, List<RoadClass> roads){
		
		int longest = 1;
			// This function means: having rd as road array No.1, find the longest road 
			//keep track of its adjacent roads and checks if any of them is in the built road array, this will be the possible next road
		    // linking to the previous starting road 
			List<RoadClass> adjacent = new List<RoadClass> ();
			adjacent = getAdjacentRoads(rd);


			//use recursive method to iteratively check longest road 
			foreach(RoadClass r in adjacent){

			//This new road list keeps track of the subgraph when 1 road is deleted from the road list   
				List<RoadClass> leftroads = new List<RoadClass> ();
				leftroads = roads;

			//if one of the adjacent roads is contained in the built road array, then use this as the next road and the leftover road 
			//array as the total sub array 
				if(roads.Contains(r)){     
				leftroads.Remove(rd);

				//call the recursive to see the sub graph 
				int sublongest = longestroadfromone(r, leftroads);
					//int partialresult = longestroad(left);
				    if(sublongest + 1 > longest){
						longest = sublongest + 1; 
				    }
				}
			// if "Contains" bool is false, then it means there is no more adjacent road available, meaning the sublongest is 0, meaning 
			// that state will have longest as 1; so no need for "else branch".
			}
	
		//This function currently cannot solve an edge case: when player builds 3 roads that are mutually adjacent to each other; in
		// this case, it will likely be miscounted. 
		return longest; 
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
