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
	private static ResourceType wood = ResourceType.wood;
	private static ResourceType sheep = ResourceType.sheep;
	private static ResourceType brick = ResourceType.brick;
	private static ResourceType ore = ResourceType.ore;
	private static ResourceType wheat = ResourceType.wheat;

	private static Dictionary<BuyableType, Cost> costMap = new Dictionary<BuyableType, Cost>();

	private class Cost {
		// A resourceType index, just like player costs
		public int[] costs;
		public BuyableType buyableType;

		public Cost(BuyableType buyable, ResourceType[] resources){
			costMap.Add(buyable, this);

			costs = new int[5];
			buyableType = buyable;
			// Debugger.Log("Charlie", "Cost: " + buyableType.ToString());
			foreach (ResourceType res in resources){
				costs[(int)res]++;
				// Debugger.Log("Charlie", res.ToString());
			}
			// Debugger.Log("Charlie", "End Cost");
		}

		// Returns the number of a specific resource that a player has
		// returns -1 on terrible terrible failure
		public int ResourceCount(ResourceType resource){
			// Debug.Log((int)resource);
			return costs[(int) resource];
		}
	};
	


	// Well this is roundabout...
	private static Cost roadCost = new Cost( BuyableType.road, new ResourceType[] {wood, brick} );
	private static Cost settlementCost = new Cost( BuyableType.settlement, new ResourceType[] {sheep, wheat, brick, wood} );
	private static Cost cityCost = new Cost( BuyableType.city, new ResourceType[] {ore, ore, ore, wheat, wheat} );
	private static Cost devCardCost = new Cost( BuyableType.devCard, new ResourceType[] {ore, wheat, sheep} );
	
	// private static Cost[] buyableCosts = new Cost[] { roadCost, settlementCost, cityCost, devCardCost};

	public static void Test(Player player){
		Debugger.Log("Charlie", "Buy city: " + PlayerCanBuy(player, BuyableType.city));
	}

	// Call this everywhere you need it!
	// Pass it the player object and the  BuyableType and it will
	// return you a variable
	public static bool PlayerCanBuy(Player player, BuyableType buyable){
		// This code makes sense
		// foreach(Cost buyableCost in buyableCosts){
		Cost buyableCost = null;
		if (costMap.TryGetValue( buyable , out buyableCost)) {
			// if (buyableCost.buyableType == buyable){
			foreach(ResourceType res in Enum.GetValues(typeof(ResourceType))){
				if (ResourceType.desert != res){
					if (buyableCost.ResourceCount(res) > player.GetResourceCount(res)){
						return false;
					}
						// Debugger.Log(flag, res.ToString() + GetResourceCount(res));
				}
			}
			return true;
		}
		// You probably messed up 
		return false;
	}


	// TODO: check if they player can even buy the thing
	public static void PurchaseForPlayer(BuyableType buyable, Player player){
		// if buyable
		// then figure out the cost
		// then subtract those from the player
		Cost buyableCost = null;
		if (costMap.TryGetValue( buyable , out buyableCost)) {
			// if (buyableCost.buyableType == buyable){
			foreach(ResourceType res in Enum.GetValues(typeof(ResourceType))){
				if (ResourceType.desert != res){
					player.RemoveResource(res, buyableCost.ResourceCount(res));
				}
			}
		}

		if (buyable == BuyableType.devCard){
			player.DrawRandomCard();
		}
	}


}
