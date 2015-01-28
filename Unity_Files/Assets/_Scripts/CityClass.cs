using UnityEngine;
using System.Collections;

public class CityClass : MonoBehaviour {

	private bool visible;
	// Use this for initialization
	void Start () {
		makeInvisible();
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
