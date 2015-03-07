using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

public class PlayerTradeButton : MonoBehaviour {
	
	public TradeConsole tradeConsole;
	public GameObject tradeConfirm;
	public Text tradeGiveText;
	public Text tradeGetText;

	public ComboBox tradePlayerBox;
	public Button acceptButton;

	public TradeCounter turnPlayerToGiveCounter;
	public TradeCounter turnPlayerToGetCounter;

	Button btn;

	// Use this for initialization
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(DisplayTradeConfirm);
	}
	
	void  DisplayTradeConfirm() {
		tradeConsole.Disable ();
		Player target = Player.allPlayers [tradePlayerBox.SelectedIndex - 1];
		// Can confirm trade if and only if enough resources. 
		if (target.HasResources (turnPlayerToGetCounter)) {
			acceptButton.gameObject.SetActive (true);
		} else {
			acceptButton.gameObject.SetActive (false);
		}
		tradeGiveText.text = "Give: " + PlayerTradeButton.GetCounterText(turnPlayerToGetCounter);
		tradeGetText.text = "Get: " + PlayerTradeButton.GetCounterText(turnPlayerToGiveCounter);
		tradeConfirm.SetActive (true);
	}

	private static string GetCounterText(TradeCounter counter) {
		StringBuilder builder = new StringBuilder ();
		bool isFirst = true;
		foreach (KeyValuePair<ResourceType, int> pair in counter) {
			if (isFirst) {
				isFirst = false;
			} else {
				builder.Append(", ");
			}
			builder.Append("" + pair.Value + " " + pair.Key.ToString() + " "); 
		}
		return builder.ToString();
	}
	
}
