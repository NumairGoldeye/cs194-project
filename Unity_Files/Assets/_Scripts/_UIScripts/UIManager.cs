using UnityEngine;
using System.Collections;

// This script starts up the UI and works with the TurnState manager in
// order to make sure the UI does what it wants


public class UIManager : MonoBehaviour {

	public GameObject[] disableOnStart = new GameObject[8];
	public GameObject[] enableOnStart = new GameObject[8];


	// The gameobject that houses all the UI
//	public GameObject MainUI;

	public static UIManager instance = null;

	void Awake(){
		UIManager.instance = this;
	}


	// Use this for initialization
	void Start () {
		UIManager.DisableObjs();
		UIManager.EnableObjs();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Roundabout much
	public static void DisableObjs(){
		foreach(GameObject obj in instance.disableOnStart){
			if (obj != null){
				obj.SetActive(false);
			}
		}
	}

	public static void EnableObjs(){
		foreach(GameObject obj in instance.enableOnStart){
			if (obj != null){
				obj.SetActive(true);
			}
		}
	}



	// Puts the MainUI thing in the right place
	void SetCanvas(){

	}

}
