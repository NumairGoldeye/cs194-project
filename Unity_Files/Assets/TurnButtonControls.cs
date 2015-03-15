using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TurnButtonControls : MonoBehaviour {
		
		public Button RollDice;
		public Button TradeButton;
		public Button BuildButton;
		public Button EndPhaseButton;
		public Button DevelopmentCardButton;
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			// Only modify the buttons if the game has started
			if (!GameManager.Instance.gameStarted) return;
			// If it's my turn, make the turn buttons interactable, otherwise make them not-interactable
			if(GameManager.Instance.myTurn()) {
				
			} else {
				RollDice.interactable = false;
				TradeButton.interactable = false;
				BuildButton.interactable = false;
				EndPhaseButton.interactable = false;
				DevelopmentCardButton.interactable = false;
			}
		}
	}