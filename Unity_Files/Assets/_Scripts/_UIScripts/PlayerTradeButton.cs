using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

public class PlayerTradeButton : MonoBehaviour {
	
	public TradeConfirm tradeConfirm;

	Button btn;

	// Use this for initialization
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(tradeConfirm.requestTrade);
	}
}
