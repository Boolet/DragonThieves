using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartChain : MonoBehaviour, Resetable {

    public GameObject startDomino;
    public float _x = 100;
    public float _y = 0;
    public float _z = 0;
    public Rigidbody rb;
    public bool canReset = false;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(canReset == false && Input.GetButtonDown("Start"))
        {
            //Debug.Log("Adding force");
            canReset = true;
            KnockDomino(_x, _y, _z);

        }
        /*if(canReset && Input.GetKeyDown("Start"))
        {
            canReset = false;
        }*/
	}
    void KnockDomino(float x, float y, float z)
    {
		rb.AddRelativeForce(x, y, z);
    }

    public void Reset()
    {
        canReset = false;
    }
}
