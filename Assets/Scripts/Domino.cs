using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domino : MonoBehaviour, Resetable {

	[SerializeField] Transform positionOffset;

	DominoTracker tracker;

	Rigidbody dominoBody;
	Quaternion spawnRotation;
	Vector3 spawnPoint;

	void Awake(){
		tracker = FindObjectOfType<DominoTracker>();
		if(positionOffset != null)
			MoveToOffset();
		tracker.RegisterDomino(this);
		spawnPoint = transform.position;
		spawnRotation = transform.rotation;
		dominoBody = GetComponent<Rigidbody>();
	}

	void MoveToOffset(){
		transform.position = positionOffset.position;
	}

	void OnDestroy(){
		tracker.UnregisterDomino(this);
	}

	public void Reset(){
		dominoBody.velocity = Vector3.zero;
		dominoBody.angularVelocity = Vector3.zero;
		transform.position = spawnPoint;
		transform.rotation = spawnRotation;
	}
}
