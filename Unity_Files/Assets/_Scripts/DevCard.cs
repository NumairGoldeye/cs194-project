using UnityEngine;
using System.Collections;

public enum DevCardType { knight, roadBuilding, yearOfPlenty, monopoly};

public class DevCard : MonoBehaviour {

	// chance out of 100 for [knight, road, year, monopoly]
	static int[] cardChances;

	// Todo Gives a random card
	public static DevCardType DrawRandomCard(){
		return DevCardType.knight;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
