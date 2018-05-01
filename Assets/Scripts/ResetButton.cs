using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour {

	DominoTracker tracker;

	void Start(){
		tracker = FindObjectOfType<DominoTracker>();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.R)){
			Reset();
		}
	}

	public void Reset(){
		tracker.Reset();
	}
}
