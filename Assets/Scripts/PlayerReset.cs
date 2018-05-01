using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerReset : NetworkBehaviour {

	DominoTracker tracker;

	// Use this for initialization
	void Start () {
		tracker = FindObjectOfType<DominoTracker>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)){
			CmdReset();
		}
	}

	[Command]
	public void CmdReset(){
		tracker.CmdReset();
	}
}
