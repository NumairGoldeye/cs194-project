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

	public void ConfirmTrade() {
		// First element in ComboBox is "None", so need to add 1.
		GameManager.Instance.networkView.RPC ("executeTrade", TurnState.currentPlayer.networkPlayer);
	}

}
