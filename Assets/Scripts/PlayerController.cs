using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float moveSpeed = 7.5f;
    [SerializeField] private EzCamera m_camera = null;
    public Material localPlayerMat;
    private void Start()
    {
        SetUpCamera();
        GetComponent<Rigidbody>().freezeRotation = true;
    }
    public override void OnStartLocalPlayer()
    {
        GetComponent<Renderer>().material = localPlayerMat;
    }
    private void SetUpCamera()
    {
        if (m_camera == null)
        {
            m_camera = Camera.main.GetComponent<EzCamera>();
            if (m_camera == null)
            {
                m_camera = Camera.main.gameObject.AddComponent<EzCamera>();
            }
        }
    }
    void Update () {
        if (!isLocalPlayer)
            return;
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        Vector3 moveVector = m_camera.ConvertMoveInputToCameraSpace(x, z);

        transform.Translate(x, 0, z);
	}
}
