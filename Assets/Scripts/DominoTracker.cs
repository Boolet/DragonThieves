using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoTracker{

	static List<Resetable> dominoes = new List<Resetable>();

	public static void RegisterDomino(Resetable domino){
		dominoes.Add(domino);
	}

	public static void UnregisterDomino(Resetable domino){
		dominoes.Remove(domino);
	}

	public static void Reset(){
		foreach (Resetable dom in dominoes){
			dom.Reset();
		}
	}
}
