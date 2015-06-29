using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameClient : MonoBehaviour {
	private GameModel gameModel;
	private NetworkClient myClient;
	
	void Start() {
		myClient = new NetworkClient();
	}

	public void Connect(string hostIP, int hostPort) {
		myClient.Connect(hostIP, hostPort);
	}

	public void SetGameModel(GameModel _gameData) {
		gameModel = _gameData;
	}
}
