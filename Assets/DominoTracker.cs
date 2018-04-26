using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoTracker : MonoBehaviour {

	static List<Domino> dominoes = new List<Domino>();

	public static void RegisterDomino(Domino domino){
		dominoes.Add(domino);
	}

	public static void UnregisterDomino(Domino domino){
		dominoes.Remove(domino);
	}

	public static void Reset(){
		foreach (Domino dom in dominoes){
			dom.Reset();
		}
	}
}
