using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Tells you if you've already played one
public class DevPanelSubtext : MonoBehaviour {

	// Should set
	Text txt;
	
	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
//		DevCardType currentDev = TurnState.DevTypeForSubstate();
		if (!TurnState.cardPlayedThisTurn){
			txt.text = "";
		} else {
			txt.text = "You've already played a Dev card this turn";
		}
	}
}
