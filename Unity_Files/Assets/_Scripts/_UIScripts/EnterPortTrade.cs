using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnterPortTrade : MonoBehaviour {

	Button btn;

	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(TurnState.instance.DisplayPortTradeConsole);
	}
	
}
