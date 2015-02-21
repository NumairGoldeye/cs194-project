using System;
using System.Collections;
using System.Collections.Generic;

// TradeManager does all the logic for trading


/*

// Usage: 

// To trade with House:
// TradeManager.TradeWithHouse(toGet, toGive, player);

// To trade between players:
// tradeBetweenPlayers(
	player1, player1ResourceToGive, player1AmountToGive,
	player2, player2ResourceToGive, player2AmountToGive)

*/


public static class TradeManager {
	// The amount of a resource it costs to trade for another resource with no ports.
	private const int TRADE_COST = 4;
	
	// The amount of a resource it costs to trade for another resource with a generic port.
	private const int TRADE_COST_WITH_GENERIC_PORT = 3;
	
	// The amount of a resource it costs to trade for another resource with a resource-specific port.
	private const int TRADE_COST_WITH_SPECIFIC_PORT = 2;
	
	public static void TradeWithHouse(ResourceType toGet, ResourceType toGive, Player player){
		int cost = getCostForPlayer (toGive, player);
		if (!player.HasResourceAmount(toGive, cost)) {
			return; // TODO: Throw exception?
		}
		player.RemoveResource (toGive, cost);
		player.AddResource (toGet, 1);

	}

	public static int getCostForPlayer(ResourceType resource, Player player) {
		if (player.hasPortFor(resource)) {
			return TRADE_COST_WITH_SPECIFIC_PORT;
		} else if (player.hasPortFor(ResourceType.None)) {
			return TRADE_COST_WITH_GENERIC_PORT;
		}
		return TRADE_COST;
	}

	public static void tradeBetweenPlayers(
			Player player1, ResourceType player1ToGive, int player1AmountToGive,
			Player player2, ResourceType player2ToGive, int player2AmountToGive) {
		if (!player1.HasResourceAmount (player1ToGive, player1AmountToGive)) {
			return; // TODO: Throw exception?
		}
		if (!player2.HasResourceAmount (player2ToGive, player2AmountToGive)) {
			return; // TODO: Throw exception?
		}
		executeOneWayTrade (player1, player2, player1ToGive, player1AmountToGive);
		executeOneWayTrade (player2, player1, player2ToGive, player2AmountToGive);

	}

	private static void executeOneWayTrade(Player giver, Player receiver, ResourceType resource, int amount) {
		giver.RemoveResource (resource, amount);
		receiver.AddResource (resource, amount);
	}
		              

}
