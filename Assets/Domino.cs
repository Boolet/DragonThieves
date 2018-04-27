using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domino : MonoBehaviour {

	[SerializeField] Transform positionOffset;

	Rigidbody dominoBody;
	Quaternion spawnRotation;
	Vector3 spawnPoint;

	void Start(){
		MoveToOffset();
		DominoTracker.RegisterDomino(this);
		spawnPoint = dominoBody.transform.position;
		spawnRotation = dominoBody.transform.rotation;
		dominoBody = GetComponent<Rigidbody>();
	}

	void MoveToOffset(){
		transform.position = positionOffset.position;
		Destroy(positionOffset.gameObject);
	}

	void OnDestroy(){
		DominoTracker.UnregisterDomino(this);
	}

	public void Reset(){
		dominoBody.velocity = Vector3.zero;
		dominoBody.angularVelocity = Vector3.zero;
		transform.position = spawnPoint;
		transform.rotation = spawnRotation;
	}
}
