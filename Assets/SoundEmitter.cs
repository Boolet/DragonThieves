using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour {

	public static List<SoundEmitter> emitters = new List<SoundEmitter>();

	[SerializeField] float speedFactor = 1f;
	[SerializeField] float weightFactor = 1f;



	void OnEnable(){
		emitters.Add(this);
	}

	void OnDisable(){
		emitters.Remove(this);
	}

	public float GetCurrentVolume(){

	}

	float SpeedComponent(Vector3 velocity){
		return velocity.magnitude * speedFactor;
	}

	float WeightComponent(float weight){
		return weight * weightFactor;
	}
}
