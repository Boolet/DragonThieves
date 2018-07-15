using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StartChain : NetworkBehaviour {

    [SerializeField] bool isStartDomino = false;

    //public GameObject startDomino;
    [HideInInspector] public float _x = 100;
    [HideInInspector] public float _y = 0;
    [HideInInspector] public float _z = 0;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool canReset = false;

	void Awake ()
    {
        rb = GetComponent<Rigidbody>();
	}

    public void SetAsStarter(bool starter) {
        isStartDomino = starter;
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
        if(isStartDomino)
		    rb.AddRelativeForce(x, y, z);
    }

	[Server]
	public void Reset(){
		canReset = false;
		RpcReset();
	}
		
	[ClientRpc]
    public void RpcReset()
    {
        canReset = false;
    }
}
