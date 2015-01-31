using UnityEngine;
using System.Collections;

public class RoadButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void toggleChildren() {
		BroadcastMessage ("toggleRoad");
	}
}
