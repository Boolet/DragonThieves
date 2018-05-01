using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonSupport : MonoBehaviour {

	public HashSet<Vector3> playerPlacementDirections = new HashSet<Vector3>{
		Vector3.down, Vector3.left, Vector3.back
	};
	public bool fixedGravityMode = false;
	public bool allowSpawn = true;

}
