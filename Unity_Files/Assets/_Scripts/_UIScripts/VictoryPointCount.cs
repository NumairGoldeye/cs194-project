using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// Attach to a text to display the number of victory points
/// </summary>
public class VictoryPointCount : MonoBehaviour {

	public bool isCurrent = true; // use TurnState.currentPlayer
	public Player player;
	
	Text txt;
	
	// Use this for initialization
	void Start () {
		txt = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
		txt.text = getPlayer().victoryPoints.ToString();
		
	}
	
	Player getPlayer(){
		if 	(isCurrent){
			return TurnState.currentPlayer;
		} else {
			return player;
		}
	}
}
