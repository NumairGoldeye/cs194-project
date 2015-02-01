using UnityEngine;
using System.Collections;

// This script starts up the UI and works with the TurnState manager in
// order to make sure the UI does what it wants


public class UIManager : MonoBehaviour {

	public GameObject[] disableOnStart = new GameObject[6];
	public GameObject[] enableOnStart = new GameObject[6];


	// The gameobject that houses all the UI
	public GameObject MainUI;

	// Use this for initialization
	void Start () {
		foreach(GameObject obj in disableOnStart){
			if (obj != null){
				obj.SetActive(false);
			}
		}
		foreach(GameObject obj in enableOnStart){
			if (obj != null){
				obj.SetActive(true);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Puts the MainUI thing in the right place
	void SetCanvas(){

	}

}
