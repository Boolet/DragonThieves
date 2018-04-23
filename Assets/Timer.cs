using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public float time;
	// Use this for initialization
	void Start () {
        time = 120.0f;
		
	}
	
	// Update is called once per frame
	void Update () {
        time = time - Time.deltaTime;		
	}
}
