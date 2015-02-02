using UnityEngine;
using System.Collections;

public class CityClass : MonoBehaviour {
	
	public int ownerId;
	public bool built;
	// Use this for initialization

	void Start () {
		built = false;
		makeInvisible();
		ownerId = -1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int getPlayer(){
		return ownerId;
	}

	public bool isBuilt() {
		return built;
	}
	void makeInvisible() {
		Color temp = renderer.material.color;
		temp.a = 0;
		renderer.material.color = temp;		
	}
	
	void makeVisible() {
		Color temp = renderer.material.color;
		temp.a = 1f;
		renderer.material.color = temp;
		built = true;
		ownerId = TurnState.currentPlayer.playerId;
	}
}
