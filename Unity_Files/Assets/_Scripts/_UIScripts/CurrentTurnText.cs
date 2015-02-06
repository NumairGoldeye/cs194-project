using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CurrentTurnText : MonoBehaviour {

	public TurnStateType state; // set in inspector

	Text txt;

	static Color selectedColor = new Color(1.0f,1.0f,1.0f,1.0f);
	static Color unselectedColor = new Color(1.0f,1.0f,1.0f,.2f);

	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>();		
	}
	
	// Update is called once per frame
	void Update () {
		if (state == TurnState.stateType){
			txt.color = CurrentTurnText.selectedColor;	
		} else {
			txt.color = CurrentTurnText.unselectedColor;
		}
//		txt.text = TurnState.stateType.ToString();
	}
}
