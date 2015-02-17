#pragma warning disable 0219
#pragma warning disable 0414

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


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

	public float cardWidth = 40f;

//	public GameObject handObj;

	List<ResourceType> resources;
	List<DevCardType> devCard;
	
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

	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log(thisPlayer.playerName);
	}

	public void SetPlayer(Player p){
		thisPlayer = p;
	}

	/// <summary>
	/// Adds the card, and animates the card
	/// </summary>
	public void AddResourceCard(){

	}

	/// <summary>
	///  Removes the card. animates teh card
	/// </summary>
	public void RemoveResourceCard(){

	}

	private void AddCardPrefab(GameObject pf){
		GameObject child = Instantiate(pf, Vector3.zero, Quaternion.identity) as GameObject;
		child.transform.SetParent(this.transform);

		Image card = child.GetComponent<Image>();

		// Set the size
		card.rectTransform.sizeDelta = new Vector2(60, 60);

		int numCards = gameObject.transform.childCount;
		float cardWidth = card.rectTransform.sizeDelta.x;
//		int width = 

//		Debug.Log(numCards);

		// Set the position
		card.rectTransform.anchoredPosition = new Vector2(cardWidth/2 + (numCards-1) * cardWidth ,0);
	}

	/// <summary>
	/// Updates the hand to contain all the right cards for the player
	/// 
	/// </summary>
	public void UpdateHand(){
		Player p2 = TurnState.currentPlayer;

		SetPlayer(p2);
		ClearHand();

		foreach(DevCardType d in p2.GetDevCardArray()){
			AddCardPrefab(  GetDevCardPrefab(d) );
		}

		foreach(ResourceType r in p2.GetResourceArray() ){
			AddCardPrefab( GetResourceCardPrefab(r));
		}
	}

	void ClearHand(){
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
