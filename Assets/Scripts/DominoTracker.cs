using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DominoTracker : NetworkBehaviour{

	[Server]
	public void CmdReset(){
		foreach (Domino d in FindObjectsOfType<Domino>()){
			d.CmdReset();
		}
		foreach (StartChain sc in FindObjectsOfType<StartChain>()){
			sc.CmdReset();
		}
	}

	[ClientRpc]
	public void RpcDebugOut(){
		print("Debugging");
	}
}
