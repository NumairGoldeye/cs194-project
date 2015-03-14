using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// Game lobby. 
/// 
/// Controls all the UI elements that have to do with specific things about 
/// creating and joining games
/// 
/// 
/// 
/// 
/// </summary>
public class GameLobby : MonoBehaviour {

	// Set in inspector
	public NetworkMenu network; 
	public InputField gameNameInput;
	public InputField hostNameInput;
	public Button hostGameBtn; 

	// Not inspector vals


	// Use this for initialization
	void Start () {
		Debugger.Log("GameLobby", "start");
		network.gameLobby = this;


	}
	
	// Update is called once per frame
	void Update () {
		string gameName = gameNameInput.text;
		string hostName = hostNameInput.text;

		hostGameBtn.interactable = !(gameName.Equals("") || hostName.Equals(""));
	}

	public void HostGame(){
		string gameName = gameNameInput.text;
		string hostName = hostNameInput.text;

		network.InitializeGame(gameName, hostName);



		Debugger.Log ("GameLobby", "hosted");
	}



}
