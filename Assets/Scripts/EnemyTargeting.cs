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

	void TargetLoudest(){
		KeyValuePair<SoundEmitter, float> noisemaker = detector.GetLoudest();
		currentTarget = noisemaker.Key;
		currentVolume = noisemaker.Value;
	}

	public Vector3 GetCurrentTarget(){
		TargetLoudest();
		//print(currentTarget.gameObject.name + " is making " + currentVolume + " noise");
		return currentTarget.transform.position;
	}

	public float GetCurrentVolume(){
		return currentVolume;
	}


}
