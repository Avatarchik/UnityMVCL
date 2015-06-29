using UnityEngine;

using System;
using System.Collections;

public static class SimpleGame {
	private static GameModel model;

	public static void SetGameModel(GameModel _model) {
		model = _model;
	}

	public static void StartGame() {
		string uid = System.Guid.NewGuid().ToString();
		model.AddObject(uid, new string[] {"SimpleGameObject"});
		Property<string, int> p = new Property<string, int>("foobar", 5);
		//model.AddProperty(uid, new PropertyAttribute	
		//model.AddObject(uid, new string[] {"SimpleGameObject"});
	}
}
