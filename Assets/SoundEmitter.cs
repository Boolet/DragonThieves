using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour {

	public static List<SoundEmitter> emitters = new List<SoundEmitter>();

	[SerializeField] float speedFactor = 1f;
	[SerializeField] float weightFactor = 1f;

	TreasureCollect collector;

	void Awake(){
		collector = GetComponent<TreasureCollect>();
	}

	void OnEnable(){
		emitters.Add(this);
	}

	void OnDisable(){
		emitters.Remove(this);
	}

	public float GetCurrentVolume(){
		//will switch to references when I have them
		return SpeedComponent(Vector3.zero) * WeightComponent(0);
	}

	float SpeedComponent(Vector3 velocity){
		return velocity.magnitude * speedFactor;
	}

	float WeightComponent(float weight){
		return weight * weightFactor;
	}
}
