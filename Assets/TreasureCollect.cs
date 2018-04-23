using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCollect : MonoBehaviour {

    public float treasure;

    public Rigidbody rb;    // Use this for initialization
    public GameObject player;
    void Start () {
        treasure = 0.0f;
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
   
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Treasure")
        {
            Destroy(collision.gameObject);
            treasure++;
        }
    }
}
