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

		GameManager.Instance.networkView.RPC ("abortTrade", TurnState.currentPlayer.networkPlayer);
		tradeResponse.SetActive (false);
	}
}
