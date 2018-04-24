﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetector : MonoBehaviour {

	List<KeyValuePair<SoundEmitter, float>> perceivedVolumes = new List<KeyValuePair<SoundEmitter, float>>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	float PerceivedVolume(SoundEmitter emitter){
		Vector3 displacement = emitter.transform.position - transform.position;
		return emitter.GetCurrentVolume() / (displacement.magnitude + 1);
	}

	int CompareVolumes(KeyValuePair<SoundEmitter, float> a, KeyValuePair<SoundEmitter, float> b){
		return (int) (a.Value - b.Value);
	}

	void UpdatePerceivedVolumes(){
		perceivedVolumes.Clear();
		foreach (SoundEmitter emitter in SoundEmitter.emitters){
			float perceivedVolume = PerceivedVolume(emitter);
			perceivedVolumes.Add(new KeyValuePair<SoundEmitter, float>(emitter, perceivedVolume));
			perceivedVolumes.Sort(CompareVolumes);
		}
	}

	public SoundEmitter GetLoudest(){
		return perceivedVolumes[0].Key;
	}
}
