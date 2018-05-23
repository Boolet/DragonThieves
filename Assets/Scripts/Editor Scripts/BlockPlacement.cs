﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TO DO:
/// +Change block target color when in delete mode
/// +Allow the user to scale the block to be placed using some interface
/// 
/// The player's controls for placing and deleting blocks within the editor mode. The blocks are considered
/// to be of higher priority than other objects such as dominos and any toys that may be included in the game,
/// so placing a block over such objects will delete them.
/// 
/// Controls:
/// Left Mouse to place or delete (hardcoded for now)
/// F to switch modes (serialized)
/// 
/// Usage:
/// Attach to player object; it or its child should have the camera attached
/// </summary>
public class BlockPlacement : MonoBehaviour {

	//necessary referenes
	[SerializeField] EnvironmentBlock blockPrefab;
	[SerializeField] GameObject placementIndicator = null;	//can just be a cube
	[SerializeField] Material canPlaceMaterial;
	[SerializeField] Material noPlaceMaterial;

	[SerializeField] KeyCode modeSwitchKey = KeyCode.F;
	[SerializeField] KeyCode scaleUpVert = KeyCode.U;
	[SerializeField] KeyCode scaleDownVert = KeyCode.J;
	[SerializeField] KeyCode scaleUpHorz = KeyCode.I;
	[SerializeField] KeyCode scaleDownHorz = KeyCode.K;
	[SerializeField] float scaleAdjustSensitivity = 0.1f;
	[SerializeField][Range(1f, float.PositiveInfinity)] float minimumSize = 1f;
	[SerializeField] float placeDistAdjustSensitivity = 0.5f;
	[SerializeField] float placementDistance = 10f;
	[SerializeField] float maxPlaceDist = 30f;
	[SerializeField] float minPlaceDist = 4f;
	[SerializeField] float deleteDistance = 10f;
	[SerializeField] LayerMask castObstructions;	//objects that can block the player's interaction path
	[SerializeField] LayerMask placementObstructions;	//objects that prevent block placement
	[SerializeField] LayerMask toyLayer;	//the domino layer (but really all toy layers that should be deleted when a block is placed)

	//maybe also have a reference to a HUD for this

	enum BlockPlaceBehavior{
		Place, Delete
	}

	MeshRenderer indicatorRenderer;
	Camera playerCam;
	Vector3 internalScale = Vector3.one;
	Vector3 boxDimensions = Vector3.one;
	BlockPlaceBehavior behavior = BlockPlaceBehavior.Place;
	EnvironmentBlock currentDeleteTarget = null;


	//=============================================================================================
	// Code control
	//=============================================================================================

	void Start(){
		playerCam = GetComponent<Camera>();
		if (playerCam == null)
			playerCam = GetComponentInChildren<Camera>();
		if (playerCam == null)
			throw new MissingComponentException("No camera object found on or under BlockPlacement script");

		if (placementIndicator == null){
			placementIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
		} else{
			placementIndicator = Instantiate(placementIndicator);	//use the prefab
		}
		placementIndicator.layer = LayerMask.NameToLayer("Ignore Raycast");
		indicatorRenderer = placementIndicator.GetComponent<MeshRenderer>();
		if (indicatorRenderer == null)
			throw new MissingComponentException("BlockPlacement indicator does not have a MeshRenderer component");
	}
	
	// Update is called once per frame
	void Update () {
		ModeControl();
		if (behavior == BlockPlaceBehavior.Place)
			PlacementLogic();
		else
			DeleteLogic();
	}

	void OnDisable(){
		//need to switch the appearance of the delete-highlighted block back to normal
		if(currentDeleteTarget != null)
			currentDeleteTarget.EditorChangeMaterial(null);
	}


	//=============================================================================================
	// Mode switching
	//=============================================================================================

	//for switching whether the system is placing or deleting blocks
	void ModeControl(){
		if (Input.GetKeyDown(modeSwitchKey)){
			if (behavior == BlockPlaceBehavior.Place)
				behavior = BlockPlaceBehavior.Delete;
			else
				behavior = BlockPlaceBehavior.Place;
		}
		SetIndicatorMode(behavior);
	}

	//changes the player's indicator to reflect the mode; currently just turns off the placement
	//indicator in other modes
	void SetIndicatorMode(BlockPlaceBehavior behaviorMode){
		bool showIndicator = behaviorMode == BlockPlaceBehavior.Place;
		placementIndicator.SetActive(showIndicator);
	}

	//=============================================================================================
	// Placement behavior methods
	//=============================================================================================

	//runs the whole placement behavior
	void PlacementLogic(){
		UserChangePlaceDist();
		UserChangeBlockScale();
		AdjustIndicatorScale(boxDimensions);
		Vector3 placementPoint = SmartPlacementPoint();	//gets the location that a block would be placed, regardless of validity
		AdjustIndicatorPosition(placementPoint);	//moves the indicator to this location
		bool canPlace = CanPlace(placementPoint);	//checks whether this location is a valid placement point
		AdjustIndicatorColor(canPlace);	//changes how the indicator looks to inform the user whether the placement point is valid

		if (Input.GetMouseButtonDown(0) && canPlace){	//if the player clicks and a block can be placed...
			DestroyOverlappingDominos(placementPoint);	//destroy any obstructing dominos
			PlaceBlock(placementPoint);	//then spawn the block
		}
	}

	//--------------------------------
	// placement modifiers
	//--------------------------------

	//changes the block's placement distance when the player uses the scroll wheel
	void UserChangePlaceDist(){
		float scroll = Input.mouseScrollDelta.y;
		placementDistance += scroll * placeDistAdjustSensitivity;
		placementDistance = Mathf.Clamp(placementDistance, minPlaceDist, maxPlaceDist);
	}

	//changes the block's scale along the player's up and right axes
	//
	//note that this uses objective orientation, so if the block or 
	//object is allowed to rotate in the future this will not work
	void UserChangeBlockScale(){
		Vector3 verticalScaleAxis = GreatestComponent(playerCam.transform.up).normalized;
		Vector3 horizontalScaleAxis = GreatestComponent(playerCam.transform.right).normalized;

		internalScale += verticalScaleAxis * ( (Input.GetKey(scaleUpVert)?1:0) + (Input.GetKey(scaleDownVert)?-1:0) ) * scaleAdjustSensitivity;
		internalScale += horizontalScaleAxis * ( (Input.GetKey(scaleUpHorz)?1:0) + (Input.GetKey(scaleDownHorz)?-1:0) ) * scaleAdjustSensitivity;

		internalScale = MinimumScaleSize(internalScale);
		boxDimensions = RoundToGrid(internalScale);
	}

	//returns the greatest component of the source vector
	Vector3 GreatestComponent(Vector3 source){
		if (Mathf.Abs(source.x) > Mathf.Abs(source.y) && Mathf.Abs(source.x) > Mathf.Abs(source.z))
			return Vector3.right * source.x;
		if (Mathf.Abs(source.y) > Mathf.Abs(source.z))
			return Vector3.up * source.y;
		return Vector3.forward * source.z;
	}

	Vector3 MinimumScaleSize(Vector3 source){
		return new Vector3(Mathf.Max(source.x, minimumSize), Mathf.Max(source.y, minimumSize), Mathf.Max(source.z, minimumSize));
	}


	//--------------------------------
	// placement indicator adjustments
	//--------------------------------

	//moves the placement indicator
	void AdjustIndicatorPosition(Vector3 point){
		placementIndicator.transform.position = point;
	}

	//changes the indicator's material based on whether a block can be placed here
	void AdjustIndicatorColor(bool canPlace){
		Material toSwitch = canPlace ? canPlaceMaterial : noPlaceMaterial;
		if(indicatorRenderer.material != toSwitch)	//not sure if this saves any processing power
			indicatorRenderer.material = toSwitch;
	}

	void AdjustIndicatorScale(Vector3 scale){
		placementIndicator.transform.localScale = scale;
	}


	//--------------------------------
	// placement effects
	//--------------------------------

	//spawns a block
	void PlaceBlock(Vector3 placementPoint){
		EnvironmentBlock block = Instantiate(blockPrefab, placementPoint, Quaternion.identity);
		block.transform.localScale = boxDimensions;	//may need to be changed to a function within the environment block

	}

	//destroys all dominos that overlap with the block's placement (this could be modified to destroy any toys that may be placed)
	void DestroyOverlappingDominos(Vector3 placementPoint){
		Collider[] overlappingDominos = BlockOverlapColliders(placementPoint, toyLayer);
		foreach(Collider col in overlappingDominos){
			Destroy(col.gameObject);
		}
	}


	//--------------------------------
	// placement verification
	//--------------------------------

	//gives the colliders on MASK layers that intersect with the potential block placement at point POINT
	Collider[] BlockOverlapColliders(Vector3 point, LayerMask mask){
		Vector3 overlapDimensions = (boxDimensions / 2f) - Vector3.one * Vector3.kEpsilon;
		return Physics.OverlapBox(point, overlapDimensions, Quaternion.identity, mask);
	}

	//whether the block can be placed here
	bool CanPlace(Vector3 point){
		return BlockOverlapColliders(point, placementObstructions).Length == 0;
	}


	//--------------------------------
	// placement location
	//--------------------------------

	//returns the placement point for the block; if the raycast hits a target then it will be that point plus the relevant
	//dimension of the block as an offset, otherwise it will just be the end of the raycast
	Vector3 SmartPlacementPoint(){
		RaycastHit hit;	//if there is a hit, this will be it
		Vector3 spawnPoint;	//the actual point that the object will be spawned in; it is not correct immediately
		if (PlacementPoint(out spawnPoint, out hit)){
			Vector3 offset = hit.normal;
			offset.Scale(boxDimensions / 2f);
			spawnPoint = hit.point + offset;	//if there is a hit, stack the box onto the hit block
		}
		return RoundToGrid(spawnPoint);	//snap the point to the grid and return it
	}

	//gives a placement point if there is no hit, otherwise gives a hit
	bool PlacementPoint(out Vector3 rawPoint, out RaycastHit hit){
		rawPoint = Vector3.zero;
		if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, placementDistance, castObstructions)){
			return true;
		} else{
			rawPoint = playerCam.transform.position + playerCam.transform.forward * placementDistance;
			return false;
		}
	}

	//rounds a vector3 to the nearest grid point
	//
	//this could be changed to handle any grid size! Not just size one.
	Vector3 RoundToGrid(Vector3 point){
		return new Vector3(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), Mathf.RoundToInt(point.z));
	}


	//=============================================================================================
	// Delete behavior methods
	//=============================================================================================

	//runs the whole deletion behavior
	void DeleteLogic(){
		EnvironmentBlock newTarget = FindTargetBlock();	//try to find a block
		SetDeleteTarget(newTarget);

		if (currentDeleteTarget == null)	//no block detected?
			return;
		
		if (Input.GetMouseButtonDown(0)){
			DeleteBlock(currentDeleteTarget);
		}
		
	}

	//tries to find a target that could be deleted
	EnvironmentBlock FindTargetBlock(){
		RaycastHit hit;
		if (!Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, placementDistance, castObstructions))	//facing anything?
			return null;
		else
			return hit.collider.gameObject.GetComponent<EnvironmentBlock>();
	}

	//sets the delete target and changes the look of the old and new targets to indicate which is being selected
	void SetDeleteTarget(EnvironmentBlock newTarget){
		if (currentDeleteTarget == newTarget)	//if it's the same target, don't worry about it
			return;
		//otherwise, change this target's color back to normal
		if(currentDeleteTarget != null)
			currentDeleteTarget.EditorChangeMaterial(null);

		//and set the new target's color to the indicator color if it isn't null
		if(newTarget != null)
			newTarget.EditorChangeMaterial(noPlaceMaterial);

		//finally set the current target to the new target
		currentDeleteTarget = newTarget;
	}

	//destroys a block
	void DeleteBlock(EnvironmentBlock block){
		Destroy(block.gameObject);
	}


}