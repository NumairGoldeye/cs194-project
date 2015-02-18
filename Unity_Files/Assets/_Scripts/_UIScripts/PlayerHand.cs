#pragma warning disable 0219
#pragma warning disable 0414

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


/*

The entire should be updated on turnEnd


Receiving resources should animate and add cards
Spending resources shoudl animate and remove them

Receiving dev card should animate and add cards
Spending a dev card should animate and add Cards



 */

/// <summary>
/// The panel that controls the player's hand...
/// 
/// Controls the UI 
/// 
/// Should be assigned to the player via inspector
/// </summary>
public class PlayerHand : MonoBehaviour {

	// Set al of these in the inspector...
	public GameObject KnightCard;
	public GameObject RoadCard;
	public GameObject MonopolyCard;
	public GameObject YearCard;
	public GameObject VictoryCard;

	public GameObject WheatCard;
	public GameObject SheepCard;
	public GameObject OreCard;
	public GameObject BrickCard;
	public GameObject WoodCard;

//	public GameObject PopupPanel;
//	public GameObject PlayingDevCardPanel;

	public float cardWidth = 50f;
//	public float cardOverlap = 20f;
	public float cardPadding = 10f;
	public int numToOverlap = 6;
	private int totalCards = 0;

	// UI things so Devcards can access them
	public GameObject popupPanel;
	public GameObject resourceButtons;
	public GameObject playingDevCardPanel;

	// So i can tell which devcard to animate to death...
	private PlayerCard selectedCard;

	List<ResourceType> resources;
	List<DevCardType> devCard;
//	RectTransform rect;
	Player thisPlayer; 

	GameObject GetDevCardPrefab(DevCardType type){
		switch (type){
		case DevCardType.knight:
			return KnightCard;
		case DevCardType.monopoly:
			return MonopolyCard;
		case DevCardType.roadBuilding:
			return RoadCard;
		case DevCardType.yearOfPlenty:
			return YearCard;
		case DevCardType.victoryPoint:
			return VictoryCard;
		}
		return null;
	}

	GameObject GetResourceCardPrefab(ResourceType type){
		switch (type){
		case ResourceType.brick:
			return BrickCard;
		case ResourceType.ore:
			return OreCard;
		case ResourceType.sheep:
			return SheepCard;
		case ResourceType.wheat: 
			return WheatCard;
		case ResourceType.wood: 
			return WoodCard;
		}
		return null;
	}
	
	// Use this for initialization
	void Start () {
		resources = new List<ResourceType>();
		devCard = new List<DevCardType>();
//		rect = gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log(thisPlayer.playerName);
	}

	public void SetPlayer(Player p){
		thisPlayer = p;
	}

	public void SelectCard(PlayerCard card){
		selectedCard = card;
	}

	/// <summary>
	/// Adds the card, and animates the card
	/// </summary>
	public void AddResourceCard(ResourceType type, Player p, int amount = 1){
		SetPlayer(p);
		for(int i = 0; i < amount; i++){
			StartCoroutine(AddCardPrefab( GetResourceCardPrefab(type), true, false));
		}
		StartCoroutine(_AnimateInAll());
	}

	/// <summary>
	///  Removes the card. animates teh card
	/// </summary>
	public void RemoveResourceCard(ResourceType type, Player p){
		totalCards--;
	}

	/// <summary>
	/// Adds the singular dev card. Animates as well
	/// </summary>
	public void AddDevCard(DevCardType type, Player p){
		SetPlayer(p);
		StartCoroutine(AddCardPrefab( GetDevCardPrefab(type), true, false));
		
		StartCoroutine(_AnimateInAll());
	}

	private IEnumerator _AnimateInAll(){
		yield return new WaitForEndOfFrame();
		RepositionCards();
		AnimateInAll();
	}

	/// <summary>
	/// Removes the dev card.
	/// </summary>
	public void RemoveDevCard(DevCardType type, Player p){
		SetPlayer(p);
		if (selectedCard){
			selectedCard.AnimOut();
		}
		totalCards--;
	}

	/// <summary>
	/// triggers the animateIn for all the cards. 
	/// 
	/// doesn't work on old cards...
	/// </summary>
	private void AnimateInAll(){
//		RepositionCards();
		foreach(Transform child in transform){
//			Debug.Log(child.name);
			child.GetComponent<PlayerCard>().AnimIn();
		}
	}

	public void RepositionCards(){
//		Debug.Log ("totalCards: " + totalCards);

		foreach(Transform child in transform){
//			Debug.Log(child.name);
			PositionCard(child.gameObject);
		}
	}

	private int IndexOfCard(GameObject card){
		for(int i = 0; i < transform.childCount; i++){
			if (transform.GetChild(i) == card.transform)
				return i;
		}
		return -1;
	}

	private IEnumerator AddCardPrefab(GameObject pf, bool isDev = true, bool animate = false){

//		Debug.Log (pf.transform.name + " added");

		GameObject child = Instantiate(pf, Vector3.zero, Quaternion.identity) as GameObject;
		child.transform.SetParent(this.transform);
		child.GetComponent<PlayerCard>().SetHand(this);
		totalCards++;
//		PositionCard(child);

//		Debug.Log (child.transform.name + " added2");

		if (animate){
			yield return new WaitForEndOfFrame();
//			Debug.Log ("addcard");
//			if (pf.animation)
			child.GetComponent<PlayerCard>().AnimIn();
		}


//		PositionCard(child);
	}


	private void PositionCard(GameObject cardObj){
		Image card = cardObj.GetComponent<Image>();
		// Set the size
		card.rectTransform.sizeDelta = new Vector2(cardWidth, cardWidth);
		int numCards = totalCards;
		int thisIndex = IndexOfCard(cardObj);
//		float panelWidth = rect.rect.width - cardWidth;
		float panelWidth = 349f;
		int numDeltas = numToOverlap;
		if (numCards > numToOverlap){
			numDeltas = numCards;
		}
		float xDelta = panelWidth/(float)numDeltas * thisIndex;
//		Debug.Log("width: " + panelWidth);
//		Debug.Log("numDeltas: " + numDeltas);
//		Debug.Log("thisIndex: " + thisIndex);
		// Set the position
		card.rectTransform.anchoredPosition = new Vector2(cardWidth/2 + cardPadding + xDelta ,0);
	}



	/// <summary>
	/// Updates the hand to contain all the right cards for the player
	/// 
	/// </summary>
	public void UpdateHand(){
		Player p2 = TurnState.currentPlayer;

		SetPlayer(p2);
		ClearHand();

		DevCardType[] devCards = p2.GetDevCardArray();
		ResourceType[] resourceCards = p2.GetResourceArray();

//		Debug.Log ("totalCards: " + totalCards);
//		totalCards += devCards.Length + resourceCards.Length;

//		Debug.Log ("totalCards: " + totalCards);

		foreach(DevCardType d in devCards){
			StartCoroutine( AddCardPrefab(  GetDevCardPrefab(d), true, true ));
		}

		foreach(ResourceType r in resourceCards ){
			StartCoroutine( AddCardPrefab( GetResourceCardPrefab(r), false, true) );
		}

		RepositionCards();
	}

	void ClearHand(){
		totalCards  = 0;

//		Debug.Log(transform.childCount);
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in transform) children.Add(child.gameObject);
		foreach ( GameObject child in children){
			child.transform.SetParent(null);
			GameObject.Destroy(child);
		}

//		Debug.Log(transform.childCount);
	}


}
