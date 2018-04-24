using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCollect : MonoBehaviour {

    public float treasure;

    //public Rigidbody rb;    // Use this for initialization
    public GameObject player;
    public GameObject treasureObj;
    public bool canGrab;
    void Start () {
        treasure = 0.0f;
        canGrab = false;
       // rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (canGrab)
        {
            if (Input.GetButtonDown("Pick Up"))
            {
                treasure++;
                treasureObj.SetActive(false);
            }
        }
    }


    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Treasure")
        {
            canGrab = true;
            treasureObj = collision.gameObject;
            collision.gameObject.SetActive(false);
        }
     
    }
    void OnTriggerExit(Collider collision)
    {
        canGrab = false;
    }
}
