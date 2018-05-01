using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StartChain : NetworkBehaviour, Resetable {

    public GameObject startDomino;
    public float _x = 100;
    public float _y = 0;
    public float _z = 0;
    public Rigidbody rb;
    public bool canReset = false;

	void Awake ()
    {
        rb = GetComponent<Rigidbody>();
		CmdSubscribe();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(canReset == false && Input.GetButtonDown("Start") && isLocalPlayer)
        {
            //Debug.Log("Adding force");
            canReset = true;
            RpcKnockDomino(_x, _y, _z);

        }
        /*if(canReset && Input.GetKeyDown("Start"))
        {
            canReset = false;
        }*/
	}

	[ClientRpc]
    void RpcKnockDomino(float x, float y, float z)
    {
		rb.AddRelativeForce(x, y, z);
    }
		
	void CmdSubscribe(){
		print("Start domino subscribing");
		FindObjectOfType<DominoTracker>().Subscribe(this);
	}

	public void CmdReset(){
		RpcReset();
	}
		
	[ClientRpc]
    public void RpcReset()
    {
        canReset = false;
    }
}
