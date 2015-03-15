using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TradeConsole : MonoBehaviour {

	public GameObject portTradeConsole;
	public GameObject playerTradeConsole;
	public GameObject tradeOptionConsole;
	public Button tradeButton;

	public ComboBox playerToTrade;

	public ComboBox portToGiveBox;
	public ComboBox playerToTradeBox;

	public TradeCounter toGive;

	void Update() {
		if (playerToTrade.SelectedIndex > 0 && TurnState.currentPlayer.HasResources (toGive)) {
			tradeButton.gameObject.SetActive(true);
		} else {
			tradeButton.gameObject.SetActive(false);
		}
	}

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
		ConfigurePlayerTradeConsole();
		gameObject.SetActive (true);
		portTradeConsole.SetActive (false);
		playerTradeConsole.SetActive (true);
		tradeOptionConsole.SetActive (false);
	}

	public void ConfigurePlayerTradeConsole() {
		playerToTradeBox.SelectedIndex = 0;
		// Currently, the configuration is the same for every player, so
		// this is a sort of lazy-shortcut to make this only done once.
		if (playerToTradeBox.Items.Length > 1) return;
		string[] playerNames = new string[GameManager.Instance.players.Count];
		for (int i = 0; i < GameManager.Instance.players.Count; ++i) {
			playerNames[i] = GameManager.Instance.players[i].playerName;
		}
		playerToTradeBox.ItemsToDisplay = GameManager.Instance.players.Count;
		playerToTradeBox.AddItems(playerNames);
		playerToTradeBox.Refresh ();
	}
	
	public void ResetPortTradeConsole() {
		DisplayPortTradeConsole ();
	}

	public void Disable() {
		gameObject.SetActive (false);
	}
}
