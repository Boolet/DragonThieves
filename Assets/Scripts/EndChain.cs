using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndChain : MonoBehaviour {

    GameObject WinScreen;

    private void Awake()
    {
        WinScreen = GameObject.FindGameObjectWithTag("Win");
        WinScreen.SetActive(false);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Domino")
            if (collision.gameObject.GetComponent<DominoChain>().hitByDomino)
                WinScreen.SetActive(true);
    }

    public void Reset()
    {
        WinScreen.SetActive(false);
    }
}
