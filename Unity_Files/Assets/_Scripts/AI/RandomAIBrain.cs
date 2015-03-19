using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomAIBrain : AbstractAIBrain {

	BoardGraph graph;
	Player player;

	static System.Random rnd = new System.Random();

	public RandomAIBrain(Player player, BoardGraph graph) {
		this.graph = graph;
		this.player = player;
	}

	public override void SetupRoad() {
		List<RoadClass> possibleRoads = this.graph.BuildableRoads(player);
		int randomIndex = rnd.Next(possibleRoads.Count);
		RoadClass toBuild = possibleRoads [randomIndex];
		toBuild.buildRoad();
	}

	public override void SetupSettlement() {
		List<SettlementClass> possibleSettlements = new List<SettlementClass>();
		foreach (SettlementClass temp in this.graph.getSettlements()) {
			if(!this.graph.HasNeighboringSettlement(temp)){
				possibleSettlements.Add(temp);
			}
		}
		SettlementClass toBuild = RandomSettlement(possibleSettlements);
		toBuild.buildSettlement ();
	}

	// This AI cheats and gives itself 1 of each resource every turn!
	protected override void SetupTurn() {
		player.AddResource(ResourceType.Brick, 1);
		player.AddResource(ResourceType.Wheat, 1);
		player.AddResource(ResourceType.Wood, 1);
		player.AddResource(ResourceType.Sheep, 1);
		player.AddResource(ResourceType.Ore, 1);
	}
	
	protected override void PlayTradePhase() {
		// No trading.
	}
	
	protected override void PlayBuyPhase() {
		TryToBuyCity ();
		TryToBuySettlement ();
		TryToBuyRoad ();
	}
	
	protected override void TearDownTurn() {
		// Do Nothing.
	}

	private void TryToBuyCity() {
		if (!BuyManager.PlayerCanBuy (player, BuyableType.city)) return;
		List<SettlementClass> possibileCities = graph.BuildableCity(player);
		if (possibileCities.Count == 0) return;
		RandomSettlement (possibileCities).upgradeToCity();
		ChatLog.Instance.AddChatMessage("AI built a city!");
	}

	private void TryToBuySettlement() {
		if (!BuyManager.PlayerCanBuy (player, BuyableType.settlement)) return;
		List<SettlementClass> possibleSettlements = graph.BuildableSettlements(player);
		if (possibleSettlements.Count == 0) return;
		RandomSettlement (possibleSettlements).buildSettlement();
		ChatLog.Instance.AddChatMessage("AI built a settlement!");
	}

	private void TryToBuyRoad() {
		if (!BuyManager.PlayerCanBuy (player, BuyableType.road)) return;
		List<RoadClass> possibleRoads = graph.BuildableRoads(player);
		if (possibleRoads.Count == 0) return;
		RandomRoad(possibleRoads).buildRoad();
		ChatLog.Instance.AddChatMessage("AI built a road!");
	}

	private SettlementClass RandomSettlement(List<SettlementClass> list) {
		int randomIndex = rnd.Next(list.Count);
		return list[randomIndex];
	}

	private RoadClass RandomRoad(List<RoadClass> list) {
		int randomIndex = rnd.Next(list.Count);
		return list[randomIndex];
	}
}
