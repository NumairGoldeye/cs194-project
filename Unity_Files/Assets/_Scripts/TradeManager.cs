using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public static void TradeWithHouse(ResourceType toGet, ResourceType toGive, Player player){
		int cost = player.getTradeRatioFor (toGive);
		if (!player.HasResourceAmount(toGive, cost)) {
			return; // TODO: Throw exception?
		}
		player.RemoveResource (toGive, cost);
		player.AddResource (toGet, 1);
		GameManager.Instance.networkView.RPC ("syncResources", RPCMode.Others, player.playerId, (int)toGive, cost);
		GameManager.Instance.networkView.RPC ("syncResources", RPCMode.Others, player.playerId, (int)toGet, 1);
	}

	public static void tradeBetweenPlayers(
			Player player1, ResourceCounter turnPlayerToGiveCounter,
			Player player2, ResourceCounter turnPlayerToGetCounter) {
		executeOneWayTrade (player1, player2, turnPlayerToGiveCounter);
		executeOneWayTrade (player2, player1, turnPlayerToGetCounter);

	}

	private static void executeOneWayTrade(Player giver, Player receiver, ResourceCounter counter) {
		foreach (KeyValuePair<ResourceType, int> pair in counter) {
			giver.RemoveResource (pair.Key, pair.Value);
			receiver.AddResource (pair.Key, pair.Value);
			GameManager.Instance.networkView.RPC ("syncResources", RPCMode.Others, giver.playerId, (int)pair.Key, -pair.Value);
			GameManager.Instance.networkView.RPC ("syncResources", RPCMode.Others, receiver.playerId, (int)pair.Key, pair.Value);
		}
	}
		              

}
