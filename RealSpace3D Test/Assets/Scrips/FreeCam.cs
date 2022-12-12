using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour {

	[Range(0f, 1f)]
	public float acceleration;
	[Range(0f, 1f)]
	public float friction;
	[Range(0f, 1f)]
	public float lookSensitivity;

	Vector3 vel;

    void Start() {
		vel = Vector3.zero;
		Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void FixedUpdate() {
        
		if (Input.GetKey(KeyCode.W) == true) {
			vel += transform.forward * acceleration;
		}
		if (Input.GetKey(KeyCode.S) == true) {
			vel -= transform.forward * acceleration;
		}
		if (Input.GetKey(KeyCode.D) == true) {
			vel += transform.right * acceleration;
		}
		if (Input.GetKey(KeyCode.A) == true) {
			vel -= transform.right * acceleration;
		}
		if (Input.GetKey(KeyCode.E) == true) {
			vel += transform.up * acceleration;
		}
		if (Input.GetKey(KeyCode.Q) == true) {
			vel -= transform.up * acceleration;
		}

		transform.position += vel;
		vel *= friction;


	}

	void Update() {

		Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * lookSensitivity;

		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(-mouseInput.y, mouseInput.x, 0f));

	}
}
