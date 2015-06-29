using UnityEngine;
using UnityEngine.Networking;

using System.Collections;
using System.Collections.Generic;

public struct GameModelObject {
	public string uid;
	public string[] scriptChain;
};

public class Property<T, K>
{
	T key;
	K value;
	
	public Property(T _key, K _value) {
		this.key = _key;
		this.value = _value;
	}

	public K Get() {
		return this.value;
	}
}

public interface IGameModelListener
{
	void ObjectCreated(string uid);
	void ObjectDestroyed(string uid);
	void PropertyChanged(string uid, object p);
}

public class GameModel : NetworkBehaviour {
	public GameObject playerPrefab;
	public GameObject playerCameraPrefab;

	public List<GameModelObject> gameModelObjectsList = new List<GameModelObject>();
	public Dictionary<string, List<object>> gameModelObjectsHash = new Dictionary<string, List<object>>();

	private List<IGameModelListener> listeners = new List<IGameModelListener>(); 

	public void RegisterListener(IGameModelListener listener) {
		listeners.Add(listener);
	}

	public void AddObject(string id, string[] scriptChain) {
		AddObject(id, scriptChain, null);
	}

	public void AddObject(string uid, string[] scriptChain, List<object> initialState) {
		if(gameModelObjectsHash.ContainsKey(uid))
			throw new System.Exception("GameModel.AddObject called on existing uid " + uid);

		GameModelObject newGameModelObject = new GameModelObject();
		newGameModelObject.scriptChain = scriptChain;
		newGameModelObject.uid = uid;
		gameModelObjectsList.Add(newGameModelObject);
		gameModelObjectsHash[uid] = initialState;
		foreach(IGameModelListener listener in listeners) {
			listener.ObjectCreated(uid);
		}

		Debug.Log("Added GameModelObject: " + uid);
	}

	public void SetProperty(string uid, object property) {
		if(gameModelObjectsHash.ContainsKey(uid))
			throw new System.Exception("GameModel.SetProperty called on non-existing uid " + uid);
		List<object> properties = gameModelObjectsHash[uid];
		properties.Add(property);
	}

	void Start() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
