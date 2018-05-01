using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SingletonSupport : NetworkBehaviour {
	[SyncVar]
	public HashSet<Vector3> playerPlacementDirections = new HashSet<Vector3>{
		Vector3.down, Vector3.left, Vector3.back
	};
	[SyncVar]
	public bool fixedGravityMode = false;
	[SyncVar]
	public bool allowSpawn = true;

}
