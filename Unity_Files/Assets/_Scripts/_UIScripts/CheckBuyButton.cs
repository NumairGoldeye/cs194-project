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
		btn.onClick.AddListener(Purchase);
	}
	
	// Update is called once per frame
	void Update () {
		player = TurnState.currentPlayer;
		btn.interactable = BuyManager.PlayerCanBuy(player, buyType);
	}

	void Purchase(){
		// The purchase should actulaly only happen after the thing is buit
		player = TurnState.currentPlayer;
		BuyManager.PurchaseForPlayer(buyType, player);
	}
}

