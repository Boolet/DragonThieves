using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Domino : NetworkBehaviour, Resetable {

	[SerializeField] Transform positionOffset;

	DominoTracker tracker;

	Rigidbody dominoBody;
	Quaternion spawnRotation;
	Vector3 spawnPoint;

	void Awake(){
		tracker = FindObjectOfType<DominoTracker>();
		if(positionOffset != null)
			MoveToOffset();
		spawnPoint = transform.position;
		spawnRotation = transform.rotation;
		dominoBody = GetComponent<Rigidbody>();
		CmdSubscribe();
	}

	void MoveToOffset(){
		transform.position = positionOffset.position;
	}

	void CmdSubscribe(){
		FindObjectOfType<DominoTracker>().Subscribe(this);
	}

	public void CmdReset(){
		dominoBody.velocity = Vector3.zero;
		dominoBody.angularVelocity = Vector3.zero;
		transform.position = spawnPoint;
		transform.rotation = spawnRotation;
	}
}
