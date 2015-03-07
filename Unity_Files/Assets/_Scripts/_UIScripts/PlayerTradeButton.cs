using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerTradeButton : MonoBehaviour {
	
	public TradeConsole tradeConsole;
	public GameObject tradeConfirm;
	public ComboBox tradePlayerBox;
	public Button acceptButton;

	public TradeCounter turnPlayerToGiveCounter;
	public TradeCounter turnPlayerToGetCounter;

	Button btn;
	/// <summary>
	/// The order that resources occur in the menu.
	/// </summary>
	ResourceType[] menuOrder = {
		ResourceType.Sheep,
		ResourceType.Wheat,
		ResourceType.Brick,
		ResourceType.Ore,
		ResourceType.Wood
	};
	
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
		tradeConfirm.SetActive (true);
	}
	
}
