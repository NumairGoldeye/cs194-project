using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckBuyButton : MonoBehaviour {

	public BuyableType buyType; // should be set in inspector
	public Player player;
	Button btn; 

	// Use this for initialization
	void Start () {
		btn = gameObject.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
		player = TurnState.currentPlayer;
		btn.interactable = BuyManager.PlayerCanBuy(player, buyType);
	}
}

