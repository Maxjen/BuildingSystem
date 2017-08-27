using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTriangleBehaviour : MonoBehaviour {

	GameObject[] edges = new GameObject[3];

	public GameObject edgePrefab;

	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	public GameObject[] CreateEdges() {
		var vertex1 = new Vector3(EdgeBehaviour.triangleOffset, 0.0f, 0.0f);
		var vertex2 = Quaternion.Euler(0.0f, 120.0f, 0.0f) * vertex1;
		var vertex3 = Quaternion.Euler(0.0f, -120.0f, 0.0f) * vertex1;

		var Result = new GameObject[3];
		Result[0] = Instantiate(edgePrefab, transform.TransformPoint(vertex1), transform.rotation * Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)));
		Result[0].GetComponent<EdgeBehaviour>().SetFloor(1, gameObject);
		Result[1] = Instantiate(edgePrefab, transform.TransformPoint(vertex2), transform.rotation * Quaternion.Euler(new Vector3(0.0f, -60.0f, 0.0f)));
		Result[1].GetComponent<EdgeBehaviour>().SetFloor(1, gameObject);
		Result[2] = Instantiate(edgePrefab, transform.TransformPoint(vertex3), transform.rotation * Quaternion.Euler(new Vector3(0.0f, 60.0f, 0.0f)));
		Result[2].GetComponent<EdgeBehaviour>().SetFloor(1, gameObject);
		return Result;
	}
}
