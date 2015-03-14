using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// Create game UI, just create the two input fields that lets somebody host a game
/// </summary>
public class CreateGameUI : MonoBehaviour {

	// Set in inspector
	public NetworkMenu networkMenu; 
	public InputField gameNameInput;
	public InputField hostNameInput;
	public Button hostGameBtn; 

	public GameObject gameLobby;

	void Start() {
		hostGameBtn.onClick.AddListener(HostGame);
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
		
		networkMenu.InitializeGame(gameName, hostName);

		SwitchToLobbyView();
	}

	void SwitchToLobbyView(){
		gameObject.SetActive(false);
		gameLobby.SetActive(true);
	}
}
