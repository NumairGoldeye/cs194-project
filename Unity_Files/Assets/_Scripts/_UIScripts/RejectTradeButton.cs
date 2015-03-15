using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RejectTradeButton : MonoBehaviour {

	public TradeConsole tradeConsole;
	public GameObject tradeResponse;
	public ComboBox tradePlayerBox;

	Button btn;
	
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(RejectTrade);
	}

	private void RejectTrade() {
		Player current = TurnState.currentPlayer;
		Player target = GameManager.Instance.players [tradePlayerBox.SelectedIndex - 1];
		ChatLog.Instance.AddChatEvent(
			"Player " + current.playerName + " " + "rejected trade from " + target.playerName + "!");
		tradeResponse.SetActive (false);
		tradeConsole.DisplayTradeOptionConsole ();
	}
}
