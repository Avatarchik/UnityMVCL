using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameSessionManager : MonoBehaviour {
	public string gameName = "";
	public string gameRootClass = "";
	public bool forceServer = false;
	public string masterServerIP = "127.0.0.1";
	public int masterServerPort = 23466;

	public int serverPort = 25001;

	public bool isServer;
	public bool isClient;

	public GameObject gameLogicPrefab;
	public GameObject gameModelPrefab;
	public GameObject gameViewPrefab;
	public GameObject gameControllerPrefab;

	private bool lookingForGame = false;
	private NetworkManager networkManager;
	
	void Start() {
		networkManager = GetComponent<NetworkManager>();

		MasterServer.ipAddress = masterServerIP;
		MasterServer.port = masterServerPort;

		StartLocalSession();

		/*
		bool startServer = HasCommandLineArg("-server");
		if(startServer || forceServer)
			StartServer();
		else
			LookForServer();
		*/
	}

	bool HasCommandLineArg(string queriedArg) {
		string[] args = System.Environment.GetCommandLineArgs();
		foreach(string arg in args) {
			if(arg == queriedArg)
				return true;
		}
		return false;
	}

	void StartServer() {
		/*
		if(!IsConnected()) {
			Network.InitializeServer(32, serverPort, false);
			MasterServer.RegisterHost("CryStone", "DefaultGame");
		}*/
		networkManager.StartServer();

		/*
		GameObject logic = ObjectFactory.SpawnLocal(gameLogicPrefab, transform.position, Quaternion.identity);
		GameObject view = ObjectFactory.SpawnLocal(gameViewPrefab, transform.position, Quaternion.identity);
		GameObject model = ObjectFactory.Spawn(gameModelPrefab, transform.position, Quaternion.identity);
		GameObject controller = ObjectFactory.SpawnLocal(gameControllerPrefab, transform.position, Quaternion.identity);
		
		logic.GetComponent<GameLogic>().SetupServer(model.GetComponent<GameModel>(), gameRootClass);
		*/
	}
	
	public void StartClient() {
		networkManager.StartClient();

		//GameObject view = ObjectFactory.SpawnLocal(gameViewPrefab, transform.position, Quaternion.identity);
		//GameObject model = ObjectFactory.Spawn(gameModelPrefab, transform.position, Quaternion.identity);
		//logic.GetComponent<GameLogic>().SetupClient(model.GetComponent<GameModel>(), gameRootClass);
	}

	public void StartLocalSession() {
		GameObject logic = ObjectFactory.SpawnLocal(gameLogicPrefab, transform.position, Quaternion.identity);
		GameObject view = ObjectFactory.SpawnLocal(gameViewPrefab, transform.position, Quaternion.identity);
		GameObject model = ObjectFactory.Spawn(gameModelPrefab, transform.position, Quaternion.identity);

		view.GetComponent<GameView>().SetGameModel(model.GetComponent<GameModel>());
		logic.GetComponent<GameLogic>().SetupLocalGame(
			model.GetComponent<GameModel>(),
			view.GetComponent<GameView>(),
			gameRootClass);
	}

	public bool IsConnected() {
		return (NetworkServer.active || NetworkClient.active);
	}

	void LookForServer() {
		if(IsConnected())
			return;
		MasterServer.RequestHostList(gameName);
		lookingForGame = true;
	}
	
	void Update() {
		if(!lookingForGame)
			return;
		FindAndJoinGame();
	}

	void FindAndJoinGame() {
		int i = 0;
		HostData[] hostDataList = MasterServer.PollHostList();

		while (i < hostDataList.Length) {
			HostData hostData = hostDataList[i];
			Debug.Log("Found game " + hostData.gameName);
			if(hostData.connectedPlayers < hostData.playerLimit) {
				JoinServer(hostData.gameName, hostData.ip, hostData.port);
				return;
			}
			i++;
		}
	}

	void JoinServer(string gameName, string[] ip, int port) {
		Debug.Log("Joining name: " + gameName);
		ObjectFactory.SpawnLocal(gameViewPrefab, transform.position, Quaternion.identity);
		ObjectFactory.SpawnLocal(gameControllerPrefab, transform.position, Quaternion.identity);
		lookingForGame = false;
		Network.Connect(ip, port);
	}
}
