using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSquareBehaviour : MonoBehaviour {

	GameObject[] edges = new GameObject[4];

	public GameObject edgePrefab;

	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	public GameObject[] CreateEdges() {
		float halfEdgeLength = EdgeBehaviour.edgeLength * 0.5f;
		var Result = new GameObject[4];
		Result[0] = Instantiate(edgePrefab, transform.TransformPoint(new Vector3(-halfEdgeLength, 0.0f, 0.0f)), transform.rotation);
		Result[0].GetComponent<EdgeBehaviour>().SetFloor(1, gameObject);
		Result[1] = Instantiate(edgePrefab, transform.TransformPoint(new Vector3(0.0f, 0.0f, halfEdgeLength)), transform.rotation * Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f)));
		Result[1].GetComponent<EdgeBehaviour>().SetFloor(1, gameObject);
		Result[2] = Instantiate(edgePrefab, transform.TransformPoint(new Vector3(halfEdgeLength, 0.0f, 0.0f)), transform.rotation * Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)));
		Result[2].GetComponent<EdgeBehaviour>().SetFloor(1, gameObject);
		Result[3] = Instantiate(edgePrefab, transform.TransformPoint(new Vector3(0, 0.0f, -halfEdgeLength)), transform.rotation * Quaternion.Euler(new Vector3(0.0f, 270.0f, 0.0f)));
		Result[3].GetComponent<EdgeBehaviour>().SetFloor(1, gameObject);
		return Result;
	}
}
