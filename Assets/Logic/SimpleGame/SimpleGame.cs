using UnityEngine;

using System;
using System.Collections;

public static class SimpleGame {
	private static GameModel model;

	public static void SetGameModel(GameModel _model) {
		model = _model;
	}

	public static void StartGame() {
		CreateGameWorld();
		string playerUID = CreatePlayer();
		string camerUID = CreateCamera();

		Property p = new Property("target", playerUID);
		model.SetProperty(camerUID, p);
	}

	static string CreateGameWorld() {
		string uid = System.Guid.NewGuid().ToString();
		model.AddObject(uid, null);
		model.SetProperty(uid, new Property("viewPrefab", "SimpleGame/SimpleGameWorld"));
		return uid;
	}

	static string CreatePlayer() {
		string uid = System.Guid.NewGuid().ToString();
		string[] scriptChain = new string[] { "SimplePlayer" };
		model.AddObject(uid, scriptChain);
		model.SetProperty(uid, new Property("viewPrefab", "SimpleGame/SimplePlayer"));
		return uid;
	}

	static string CreateCamera() {
		string uid = System.Guid.NewGuid().ToString();
		string[] scriptChain = new string[] { "SimpleCamera" };
		model.AddObject(uid, scriptChain);

		model.SetProperty(uid, new Property("viewPrefab", "SimpleGame/SimpleCamera"));
		model.SetProperty(uid, new Property("offset", new Vector3(0, 2, -5)));
		return uid;
	}
}