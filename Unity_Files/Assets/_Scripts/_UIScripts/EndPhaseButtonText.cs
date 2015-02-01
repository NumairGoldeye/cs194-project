using UnityEngine;
using UnityEngine.UI;
using System.Collections;


// Controls the EndPhaseButton text

public class EndPhaseButtonText : MonoBehaviour {



	Text txt;

	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
		switch (TurnState.stateType){
			case (TurnStateType.trade) :
				// Debugger.Log("TurnState", "FOOOOO");
				txt.text = "End Trade Phase";
				break;
			case (TurnStateType.build) : 
				txt.text = "End Turn";
				break;
		}
	}
}
