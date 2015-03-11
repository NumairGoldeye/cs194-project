using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// List of all active games, controls selecting game behavior etc.
/// </summary>
public class GameList : MonoBehaviour {


	private NetworkMenu network; // set in inspector from NetworkMenu gameobject
	public GameObject networkObj;

	// The little text tag for the name of a game
	public GameObject gameListNamePrefab; // Set in inspector

	// Children of these will be the game names
	public GameObject listPanel;

	bool updated = false;

	// Use this for initialization
	void Start () {

		network = networkObj.GetComponent<NetworkMenu>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!updated){
			UpdateGameList();
			updated = true;
		}
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
		HostData[] data = network.getHostData();
		for(int i = 0; i < data.Length; i ++){
			HostData hd = data[i];
			Debug.Log(hd.gameName);
		}
	}

	void UpdateGameList(){
		ClearGameList();
		PopulateGameList();
	}


	public void RefreshList(){
		updated = false;

		Debug.Log("refresh!");
	}


	// when the gamelistname is clicked
	public void ChooseGame(HostData data){

	}

	// when the join game is joined
	public void JoinGame(HostData data){

	}



}
