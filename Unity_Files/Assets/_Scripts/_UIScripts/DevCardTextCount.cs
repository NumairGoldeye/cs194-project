using UnityEngine;
using UnityEngine.UI;
using System.Collections;


// This will be attached to a UI text element representing
// the number of a specific Development card a player has

public class DevCardTextCount : MonoBehaviour {

	// Should set
	public DevCardType cardType; // should be set in inspector
	public Player player;

	Text txt;

	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
		// if (cardType){
		// check current player
		player = TurnState.currentPlayer;


		txt.text = player.DevCardCount(cardType).ToString();
		// }
	}
}
