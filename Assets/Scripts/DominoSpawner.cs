﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DominoSpawner : NetworkBehaviour {

	[SerializeField] Camera cameraObject;
	[SerializeField] GameObject dominoPrefab;
	[SerializeField] Vector3 gravityDirection = Vector3.down;
	[SerializeField] LayerMask spawnTargets;
	[SerializeField] LayerMask dominoTargets;

	Quaternion currentRotation = Quaternion.identity;

	void Update () {
		if (!isLocalPlayer)
			return;
		if (Input.GetMouseButtonUp(0)){
			TrySpawnDomino();
		}
	}

	void TrySpawnDomino(){
		RaycastHit hit;
		if(Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit, 200, spawnTargets.value)){
			if (hit.normal == gravityDirection * -1){
				CmdSpawnDomino(hit.point);
			}
		}
	}

	[Command]
	void CmdSpawnDomino(Vector3 point){
		GameObject obj = Instantiate(dominoPrefab, point, currentRotation);
		NetworkServer.Spawn(obj);
	}
}