using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnterPlayerTrade : MonoBehaviour {

	public TradeConsole tradeConsole;
	Button btn;
	
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(tradeConsole.DisplayPlayerTradeConsole);
	}
	
}
