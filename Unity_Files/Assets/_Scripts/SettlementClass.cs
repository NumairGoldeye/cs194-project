using UnityEngine;
using System.Collections;

public class SettlementClass : MonoBehaviour {

	private bool built;
	private bool visible;
	public GameObject settlements;
	// Use this for initialization
	void Start () {
		built = false;
		visible = false;
		makeInvisible();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isBuilt() {
		return built;
	}

	private void upgradeToCity() {

	}

	public void toggleSettlements() {
		if (!built) {
			if (!visible)
				makeVisible();
			else
				makeInvisible();
		} else {
			//Upgrade to city
			upgradeToCity();
		}
	}
	void makeInvisible() {
		visible = false;
		Color temp = renderer.material.color;
		temp.a = 0;
		renderer.material.color = temp;		
	}
	
	void makeVisible() {
		visible = true;
		Color temp = renderer.material.color;
		temp.a = 0.8f;
		renderer.material.color = temp;
	}
	
	void OnMouseDown() {
		built = true;
		Color temp = renderer.material.color;
		temp.a = 1;
		renderer.material.color = temp;
		settlements.BroadcastMessage ("toggleSettlements");
	}
}
