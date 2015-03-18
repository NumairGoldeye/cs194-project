using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Victory panel
/// </summary>
public class VictoryPanel : MonoBehaviour {

	// Set in inspector
	public Text victoryTitleTxt;
	public Text victoryTxt;

	public bool set = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Setup(){
		victoryTitleTxt.text = TurnState.winningPlayer.playerName + " has won!";
		
		string otherPoints = "";
		
		foreach(Player p in GameManager.Instance.players){
			otherPoints += p.playerName + ": " + p.victoryPoints + " points \n";
		}
		
		victoryTxt.text = otherPoints;

		Debugger.Log ("Victory", victoryTxt.text);
	}


}
