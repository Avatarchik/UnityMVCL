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
	private GameView gameView;

	void Update() {
		for(int i = 0; i < gameModel.gameModelObjectsList.Count; i++) {
			GameModelObject gameModelObject = gameModel.gameModelObjectsList[i];
			CallScriptChain(gameModelObject, "Update");
		}
	}

	void CallScriptChain(GameModelObject gameModelObject, string functionName) {
		if(gameModelObject.scriptChain == null)
			return;

		for(int i = 0; i < gameModelObject.scriptChain.Length; i++) {
			CallScript(gameModelObject.uid, gameModelObject.scriptChain[i], functionName);
		}
	}

	void CallScript(string uid, string script, string functionName) {
		Type t = Type.GetType(script);
		if(t == null)
			throw new System.Exception("Script: " + script + " could not be loaded.");

		MethodInfo updateMethod = t.GetMethod(functionName, BindingFlags.Static | BindingFlags.Public);
		updateMethod.Invoke(null, new object[]{uid});
	}

	public void SetupServer(GameModel _gameModel, GameView _gameView, string gameRootClass) {
		gameModel = _gameModel;
		gameView = _gameView;
		LogicScript.gameModel = _gameModel;
		LogicScript.gameView = _gameView;

		GameObject gameServerObj = ObjectFactory.SpawnLocal(gameServerPrefab, Vector3.zero, Quaternion.identity);
		gameServer = gameServerObj.GetComponent<GameServer>();
		gameServer.SetGameModel(gameModel);
	}

	public void SetupClsient(GameModel _gameModel, GameView _gameView, string gameRootClass) {
		gameModel = _gameModel;
		gameView = _gameView;
		LogicScript.gameModel = _gameModel;
		LogicScript.gameView = _gameView;

		GameObject gameClientObj = ObjectFactory.SpawnLocal(gameClientPrefab, Vector3.zero, Quaternion.identity);
		gameClient = gameClientObj.GetComponent<GameClient>();
		gameClient.SetGameModel(gameModel);
	}

	public void SetupLocalGame(GameModel _gameModel, GameView _gameView, string gameRootClass) {
		gameModel = _gameModel;
		gameView = _gameView;
		LogicScript.gameModel = _gameModel;
		LogicScript.gameView = _gameView;

		Type t = Type.GetType(gameRootClass);

		System.Object[] setGameModelArgs = new object[]{ gameModel };
		MethodInfo setGameModelMethod = t.GetMethod("SetGameModel", BindingFlags.Static | BindingFlags.Public);
		setGameModelMethod.Invoke(null, setGameModelArgs);

		MethodInfo startGameMethod = t.GetMethod("StartGame", BindingFlags.Static | BindingFlags.Public);
		startGameMethod.Invoke(null, null);
	}
}
