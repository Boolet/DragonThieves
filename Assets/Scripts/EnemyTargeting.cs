using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundDetector))]
public class EnemyTargeting : MonoBehaviour {

	Vector3 currentTarget;
	float currentVolume;

	SoundDetector detector;

	// Use this for initialization
	void Start () {
		detector = GetComponent<SoundDetector>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 GetCurrentTarget(){
		return currentTarget;
	}

	public float GetCurrentVolume(){
		return currentVolume;
	}


}
