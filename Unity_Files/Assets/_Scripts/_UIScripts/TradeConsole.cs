using UnityEngine;
using System.Collections;

public class TradeConsole : MonoBehaviour {

	public GameObject portTradeConsole;
	public GameObject playerTradeConsole;
	public GameObject tradeOptionConsole;

	public ComboBox portToGiveBox;

	public void DisplayTradeOptionConsole() {
		gameObject.SetActive (true);
		portTradeConsole.SetActive (false);
		playerTradeConsole.SetActive (false);
		tradeOptionConsole.SetActive (true);
	}
	
	public void DisplayPortTradeConsole() {
		ConfigurePortTradeConsole ();
		gameObject.SetActive (true);
		portTradeConsole.SetActive (true);
		playerTradeConsole.SetActive (false);
		tradeOptionConsole.SetActive (false);
	}
	
	private void ConfigurePortTradeConsole() {
		// Assumes the order: Sheep, Wheat, Brick, Ore, Wood
		ResourceType[] resourceOrder =
		{ResourceType.Sheep, ResourceType.Wheat, ResourceType.Brick, ResourceType.Ore, ResourceType.Wood};
		for (int i = 0; i < resourceOrder.Length; ++i) {
			ComboBoxItem item = portToGiveBox.Items[i+1]; // 0th item is "None"
			ResourceType resource = resourceOrder[i];
			int cost = TurnState.currentPlayer.getTradeRatioFor(resource);
			item.Caption = "" + cost + " " + resource;
			if (TurnState.currentPlayer.HasResourceAmount(resource, cost)) {
				item.IsDisabled = false;
			} else {
				item.IsDisabled = true;
			}
		}
	}
	
	public void DisplayPlayerTradeConsole() {
		gameObject.SetActive (true);
		portTradeConsole.SetActive (false);
		playerTradeConsole.SetActive (true);
		tradeOptionConsole.SetActive (false);
	}
	
	public void ResetPortTradeConsole() {
		DisplayPortTradeConsole ();
	}

	public void Disable() {
		gameObject.SetActive (false);
	}
}
