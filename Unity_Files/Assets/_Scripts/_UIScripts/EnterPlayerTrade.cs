using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnterPlayerTrade : MonoBehaviour {
	
	Button btn;
	
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(TurnState.instance.DisplayPlayerTradeConsole);
	}
	
}
