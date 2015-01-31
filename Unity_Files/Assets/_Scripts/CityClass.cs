using UnityEngine;
using System.Collections;

public class CityClass : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		makeInvisible();
	}
	
	// Update is called once per frame
	void Update () {
		
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
	}
}
