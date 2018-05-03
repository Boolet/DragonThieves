using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StartChain : NetworkBehaviour {

    public GameObject startDomino;
    public float _x = 100;
    public float _y = 0;
    public float _z = 0;
    public Rigidbody rb;
    public bool canReset = false;

	void Awake ()
    {
        rb = GetComponent<Rigidbody>();
	}

	[ClientRpc]
	public void RpcPlayerKnock(){
		if (canReset == false){
			canReset = true;
			RpcKnockDomino(_x, _y, _z);
		}
	}

    void RpcKnockDomino(float x, float y, float z)
    {
		rb.AddRelativeForce(x, y, z);
    }

	[Server]
	public void CmdReset(){
		RpcReset();
	}
		
	[ClientRpc]
    public void RpcReset()
    {
        canReset = false;
    }
}
