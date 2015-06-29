using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

public class GameLogic : MonoBehaviour {
	public GameObject gameServerPrefab;
	public GameObject gameClientPrefab;

	private GameServer gameServer;
	private GameClient gameClient;

	private GameModel gameModel;

	void Update() {
		for(int i = 0; i < gameModel.gameModelObjectsList.Count; i++) {
			GameModelObject gameModelObject = gameModel.gameModelObjectsList[i];
			CallScriptChain(gameModelObject);
		}
	}

	void CallScriptChain(GameModelObject gameModelObject) {
		for(int i = 0; i < gameModelObject.scriptChain.Length; i++) {
			CallScript(gameModelObject.scriptChain[i]);
		}
	}

	void CallScript(string script) {
		Type t = Type.GetType(script);
		MethodInfo updateMethod = t.GetMethod("Update", BindingFlags.Static | BindingFlags.Public);
		updateMethod.Invoke(null, null);
	}

	public void SetupServer(GameModel _gameModel, string gameRootClass) {
		gameModel = _gameModel;
		GameObject gameServerObj = ObjectFactory.SpawnLocal(gameServerPrefab, Vector3.zero, Quaternion.identity);
		gameServer = gameServerObj.GetComponent<GameServer>();
		gameServer.SetGameModel(gameModel);
	}

	public void SetupClient(GameModel _gameModel, string gameRootClass) {
		gameModel = _gameModel;
		GameObject gameClientObj = ObjectFactory.SpawnLocal(gameClientPrefab, Vector3.zero, Quaternion.identity);
		gameClient = gameClientObj.GetComponent<GameClient>();
		gameClient.SetGameModel(gameModel);
	}

	public void SetupLocalGame(GameModel _gameModel, string gameRootClass) {
		gameModel = _gameModel;
		Type t = Type.GetType(gameRootClass);

		System.Object[] setGameModelArgs = new object[]{ gameModel };
		MethodInfo setGameModelMethod = t.GetMethod("SetGameModel", BindingFlags.Static | BindingFlags.Public);
		setGameModelMethod.Invoke(null, setGameModelArgs);

		MethodInfo startGameMethod = t.GetMethod("StartGame", BindingFlags.Static | BindingFlags.Public);
		startGameMethod.Invoke(null, null);
	}
}
