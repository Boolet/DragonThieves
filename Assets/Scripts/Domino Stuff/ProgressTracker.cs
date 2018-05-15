using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProgressTracker : NetworkBehaviour {

	GameObject winDisplay;
	int numberOfTargets = 0;
	int targetsHit = 0;

	// Use this for initialization
	void Start () {
		winDisplay = GameObject.FindGameObjectWithTag("Win");
		winDisplay.SetActive(false);
		numberOfTargets = FindObjectsOfType<EndChain>().Length;
		print("Number of targets: " + numberOfTargets);
	}
	
	public void TargetHit(){
		++targetsHit;
		CheckWin();
		print("Targets hit: " + targetsHit);
	}

	void CheckWin(){
		if (targetsHit >= numberOfTargets)
			winDisplay.SetActive(true);
	}

	[Server]
	public void Reset(){
		targetsHit = 0;
		winDisplay.SetActive(false);
		RpcReset();
	}

	[ClientRpc]
	public void RpcReset(){
		targetsHit = 0;
		winDisplay.SetActive(false);
		print("Targets hit: " + targetsHit);
	}
}
