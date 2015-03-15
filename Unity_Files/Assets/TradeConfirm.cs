using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TradeConfirm : MonoBehaviour {

	public TradeConsole tradeConsole;
	public Text playerText;
	public Text tradeGiveText;
	public Text tradeGetText;
	
	public ComboBox tradePlayerBox;
	public Button rejectButton;
	public Button acceptButton;
	
	public TradeCounter turnPlayerToGiveCounter;
	public TradeCounter turnPlayerToGetCounter;

	public static Player targetPlayer;


	public void Display(string receiveMessage, string giveMessage) {
		tradeConsole.Disable ();
		// Can confirm trade if and only if enough resources. 
		if (!GameManager.Instance.myTurn()) {
			Debugger.Log("Trade", receiveMessage + " & " + giveMessage);
			if (GameManager.Instance.myPlayer.HasResourcesString(giveMessage)) {
				acceptButton.gameObject.SetActive (true);
			} else {
				acceptButton.gameObject.SetActive (false);
			}
			playerText.text = TurnState.currentPlayer.playerName + " wants to trade with you";
		} else {
			playerText.text = "You want to trade with " + targetPlayer.playerName;
			rejectButton.gameObject.SetActive(false);
			acceptButton.gameObject.SetActive(false);
		}

		tradeGiveText.text = "Give: " + giveMessage;
		tradeGetText.text = "Get: " + receiveMessage;
		gameObject.SetActive (true);
	}

	public void requestTrade() {
		targetPlayer = Player.FindByPlayerName(Player.FindByPlayerName(tradeConsole.playerNames[tradePlayerBox.SelectedIndex - 1]).playerName);

		GameManager.Instance.networkView.RPC ("requestTrade", targetPlayer.networkPlayer, turnPlayerToGiveCounter.GetText (), turnPlayerToGetCounter.GetText ());
		Display (turnPlayerToGetCounter.GetText (), turnPlayerToGiveCounter.GetText ());
	}
	
	public void executeTrade() {
		TradeManager.tradeBetweenPlayers(TurnState.currentPlayer, turnPlayerToGiveCounter, targetPlayer, turnPlayerToGetCounter);
		playerText.text = "Your trade with " + targetPlayer.playerName + " was accepted";
//		playerText.font.material.color = Color.red;
		Wait ();
		gameObject.SetActive (false);
		tradeConsole.DisplayTradeOptionConsole ();
	}

	public void rejectTrade() {
		playerText.text = "Your trade with " + targetPlayer.playerName + " was declined";
//		playerText.font.material.color = Color.red;
		Wait ();
		gameObject.SetActive (false);
		tradeConsole.DisplayTradeOptionConsole ();
	}

	private IEnumerator Wait() {
		yield return new WaitForSeconds(3);
	}
}
