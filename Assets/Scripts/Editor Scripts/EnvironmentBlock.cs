using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the domino map editor. This will not be necessary in play mode
/// </summary>
public class EnvironmentBlock : MonoBehaviour {

	Dictionary<Vector3, BlockFace> quadsDict = new Dictionary<Vector3, BlockFace>();

	// Use this for initialization
	void Start () {
		foreach (BlockFace bf in transform.GetComponentsInChildren<BlockFace>()){
			quadsDict.Add(bf.transform.localPosition.normalized, bf);
		}
	}

	public void EditorChangeMaterial(Material editorOverride){
		foreach (KeyValuePair<Vector3, BlockFace> kv in quadsDict)
			kv.Value.SetMaterialOverride(editorOverride);
	}
}
