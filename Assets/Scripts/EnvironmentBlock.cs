using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBlock : MonoBehaviour {

	MeshRenderer[] childQuads;
	Material[] childDefaultMaterials;

	// Use this for initialization
	void Start () {
		childQuads = transform.GetComponentsInChildren<MeshRenderer>();
		childDefaultMaterials = new Material[childQuads.Length];
		for (int i = 0; i < childQuads.Length; ++i)
			childDefaultMaterials[i] = childQuads[i].material;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void EditorChangeMaterial(Material editorOverride){
		foreach (MeshRenderer mr in childQuads)
			mr.material = editorOverride;
	}

	public void EditorResetMaterial(){
		for (int i = 0; i < childQuads.Length; ++i)
			childQuads[i].material = childDefaultMaterials[i];
	}
}
