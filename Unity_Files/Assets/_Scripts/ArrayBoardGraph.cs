﻿using System.Collections;
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

	/**
     * Returns the total length of longest road that belongs to a list of roads if starting from one specific road  
     * This is the recursive function used to calculate the longest road(has to contain two arguments to keep track of the invariance 
     * in the recursive function)
     */
	private int longestroadfromone(RoadClass rd, List<RoadClass> roads){
		
				int longest = 1;
				// This function means: having rd as road array No.1, find the longest road 
				//keep track of its adjacent roads and checks if any of them is in the built road array, this will be the possible next road
				// linking to the previous starting road 
				List<RoadClass> adjacent = new List<RoadClass> ();
				adjacent = getAdjacentRoads (rd);


				//use recursive method to iteratively check longest road 
				foreach (RoadClass r in adjacent) {

						//This new road list keeps track of the subgraph when 1 road is deleted from the road list   
						List<RoadClass> leftroads = new List<RoadClass> (roads);

						//if one of the adjacent roads is contained in the built road array, then use this as the next road and the leftover road 
						//array as the total sub array 
						if (roads.Contains (r)) {     
								leftroads.Remove (rd);

								//call the recursive to see the sub graph 
								int sublongest = longestroadfromone (r, leftroads);
								//int partialresult = longestroad(left);
								if (sublongest + 1 > longest) {
										longest = sublongest + 1; 
								}
						}
						// if "Contains" bool is false, then it means there is no more adjacent road available, meaning the sublongest is 0, meaning 
						// that state will have longest as 1; so no need for "else branch".
				}
	
				//This function currently cannot solve an edge case: when player builds 3 roads that are mutually adjacent to each other; in
				// this case, it will likely be miscounted. 

				// One possible solution: delete the mutually adjacent 3rd edge from the leftover array before entering longestroadfromone recursive
				//function; however that introduces another edge case which is when the longest road contains a cyclic partial continuous road 
		return longest; 
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

	//------ AI related computer mode functions--------
	// The AI strategy: 
	// 1. build two settlements at where there are the most frequency 
	// 2. Always build roads that lead to the most winnable potential settlement place 
	// 3. Where to build city: always build hwere there is the most frequency 
	// 4. 




	//where to build settlements: 1. first two settlements   2. 3rd settlement and beyond 

	//The priority of the first two settlements, is firstly having more chances to hit the dice, secondly having enough variety of resources
	//to build settlement and city in the future. 

	public SettlementClass BuildFirstSettlement(Player player){
		List<SettlementClass> set = BuildableSettlements(player);
		int sum = 0;
		SettlementClass result = new SettlementClass();
		foreach (SettlementClass s in set) {
			List<TileClass> adjacent = getTilesForSettlement(s);
			int subsum = 0; 
			foreach(TileClass t in adjacent){
				int f =  frequency(t);
				subsum = subsum + f;
			}
			if(subsum > sum){
				sum = subsum;
				result = s;			
			}
		}

		//This function returns the settlement that is buildable and has the most frequency sum index 
		return result;
	}


	//Where to build city: 

	public SettlementClass BuildCity(Player player){
		SettlementClass[] settlements = player.GetSettlements();
		List<SettlementClass> set = settlements.ToList<SettlementClass>();
		return null;
	}

		
	//Frequecny module: return the frequecny of happening for each tile 
	public int frequency(TileClass tile){
		int result = 0; 
		int dice = tile.diceValue;
		if (dice == 6 || dice == 8) {
						result = 5;
				}
		if (dice == 5 || dice == 9) {
			result = 4;
		}
		if (dice == 4 || dice == 10) {
			result = 3;
		}
		if (dice == 3 || dice == 11) {
			result = 2;
		}

		if (dice == 2 || dice == 12) {
			result = 1;
		}
		return result;
	}


}
