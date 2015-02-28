using UnityEngine;
using System.Collections;

public class NetworkMenu : MonoBehaviour {

	private string connectionIP = "52.10.157.157";
	public string gameName = "";
	private string gameType = "Settlers of Catan";
	private int masterServerPortNumber = 23466;
	private int facilitatorPortNumber = 50005;

	private bool connected = false;

	private void OnConnectedToServer()
	{
		Debugger.Log ("Network", "Connected To Server");
		connected = true;
	}

	private void OnPlayerConnected(NetworkPlayer player) 
	{
		Debugger.Log ("Network", "Player Connected");		
	}

	private void OnServerInitialized (NetworkPlayer player)
	{
		connected = true;
		MasterServer.RegisterHost(gameType, gameName);
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
//		else
//			//handle other cases if neccessary
	}

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

			if (GUILayout.Button ("Host")) {
				//ASSUMPTION: GameName is not empty
				NetworkConnectionError error = Network.InitializeServer(1000, 6832 ,true);
			}
			if (GUILayout.Button("Connect")) {
				MasterServer.RequestHostList(gameType);
			}
		} else {
			GUILayout.Label("Connections: " + Network.connections.Length.ToString());
		}
	}
}
