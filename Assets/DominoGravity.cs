using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoGravity : MonoBehaviour {

	Vector3 gravityOrientation = Vector3.zero;

	Rigidbody body;

	// Use this for initialization
	void Start () {
		body = GetComponentInChildren<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (gravityOrientation == Vector3.zero)
			return;
		body.AddForce(gravityOrientation.normalized * Physics.gravity.magnitude * Time.fixedDeltaTime, ForceMode.Acceleration);
	}

	public Vector3 Gravity{
		set{ gravityOrientation = value.normalized; }
		get{ return gravityOrientation; }
	}
}
