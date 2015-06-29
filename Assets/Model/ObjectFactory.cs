using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public static class ObjectFactory {
	private static bool initialized = false;
	private static GameObject localPlayer;
	private static int tmpGroupId = 99;
	private static int enemyGroupId = 100;
	

	public static GameObject SpawnPlayer(GameObject playerPrefab, Vector3 pos, Quaternion rot) {
		if((Network.isClient || Network.isServer)) {
			localPlayer =  Network.Instantiate(playerPrefab, pos, rot, 0) as GameObject;
		} else {
			localPlayer = GameObject.Instantiate(playerPrefab, pos, rot) as GameObject;
		}
		return localPlayer;
	}

	public static GameObject SpawnEnemy(GameObject prefab, Vector3 pos, Quaternion rot) {
		if((Network.isClient || Network.isServer)) {
			return Network.Instantiate(prefab, pos, rot, enemyGroupId) as GameObject;
		} else {
			return GameObject.Instantiate(prefab, pos, rot) as GameObject;
		}
	}

	public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot) {
		if(Network.isClient)
			throw(new System.Exception("Can not Spawn object on client!"));


		GameObject ob = (GameObject) GameObject.Instantiate(prefab, pos, rot) as GameObject;
		NetworkServer.Spawn(ob);
		return ob;
	}

	public static GameObject SpawnLocal(GameObject prefab, Vector3 pos, Quaternion rot) {
		GameObject ob = GameObject.Instantiate(prefab, pos, rot) as GameObject;
		return ob;
	}

	public static GameObject GetPlayer() {
		return localPlayer;
	}

	public static void DestroyTemporary(GameObject ob) {
		Network.RemoveRPCs(ob.GetComponent<NetworkView>().viewID);
		Destroy(ob);
	}

	public static void DestroyEnemy(GameObject ob) {
		DestroyTemporary(ob);
	}

	public static void Destroy(GameObject ob) {
		if(ob == null)
			return;

		bool hasNetView = ob.GetComponent<NetworkView>();

	
		if(hasNetView && Network.isClient && ob.GetComponent<NetworkView>().isMine) {
			Network.Destroy(ob);
		} else if(hasNetView && Network.isServer) {
			Network.Destroy(ob);
		} else {
			GameObject.Destroy(ob);
		}
	}
}
