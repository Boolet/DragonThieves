using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndChain : MonoBehaviour {

	ProgressTracker tracker;
	bool hit = false;

    private void Awake()
    {
		tracker = FindObjectOfType<ProgressTracker>();
    }


    void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.tag == "Domino" && !hit){
			if (collision.gameObject.GetComponent<DominoChain>().hitByDomino){
				hit = true;
				tracker.TargetHit();
			}
		}
    }

    public void Reset()
    {
		hit = false;
    }
}
