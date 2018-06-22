using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DominoSpawnerTwo : NetworkBehaviour {

	[SerializeField] Camera cameraObject;
	//[SerializeField] Vector3 gravityDirection = Vector3.down;		//the gravity thing will be dealt with later
	[SerializeField] Material canPlaceMaterial;
	[SerializeField] Material noPlaceMaterial;
	[SerializeField] DominoGravity dominoPrefab;
    [SerializeField] GameObject placementIndicatorPrefab;
	[SerializeField] float surfaceTolerance = 2f;
	[SerializeField] LayerMask raycastTargets = ~0;	//must be a superset of both spawnTargets and dominoTargets; default to everything
	[SerializeField] LayerMask spawnTargets;
	[SerializeField] LayerMask dominoTargets;
	[SerializeField] float rotationSensitivity = 2f;
	[SerializeField] float placeDistance = 10f;

	GameObject placementIndicator;
	MeshRenderer indicatorRenderer;
	SingletonSupport supporter;
	float dominoRotation = 0f;

	GameObject targetToDelete = null;
	Material targetOldMaterial;	//note: give the domino script a reference to the meshrenderer component to make this easier and faster


	//--------------------------------
	// control
	//--------------------------------

	void Awake(){
		/*
		supporter = FindObjectOfType<SingletonSupport>();
		if(supporter != null && !supporter.fixedGravityMode)
			TakeAvailablePlacement();

		//rotate the player object - hopefully it works without issue
		transform.up = -gravityDirection;
		*/
		SpawnIndicator();
	}

    void Update() {
        if (!isLocalPlayer)
            return;
        //run functionality
    }

    void OnEnable(){
        if (placementIndicator != null)
            CmdSetActive(placementIndicator, true);
            //placementIndicator.SetActive(true);
	}

	void OnDisable(){
        //need to switch the appearance of the delete-highlighted domino back to normal
        DelAbandonDomino();
        //and make the indicator disappear
        if (placementIndicator != null)
            CmdSetActive(placementIndicator, false);
            //placementIndicator.SetActive(false);
	}

    void OnDestroy(){
        CmdDestroy(placementIndicator);
    }

    //--------------------------------
    // initialization functions
    //--------------------------------

    void SpawnIndicator(){
		//placementIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//placementIndicator.transform.localScale = dominoPrefab.transform.localScale;
		//placementIndicator.layer = LayerMask.NameToLayer("Ignore Raycast");
		//then spawn for server
		CmdSpawnIndicator();
	}

	//--------------------------------
	// user input
	//--------------------------------

	void RotateDomino(){
		float scroll = Input.mouseScrollDelta.y;
		dominoRotation += scroll * rotationSensitivity;
		dominoRotation = dominoRotation % 360;
	}

	//--------------------------------
	// domino system behavior
	//--------------------------------

	void DominoAddRemoveLogic(bool activate){
		//do a raycast
		RaycastHit hit;
		Vector3 miss;
		if (!CastFromCamera(out hit, out miss)){
			//if we didn't find a hit, just hover the indicator there
			return;
		}

		//check if what we hit is a domino
		if (((1 << hit.collider.gameObject.layer) & dominoTargets.value) != 0){
			//if it is a domino, then we should be in delete mode
		}

		//determine whether to use delete mode, place mode, or neither
		//update the indicator

		//if activate, perform the correct behavior
	}

	//we did not hit anything at all
	void HoverLogic(Vector3 miss){
		Vector3 hoverPoint = miss;
		Quaternion placementRotation = CreateDominoRotation(transform.up, dominoRotation);

		DelAbandonDomino();
		AdjustIndicatorVisibility(true);
		AdjustIndicatorPosition(hoverPoint);
		AdjustIndicatorRotation(placementRotation);
		AdjustIndicatorColor(false);
	}

	//at this point we know that we did not hit a domino
	void PlacementLogic(RaycastHit hit, bool activate){
		Vector3 placementPoint = SmartPlacePoint(hit);
		Quaternion placementRotation = CreateDominoRotation(hit.normal, dominoRotation);
		bool canPlace = CommonFunctions.CanPlace(placementPoint, placementIndicator.transform.localScale, placementRotation, raycastTargets);

		DelAbandonDomino();
		AdjustIndicatorVisibility(true);
		AdjustIndicatorPosition(placementPoint);
		AdjustIndicatorRotation(placementRotation);
		AdjustIndicatorColor(canPlace);

		if (canPlace && activate){
			//then spawn the domino
		}
	}

	//at this point we know that we hit a domino
	void DeletionLogic(RaycastHit hit, bool activate){
		AdjustIndicatorVisibility(false);

		DelAdoptDomino(hit.collider.gameObject);

		if (targetToDelete != null && activate){
            //then delete the domino
            CmdDestroy(hit.collider.gameObject);
		}
	}

	//--------------------------------
	// helper functions
	//--------------------------------

	//does a raycast from the camera against raycast targets
	bool CastFromCamera(out RaycastHit hit, out Vector3 miss){
		miss = cameraObject.transform.position + cameraObject.transform.forward * placeDistance;
		return Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit, placeDistance, raycastTargets);
	}

	//creates a rotation using an up direction and a number of degrees around that axis
	Quaternion CreateDominoRotation(Vector3 localUp, float rotationAngle){
		Vector3 localForward = Quaternion.FromToRotation(Vector3.up, localUp) * Vector3.forward;
		localForward = Quaternion.AngleAxis(rotationAngle, localUp) * localForward;
		return Quaternion.LookRotation(localForward, localUp);
	}

	//returs a point hovering over the hit location
	Vector3 SmartPlacePoint(RaycastHit hit){
		return hit.point + hit.normal * placementIndicator.transform.localScale.y / 2;
	}

	//--------------------------------
	// placement indicator adjustments
	//--------------------------------

	//moves the placement indicator
	void AdjustIndicatorPosition(Vector3 point){
		placementIndicator.transform.position = point;
	}

	//changes the indicator's material
	void AdjustIndicatorColor(bool canPlace){
		Material toSwitch = canPlace ? canPlaceMaterial : noPlaceMaterial;
		if(indicatorRenderer.material != toSwitch)	//not sure if this saves any processing power
			indicatorRenderer.material = toSwitch;
	}

	//changes the indicator's visibility
	void AdjustIndicatorVisibility(bool visible){
		indicatorRenderer.enabled = visible;
	}

	//changes the indicator's orientation
	void AdjustIndicatorRotation(Quaternion rotation){
		placementIndicator.transform.rotation = rotation;
	}

	//--------------------------------
	// deletion indicator functions
	//--------------------------------

	//sets the targetToDelete's material back to what it was before it was selected
	void DelRestoreDominoAppearance(){
		if (targetToDelete != null)
			targetToDelete.GetComponent<MeshRenderer>().material = targetOldMaterial;
	}

	//stops keeping track of the targetToDelete and restores its appearance
	void DelAbandonDomino(){
		DelRestoreDominoAppearance();
		targetToDelete = null;
	}

	//chooses a new domino to be the delete target and changes its material to the noPlace material
	void DelAdoptDomino(GameObject newTarget){
		if (targetToDelete == newTarget)
			return;
		DelAbandonDomino();
		if (newTarget == null)
			return;
		targetToDelete = newTarget;
		MeshRenderer renderer = targetToDelete.GetComponent<MeshRenderer>();
		targetOldMaterial = renderer.material;
		renderer.material = noPlaceMaterial;
	}

	//--------------------------------
	// network spawning and despawning
	//--------------------------------

    /// <summary>
    /// This command instantiates the placement indicator for the player,
    /// gives authority over it to the player, spawns it for all clients,
    /// and finally sends back a reference to the object for the player to use.
    /// </summary>
	[Command]
    void CmdSpawnIndicator(){
        placementIndicator = Instantiate(placementIndicatorPrefab);
        NetworkServer.Spawn(placementIndicator);
        placementIndicator.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        TargetSpawnIndicator(connectionToClient, placementIndicator);
	}

    [TargetRpc]
    void TargetSpawnIndicator(NetworkConnection target, GameObject spawnedIndicator) {
        placementIndicator = spawnedIndicator;
    }

	[Command]
	void CmdSpawnDomino(Vector3 point, Vector3 normal, Vector3 gravity){
		//bool reversed = Vector3.Angle(normal, gravity) > 90f;
		//DominoGravity grav = Instantiate(dominoPrefab, detector.ColliderTransform().position, detector.ColliderTransform().rotation);
		//NetworkServer.Spawn(grav.gameObject);
		//grav.ServerSetGravity(gravity * (reversed?1:-1));
		//grav.RpcSetGravity(gravity * (reversed?1:-1));
	}

    [Command]
    void CmdSetActive(GameObject obj, bool active) {
        obj.SetActive(false);
        RpcSetActive(obj, active);
    }

    [ClientRpc]
    void RpcSetActive(GameObject obj, bool active) {
        obj.SetActive(active);
    }

	[Command]
	void CmdDestroy(GameObject toDelete){
		NetworkServer.Destroy(toDelete);
	}
}
