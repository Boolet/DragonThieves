using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DominoSpawner : NetworkBehaviour {

	[SerializeField] Camera cameraObject;
	[SerializeField] DominoGravity dominoPrefab;
	[SerializeField] GameObject dominoGhost;
	[SerializeField] Vector3 gravityDirection = Vector3.down;
	[SerializeField] float surfaceTolerance = 2f;
	[SerializeField] LayerMask spawnTargets;
	[SerializeField] LayerMask dominoTargets;
	[SerializeField] float rotationSensitivity = 2f;

	float currentRotationAngle = 0f;
	GameObject ghostInstance;
	Vector3 targetPoint;
	Vector3 targetNormal;
	DominoSpawnBehavior currentMode = DominoSpawnBehavior.None;

	enum DominoSpawnBehavior{
		None, Spawn, Delete
	}

	void Start(){
		ghostInstance = Instantiate(dominoGhost);
		NetworkServer.Spawn(ghostInstance);
		ghostInstance.SetActive(false);
	}

	void Update () {
		if (!isLocalPlayer)
			return;
		RotateDomino();
		UpdateTarget();
		if (Input.GetMouseButtonUp(0)){
			TrySpawnDomino();
		}
		PlaceGhost();
	}

	void RotateDomino(){
		float scroll = Input.mouseScrollDelta.y;
		currentRotationAngle += scroll * rotationSensitivity;
		currentRotationAngle = currentRotationAngle % 360;
	}

	bool AdaptiveRaycast(out RaycastHit hit, out DominoSpawnBehavior behavior){
		behavior = DominoSpawnBehavior.None;
		if(Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit, 200)){
			if (( (1 << hit.collider.gameObject.layer) & spawnTargets) != 0){
				//hit a spawnable area
				if (Vector3.Angle(gravityDirection * -1, hit.normal) <= surfaceTolerance){
					behavior = DominoSpawnBehavior.Spawn;
				}
			} else if(((1 << hit.collider.gameObject.layer) & dominoTargets) != 0){
				//hit a domino
				if (hit.collider.gameObject.GetComponentInParent<DominoGravity>().Gravity == gravityDirection.normalized){
					behavior = DominoSpawnBehavior.Delete;
				}
			}
			return true;
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

	void PlaceGhost(){
		if (currentMode == DominoSpawnBehavior.Spawn){
			ghostInstance.transform.position = targetPoint;
			ghostInstance.transform.rotation = Quaternion.AngleAxis(currentRotationAngle, gravityDirection);
			ghostInstance.SetActive(true);
		} else{
			ghostInstance.SetActive(false);
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
		GameObject obj = Instantiate(dominoPrefab.gameObject, point, currentRotation);
		dominoPrefab.Gravity = gravityDirection;
		NetworkServer.Spawn(obj);
	}
}
