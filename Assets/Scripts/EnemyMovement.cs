using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyTargeting))]
[RequireComponent(typeof(SoundEmitter))]
public class EnemyMovement : MonoBehaviour {

	[SerializeField] float runningSpeed = 10f;
	[SerializeField] float walkingSpeed = 5f;
	[SerializeField] float selfSoundFactor = 0.05f;

	Vector3 targetPosition;
	float targetVolume;
	float currentSpeed = 0f;

	EnemyTargeting targetingSystem;
	SoundEmitter selfSounds;

	// Use this for initialization
	void Start () {
		targetingSystem = GetComponent<EnemyTargeting>();
		selfSounds = GetComponent<SoundEmitter>();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTarget();
		UpdateSpeed();
	}

	void UpdateTarget(){
		targetPosition = targetingSystem.GetCurrentTarget();
		targetVolume = targetingSystem.GetCurrentVolume();
	}

	//the enemy should run when there is a very loud noise, but walk when the noises are quieter

	void UpdateSpeed(){
		if (selfSounds.GetCurrentVolume() > targetVolume)
			currentSpeed = walkingSpeed;
		else
			currentSpeed = runningSpeed;
	}



}
