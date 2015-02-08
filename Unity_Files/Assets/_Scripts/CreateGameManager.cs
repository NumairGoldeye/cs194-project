using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// CreateGameManager controls everything from the startup screen - "Create/Join Game"
/// up until actually starting the game. 
/// 
/// Like TurnState.cs this will have its own public enum that will control gamestates?
/// 
/// This will control the UI
/// 
/// The pipeline will be - CreateGameManager - StartGameManager - TurnStateManager
/// 
/// 
/// </summary>

public class CreateGameManager : MonoBehaviour {
	
	public static List<PlayerGame> createdGames;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
