﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;




public class CurrentPlayerText : MonoBehaviour {

	// public DevCardType cardType;
	// public Player player;
	Text txt;
	Color color;

	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
		// player = TurnState.currentPlayer;

		if (GameManager.Instance.gameStarted) {
			txt.text = TurnState.currentPlayer.playerName + "'s Turn";
			color = TurnState.currentPlayer.playerColor;
		}
	}

}
