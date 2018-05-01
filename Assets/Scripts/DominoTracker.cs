using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DominoTracker : NetworkBehaviour{

	public List<Resetable> resetables = new List<Resetable>();

	public void Subscribe(Resetable r){
		resetables.Add(r);
	}
		
	public void Unsubscribe(Resetable r){
		resetables.Remove(r);
	}

	[Command]
	public void CmdReset(){
		foreach (Resetable r in resetables){
			r.CmdReset();
		}
	}
}
