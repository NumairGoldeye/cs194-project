using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;



/// <summary>
/// The input UI thing that manages adding, naming, and coloring players.
/// </summary>
public class CreatePlayerInput : MonoBehaviour {

	// Set in inspector
	public int index;
	public GameObject inputObj;
	public GameObject playerColorObj;


	Player p;
	Text txt;
	InputField input;
	Image colorImg;

	public void ChangePlayerName(String s){
//		Debug.Log (s);
		p.playerName = s;
	}

	// Use this for initialization
	void Start () {
		input = inputObj.GetComponent<InputField>();
		txt = inputObj.transform.GetChild(0).GetComponent<Text>();
		colorImg = playerColorObj.GetComponent<Image>();

		p = Player.allPlayers[index];
		txt.text = "Player " + (p.playerId + 1).ToString();

	}
	
	// Update is called once per frame
	void Update () {
		if (p != null){
			colorImg.color = p.playerColor;
		}
	}
}
