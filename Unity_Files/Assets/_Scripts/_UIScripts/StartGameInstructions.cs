using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Tells the player what to do in the beginning of the game

public class StartGameInstructions : MonoBehaviour {

	// Set in inspector
	public GameObject startGameButton; // Should have as onclick UIManager.TSMStartMainGameplay();
	public GameObject playerNameText;

	// don't
	Text txt;

	
	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {

		string result = "";

		if (StartGameManager.finished){
			TurnState.Startup();
		}


		if (!StartGameManager.builtSettlement){
			result += "Build a settlement by clicking on one. It must be at least 1 vertex away from any existing settlement.";
			if (StartGameManager.secondPhase)
				result += ". Gain resources for each tile adjacent to this settlement.";
		} else {
			result += "Build a road by clicking on one. It must be attached to a road or a settlement you control.";
		}
		txt.text = result;
	}
}