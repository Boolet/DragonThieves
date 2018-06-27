using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Script for the domino map editor. This will not be necessary in play mode
/// </summary>
public class EnvironmentBlock : NetworkBehaviour {

	Dictionary<Vector3, BlockFace> quadsDict = new Dictionary<Vector3, BlockFace>();
    List<GameObject> attachedDominos = new List<GameObject>();

	// Use this for initialization
	void Start () {
		foreach (BlockFace bf in transform.GetComponentsInChildren<BlockFace>()){
			quadsDict.Add(bf.transform.localPosition.normalized, bf);
		}
	}

	//this is local - used when the player mouses over a block with delete mode on
	public void EditorChangeMaterial(Material editorOverride){
		foreach (KeyValuePair<Vector3, BlockFace> kv in quadsDict)
			kv.Value.SetMaterialOverride(editorOverride);
	}

    public void AddDomino(GameObject domino) {
        attachedDominos.Add(domino);
    }

    public void RemoveDomino(GameObject domino) {
        attachedDominos.Remove(domino);
    }

    private void OnDestroy() {
        //destroy all attached dominos at the server if this block is destroyed
        if (!isServer)
            return;
        GameObject[] dominosArray = attachedDominos.ToArray();
        foreach (GameObject o in dominosArray) {
            NetworkServer.Destroy(o);
        }
    }
}
