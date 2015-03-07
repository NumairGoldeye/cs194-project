using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RejectTradeButton : MonoBehaviour {

	public TradeConsole tradeConsole;
	public GameObject tradeResponse;

	Button btn;
	
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(RejectTrade);
	}

	private void RejectTrade() {
		ChatLog.Instance.AddChatEvent("Player TODO rejected trade offer from TODO!");
		tradeResponse.SetActive (false);
		tradeConsole.DisplayTradeOptionConsole ();
	}
}
