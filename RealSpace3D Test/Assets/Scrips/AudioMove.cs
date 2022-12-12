using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMove : MonoBehaviour {

	MeshRenderer mr;

	private void Start() {

		mr = GetComponent<MeshRenderer>();

	}

	void Update() {
		
		if (Input.GetKeyDown(KeyCode.K) == true) {
			transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(0f, 2.5f), Random.Range(-5f, 5f));
		}
		if (Input.GetKeyDown(KeyCode.L) == true) {
			mr.enabled = mr.enabled != true;
		}

	}
}
