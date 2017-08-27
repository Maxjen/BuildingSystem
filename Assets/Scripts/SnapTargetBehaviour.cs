using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTargetBehaviour : MonoBehaviour {

	public enum SnapTargetType {
		FloorSquare,
		FloorTriangle,
		Wall
	}

	public SnapTargetType type;
	public Material normalMaterial;
	public Material highlightMaterial;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Building")) {
			Destroy(gameObject);
		}
	}

	public bool IsEqual(SnapTargetBehaviour Other) {
		Vector3[] vertices;
		Vector3[] verticesOther;

		switch (type) {
		case SnapTargetType.FloorSquare:
			vertices = GetSquareVertices();
			verticesOther = Other.GetSquareVertices();
			break;
		case SnapTargetType.FloorTriangle:
			vertices = GetTriangleVertices();
			verticesOther = Other.GetTriangleVertices();
			break;
		default:
			vertices = GetSquareVertices();
			verticesOther = Other.GetSquareVertices();
			break;
		}

		foreach (var vertex in vertices) {
			bool bFoundVertex = false;
			foreach (var vertexOther in verticesOther) {
				//if (Vector3.SqrMagnitude(vertex - vertexOther) < 0.01f) {
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

	public void SetHighlight(bool bHighlight) {
		var mesh = transform.Find("Mesh");
		var meshRenderer = mesh.GetComponent<MeshRenderer>();

		if (bHighlight) {
			meshRenderer.material = highlightMaterial;
		} else {
			meshRenderer.material = normalMaterial;
		}
	}

	Vector3[] GetSquareVertices() {
		float halfEdgeLength = EdgeBehaviour.edgeLength * 0.5f;
		Vector3[] Result = new Vector3[4];
		Result[0] = transform.TransformPoint(new Vector3(-halfEdgeLength, 0.0f, -halfEdgeLength));
		Result[1] = transform.TransformPoint(new Vector3(-halfEdgeLength, 0.0f, halfEdgeLength));
		Result[2] = transform.TransformPoint(new Vector3(halfEdgeLength, 0.0f, halfEdgeLength));
		Result[3] = transform.TransformPoint(new Vector3(halfEdgeLength, 0.0f, -halfEdgeLength));
		return Result;
	}

	Vector3[] GetTriangleVertices() {
		var vertex1 = new Vector3(-EdgeBehaviour.edgeLength * 0.5f, 0.0f, -EdgeBehaviour.triangleOffset);
		var vertex2 = Quaternion.Euler(0.0f, 120.0f, 0.0f) * vertex1;
		var vertex3 = Quaternion.Euler(0.0f, -120.0f, 0.0f) * vertex1;

		Vector3[] Result = new Vector3[3];
		Result[0] = transform.TransformPoint(vertex1);
		Result[1] = transform.TransformPoint(vertex2);
		Result[2] = transform.TransformPoint(vertex3);
		return Result;
	}
}
