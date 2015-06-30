using UnityEngine;
using UnityEngine.Networking;

using System.Collections;
using System.Collections.Generic;

public struct GameModelObject {
	public string uid;
	public string[] scriptChain;
};

public class Property {
	public string key;
	public object value;

	enum ValueType {
		INT,
		FLOAT,
		STRING,
		VECTOR3,
		GAME_OBJECT,
		OBJECT
	};

	private ValueType valueType = ValueType.OBJECT;

	public Property(string key, int value) {
		this.key = key;
		this.value = value;
		valueType = ValueType.INT;
	}

	public Property(string key, float value) {
		this.key = key;
		this.value = value;
		valueType = ValueType.FLOAT;
	}

	public Property(string key, string value) {
		this.key = key;
		this.value = value;
		valueType = ValueType.STRING;
	}

	public Property(string key, Vector3 value) {
		this.key = key;
		this.value = value;
		valueType = ValueType.VECTOR3;
	}

	public Property(string key, GameObject value) {
		this.key = key;
		this.value = value;
		valueType = ValueType.GAME_OBJECT;
	}

	public Property(string key, object value) {
		this.key = key;
		this.value = value;
		valueType = ValueType.OBJECT;
	}

	public int valueInt {
		get {
			if(valueType == ValueType.INT)
				return (int) value;
			throw new System.Exception("Property.valueInt called on non integer");
		}
	}

	public float valueFloat {
		get {
			if(valueType == ValueType.FLOAT)
				return (float) value;
			throw new System.Exception("Property.valueFLoat called on non float");
		}
	}

	public string valueStr {
		get {
			if(valueType == ValueType.STRING)
				return (string) value;
			throw new System.Exception("Property.valueStr called on non string");
		}
	}

	public Vector3 valueVec3 {
		get {
			if(valueType == ValueType.VECTOR3)
				return (Vector3) value;
			throw new System.Exception("Property.valueVec3 called on non Vector3");
		}
	}

	public GameObject valueGameObject {
		get {
			if(valueType == ValueType.GAME_OBJECT)
				return (GameObject) value;
			throw new System.Exception("Property.valueGameObject called on non GameObject");
		}
	}

	public object valueObj {
		get {
			if(valueType == ValueType.OBJECT)
				return value;
			throw new System.Exception("Property.valueObj called on non object");
		}
	}
}

public interface IGameModelListener
{
	void ObjectCreated(string uid);
	void ObjectDestroyed(string uid);
	void PropertyChanged(string uid, Property p);
}

public class GameModel : NetworkBehaviour {
	public GameObject playerPrefab;
	public GameObject playerCameraPrefab;

	public List<GameModelObject> gameModelObjectsList = new List<GameModelObject>();
	public Dictionary<string, Dictionary<string, Property>> gameModelObjectsProperties = new Dictionary
		<string, Dictionary<string, Property>>();

	private List<IGameModelListener> listeners = new List<IGameModelListener>(); 

	public void RegisterListener(IGameModelListener listener) {
		listeners.Add(listener);
	}

	public void AddObject(string id, string[] scriptChain) {
		AddObject(id, scriptChain, null);
	}

	public void AddObject(string uid, string[] scriptChain, Dictionary<string, Property> initialState) {
		if(gameModelObjectsProperties.ContainsKey(uid))
			throw new System.Exception("GameModel.AddObject called on existing uid " + uid);

		GameModelObject newGameModelObject = new GameModelObject();
		newGameModelObject.scriptChain = scriptChain;
		newGameModelObject.uid = uid;
		gameModelObjectsList.Add(newGameModelObject);
		if(initialState == null)
			gameModelObjectsProperties[uid] = new Dictionary<string, Property>();
		else
			gameModelObjectsProperties[uid] = initialState;
		foreach(IGameModelListener listener in listeners) {
			listener.ObjectCreated(uid);
		}
		Debug.Log("Added GameModelObject: " + uid);
	}

	public void SetProperty(string uid, Property property) {
		if(!gameModelObjectsProperties.ContainsKey(uid))
			throw new System.Exception("GameModel.SetProperty called on non-existing uid " + uid);
		Dictionary<string, Property> properties = gameModelObjectsProperties[uid];
		if(properties == null)
			properties = new Dictionary<string, Property>();
		properties[property.key] = property;
		foreach(IGameModelListener listener in listeners) {
			listener.PropertyChanged(uid, property);
		}
	}
	
	public Dictionary<string, Property> GetProperties(string uid) {
		if(!gameModelObjectsProperties.ContainsKey(uid))
			throw new System.Exception("GameModel.GetProperties called on non-existing uid " + uid);
		return gameModelObjectsProperties[uid];
	}

	public Property GetProperty(string uid, string key) {
		if(!gameModelObjectsProperties.ContainsKey(uid))
			throw new System.Exception("GameModel.GetProperty called on non-existing uid " + uid);

		Dictionary<string, Property> properties = gameModelObjectsProperties[uid];
		if(!properties.ContainsKey(key))
			throw new System.Exception("GameModel.GetProperty called on non-existing key " + uid);
		return properties[key];
	}

	void Start() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
