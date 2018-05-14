using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will manage the multiple player functionalities spread across different scripts
/// by passing the player input to those scripts or by disabling or enabling them.
/// 
/// The camera look functionality and the camera movement ought to be split into two separate
/// scripts for this purpose
/// </summary>
public class PlayerMasterControl : MonoBehaviour {

	DominoSpawner dominoBuilder;
	BlockPlacement blockBuilder;
	PlayerChainControl chainControl;
	CameraGhostControl playerMovement;

	// Use this for initialization
	void Start () {
		Initialize();
	}

	void Initialize(){
		dominoBuilder = GetComponent<DominoSpawner>();
		if (dominoBuilder == null)
			dominoBuilder = GetComponentInChildren<DominoSpawner>();

		blockBuilder = GetComponent<BlockPlacement>();
		if (blockBuilder == null)
			blockBuilder = GetComponentInChildren<BlockPlacement>();

		chainControl = GetComponent<PlayerChainControl>();
		if (chainControl == null)
			chainControl = GetComponentInChildren<PlayerChainControl>();

		playerMovement = GetComponent<CameraGhostControl>();
		if (playerMovement == null)
			playerMovement = GetComponentInChildren<CameraGhostControl>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
