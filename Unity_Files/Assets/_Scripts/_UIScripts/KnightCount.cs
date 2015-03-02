using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Attach to a text to display the number of knights used
/// </summary>
public class KnightCount : MonoBehaviour {

	public bool isCurrent = true; // use TurnState.currentPlayer
	public Player player;

	Text txt;

	// Use this for initialization
	void Start () {
		txt = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

		txt.text = getPlayer().numUsedKnights.ToString();

	}

	Player getPlayer(){
		if 	(isCurrent){
			return TurnState.currentPlayer;
		} else {
			return player;
		}
	}

}
