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

	//player calls reset - call reset on every player

	//on player join, existing dominoes need to subscribe for that player

	void Awake(){
		tracker = FindObjectOfType<DominoTracker>();
		if(positionOffset != null)
			MoveToOffset();
		spawnPoint = transform.position;
		spawnRotation = transform.rotation;
		dominoBody = GetComponent<Rigidbody>();
		CmdSubscribe();
	}

	void OnStartClient(){
		CmdSubscribe();
	}

	void MoveToOffset(){
		transform.position = positionOffset.position;
	}

	void CmdSubscribe(){
		FindObjectOfType<DominoTracker>().Subscribe(this);
	}

	void OnDestroy(){
		FindObjectOfType<DominoTracker>().Unsubscribe(this);
	}

	public void CmdReset(){
		dominoBody.velocity = Vector3.zero;
		dominoBody.angularVelocity = Vector3.zero;
		transform.position = spawnPoint;
		transform.rotation = spawnRotation;
	}
}
