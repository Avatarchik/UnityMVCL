using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SimpleCamera {
	
	public static void Update(string uid) {
		Property targetProp = LogicScript.gameModel.GetProperty(uid, "target");
		GameObject target = LogicScript.gameView.viewObjects[targetProp.valueStr];

		//Debug.Log ("SimpleCamera.Update target - " + targetProp.valueStr);

		Property offsetProp = LogicScript.gameModel.GetProperty(uid, "offset");
		Vector3 offset = offsetProp.valueVec3;
		//Debug.Log ("SimpleCamera.Update offset - " + offsetProp.valueVec3);

		GameObject camera = LogicScript.gameView.viewObjects[uid];

		Vector3 targetPosition = target.transform.TransformPoint(offset);
		Vector3 velocity = Vector3.zero;
		camera.transform.position = Vector3.SmoothDamp(camera.transform.position, targetPosition, ref velocity, 0.2f);

		//GameObject viewInstance instanceProp.valueGameObject;
		//viewInstance.transform.position = 

		/*
		var enumerator = props.GetEnumerator();
		while (enumerator.MoveNext()) {
			var element = enumerator.Current;
			Debug.Log ("SimpleCamera.Property: " + element.Key);
		}

		*/
	}
}
