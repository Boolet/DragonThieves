using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeToggle : MonoBehaviour {

    [SerializeField] KeyCode switchKey = KeyCode.Tab;

    DominoSpawnerTwo dominoSpawner;
    BlockPlacement blockSpawner;
    BlockFaceEditor faceEditor;

	// Use this for initialization
	void Start () {
        dominoSpawner = GetComponent<DominoSpawnerTwo>();
        blockSpawner = GetComponent<BlockPlacement>();
        faceEditor = GetComponent<BlockFaceEditor>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(switchKey))
            SwitchMode();
	}

    void SwitchMode() {
        if (dominoSpawner.enabled) {
            dominoSpawner.enabled = false;
            blockSpawner.enabled = true;
            faceEditor.enabled = false;
        } else if (blockSpawner.enabled) {
            dominoSpawner.enabled = true;
            blockSpawner.enabled = false;
            faceEditor.enabled = false;
        } else {
            dominoSpawner.enabled = false;
            blockSpawner.enabled = false;
            faceEditor.enabled = true;
        }
    }
}
