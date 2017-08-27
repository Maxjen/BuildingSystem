using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour {

	GameObject[] edges = new GameObject[2];

	public GameObject edgePrefab;

	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	public GameObject[] CreateEdges() {
		var vertex1 = new Vector3(0.0f, 0.0f, 0.0f);
		var vertex2 = new Vector3(0.0f, 5.0f, 0.0f);

		var Result = new GameObject[2];
		Result[0] = Instantiate(edgePrefab, transform.TransformPoint(vertex1), transform.rotation);
		Result[0].GetComponent<EdgeBehaviour>().SetWall(1, gameObject);
		Result[1] = Instantiate(edgePrefab, transform.TransformPoint(vertex2), transform.rotation);
		Result[1].GetComponent<EdgeBehaviour>().SetWall(0, gameObject);
		return Result;
	}
}
