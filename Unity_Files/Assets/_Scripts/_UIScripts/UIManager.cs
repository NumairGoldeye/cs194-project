using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// This script starts up the UI and works with the TurnState manager in
// order to make sure the UI does what it wants

// SHOULD BE EXECUTED BEFORE TURNMANAGER!!!!


public enum MajorUIState {create, start, play};

public class UIManager : MonoBehaviour {

	public GameObject[] disableOnStart = new GameObject[8];
	public GameObject[] enableOnStart = new GameObject[8];
	public static GameObject CreateUI; // Creating the games...
	public static GameObject StartUI; // GameStart UI - first cities and roads etc.
	public static GameObject MainUI; // Gameplay UI -
	public static MajorUIState state = MajorUIState.start;

	public static Dictionary<MajorUIState, GameObject> stateUIObjectMap;

	
	// Singleton things that will be passed onto Static objects for ease of use

	public static UIManager instance = null;
	public MajorUIState instanceState = MajorUIState.start; // 
	public GameObject CreateUIInstance; // Creating the games...
	public GameObject StartUIInstance; // GameStart UI - first cities and roads etc.
	public GameObject MainUIInstance; // Gameplay UI - 
	public bool setup = false;

	/// <summary>
	/// Setups the user interface map.
	/// </summary>
	static void SetupUIMap(){
		CreateUI = instance.CreateUIInstance;
		StartUI = instance.StartUIInstance;
		MainUI = instance.MainUIInstance;

		stateUIObjectMap = new Dictionary<MajorUIState, GameObject >();

		// So much easier to do with Javascript objects. Sigh
		stateUIObjectMap.Add(MajorUIState.create, CreateUI);
		stateUIObjectMap.Add(MajorUIState.start, StartUI);
		stateUIObjectMap.Add(MajorUIState.play, MainUI);
	}

	// Hides the non-current Major UI game objects that aren't current
	/// <summary>
	/// Updates the major UI
	/// </summary>
	public static void UpdateMajorUI(){
		GameObject uiObject = null;
		foreach( MajorUIState s in Enum.GetValues(typeof(MajorUIState))){
			if (stateUIObjectMap.TryGetValue(s, out uiObject) ){
				if (s != state){
					uiObject.SetActive(false);
				} else {
					uiObject.SetActive(true);
				}
			} else {
				Debugger.Log ("UIMANAGER", "WHAT HAVE YOU DONE");
			}
		}
	}


	/// <summary>
	/// Changes the state of the major user interface.
	/// </summary>
	/// <param name="s">S.</param>
	public static void ChangeMajorUIState(MajorUIState s){
		state = s;
		UpdateMajorUI();
	}
	

	void Awake(){
		UIManager.instance = this;

	}


	// Use this for initialization
	void Start () {
		// Singleton to static translation... sigh
		// Needs the gameobject to do stuff
		UIManager.state = instanceState;
		UIManager.SetupUIMap();
		UIManager.DisableObjs();
		UIManager.EnableObjs();
		UIManager.UpdateMajorUI();


	}
	
	// Update is called once per frame
	void Update () {
		if ( !setup ){
			setup = true;
			if (state == MajorUIState.start) 
				StartGameManager.Startup();
		}
	}


	// These wrapper functions allow buttons to call Static functions. Sigh - unity.
	public void SGMStartupWrapper(){
		StartGameManager.Startup();
	}
	public void SGMNextPhaseWrapper(){
		StartGameManager.NextPhase();
	}

	public void TSMStartMainGameplay(){
		TurnState.Startup();
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

	public static void StaticReset(){

	}

}
