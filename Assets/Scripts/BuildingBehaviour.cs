using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviour : MonoBehaviour {

	SnapTargetBehaviour.SnapTargetType snapTargetType;

	List<GameObject> buildingParts = new List<GameObject>();
	List<GameObject> edges = new List<GameObject>();

	List<GameObject> snapTargets = new List<GameObject>();

	public GameObject floorSquarePrefab;
	public GameObject floorSquareSnapTargetPrefab;
	public GameObject floorTrianglePrefab;
	public GameObject floorTriangleSnapTargetPrefab;
	public GameObject wallPrefab;
	public GameObject wallSnapTargetPrefab;

	void AddFloorSquare(Vector3 position, Quaternion rotation) {
		var floor = Instantiate(floorSquarePrefab, position, rotation);
		buildingParts.Add(floor);

		var floorBehaviour = floor.GetComponent<FloorSquareBehaviour>();
		var newEdges = floorBehaviour.CreateEdges();
		foreach (var edge in newEdges) {
			edges.Add(edge);
		}
	}

	void AddFloorTriangle(Vector3 position, Quaternion rotation) {
		var floor = Instantiate(floorTrianglePrefab, position, rotation);
		buildingParts.Add(floor);

		var floorBehaviour = floor.GetComponent<FloorTriangleBehaviour>();
		var newEdges = floorBehaviour.CreateEdges();
		foreach (var edge in newEdges) {
			edges.Add(edge);
		}
	}

	// Use this for initialization
	void Start() {
		AddFloorTriangle(Vector3.zero, Quaternion.identity);
		//AddFloorSquare(Vector3.zero, Quaternion.identity);
		//AddFloorSquare(new Vector3(10.0f, 0.0f, 0.0f), Quaternion.identity);

		/*var halfEdgeLength = EdgeBehaviour.edgeLength * 0.5f;
		var position = new Vector3(halfEdgeLength, 0.0f, halfEdgeLength);
		var offset = Quaternion.Euler(0.0f, -120.0f, 0.0f) * new Vector3(halfEdgeLength, 0.0f, -halfEdgeLength);
		AddFloorSquare(position + offset, Quaternion.Euler(0.0f, 60.0f, 0.0f));*/

		CreateFloorTriangleSnapTargets();
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	public void ClearSnapTargets() {
		foreach (var snapTarget in snapTargets) {
			Destroy(snapTarget);
		}
		snapTargets.Clear();
	}

	void AddSnapTarget(GameObject snapTargetPrefab, SnapCoord snapCoord) {
		var newSnapTarget = Instantiate(snapTargetPrefab, snapCoord.position, snapCoord.rotation);
		var newSnapTargetBehaviour = newSnapTarget.GetComponent<SnapTargetBehaviour>();
		var bExistsAlready = false;
		foreach (var snapTarget in snapTargets) {
			var snapTargetBehaviour = snapTarget.GetComponent<SnapTargetBehaviour>();
			if (snapTargetBehaviour.IsEqual(newSnapTargetBehaviour)) {
				bExistsAlready = true;
				break;
			}
		}
		
		if (!bExistsAlready)
			snapTargets.Add(newSnapTarget);
		else
			Destroy(newSnapTarget);
	}

	void PlaceFloor(GameObject newFloor, GameObject[] newEdges) {
		foreach (var edge in newEdges) {
			var bExistsAlready = false;
			var edgeBehaviour = edge.GetComponent<EdgeBehaviour>();
			foreach (var otherEdge in edges) {
				var otherEdgeBehaviour = otherEdge.GetComponent<EdgeBehaviour>();
				if (edgeBehaviour.IsEqual(otherEdgeBehaviour)) {
					otherEdgeBehaviour.AddFloor(newFloor);
					bExistsAlready = true;
					break;
				}
			}
			if (!bExistsAlready)
				edges.Add(edge);
			else
				Destroy(edge);
		}
	}

	void PlaceWall(GameObject newWall, GameObject[] newEdges) {
		foreach (var edge in newEdges) {
			var bExistsAlready = false;
			var edgeBehaviour = edge.GetComponent<EdgeBehaviour>();
			foreach (var otherEdge in edges) {
				var otherEdgeBehaviour = otherEdge.GetComponent<EdgeBehaviour>();
				if (edgeBehaviour.IsEqual(otherEdgeBehaviour)) {
					otherEdgeBehaviour.AddWall(newWall);
					bExistsAlready = true;
					break;
				}
			}
			if (!bExistsAlready)
				edges.Add(edge);
			else
				Destroy(edge);
		}
	}

	public void PlaceBuildingPart(Transform snapTarget) {
		var snapTargetBehaviour = snapTarget.GetComponent<SnapTargetBehaviour>();

		GameObject newBuildingPart = null;
		GameObject[] newEdges = new GameObject[0];
		switch (snapTargetBehaviour.type) {
		case SnapTargetBehaviour.SnapTargetType.FloorSquare:
			newBuildingPart = Instantiate(floorSquarePrefab, snapTarget.position, snapTarget.rotation);
			newEdges = newBuildingPart.GetComponent<FloorSquareBehaviour>().CreateEdges();
			PlaceFloor(newBuildingPart, newEdges);
			break;
		case SnapTargetBehaviour.SnapTargetType.FloorTriangle:
			newBuildingPart = Instantiate(floorTrianglePrefab, snapTarget.position, snapTarget.rotation);
			newEdges = newBuildingPart.GetComponent<FloorTriangleBehaviour>().CreateEdges();
			PlaceFloor(newBuildingPart, newEdges);
			break;
		case SnapTargetBehaviour.SnapTargetType.Wall:
			newBuildingPart = Instantiate(wallPrefab, snapTarget.position, snapTarget.rotation);
			newEdges = newBuildingPart.GetComponent<WallBehaviour>().CreateEdges();
			PlaceWall(newBuildingPart, newEdges);
			break;
		default:
			break;
		}

		snapTargetType = snapTargetBehaviour.type;
		CreateSnapTargets();
	}

	public void SetSnapTargetType(SnapTargetBehaviour.SnapTargetType snapTargetType) {
		this.snapTargetType = snapTargetType;
	}

	public void CreateSnapTargets() {
		ClearSnapTargets();

		switch (this.snapTargetType) {
		case SnapTargetBehaviour.SnapTargetType.FloorSquare:
			CreateFloorSquareSnapTargets();
			break;
		case SnapTargetBehaviour.SnapTargetType.FloorTriangle:
			CreateFloorTriangleSnapTargets();
			break;
		case SnapTargetBehaviour.SnapTargetType.Wall:
			CreateWallSnapTargets();
			break;
		default:
			break;
		}
	}

	void CreateFloorSquareSnapTargets() {
		foreach (var edge in edges) {
			var edgeBehaviour = edge.GetComponent<EdgeBehaviour>();
			if (edgeBehaviour) {
				var snapCoords = edgeBehaviour.GetFloorSquareSnapCoords();
				if (snapCoords[0] != null)
					AddSnapTarget(floorSquareSnapTargetPrefab, snapCoords[0]);
				if (snapCoords[1] != null)
					AddSnapTarget(floorSquareSnapTargetPrefab, snapCoords[1]);
			}
		}
	}

	void CreateFloorTriangleSnapTargets() {
		foreach (var edge in edges) {
			var edgeBehaviour = edge.GetComponent<EdgeBehaviour>();
			if (edgeBehaviour) {
				var snapCoords = edgeBehaviour.GetFloorTriangleSnapCoords();
				if (snapCoords[0] != null)
					AddSnapTarget(floorTriangleSnapTargetPrefab, snapCoords[0]);
				if (snapCoords[1] != null)
					AddSnapTarget(floorTriangleSnapTargetPrefab, snapCoords[1]);
			}
		}
	}

	void CreateWallSnapTargets() {
		foreach (var edge in edges) {
			var edgeBehaviour = edge.GetComponent<EdgeBehaviour>();
			if (edgeBehaviour) {
				var snapCoord = edgeBehaviour.GetWallSnapCoord();
				if (snapCoord != null)
					AddSnapTarget(wallSnapTargetPrefab, snapCoord);
			}
		}
	}
}
