using UnityEngine;
using System.Collections;

public class RoadClass : MonoBehaviour {

	private bool built;
	private GameObject go;
	private GameManager gm;
	// Use this for initialization
	void Start () {
		go = GameObject.Find ("GameManager");
		gm = (GameManager)go.GetComponent (typeof(GameManager));
		built = false;
		renderer.enabled = false;
		Debug.Log ("Road state:" + built);
	}
	
	// Update is called once per frame
	void Update () {
		if (!built && gm.validBuild()) {
			renderer.enabled = true;
		} else {
			renderer.enabled = false;
		}
	}

	void OnMouseDown() {
		built = true;
		renderer.enabled = true;
	}
}
