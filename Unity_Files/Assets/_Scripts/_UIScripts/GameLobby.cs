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
	public NetworkMenu networkMenu;

	public GameObject playerListPanel;
	public GameObject playerNameTextPrefab;
	public Button startGameBtn; 

	// Not inspector vals
	int lastCount = 0;

	// Use this for initialization
	void Start () {
		Debugger.Log("GameLobby", "start");

		startGameBtn.onClick.AddListener(StartGame);
		networkMenu.gameLobby = this;

		ClearPlayerList();
	}
	
	// Update is called once per frame
	void Update () {
		startGameBtn.interactable = !GameManager.Instance.gameStarted && Network.isServer;


		int currentCount = GameManager.Instance.players.Count;

		if (currentCount > 0 && lastCount != currentCount){
			lastCount = currentCount;
			UpdatePlayerList();
		}


//		if (Input.GetKeyDown("space")){
//			Debugger.Log("GameLobby", currentCount.ToString());
//		}
	}

	public void StartGame(){
		networkMenu.GameStart();
	}
	
	public void UpdatePlayerList(){
//		Debugger.Log("GameLobby", "update player list");
		Player p = GameManager.Instance.myPlayer;
//		networkView.RPC("syncPlayerInfo", RPCMode.Others, p.networkPlayer, p.playerId, p.playerName);

		ClearPlayerList();
		PopulatePlayerList();
	}

	void PopulatePlayerList(){

//		AddPlayerListName("Host: " + GameManager.Instance.myPlayerName);

		foreach(Player p in GameManager.Instance.players){
//			Debug.Log(p.playerName);

			// Is the first player the host always?

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
