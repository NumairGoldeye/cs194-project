using UnityEngine;
using System.Collections;

public class RoadClass : MonoBehaviour {

	private bool built;
	private bool visible;
	public GameObject roads;

	public int ownerId;

	// Use this for initialization
	void Start () {
		built = false;
		makeInvisible();
		ownerId = -1;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool isBuilt() {
		return built;
	}

	public void toggleRoad() {
		if (!built) {
			if (!visible)
				makeVisible();
			else
				makeInvisible();
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
		if (!visible || built) return;
		built = true;
		Color temp = renderer.material.color;
		temp.a = 1;
		renderer.material.color = temp;
		roads.BroadcastMessage ("toggleRoad");
//		gameObject.SendMessageUpwards ("toggleChildren");
	}
}
