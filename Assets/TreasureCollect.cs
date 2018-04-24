using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TreasureCollect : MonoBehaviour {
	public Text text;
    public float treasure;
	public int collected = 0;
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
		if (collected == 3) {
			text.text = "You Win!!";

		}
    }


    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Treasure")
        {
            canGrab = true;
            treasureObj = collision.gameObject;
            collision.gameObject.SetActive(false);
			collected++;
        }
     
    }
    void OnTriggerExit(Collider collision)
    {
        canGrab = false;
    }
}
