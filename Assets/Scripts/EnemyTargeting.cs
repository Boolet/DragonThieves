using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundDetector))]
public class EnemyTargeting : MonoBehaviour {

	//will need to eventually add an enemy memory so that it doesn't just immediately lose focus if the player stands still
	SoundEmitter currentTarget;
	float currentVolume;

	SoundDetector detector;

	// Use this for initialization
	void Start () {
		detector = GetComponent<SoundDetector>();
	}
	
	// Update is called once per frame
	void Update () {
		TargetLoudest();
	}

	void TargetLoudest(){
		KeyValuePair<SoundEmitter, float> noisemaker = detector.GetLoudest();
		currentTarget = noisemaker.Key;
		currentVolume = noisemaker.Value;
	}

	public Vector3 GetCurrentTarget(){
		return currentTarget.transform.position;
	}

	public float GetCurrentVolume(){
		return currentVolume;
	}


}
