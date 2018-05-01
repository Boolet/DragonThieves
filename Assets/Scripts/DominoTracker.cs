using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DominoTracker : NetworkBehaviour{

	[SyncVar]
	List<Resetable> dominoes = new List<Resetable>();

	public void RegisterDomino(Resetable domino){
		dominoes.Add(domino);
	}

	public void UnregisterDomino(Resetable domino){
		dominoes.Remove(domino);
	}

	public void Reset(){
		foreach (Resetable dom in dominoes){
			dom.Reset();
		}
	}
}
