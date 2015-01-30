using UnityEngine;
using UnityEngine.UI;
using System.Collections;



/*

DevCardUseButton is a script meant ot be attached to the buttons
that activate dev cards

This script disables the button if the player doesn't have enough 
of the specific dev card

*/

public class DevCardUseButton : MonoBehaviour {

	public DevCardType cardType; // should be set in inspector
	public Player player;
	Button btn; 

	// Use this for initialization
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(UseCard);
	}
	
	// Update is called once per frame
	void Update () {
		player = TurnState.currentPlayer;


		btn.interactable = player.HasCard(cardType);		
	}

	void UseCard(){
		player = TurnState.currentPlayer;
		// BuyManager.PurchaseForPlayer(buyType, player);
		player.RemoveDevCard(cardType);
	}


	// void OnClick(){
	// 	Debugger.Log("UI", "foo");
	// 	Debug.Log("FPP");
	// }
}
