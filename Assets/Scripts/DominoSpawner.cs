using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//goes on player parent
public class DominoSpawner : NetworkBehaviour {

	[SerializeField] Camera cameraObject;
	[SerializeField] DominoGravity dominoPrefab;
	[SerializeField] GameObject dominoGhost;
	[SerializeField] Material badGhostMaterial;
	[SerializeField] Vector3 gravityDirection = Vector3.down;
	[SerializeField] float surfaceTolerance = 2f;
	[SerializeField] LayerMask spawnTargets;
	[SerializeField] LayerMask dominoTargets;
	[SerializeField] float rotationSensitivity = 2f;
	[SerializeField] float placeDistance = 10f;

	float currentRotationAngle = 0f;
	[SerializeField] GameObject ghostInstance;
	OverlapDetector detector;
	MeshRenderer ghostMesh;
	Material goodGhostMaterial;
	Vector3 targetPoint;
	Vector3 targetNormal;
	DominoSpawnBehavior currentMode = DominoSpawnBehavior.Hover;

	enum DominoSpawnBehavior{
		None, Spawn, Delete, Hover
	}

	void Start(){
		CmdSpawnGhost();
	}


	void CmdSpawnGhost(){
		//ghostInstance = Instantiate(dominoGhost);
		//NetworkServer.Spawn(ghostInstance);
		ghostInstance.transform.SetParent(null);
		ghostMesh = ghostInstance.GetComponentInChildren<MeshRenderer>();
		goodGhostMaterial = ghostMesh.material;
		detector = ghostInstance.GetComponent<OverlapDetector>();
	}

	void Update () {
		if (!isLocalPlayer)
			return;
		RotateDomino();
		UpdateTarget();
		PlaceGhost();
		CheckOverlap();
		SetGhostColor();
		if (Input.GetMouseButtonUp(0)){
			TrySpawnDomino();
		}
	}

	void RotateDomino(){
		float scroll = Input.mouseScrollDelta.y;
		currentRotationAngle += scroll * rotationSensitivity;
		currentRotationAngle = currentRotationAngle % 360;
	}

	bool AdaptiveRaycast(out RaycastHit hit, out DominoSpawnBehavior behavior){
		behavior = DominoSpawnBehavior.None;
		if (Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit, placeDistance)){
			if (((1 << hit.collider.gameObject.layer) & spawnTargets.value) != 0){
				//hit a spawnable area
				if (Vector3.Angle(gravityDirection * -1, hit.normal) <= surfaceTolerance){
					behavior = DominoSpawnBehavior.Spawn;
				}
			} else if (((1 << hit.collider.gameObject.layer) & dominoTargets.value) != 0){
				//hit a domino
				if (hit.collider.gameObject.GetComponentInParent<DominoGravity>().Gravity == gravityDirection.normalized){
					behavior = DominoSpawnBehavior.Delete;
				}
			}
			return true;
		} else{
			behavior = DominoSpawnBehavior.Hover;
		}
		return false;
	}

	void UpdateTarget(){
		RaycastHit hit;
		if (AdaptiveRaycast(out hit, out currentMode)){
			targetPoint = hit.point;
			targetNormal = hit.normal;
		}
	}

	void SetGhostColor(){
		if (currentMode == DominoSpawnBehavior.Spawn)
			ghostMesh.material = goodGhostMaterial;
		else
			ghostMesh.material = badGhostMaterial;
	}

	void PlaceGhost(){
		ghostInstance.transform.rotation = Quaternion.AngleAxis(currentRotationAngle, gravityDirection);
		if (currentMode != DominoSpawnBehavior.Hover){
			ghostInstance.transform.position = targetPoint;
		} else{
			ghostInstance.transform.position = cameraObject.transform.position + cameraObject.transform.forward * placeDistance;
		}
	}

	void CheckOverlap(){
		if (detector.IsOverlapping()){
			currentMode = DominoSpawnBehavior.None;
		}
	}

	void TrySpawnDomino(){
		if (currentMode == DominoSpawnBehavior.Spawn){
			CmdSpawnDomino(targetPoint);
		}
	}

	[Command]
	void CmdSpawnDomino(Vector3 point){
		Quaternion currentRotation = Quaternion.AngleAxis(currentRotationAngle, gravityDirection);
		DominoGravity grav = Instantiate(dominoPrefab, point, currentRotation);
		grav.Gravity = gravityDirection;
		NetworkServer.Spawn(grav.gameObject);
	}
}
