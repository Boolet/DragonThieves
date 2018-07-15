using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Potential bug: changing whether this is an end domino does not cause the
//progress tracker to re-check its count of hit end dominos against the number
//of registered end dominos
public class EndChain : MonoBehaviour {

    [SerializeField] bool isEndDomino = false;

	ProgressTracker tracker;
	bool hit = false;

    private void Awake()
    {
        tracker = FindObjectOfType<ProgressTracker>();
        RegisterAsEnd(isEndDomino);
    }

    void RegisterAsEnd(bool end) {
        tracker.RegisterEnder(this, end);
    }

    //sets whether this script will essentially be 'active'
    //if false, this is not an end domino
    public void SetAsEnd(bool end) {
        isEndDomino = end;
        RegisterAsEnd(end);
    }

    void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.tag == "Domino" && !hit){
			if (collision.gameObject.GetComponent<DominoChain>().hitByDomino){
				hit = true;
                HitResponse();
			}
		}
    }

    void HitResponse() {
        if (isEndDomino)
            tracker.TargetHit();
    }

    public void Reset()
    {
		hit = false;
    }
}
