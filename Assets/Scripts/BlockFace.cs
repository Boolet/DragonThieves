using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A face of a map block; used for the editor for now, but may have uses in the game scene too
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class BlockFace : MonoBehaviour {

	[SerializeField] Material placeableMaterial;
	[SerializeField] int placeableLayer;
	[SerializeField] Material unplaceableMaterial;
	[SerializeField] int unplaceableLayer;

	MeshRenderer meshRenderer;
	Material correctMaterial;
	Material overrideMaterial = null;	//for the editor

	bool placeable = true;
	public bool Placeable
	{
		get{
			return placeable;
		}
		set{
			placeable = value;
			PlaceableUpdate();
		}
	}

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer>();
		PlaceableUpdate();
	}

	void PlaceableUpdate(){
		if (placeable){
			correctMaterial = placeableMaterial;
			gameObject.layer = placeableLayer;
		} else{
			correctMaterial = unplaceableMaterial;
			gameObject.layer = unplaceableLayer;
		}
		if (overrideMaterial == null)
			meshRenderer.material = correctMaterial;
	}

	//overrides the look of this object; set it to null to reset
	public void SetMaterialOverride(Material material){
		overrideMaterial = material;
		if (overrideMaterial != null)
			meshRenderer.material = overrideMaterial;
		else
			meshRenderer.material = correctMaterial;
	}
}
