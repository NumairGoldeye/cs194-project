using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*

The confirm button on the monopoly/resource panel

makes sure you can't click until a resource is selected

onlick in inspector should show alert panel

 */

public class DevConfirmButton : MonoBehaviour {

	// Set in inspector
	public GameObject alertPanel;
	public Text alertPanelTxt;
	

	private static Button btn;
	
	// Use this for initialization
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(ExecuteCard);
	}
	
	// Update is called once per frame
	void Update () {
		if (TurnState.subStateType == TurnSubStateType.monopolyChoose || TurnState.subStateType == TurnSubStateType.yearOfPlentyChoose){
			btn.interactable = TurnState.chosenResource != ResourceType.None;
		} else if (TurnState.subStateType == TurnSubStateType.roadBuilding){
			btn.interactable = TurnState.secondRoadBuilt;
		} else if (TurnState.subStateType == TurnSubStateType.robbering) {
			btn.gameObject.SetActive(false);
		}
	}


	void ExecuteCard(){

		DevCardType cardType = TurnState.DevTypeForSubstate();
		int info = DevCard.ExecuteCard(cardType);
		TurnState.currentPlayer.RemoveDevCard(cardType);
		TurnState.cardPlayedThisTurn = true;

		if (TurnState.PlayCardOnConfirm()){
			alertPanelTxt.text = DevCard.ExecutedCardDesc(cardType, info);
			alertPanel.SetActive(true);
		}

		TurnState.ResetSubStateType2();
	}


	public static void clickButton() {
		btn.onClick.Invoke();
	}
}