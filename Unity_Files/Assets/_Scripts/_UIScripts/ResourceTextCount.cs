using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourceTextCount : MonoBehaviour {


	public ResourceType type; // should be set in inspector
	public Player player;
	Text txt;


	// Use this for initialization
	void Start () {
			txt = gameObject.GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
		player = TurnState.currentPlayer;

		txt.text = player.GetResourceCount(type).ToString();
	}
}
