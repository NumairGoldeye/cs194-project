using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


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


	public GameObject playerListPanel;
	public GameObject playerNameTextPrefab;
	public Button startGameBtn; 

	// Not inspector vals


	// Use this for initialization
	void Start () {
		Debugger.Log("GameLobby", "start");
		network.gameLobby = this;

		hostGameBtn.onClick.AddListener(HostGame);
		startGameBtn.onClick.AddListener(StartGame);


		ClearPlayerList();
	}
	
	// Update is called once per frame
	void Update () {
		string gameName = gameNameInput.text;
		string hostName = hostNameInput.text;

		hostGameBtn.interactable = !(gameName.Equals("") || hostName.Equals(""));
		startGameBtn.interactable = !GameManager.Instance.gameStarted && Network.isServer;
	}

	public void HostGame(){
		string gameName = gameNameInput.text;
		string hostName = hostNameInput.text;

		network.InitializeGame(gameName, hostName);
	}

	public void StartGame(){
		network.GameStart();
	}
	
	public void UpdatePlayerList(){
		Debugger.Log("GameLobby", "update player list");

		ClearPlayerList();
		PopulatePlayerList();
	}

	void PopulatePlayerList(){
		foreach(Player p in GameManager.Instance.players){
			Debug.Log(p.playerName);
			AddPlayerListName(p.playerName);

		}
	}

	void AddPlayerListName(string playerName){
		GameObject child = Instantiate(playerNameTextPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		child.transform.SetParent(playerListPanel.transform);
		child.GetComponent<Text>().text = playerName;
	}

	void ClearPlayerList(){
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in playerListPanel.transform) children.Add(child.gameObject);
		foreach ( GameObject child in children){
			child.transform.SetParent(null);
			GameObject.Destroy(child);
		}
	}


}
