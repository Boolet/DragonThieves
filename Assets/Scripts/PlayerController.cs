using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float moveSpeed = 7.5f;

    public Material localPlayerMat;
    private void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
    }
    public override void OnStartLocalPlayer()
    {
        GetComponent<Renderer>().material = localPlayerMat;
    }

    void Update () {
        if (!isLocalPlayer)
            return;
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        transform.Translate(x, 0, z);
	}
}
