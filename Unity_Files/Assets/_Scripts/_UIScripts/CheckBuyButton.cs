using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckBuyButton : MonoBehaviour {

	// should be set in inspector
	public BuyableType buyType; 
	public GameObject alertPanel;
	public Text alertPanelTxt;

	// don't need inspector
	public Player player;
	Button btn; 

	// Use this for initialization
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(ReactToPurchase);
	}
	
	// Update is called once per frame
	void Update () {
		player = TurnState.currentPlayer;
		btn.interactable = BuyManager.PlayerCanBuy(player, buyType);
	}

	// Most purchases happen after you click the game objects, but for dev cards it does something else
	void ReactToPurchase(){
		if (buyType == BuyableType.devCard){


			BuyManager.PurchaseForPlayer(buyType, TurnState.currentPlayer);
			DevCardType newCard = TurnState.currentPlayer.lastCardTypeDrawn;
			alertPanelTxt.text = "You have bought a " + DevCard.NameForCardType(newCard) + "\n" + DevCard.DescForCardType(newCard); 
			alertPanel.SetActive(true);
		}
	}


}

