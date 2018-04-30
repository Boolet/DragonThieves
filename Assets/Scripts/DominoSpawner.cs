using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//goes on player parent
public class DominoSpawner : NetworkBehaviour {

	static HashSet<Vector3> playerPlacementDirections = new HashSet<Vector3>{
		Vector3.down, Vector3.left, Vector3.back
	};
	public static bool fixedGravityMode = false;

	[SerializeField] Camera cameraObject;
	[SerializeField] DominoGravity dominoPrefab;
	[SerializeField] GameObject ghostInstance;
	[SerializeField] Material badGhostMaterial;
	[SerializeField] Vector3 gravityDirection = Vector3.down;
	[SerializeField] float surfaceTolerance = 2f;
	[SerializeField] LayerMask spawnTargets;
	[SerializeField] LayerMask dominoTargets;
	[SerializeField] float rotationSensitivity = 2f;
	[SerializeField] float placeDistance = 10f;

	float currentRotationAngle = 0f;
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
		if(!fixedGravityMode)
			TakeAvailablePlacement();
		ghostInstance.transform.SetParent(null);
		ghostMesh = ghostInstance.GetComponentInChildren<MeshRenderer>();
		goodGhostMaterial = ghostMesh.material;
		detector = ghostInstance.GetComponent<OverlapDetector>();
	}

	void TakeAvailablePlacement(){
		foreach (Vector3 v in playerPlacementDirections){
			gravityDirection = v;
			playerPlacementDirections.Remove(v);
			return;
		}
		throw new KeyNotFoundException("No elements in placement direction set to pick!");
	}

	void OnDestroy(){
		RelinquishPlacement();
	}

	void RelinquishPlacement(){
		playerPlacementDirections.Add(gravityDirection);
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
				if (Vector3.Angle(gravityDirection * -1, hit.normal) <= surfaceTolerance
					|| Vector3.Angle(gravityDirection, hit.normal) <= surfaceTolerance){
					behavior = DominoSpawnBehavior.Spawn;
				}
			} else if (((1 << hit.collider.gameObject.layer) & dominoTargets.value) != 0){
				//hit a domino
				if (hit.collider.gameObject.GetComponentInParent<DominoGravity>().Gravity == gravityDirection.normalized
					|| hit.collider.gameObject.GetComponentInParent<DominoGravity>().Gravity == -gravityDirection.normalized){
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

	Quaternion DominoRotation(bool reversed){
		return Quaternion.FromToRotation(Vector3.up, -gravityDirection * (reversed?1:-1)) * Quaternion.AngleAxis(currentRotationAngle, Vector3.up);
	}

	void SetTransformToDominoPlacement(Transform t, bool reversed){
		t.rotation = DominoRotation(reversed);
	}

	void PlaceGhost(){
		SetTransformToDominoPlacement(ghostInstance.transform, Vector3.Angle(targetNormal, gravityDirection) > 90f);
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
			CmdSpawnDomino(targetPoint, targetNormal, gravityDirection);
		}
	}

	[Command]
	void CmdSpawnDomino(Vector3 point, Vector3 normal, Vector3 gravity){
		bool reversed = Vector3.Angle(normal, gravity) > 90f;
		DominoGravity grav = Instantiate(dominoPrefab, detector.ColliderTransform().position, detector.ColliderTransform().rotation);
		grav.Gravity = gravity * (reversed?1:-1);
		NetworkServer.Spawn(grav.gameObject);
	}
}
