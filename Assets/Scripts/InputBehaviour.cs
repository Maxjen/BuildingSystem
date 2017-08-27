using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBehaviour : MonoBehaviour {

	public BuildingBehaviour building;
	public LayerMask raycastMask;

	Transform cameraTransform;
	Transform lastSnapTarget = null;

	bool bHideSnapTargets = false;

	// Use this for initialization
	void Start () {
		cameraTransform = transform.Find("FirstPersonCharacter");
	}

	void SetSnapTargetHighlight(bool bHighlight, Transform snapTargetTransform) {
		var snapTargetBehaviour = snapTargetTransform.GetComponent<SnapTargetBehaviour>();
		if (snapTargetBehaviour) {
			snapTargetBehaviour.SetHighlight(bHighlight);
		}
	}

	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		//var bHit = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 10.0f, ~LayerMask.NameToLayer("SnapTargets"));
		var bHit = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 30.0f, raycastMask);
		if (bHit) {
			if (lastSnapTarget && lastSnapTarget != hit.transform) {
				SetSnapTargetHighlight(false, lastSnapTarget);
			}
			lastSnapTarget = hit.transform;
			if (lastSnapTarget) {
				SetSnapTargetHighlight(true, lastSnapTarget);
			}
		} else {
			if (lastSnapTarget) {
				SetSnapTargetHighlight(false, lastSnapTarget);
				lastSnapTarget = null;
			}
		}

		if (Input.GetButtonDown("Build")) {
			if (lastSnapTarget) {
				building.PlaceBuildingPart(lastSnapTarget);
			}
		}
		if (Input.GetButtonDown("FloorSquare")) {
			bHideSnapTargets = false;
			building.SetSnapTargetType(SnapTargetBehaviour.SnapTargetType.FloorSquare);
			building.CreateSnapTargets();
		}
		if (Input.GetButtonDown("FloorTriangle")) {
			bHideSnapTargets = false;
			building.SetSnapTargetType(SnapTargetBehaviour.SnapTargetType.FloorTriangle);
			building.CreateSnapTargets();
		}
		if (Input.GetButtonDown("Wall")) {
			bHideSnapTargets = false;
			building.SetSnapTargetType(SnapTargetBehaviour.SnapTargetType.Wall);
			building.CreateSnapTargets();
		}
		if (Input.GetButtonDown("HideSnapTargets")) {
			bHideSnapTargets = !bHideSnapTargets;
			if (bHideSnapTargets) {
				building.ClearSnapTargets();
			} else {
				building.CreateSnapTargets();
			}

		}
	}
}
