using UnityEngine;
using System.Collections;

public enum DevCardType { knight, roadBuilding, yearOfPlenty, monopoly, victoryPoint};

public class DevCard : MonoBehaviour {

	// chance out of 100 for [knight, road, year, monopoly]
	static int[] cardChances;
	static DevCardType knight = DevCardType.knight;
	static DevCardType roadBuilding = DevCardType.roadBuilding;
	static DevCardType yearOfPlenty = DevCardType.yearOfPlenty;
	static DevCardType monopoly = DevCardType.monopoly;
	static DevCardType victoryPoint = DevCardType.victoryPoint;

	static DevCardType[] devCardsByChance = new DevCardType[] {knight, roadBuilding, yearOfPlenty, monopoly, victoryPoint}

	// Todo Gives a random card
	public static DevCardType RandomCard(){
		return devCardsByChance[ Random.Range(0, devCardsByChance.Length)];
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
