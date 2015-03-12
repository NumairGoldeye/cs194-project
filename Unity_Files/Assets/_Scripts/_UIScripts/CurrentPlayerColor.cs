using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Sets the current player's color to this objs color
/// </summary>
public class CurrentPlayerColor : MonoBehaviour {

	Image img;

	// Use this for initialization
	void Start () {
		img = gameObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.gameStarted)
			img.color = TurnState.currentPlayer.playerColor;
	}
}
