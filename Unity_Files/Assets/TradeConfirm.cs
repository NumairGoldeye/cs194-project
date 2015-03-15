using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TradeConfirm : MonoBehaviour {

	public TradeConsole tradeConsole;
	public Text tradeGiveText;
	public Text tradeGetText;
	
	public ComboBox tradePlayerBox;
	public Button acceptButton;
	
	public TradeCounter turnPlayerToGiveCounter;
	public TradeCounter turnPlayerToGetCounter;


	public void Display() {
		tradeConsole.Disable ();
		Player target = GameManager.Instance.players [tradePlayerBox.SelectedIndex - 1];
		// Can confirm trade if and only if enough resources. 
		if (target.HasResources (turnPlayerToGetCounter)) {
			acceptButton.gameObject.SetActive (true);
		} else {
			acceptButton.gameObject.SetActive (false);
		}
		tradeGiveText.text = "Give: " + turnPlayerToGetCounter.GetText();
		tradeGetText.text = "Get: " + turnPlayerToGiveCounter.GetText();
		// Chris do networking
		gameObject.SetActive (true);
	}

}
