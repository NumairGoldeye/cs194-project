#pragma warning disable 0414 // variable assigned but not used.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// BuyManager does all the logic for purchasing


/*

// Usage: 

// TO check if player can buy something
BuyManager.PlayerCanBuy(BuyableType.road, player);

// To acually purchase and remove resources from the player
BuyManager.PurchaseForPlayer(BuyableType.road, player);


*/

public enum BuyableType { road, settlement, city, devCard};

public static class BuyManager {

	// Just some shorthand
	private static ResourceType wood = ResourceType.Wood;
	private static ResourceType sheep = ResourceType.Sheep;
	private static ResourceType brick = ResourceType.Brick;
	private static ResourceType ore = ResourceType.Ore;
	private static ResourceType wheat = ResourceType.Wheat;

	private static Cost roadCost = new Cost(new ResourceType[] {wood, brick} );
	private static Cost settlementCost = new Cost(new ResourceType[] {sheep, wheat, brick, wood} );
	private static Cost cityCost = new Cost(new ResourceType[] {ore, ore, ore, wheat, wheat} );
	private static Cost devCardCost = new Cost(new ResourceType[] {ore, wheat, sheep} );

	private static Dictionary<BuyableType, Cost> costMap = new Dictionary<BuyableType, Cost>() {
		{ BuyableType.road, roadCost },
		{ BuyableType.settlement, settlementCost },
		{ BuyableType.city, cityCost },
		{ BuyableType.devCard, devCardCost },
	};

	public class Cost {
		// A resourceType index, just like player costs
		public int[] costs;

		public Cost(ResourceType[] resources){
			costs = new int[5];
			foreach (ResourceType res in resources){
				costs[(int)res]++;
			}
		}

		// Returns the number of a specific resource that a player has
		// returns -1 on terrible terrible failure
		public int ResourceCount(ResourceType resource){
			return costs[(int) resource];
		}

		public bool Requires(ResourceType resource) {
			return ResourceCount(resource) > 0;
		}
	};
	
	public static void Test(Player player){
		Debugger.Log("Charlie", "Buy city: " + PlayerCanBuy(player, BuyableType.city));
	}

	public static Cost GetCostFor(BuyableType item) {
		return costMap[item];
	}

	// Call this everywhere you need it!
	// Pass it the player object and the  BuyableType and it will
	// return you a variable
	public static bool PlayerCanBuy(Player player, BuyableType buyable){
		foreach(ResourceType res in Enum.GetValues(typeof(ResourceType))){
			if (ResourceType.None != res){
				if (costMap[buyable].ResourceCount(res) > player.GetResourceCount(res)){
					return false;
				}
			}
		}
		return true;
	}


	// TODO: check if they player can even buy the thing
	public static void PurchaseForPlayer(BuyableType buyable, Player player){
		// if buyable
		// then figure out the cost
		// then subtract those from the player
		foreach(ResourceType res in Enum.GetValues(typeof(ResourceType))){
			if (ResourceType.None != res){
				player.RemoveResource(res, costMap[buyable].ResourceCount(res));
			}
		}

		if (buyable == BuyableType.devCard){
			player.DrawRandomCard();
		}
	}


}
