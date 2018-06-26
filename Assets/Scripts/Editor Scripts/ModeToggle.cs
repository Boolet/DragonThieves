using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeToggle : MonoBehaviour {

    [SerializeField] KeyCode switchKey = KeyCode.Tab;

    DominoSpawnerTwo dominoSpawner;
    BlockPlacement blockSpawner;

	// Use this for initialization
	void Start () {
        dominoSpawner = GetComponent<DominoSpawnerTwo>();
        blockSpawner = GetComponent<BlockPlacement>();
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
        } else {
            dominoSpawner.enabled = true;
            blockSpawner.enabled = false;
        }
    }
}
