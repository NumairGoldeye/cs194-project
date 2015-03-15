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
	// 1. build settlements at where there are the most frequency 
	// 2. Always build roads that lead to the most winnable potential settlement place 
	// 3. Where to build city: always build hwere there is the most frequency 
	// 4. Always build when AI have the set of resources for settlement, road or city 
	// 5. When other players build long road, then AI builds largest army; vice versa 
	// 6. Trade with players other than the top score player for win-win  



	//where to build settlements: 1. first two settlements   2. 3rd settlement and beyond 

	//The priority of the first two settlements and beyond, is firstly having more chances to hit the dice, secondly having enough variety of resources
	//to build settlement and city in the future. 

	//This function searches through all the buildable settlement positions and identifies the one with most frequency. \



	public SettlementClass BuildSettlement(Player player){
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
		//If there is no buildable settlement, then this returns null which means we should build road 
		return result;
	}




	//Where to build city: always build city where it is most profitable 

	public SettlementClass BuildCity(Player player){
		SettlementClass[] settlements = player.GetSettlements();
		List<SettlementClass> set = settlements.ToList<SettlementClass>();
		SettlementClass result = new SettlementClass();
		int sum = 0;
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
		return result;
	}

	//Where to build road: always build road that leads to the next possible settlement; find where to build road by doing a breadth 
	//first search that identifies the most frequent node with distance 2 

	//In player's turn, first check if it has enough resources to build settlement: if yes, call BuildRoad to see if it returns null, 
	// if null, then buildSettlement; if not null, build a road first, and then build another settlement. 
	public RoadClass BuildRoad(Player player){
	    //When there is place to build settlement, and the settlement frequency is over 6, then build the settlement instead 
		List<SettlementClass> set = BuildableSettlements(player);
		SettlementClass s = BuildSettlement (player);
		List<TileClass> adjacent  = getTilesForSettlement(s);
		int sum = 0; 
		foreach(TileClass t in adjacent){
			int f = frequency(t);
			sum = sum + f;
		}
		if (set.Count != 0 && sum > 5) {
						return null; //do not build road, but build settlement first 		
		} else {
			//in this case, either there is no place to build settlement or frequency of buildable is too low
			// we choose to build a road to extend buildable settlement for higher frequency 
			List<RoadClass> buildable = BuildableRoads(player);
			//the new buildable frequency is updated by the new settlement position the new road connects to 
			RoadClass roadbuild = new RoadClass();
			int freqtarget = 5;
			//examine each possible buildable road
			foreach(RoadClass r in buildable){
				List<SettlementClass> adj = getSettlementsForRoad(r);
				//see the two adjacent settlement positions 
				foreach(SettlementClass settlement in adj){
					if(!settlement.isBuilt()){
						List<SettlementClass> adjset = getSettlementsForSettlement(settlement);
						bool judge = true; 

						foreach(SettlementClass settlement2 in adjset){
							if(settlement2.isBuilt()){
								judge = false; 
							}
						}

					//in this case, the settlement "settlement" is buildable 
						if(judge){
							int freq = SettlementFrequency(settlement); //This is the frequency index that we want
						    if(freq > freqtarget){
							//If it is expanding into a good potential settlement position, then keep track 
								freqtarget = freq;
								roadbuild = r;
							}
						}

					}
				}
			}
			//now this is looping over all the possible roads and returning the one that gives us the best potential road 
			if(!roadbuild){
				return roadbuild;
			}
		}
		
		return null; 
	}

	//Frequency of a settlement

	public int SettlementFrequency(SettlementClass s){
		List<TileClass> adjacent  = getTilesForSettlement(s);
		int sum = 0; 
		foreach(TileClass t in adjacent){
			int f = frequency(t);
			sum = sum + f;
		}
		return sum;
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

	//Player AI strategy updater: determine and adjust in real time what strategy to adopt 
	// 1 indicates longest road strategy; 2 means largest army; 3 means just keeping on building city and settlement 
	public int strategyUpdate(Player player){
		//initialize strategy as 1, longest road strategy 
		int strategy = 1; 
		
		List <Player> currentplayers = Player.allPlayers; 
		//If the current AI's longestroad is no shorter than the best longestroad - 2, then go for the longest road
		foreach (Player x in currentplayers) {
			if (longestroad (player) < longestroad (x) - 2) {
				strategy = 2;
				break; 
			}
		}
		
		//check the largestarmy difference, if larger than 1, give up largest army
		foreach (Player y in currentplayers) {
			if(y.largestarmyForAI() > player.largestarmyForAI() + 1){
				strategy  =3 ;
				break ;
			}
		}
		
		
		return strategy;
	}

	//Final Integration function that serves as the AI brain, taking into account turn by turn real time scenario of all other 
	//players to decide AI's strategy at the turn, and move accordingly to build settlement, road, city or get dev card.
	//This function should be called in AI's turn  

	
	
	// In the beginnning, the AI calls "BuildSettlement" to set 2 settlements of highest frequencies, and build 2 roads randomly
	//At each turn, the AI calls: 
	// 0. Update strategy with strateyUpdate, decide 1-3; trade console with the house 
	// 1. Check resource cards that he has, get a list of them; 
	//2. If contain " 2 wheat 3 ore", build city at all times

	// 3.if they contain "wood, brick" then call "BuildRoad"
	//								if return null(1. has place to build good set 2. no good road), then do not build road 
	// 										if contain "wood, brick, sheep, wheat", BuildSettlement (if null, build random road) 
	//										if not contain, call BuildSettlement to see if null-> build Random Road 
	//								if return yes, then check strategy, if strategy is 1 and longestroad is not bigger or equal to 4
	// 										then build road; else do not build, save for future settlement 
	//4. If contain "1 wheat 1 sheep 1 ore" and strategy is 2 then get dev card 
	//City always trumphs  
	//full strategy 

	void AIBrain(Player player){
				//This gives the current optimal strategy for the AI player 
				int strategy = strategyUpdate (player);


		//-----------------------------------------------------------------------------------------------------------------------------------
		//part 0: trade with the house

		//priority 1: trade to get ore for city 
		if (player.orecount (player) == 2 && player.wheatcount (player) >= 2) {
				if(player.sheepcount(player)>=4){
				player.resourceCounts[0] -- ; 
				player.resourceCounts[0] -- ; 
				player.resourceCounts[0] -- ; 
				player.resourceCounts[0] -- ; 
				player.resourceCounts[3] ++ ; 
			}else if(player.woodcount(player) >=4){
				player.resourceCounts[1] --;
				player.resourceCounts[1] --;
				player.resourceCounts[1] --;
				player.resourceCounts[1] --;
				player.resourceCounts[3] ++ ; 
			}else if(player.brickcount(player)>=4){
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[3] ++ ; 
			}else if(player.wheatcount(player)>=6){
				player.resourceCounts[4] --;
				player.resourceCounts[4] --;
				player.resourceCounts[4] --;
				player.resourceCounts[4] --;
				player.resourceCounts[3] ++ ; 
			}
		}

		//priority 2: trade to get wheat for city 
		if (player.orecount (player) >= 3 && player.wheatcount (player) == 1) {
			if(player.sheepcount(player)>=4){
				player.resourceCounts[0] -- ; 
				player.resourceCounts[0] -- ; 
				player.resourceCounts[0] -- ; 
				player.resourceCounts[0] -- ; 
				player.resourceCounts[4] ++ ; 
			}else if(player.woodcount(player) >=4){
				player.resourceCounts[1] --;
				player.resourceCounts[1] --;
				player.resourceCounts[1] --;
				player.resourceCounts[1] --;
				player.resourceCounts[4] ++ ; 
			}else if(player.brickcount(player)>=4){
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[4] ++ ; 
			}else if(player.orecount(player)>=7){
				player.resourceCounts[3] --;
				player.resourceCounts[3] --;
				player.resourceCounts[3] --;
				player.resourceCounts[3] --;
				player.resourceCounts[4] ++ ; 
			}
		}
		//priority 3: trade to get wood for settlement 
		if (player.woodcount (player) == 0 && player.wheatcount (player) >= 1 && player.brickcount (player) >= 1 && player.sheepcount (player) >= 1) {
			if(player.sheepcount(player)>=5){
				player.resourceCounts[0] -- ; 
				player.resourceCounts[0] -- ; 
				player.resourceCounts[0] -- ; 
				player.resourceCounts[0] -- ; 
				player.resourceCounts[1] ++ ; 
			}else if(player.brickcount(player)>=5){
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[1] ++ ; 
			}else if(player.wheatcount(player)>=5){
				player.resourceCounts[4] --;
				player.resourceCounts[4] --;
				player.resourceCounts[4] --;
				player.resourceCounts[4] --;
				player.resourceCounts[1] ++ ; 
			}else if(player.orecount(player)>=4){
				player.resourceCounts[3] --;
				player.resourceCounts[3] --;
				player.resourceCounts[3] --;
				player.resourceCounts[3] --;
				player.resourceCounts[1] ++ ; 
			}
		}

		//priority 4: trade to get wheat for settlement 
		if (player.woodcount (player) >= 1 && player.wheatcount (player) == 0 && player.brickcount (player) >= 1 && player.sheepcount (player) >= 1) {
			if(player.sheepcount(player)>=5){
				player.resourceCounts[0] -- ; 
				player.resourceCounts[0] -- ; 
				player.resourceCounts[0] -- ; 
				player.resourceCounts[0] -- ; 
				player.resourceCounts[4] ++ ; 
			}else if(player.brickcount(player)>=5){
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[4] ++ ; 
			}else if(player.woodcount(player)>=5){
				player.resourceCounts[1] --;
				player.resourceCounts[1] --;
				player.resourceCounts[1] --;
				player.resourceCounts[1] --;
				player.resourceCounts[4] ++ ; 
			}else if(player.orecount(player)>=4){
				player.resourceCounts[3] --;
				player.resourceCounts[3] --;
				player.resourceCounts[3] --;
				player.resourceCounts[3] --;
				player.resourceCounts[4] ++ ; 
			}
		}

		//priority 5: trade to get sheep for settlement 
		if (player.woodcount (player) >= 1 && player.wheatcount (player) >=1 && player.brickcount (player) >= 1 && player.sheepcount (player) == 0) {
			if(player.wheatcount(player)>=5){
				player.resourceCounts[4] -- ; 
				player.resourceCounts[4] -- ; 
				player.resourceCounts[4] -- ; 
				player.resourceCounts[4] -- ; 
				player.resourceCounts[0] ++ ; 
			}else if(player.brickcount(player)>=5){
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[2] --;
				player.resourceCounts[0] ++ ; 
			}else if(player.woodcount(player)>=5){
				player.resourceCounts[1] --;
				player.resourceCounts[1] --;
				player.resourceCounts[1] --;
				player.resourceCounts[1] --;
				player.resourceCounts[0] ++ ; 
			}else if(player.orecount(player)>=4){
				player.resourceCounts[3] --;
				player.resourceCounts[3] --;
				player.resourceCounts[3] --;
				player.resourceCounts[3] --;
				player.resourceCounts[0] ++ ; 
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------

		//The first part, build city 
		if (player.wheatcount (player) >= 2 && player.orecount (player) >= 3) {
			SettlementClass nextcity = BuildCity(player);
			//Build city pointed to by the nextcity 
		}

		//The second big block deals with the tradeoff between building a road now or saving the resource to build a settlement later
				if (player.woodcount(player) >= 1 && player.brickcount(player) >= 1) {
					RoadClass nextroad = BuildRoad(player);
			        if(nextroad==null){
			          if(player.sheepcount(player)>=1 && player.wheatcount(player)>=1){
							SettlementClass nextsettlement = BuildSettlement(player);
					       if(nextsettlement==null){
							//build random road 
					       }else{
						   //build settlement pointed to by "nextsettlement"
							}

						}
					}else{
				     if(strategy == 1 ){
					//build a road pointed to by "nextroad"
				    	}else{
						  if(longestroad (player)<=4){
					      //although in this case longest road is not the strategy, AI needs to extend road reach for better settlement 
						// position 

						//build a road pointed to by "nextroad"

						//else then no need to build road, save for future settlement
					      }
						}
					}
		    	}


		//The 3rd part 
		if (player.wheatcount (player) >= 1 && player.sheepcount (player) >= 1 && player.orecount (player) >= 1 && strategy == 2) {
		// Get a dev card 		
	    // Use the dev card right away 
		}

		//The 4th part where AI does not use longest road or largest army strategy, just go buid settlement and city; note that buildcity 
		//function is always called in the beginning so we only worry about building settlements; in this case since we already discard the
		//other two strategies, we assume that there is not much value in building road, but instead we only build settelments as long as 
		//there is still buildablesettlement positions left
		if (strategy == 3) {
			SettlementClass nextleftset = BuildSettlement(player);
			if (nextleftset ==  null){
				RoadClass nextleftroad = BuildRoad(player);

				if(nextleftroad){
				//build a road pointed to by nextleftroad, else leave it there 
				}

				}else{
				//build a settlement at position nextleftset
			}
		}





	}

}
