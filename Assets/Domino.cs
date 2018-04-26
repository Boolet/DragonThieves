using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domino : MonoBehaviour {

	[SerializeField] Rigidbody dominoBody;

	Quaternion spawnRotation;
	Vector3 spawnPoint;

	void Start(){
		DominoTracker.RegisterDomino(this);
		spawnPoint = dominoBody.transform.position;
		spawnRotation = dominoBody.transform.rotation;
	}

	void OnDestroy(){
		DominoTracker.UnregisterDomino(this);
	}

	public void Reset(){
		dominoBody.velocity = Vector3.zero;
		dominoBody.angularVelocity = Vector3.zero;
		dominoBody.transform.position = spawnPoint;
		dominoBody.transform.rotation = spawnRotation;
	}
}
