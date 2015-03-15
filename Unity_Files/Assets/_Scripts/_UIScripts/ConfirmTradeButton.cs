using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfirmTradeButton : MonoBehaviour {

	public TradeConsole tradeConsole;
	public GameObject tradeConfirm;

	public TradeCounter turnPlayerToGiveCounter;
	public TradeCounter turnPlayerToGetCounter;

	public ComboBox tradePlayerBox;
	Button btn;
	
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(ConfirmTrade);
	}

	private void ConfirmTrade() {
		// First element in ComboBox is "None", so need to add 1.
		Player current = TurnState.currentPlayer;
		Player target = Player.allPlayers [tradePlayerBox.SelectedIndex - 1];
		TradeManager.tradeBetweenPlayers(
			current, turnPlayerToGiveCounter, target, turnPlayerToGetCounter);
		ChatLog.Instance.AddChatEvent(
			"Player " + current.playerName + " " + "traded with " + target.playerName + "!");
		tradeConfirm.SetActive (false);
		tradeConsole.DisplayTradeOptionConsole ();
	}

}
