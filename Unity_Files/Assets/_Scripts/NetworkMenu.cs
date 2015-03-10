using UnityEngine;
using System.Collections;

public class NetworkMenu : MonoBehaviour {

	private string connectionIP = "52.10.157.157";
	public string gameName = "";
	private string gameType = "Settlers of Catan";
	private int masterServerPortNumber = 23466;
	private int facilitatorPortNumber = 50005;
	
	private bool connected = false;

	//Client Initializes their GameManager
	private void OnConnectedToServer()
	{
		Debugger.Log ("Network", "Connected To Server");
		connected = true;
	}

	private void OnPlayerConnected(NetworkPlayer player) 
	{
		Debugger.Log ("Network", "Player Connected");
		Player p = GameManager.Instance.createPlayer (player, "");
//		Debugger.Log ("Network", p.playerId.ToString ());
		//Respond to player with it's information
		GameManager.Instance.respondToPlayerJoin (player, p.playerId);
	}

	private void OnServerInitialized ()
	{
		connected = true;
		MasterServer.RegisterHost(gameType, gameName);
		GameManager.Instance.createPlayer (Network.player, GameManager.Instance.myPlayerName);
	}

	private void OnDisconnectedFromServer()
	{
		connected = false;
	}

	private void OnFailedToConnect(NetworkConnectionError error)
	{
		//Error occured
	}

	private void OnMasterServerEvent(MasterServerEvent message)
	{
		if (message == MasterServerEvent.HostListReceived)
			processHostList();

	}

	private void OnPlayerDisconnected(NetworkPlayer player)
	{
		GameManager.Instance.networkView.RPC ("removePlayer", RPCMode.All, player);
	}

	//TODO: Make a display of all of the available games
	private void processHostList()
	{
		HostData[] hostData = MasterServer.PollHostList ();
		for (int i = 0; i < hostData.Length; i++) {
			//Display all of the names and make them clickable
			//For now, we will simply connect to the game whose name is "test"

			if (hostData[i].gameName.Equals("test")) {
				Debugger.Log ("Network", "Connecting to 'test' game");
				Network.Connect(hostData[i]);
			}
		}
	}

	private void OnGUI()
	{
		Network.natFacilitatorIP = connectionIP;
		Network.natFacilitatorPort = facilitatorPortNumber;
		MasterServer.ipAddress = connectionIP;
		MasterServer.port = masterServerPortNumber;
		if (!connected) {
			//We should rework this GUI so that there are two buttons: Host & Connect
			// Clicking host will prompt a host name, and connect will display a list of hosted games
			GUILayout.Label("Game Name");
			gameName = GUILayout.TextField(gameName);
			GUILayout.Label("Player Name");
			GameManager.Instance.myPlayerName = GUILayout.TextField(GameManager.Instance.myPlayerName);

			if (GUILayout.Button ("Host")) {
				//TODO Notify user of error
				if (gameName.Equals("") || GameManager.Instance.myPlayerName.Equals("")) return;
				NetworkConnectionError error = Network.InitializeServer(1000, 6832 ,true);
			}
			if (GUILayout.Button("Connect")) {
				//TODO Notify user of error
				if (GameManager.Instance.myPlayerName.Equals("")) return;
				MasterServer.RequestHostList(gameType);
			}
		} else if (!GameManager.Instance.gameStarted) { 
			if (Network.isServer) {
				GUILayout.Label("Running as a server");
				if (GUILayout.Button("Start")) {
//					int startingPlayerID = Random.Range(0, GameManager.Instance.players.Count);
					int startingPlayerID = 0;
					GameManager.Instance.networkView.RPC("syncCurrentPlayer", RPCMode.All, startingPlayerID);
					GameManager.Instance.createTiles();
					GameManager.Instance.syncStartStateWithClients();
					GameManager.Instance.gameStarted = true;

					GameManager.Instance.graph.getSettlement(4).buildSettlement();
					GameManager.Instance.myPlayer.AddResource(ResourceType.Brick, 1);
					GameManager.Instance.myPlayer.AddResource(ResourceType.Sheep, 1);
					GameManager.Instance.myPlayer.AddResource(ResourceType.Wheat, 1);
					GameManager.Instance.myPlayer.AddResource(ResourceType.Wood, 1);
					Debugger.Log("PlayerHand", "Player has: " + GameManager.Instance.myPlayer.totalResources.ToString() + " resources");
//					Debugger.Log ("PlayerHand", "Players in game: " + GameManager.Instance.players.Count.ToString ());
				}
			}
			GUILayout.Label("Name: " + GameManager.Instance.players.Find(x => x.playerId == GameManager.Instance.myPlayer.playerId).playerName);
			GUILayout.Label("Connections: " + Network.connections.Length.ToString());
		} else if (GameManager.Instance.gameStarted) {
			Player player = GameManager.Instance.players.Find(x => x.playerId == GameManager.Instance.myPlayer.playerId);
			GUILayout.Label("Name: " + player.playerName + ", ID: " + player.playerId);
		}
	}
}
