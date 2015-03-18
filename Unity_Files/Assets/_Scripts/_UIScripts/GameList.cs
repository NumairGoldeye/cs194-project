using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// List of all active games, controls selecting game behavior etc.
/// </summary>
public class GameList : MonoBehaviour {


	public NetworkMenu network; // set in inspector from NetworkMenu gameobject
	public GameObject gameLobby; //also inspector

	// The little text tag for the name of a game
	public GameObject gameListNamePrefab; // Set in inspector

	// Children of these will be the game names
	public GameObject listPanel;
	public InputField nameField; // set in inspector
	public Text nameFieldLabel;

	public HostData chosenGame;
	public Button joinGameButton; // set in inspector

	bool updated = false;

	// Use this for initialization
	void Start () {
		network.gameList = this;
		joinGameButton.onClick.AddListener(JoinGameListener);
	}
	
	// Update is called once per frame
	void Update () {
		if (!updated){
			UpdateGameList();
			updated = true;
		}

		joinGameButton.interactable = chosenGame != null;
	}

	void ClearGameList(){
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in listPanel.transform) children.Add(child.gameObject);
		foreach ( GameObject child in children){
			child.transform.SetParent(null);
			GameObject.Destroy(child);
		}
	}

	void PopulateGameList(){
//		Debugger.Log ("GameList", "try populate");

		if (!network.hostListAvailable) {
			network.getHostData();
			return;
		}

//		Debugger.Log ("GameList", "populated!");

		HostData[] data = network.getHostData();
		for(int i = 0; i < data.Length; i ++){
			HostData hd = data[i];
			Debug.Log(hd.gameName);

			AddGameListName(hd);
		}
	}

	void AddGameListName(HostData hd){
		GameObject child = Instantiate(gameListNamePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		child.transform.SetParent(listPanel.transform);
		child.GetComponent<GameListName>().Setup(hd, this);
	}

	void UpdateGameList(){
		ClearGameList();
		PopulateGameList();
	}


	public void RefreshList(){
		updated = false;

//		Debugger.Log("GameList", "refresh!");
	}


	// when the gamelistname is clicked
	public void ChooseGame(HostData data){
		chosenGame = data;
	}

	// when the join game is joined
	public void JoinGame(){
		// Check if they have selected a game and they have a player name
		if (nameField.text.Equals("")){
			nameFieldLabel.color = new Color (1f, 0f, 0f);
		}
		 
		if (chosenGame != null && !nameField.text.Equals("")) {
			GameManager.Instance.myPlayerName = nameField.text;
			network.Connect(chosenGame);
			SwitchToLobbyView();

		}
	}

	public void JoinGameListener(){
		Debugger.Log("GameList", "joinGame");
		JoinGame();
	}

	void SwitchToLobbyView(){
		gameObject.SetActive(false);
		gameLobby.SetActive(true);
	}
}
