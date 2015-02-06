using UnityEngine;
using UnityEngine.UI;
using System.Collections;


// Script controls the interactability of the end phase button

public class EndPhaseButton : MonoBehaviour {

	// Set in inspector
	public Button rollButton;
	public Button buildButton;
	public Button tradeButton;

	Button btn;


	// Use this for initialization
	void Start () {
		btn = gameObject.GetComponent<Button>();	
		btn.onClick.AddListener(EndClicked);
		
	}
	
	// Update is called once per frame
	void Update () {

		btn.interactable = TurnStateType.roll != TurnState.stateType;


	}

	void EndClicked(){
		TurnState.NextTurnState();
		switch (TurnState.stateType){
			case (TurnStateType.roll) :
				rollButton.gameObject.SetActive(true);
				buildButton.gameObject.SetActive(false);
				break;
			case (TurnStateType.trade) :
				rollButton.gameObject.SetActive(false);	
				tradeButton.gameObject.SetActive(true);
				// Debugger.Log("TurnState", "FOOOOO");
				break;
			case (TurnStateType.build) : 
				tradeButton.gameObject.SetActive(false);
				buildButton.gameObject.SetActive(true);
				break;
		}
	}

	
}
