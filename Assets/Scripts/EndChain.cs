using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndChain : MonoBehaviour {


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Domino")
            if(collision.gameObject.GetComponent<DominoChain>().hitByDomino)
                Debug.Log("Endgame!");
    }
}
