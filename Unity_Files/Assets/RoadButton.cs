using UnityEngine;
using System.Collections;

public class RoadButton : MonoBehaviour {

	void toggleChildren() {
		BroadcastMessage ("toggleRoad");
	}
}
