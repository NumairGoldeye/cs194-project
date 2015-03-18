using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class SimpleRulesAIBrain : AIBrain {

	private enum AIStrategy { LongestRoad, LargestArmy, Build }

	Player player;
	BoardGraph graph;

	public SimpleRulesAIBrain(Player player, BoardGraph graph) {
		this.player = player;
		this.graph = graph;
	}


	//-----------------------------------------------------------------------------------------------------------------------------------
	//memo: 1. action functions for building city, road, settlement 2. Use dev card functions 
	//   3. AI initializer: AI's name is computer, text recognition to get into the AI   4. work with Chris to call the final AI brain 
	//  function at the right place/turn 
	
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
	
	
	
	private SettlementClass BuildSettlement(){
		List<SettlementClass> set = this.graph.BuildableSettlements(this.player);
		int sum = 0;
		SettlementClass result = null;
		foreach (SettlementClass s in set) {
			List<TileClass> adjacent = this.graph.getTilesForSettlement(s);
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
	
	
	private SettlementClass BuildInitialSettlement(){
		List<SettlementClass> set = new List<SettlementClass>();
		List<SettlementClass> allset = this.graph.getSettlements();
		foreach (SettlementClass temp in allset) {
			if(!this.graph.HasNeighboringSettlement(temp)){
				set.Add(temp);
			}
		}

		int sum = 0;
		SettlementClass result = null;
		foreach (SettlementClass s in set) {
			List<TileClass> adjacent = this.graph.getTilesForSettlement(s);
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
	
	private SettlementClass BuildCity(){
		SettlementClass[] settlements = this.player.GetSettlements();
		List<SettlementClass> set = settlements.ToList<SettlementClass>();
		SettlementClass result = null;
		int sum = 0;
		foreach (SettlementClass s in set) {
			List<TileClass> adjacent = this.graph.getTilesForSettlement(s);
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
	private RoadClass BuildRoad(){
		//When there is place to build settlement, and the settlement frequency is over 6, then build the settlement instead 
		List<SettlementClass> set = this.graph.BuildableSettlements(this.player);
		SettlementClass s = BuildSettlement();
		List<TileClass> adjacent = this.graph.getTilesForSettlement(s);
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
			List<RoadClass> buildable = this.graph.BuildableRoads(this.player);
			//the new buildable frequency is updated by the new settlement position the new road connects to 
			RoadClass roadbuild = null;
			int freqtarget = 5;
			//examine each possible buildable road
			foreach(RoadClass r in buildable){
				List<SettlementClass> adj = this.graph.getSettlementsForRoad(r);
				//see the two adjacent settlement positions 
				foreach(SettlementClass settlement in adj){
					if(!settlement.isBuilt()){
						List<SettlementClass> adjset = this.graph.getSettlementsForSettlement(settlement);
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
	
	private int SettlementFrequency(SettlementClass s){
		List<TileClass> adjacent = this.graph.getTilesForSettlement(s);
		int sum = 0; 
		foreach(TileClass t in adjacent){
			int f = frequency(t);
			sum = sum + f;
		}
		return sum;
	}
	
	//Frequecny module: return the frequecny of happening for each tile 
	private int frequency(TileClass tile){
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
	private AIStrategy GetStrategy(){
		//initialize strategy as 1, longest road strategy 
		AIStrategy strategy = AIStrategy.LongestRoad; 
		
		List <Player> currentplayers = GameManager.Instance.players; 
		//If the current AI's longestroad is no shorter than the best longestroad - 2, then go for the longest road
		foreach (Player x in currentplayers) {
			if (this.graph.GetLongestRoadForPlayer (player) < this.graph.GetLongestRoadForPlayer (x) - 2) {
				strategy = AIStrategy.LargestArmy;
				break; 
			}
		}
		
		//check the largestarmy difference, if larger than 1, give up largest army
		foreach (Player y in currentplayers) {
			if(y.largestarmyForAI() > player.largestarmyForAI() + 1){
				strategy = AIStrategy.Build;
				break ;
			}
		}
		
		
		return strategy;
	}
	
	//-----------------------------------------------------------------------------------------------------------------------------------
	
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
	
	//To be called in the beginning two rounds 
	public void PerformSetup(){
		//In the beginning, when the roads number is 0 or 1, build a settlement and build a road 
		RoadClass[] playerBeginRoads = player.GetRoads ();
		
		if (playerBeginRoads.Count () == 0 || playerBeginRoads.Count () == 1) {
			//build a road and a settlement without consuming resources, starting game 
			//display implementation! 
			
			SettlementClass firstset = BuildInitialSettlement();
			Debugger.Log("Settlement", firstset);
			Debugger.Log ("Computer", firstset.vertexIndex.ToString());
			Debugger.Log("Computer", TurnState.currentPlayer.playerName);
			
			firstset.buildSettlement(true);
			
			//build random road 
			List<RoadClass> buildable = this.graph.BuildableRoads(this.player);
			if (buildable.Count () > 0) {
				RoadClass randomr = buildable[0];
				//build a road pointed to by randomroad 
				//Display the road pointed to by randomroad! 
				randomr.buildRoad();
			}
		}
		StartGameManager.NextPlayer();
	}
	
	
	public void PlayTurn(){
		//for (int i = 0; i < player.resourceCounts.Count(); ++i) {
		//	player.resourceCounts[i] += 3;
		//}
		//Debugger.Log ("Computer", "It's my turn");
		Player player = this.player;
		TurnState.stateType = TurnStateType.roll;
		
		GameManager.Instance.BroadcastMessage ("rollDice");
		
		TurnState.NextTurnState (); 
		AIStrategy strategy = GetStrategy(); //This gives the current optimal strategy for the AI player
		PlayTradePhase (player, strategy);
		PlayBuyPhase (player, strategy);
		TurnState.EndTurn ();
	}

	private void PlayTradePhase (Player player, AIStrategy strategy) {
		TradeForBuilding (player);
		if (strategy == AIStrategy.LargestArmy) {
			TradeForDevcard (player);
		}
		if (strategy == AIStrategy.LongestRoad) {
			TradeForLongestRoad (player);
		}
		//Now AI is fully traded and optimized for different scenarios: 
		// city first always, settlement always second then 
		// based on strategy:  1 for trading for road, 2 for trading for dev card 
		TurnState.NextTurnState ();
	}

	private void TradeForBuilding (Player player) {
		//priority 1: trade to get ore for city 
		if (player.orecount () == 2 && player.wheatcount () >= 2) {
			TradeForBuyable (ResourceType.Ore, BuyableType.city);
		}
		//priority 2: trade to get wheat for city 
		if (player.orecount () >= 3 && player.wheatcount () == 1) {
			TradeForBuyable (ResourceType.Wheat, BuyableType.city);
		}
		//priority 3: trade to get wood for settlement 
		if (player.woodcount () == 0 && player.wheatcount () >= 1 && player.brickcount () >= 1 && player.sheepcount () >= 1) {
			TradeForBuyable (ResourceType.Sheep, BuyableType.settlement);
		}
		//priority 4: trade to get wheat for settlement 
		if (player.woodcount () >= 1 && player.wheatcount () == 0 && player.brickcount () >= 1 && player.sheepcount () >= 1) {
			TradeForBuyable (ResourceType.Wheat, BuyableType.settlement);
		}
		//priority 5: trade to get sheep for settlement 
		if (player.woodcount () >= 1 && player.wheatcount () >= 1 && player.brickcount () >= 1 && player.sheepcount () == 0) {
			TradeForBuyable (ResourceType.Sheep, BuyableType.settlement);
		}
		//priority 6: trade to get brick for settlement 
		if (player.woodcount () >= 1 && player.wheatcount () >= 1 && player.brickcount () == 0 && player.sheepcount () >= 1) {
			TradeForBuyable (ResourceType.Brick, BuyableType.settlement);
		}
	}

	private void TradeForDevcard (Player player) {
		// trade to get sheep for dev card 
		if (player.wheatcount () >= 1 && player.orecount () >= 1 && player.sheepcount () == 0) {
			TradeForBuyable (ResourceType.Sheep, BuyableType.devCard);
		}
		// trade to get ore for dev card 
		if (player.wheatcount () >= 1 && player.orecount () == 0 && player.sheepcount () >= 1) {
			TradeForBuyable (ResourceType.Ore, BuyableType.devCard);
		}
		// trade to get wheat for dev card 
		if (player.wheatcount () >= 1 && player.orecount () == 0 && player.sheepcount () >= 1) {
			TradeForBuyable (ResourceType.Wheat, BuyableType.devCard);
		}
	}

	private void TradeForLongestRoad (Player player) {
		//Since strategy is not 2, so we know AI has not traded for dev card yet 
		//Since trade for settlement session has happened, this trade for road scenario only happens if no trade for settlement 
		//has happened. 
		//So the only scenario we want to prevent is when there is already enough to build city but we trade the city materials 
		//for the road, meh....
		//So if there are already resources to build city, we only trade the other resources(brick, wood, sheep) for road building 
		// if there are not enough resources, it is possible that we have one of the wheat and ore tradable to build a road 
		if (player.orecount () >= 3 && player.wheatcount () >= 2) {
			//trade to get wood for road 
			if (player.woodcount () == 0 && player.brickcount () >= 1) {
				TradeForBuyable (ResourceType.Wood, BuyableType.road);
			}
			//trade to get brick for road 
			if (player.woodcount () >= 1 && player.brickcount () == 0) {
				TradeForBuyable (ResourceType.Brick, BuyableType.road);
			}
		} else {
			//trade to get wood for road 
			if (player.woodcount () == 0 && player.brickcount () >= 1) {
				TradeForBuyable (ResourceType.Wood, BuyableType.road);
			}
			//trade to get brick for road 
			if (player.woodcount () >= 1 && player.brickcount () == 0) {
				TradeForBuyable (ResourceType.Brick, BuyableType.road);
			}
		}
	}

	private void PlayBuyPhase (Player player, AIStrategy strategy) {
		//The first part, build city 
		if (player.wheatcount () >= 2 && player.orecount () >= 3) {
			SettlementClass nextcity = BuildCity ();
			//Build city pointed to by the nextcity 
			if (BuyManager.PlayerCanBuy (player, BuyableType.city) == true) {
				//Display the city!!!! pointed to by nextcity  
				nextcity.upgradeToCity ();
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------
		//The second part: deals with the tradeoff between building a road now or saving the resource to build a settlement later
		if (player.woodcount () >= 1 && player.brickcount () >= 1) {
			TryToBuildRoad (player, strategy);
		}
		//The 3rd part deals with getting a dev card when strategy is 2 
		if (player.wheatcount () >= 1 && player.sheepcount () >= 1 && player.orecount () >= 1 && strategy == AIStrategy.LargestArmy) {
			// Get a dev card 		
			if (BuyManager.PlayerCanBuy (player, BuyableType.devCard) == true) {
				BuyManager.PurchaseForPlayer (BuyableType.devCard, player);
				// Use the dev card right away 
			}
		}
		//The 4th part where AI does not use longest road or largest army strategy, just go buid settlement and city; note that buildcity 
		//function is always called in the beginning so we only worry about building settlements; in this case since we already discard the
		//other two strategies, we assume that there is not much value in building road, but instead we only build settelments as long as 
		//there is still buildablesettlement positions left
		if (strategy == AIStrategy.Build) {
			TryToBuildGeneric(player);
		}
	}

	private void TryToBuildRoad (Player player, AIStrategy strategy) {
		RoadClass nextroad = BuildRoad ();
		if (nextroad == null) {
			//If capabable of building settlement
			if (player.sheepcount () >= 1 && player.wheatcount () >= 1) {
				SettlementClass nextsettlement = BuildSettlement ();
				if (nextsettlement == null) {
					//build random road 
					List<RoadClass> buildablelist = this.graph.BuildableRoads (player);
					if (buildablelist.Count () > 0) {
						RoadClass randomroad = buildablelist [0];
						//build a road pointed to by randomroad 
						if (BuyManager.PlayerCanBuy (player, BuyableType.road) == true) {
							//Display the road pointed to by randomroad! 
							randomroad.buildRoad ();
						}
					}
				} else {
					//build settlement pointed to by "nextsettlement"
					if (BuyManager.PlayerCanBuy (player, BuyableType.settlement) == true) {
						//Display the settlement pointed to by "nextsettlement"
						nextsettlement.buildSettlement ();
					}
				}
			} else {
				//does not have resources to build settlement, but has resources to build road
				if (strategy == AIStrategy.LongestRoad) {
					//build a road pointed to by "nextroad"
					if (BuyManager.PlayerCanBuy (player, BuyableType.road) == true) {
						//Display the road pointed to by "nextroad"
						nextroad.buildRoad ();
					}
				} else {
					if (this.graph.GetLongestRoadForPlayer (player) <= 4) {
						//although in this case longest road is not the strategy, AI needs to extend road reach for better settlement 
						// position 
						//build a road pointed to by "nextroad"
						if (BuyManager.PlayerCanBuy (player, BuyableType.road) == true) {
							//Display the road pointed to by "nextroad"
							nextroad.buildRoad ();
							//else then no need to build road, save for future settlement
						}
					}
				}
			}
		} else {
			//when nextroad is not null 
			//build the road pointed to by nextroad 
			if (BuyManager.PlayerCanBuy (player, BuyableType.road) == true) {
				//Display the road pointed to by "nextroad"
				nextroad.buildRoad ();
				//else then no need to build road, save for future settlement
			}
		}
	}

	private void TryToBuildGeneric (Player player) {
		SettlementClass nextleftset = BuildSettlement ();
		if (nextleftset == null) {
			RoadClass nextleftroad = BuildRoad ();
			if (nextleftroad) {
				//build a road pointed to by nextleftroad, else leave it there 
				if (BuyManager.PlayerCanBuy (player, BuyableType.road) == true) {
					//Display the road pointed to by "nextleftroad"
					nextleftroad.buildRoad ();
				}
			} else {
				//build a settlement at position nextleftset
				if (BuyManager.PlayerCanBuy (player, BuyableType.settlement) == true) {
					//Display the settlement pointed to by "nextleftset"
					nextleftset.buildSettlement ();
				}
			}
		}
	}
	
	/// <summary>
	/// Attempts to execute a trade to get enough of the needed resource to buy
	/// the given buyable, while not dropping below the needed number of other resources
	/// for the given Buyable.  Will only execute one such trade, may need to be called
	/// multiple times if multiple resources are needed.  Will not trade away neededResource.
	/// </summary>
	///  <param name="player">The player to do the trade.</param>
	/// <param name="neededResource">Needed resource.</param>
	/// <param name="desiredItem">Desired item.</param>
	private void TradeForBuyable(ResourceType neededResource, BuyableType desiredItem) {
		BuyManager.Cost cost = BuyManager.GetCostFor (desiredItem);
		// No trade needs to be executed, just return.
		if (player.HasResourceAmount(neededResource, cost.ResourceCount(neededResource))) {
			return;
		}
		foreach (ResourceType tradeCandidate in Enum.GetValues(typeof(ResourceType))) {
			if (tradeCandidate == neededResource) continue; // Would be pointless trade.
			int tradeCost = player.getTradeRatioFor(tradeCandidate);
			int numNeededForBuild = cost.ResourceCount(tradeCandidate);
			if (player.HasResourceAmount(tradeCandidate, numNeededForBuild + tradeCost)) { // Has enough to spare.
				TradeManager.TradeWithHouse(neededResource, tradeCandidate, this.player);
				return;
			}
		}
	}
	

}
