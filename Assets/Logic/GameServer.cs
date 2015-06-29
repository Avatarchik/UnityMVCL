using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameServer : MonoBehaviour {
	private GameModel gameModel;

	void Start() {
		DontDestroyOnLoad(transform.gameObject);
	}

	public void SetGameModel(GameModel _gameModel) {
		gameModel = _gameModel;
	}

	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("Player " + player.ToString() + " connected from " + player.ipAddress + ":" + player.port);
	}

	void OnServerInitialized() {
		Spawn(gameModel.playerPrefab, Vector3.zero, Quaternion.identity);
	}

	void Spawn(GameObject prefab, Vector3 pos, Quaternion rot) {
		GameObject ob = (GameObject) Network.Instantiate(prefab, pos, rot, 0);
		NetworkServer.Spawn(ob);
	}
}
