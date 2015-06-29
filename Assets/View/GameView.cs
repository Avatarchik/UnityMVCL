using UnityEngine;
using System.Collections;

public class GameView : MonoBehaviour, IGameModelListener {
	private GameModel gameModel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region IGameModelListener implementation
	public void ObjectCreated(string uid) {
		throw new System.NotImplementedException ();
	}
	public void ObjectDestroyed(string uid) {
		throw new System.NotImplementedException ();
	}
	public void PropertyChanged(string uid, object property) {
		throw new System.NotImplementedException ();
	}
	#endregion

	public void SetGameModel(GameModel _gameModel) {
		gameModel = _gameModel;

		gameModel.RegisterListener(this);
	}
}
