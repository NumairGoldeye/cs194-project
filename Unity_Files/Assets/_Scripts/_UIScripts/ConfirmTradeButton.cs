using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfirmTradeButton : MonoBehaviour {

	public TradeConsole tradeConsole;
	public GameObject tradeConfirm;

	public TradeCounter turnPlayerToGiveCounter;
	public TradeCounter turnPlayerToGetCounter;

	Button btn;
	
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(ConfirmTrade);
	}

	private void ConfirmTrade() {
		// TODO Add actual trade.
		ChatLog.Instance.AddChatEvent("Player TODO traded with TODO!");
		tradeConfirm.SetActive (false);
		tradeConsole.DisplayTradeOptionConsole ();
	}

}
