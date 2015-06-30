using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameView : MonoBehaviour, IGameModelListener {
	private GameModel gameModel;
	public Dictionary<string, GameObject> viewObjects = new Dictionary<string, GameObject>();

	void Start () {	
	}

	void Update () {
	}

	#region IGameModelListener implementation
	public void ObjectCreated(string uid) {
		//throw new System.NotImplementedException ();
	}
	public void ObjectDestroyed(string uid) {
		//throw new System.NotImplementedException ();
	}
	public void PropertyChanged(string uid, Property property) {
		if(property.key == "viewPrefab") {
			CreateViewObject(uid, property.valueStr);
		} else {
			Debug.Log ("GameView: Ignoring property " + property.key);
		}
	}
	#endregion

	public void SetGameModel(GameModel _gameModel) {
		gameModel = _gameModel;
		gameModel.RegisterListener(this);
	}

	void CreateViewObject(string uid, string prefab) {
		GameObject viewPrefab = Resources.Load(prefab) as GameObject;
		GameObject instance = (GameObject) Instantiate(viewPrefab, Vector3.zero, Quaternion.identity);
		instance.transform.parent = transform;
		viewObjects[uid] = instance;
	}
}
