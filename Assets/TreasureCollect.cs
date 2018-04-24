using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCollect : MonoBehaviour {

    public float treasure;

    public Rigidbody rb;    // Use this for initialization
    public GameObject player;
    private GameObject treasureObj;
    private bool canGrab = false;
    void Start () {
        treasure = 0.0f;
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (canGrab)
        {
            if (Input.GetButtonDown("Jump"))
            {
                treasure++;
                Destroy(treasureObj);
            }
        }
    }
   
    void OnCollisionEnter(Collision collision)
    {
        canGrab = true;
        treasureObj = collision.gameObject;
    }
    void OnCollisionExit(Collision collision)
    {
        canGrab = false;        
    }
}
