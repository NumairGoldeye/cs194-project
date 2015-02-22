using UnityEngine;
using UnityEngine.UI;
using System.Collections;



/*

DevCardUseButton is a script meant ot be attached to the buttons
that activate dev cards

This script disables the button if the player doesn't have enough 
of the specific dev card

UI guidelines


Knight will do something weird;

Monopoly will
	show the 5 resource panel - requires you to click confirm or cancel
	clicking confirm enacts the monopoly and creates the alert panel

RoadBuilding will
	show the roadbuilding panel with a cancel button
	will allow you to build 2 roads by changing the turnstate somehow

Year of plenty will
	show the 5 resource panel - requires you to click confirm or cancel
	clicking confirm gives you the two things and creates the alert panel




*/

public class DevCardUseButton : MonoBehaviour {

	// should be set in inspector
	public DevCardType cardType; 
	public TurnSubStateType stateType;

	// don't need
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
		btn.interactable = player.HasCard(cardType) && !TurnState.cardPlayedThisTurn;		
	}

	void UseCard(){

		player = TurnState.currentPlayer;
		
		// Need something to track knights
//		player.RemoveDevCard(cardType);


		TurnState.SetSubStateType(stateType);

		TurnState.chosenResource = ResourceType.None;

//		switch (cardType){
//		case DevCardType.knight:
//			Debug.Log()"Knights not implemented. Hah!");
//			break;
//		case DevCardType.monopoly:
//			break;
//		case DevCardType.roadBuilding:
//			break;
//		case DevCardType.victoryPoint:
//			break;
//		case DevCardType.yearOfPlenty:
//			break;
//				
//		}




	}
}
