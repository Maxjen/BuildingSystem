using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeBehaviour : MonoBehaviour {

	public const float edgeLength = 5.0f;
	public const float triangleOffset = 0.28867513459f * edgeLength;

	GameObject[] floors = new GameObject[2];
	GameObject[] walls = new GameObject[2];
	//public Vector3[] vertices = new Vector3[2];

	// Use this for initialization
	void Start() {
		/*vertices[0] = transform.TransformVector(new Vector3(0.0f, 0.0f, -edgeLength * 0.5f));
		vertices[1] = transform.TransformVector(new Vector3(0.0f, 0.0f, edgeLength * 0.5f));*/
	}
	
	// Update is called once per frame
	void Update() {
	}

	public bool IsEqual(EdgeBehaviour other) {
		Vector3[] vertices = GetEdgeVertices();
		Vector3[] verticesOther = other.GetEdgeVertices();

		foreach (var vertex in vertices) {
			bool bFoundVertex = false;
			foreach (var vertexOther in verticesOther) {
				if (vertex == vertexOther) {
					bFoundVertex = true;
					break;
				}
			}
			if (!bFoundVertex) {
				return false;
			}
		}
		return true;
	}

	public void SetFloor(int index, GameObject floor) {
		floors[index] = floor;
	}

	public void SetWall(int index, GameObject wall) {
		walls[index] = wall;
	}

	public void AddFloor(GameObject floor) {
		var positionRel = transform.InverseTransformPoint(floor.transform.position);
		if (positionRel.x < 0) {
			floors[0] = floor;
		} else {
			floors[1] = floor;
		}
	}

	public void AddWall(GameObject wall) {
		var positionRel = transform.InverseTransformPoint(wall.transform.position);
		if (positionRel.y < 0) {
			walls[0] = wall;
		} else {
			walls[1] = wall;
		}
	}

	public SnapCoord[] GetFloorSquareSnapCoords() {
		var Result = new SnapCoord[2];
		if (!floors[0])
			Result[0] = new SnapCoord(transform.TransformPoint(new Vector3(-edgeLength * 0.5f, 0.0f, 0.0f)), transform.rotation);
		if (!floors[1])
			Result[1] = new SnapCoord(transform.TransformPoint(new Vector3(edgeLength * 0.5f, 0.0f, 0.0f)), transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f));
		return Result;
	}

	public SnapCoord[] GetFloorTriangleSnapCoords() {
		var Result = new SnapCoord[2];
		if (!floors[0])
			Result[0] = new SnapCoord(transform.TransformPoint(new Vector3(-triangleOffset, 0.0f, 0.0f)), transform.rotation);
		if (!floors[1])
			Result[1] = new SnapCoord(transform.TransformPoint(new Vector3(triangleOffset, 0.0f, 0.0f)), transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f));
		return Result;
	}

	public SnapCoord GetWallSnapCoord() {
		if (!walls[1]) {
			return new SnapCoord(transform.position, transform.rotation);
		}
		return null;
	}

	Vector3[] GetEdgeVertices() {
		float halfEdgeLength = edgeLength * 0.5f;
		Vector3[] Result = new Vector3[2];
		Result[0] = transform.TransformPoint(new Vector3(0.0f, 0.0f, -halfEdgeLength));
		Result[1] = transform.TransformPoint(new Vector3(0.0f, 0.0f, halfEdgeLength));
		return Result;
	}
}
