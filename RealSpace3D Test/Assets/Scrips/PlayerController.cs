using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[Range(0f, 1f)]
	public float walkAcceleration;
	[Range(0f, 1f)]
	public float runAcceleration;
	[Range(0f, 1f)]
	public float walkFriction;
	[Range(0f, 1f)]
	public float lookSensitivity;

	[HideInInspector]
	public bool sprint;

	public GameObject screenCover;
	Rigidbody rb;
	GameObject cameraObject;
	Camera playerCamera;

	Vector2 walkInput;

	bool canSee = true;

	void Start() {
		rb = GetComponent<Rigidbody>();
		cameraObject = transform.GetChild(0).gameObject;
		playerCamera = cameraObject.GetComponent<Camera>();

		Cursor.lockState = CursorLockMode.Locked;

	}


	void FixedUpdate() {

		walkInput = Vector2.zero;

		if (Input.GetKey(KeyCode.W) == true) {
			walkInput += Vector2.up;
		}
		if (Input.GetKey(KeyCode.S) == true) {
			walkInput += Vector2.down;
		}
		if (Input.GetKey(KeyCode.A) == true) {
			walkInput += Vector2.left;
		}
		if (Input.GetKey(KeyCode.D) == true) {
			walkInput += Vector2.right;
		}
		if (walkInput.magnitude != 0f) {
			if (sprint == false) {
				walkInput = walkInput.normalized * walkAcceleration;
			}
			else {
				walkInput = walkInput.normalized * runAcceleration;
			}
		}

		rb.velocity += (transform.forward * walkInput.y + transform.right * walkInput.x);
		rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z) * walkFriction + new Vector3(0f, rb.velocity.y, 0f);


	}

	void Update() {

		Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * lookSensitivity;

		if (Input.GetKey(KeyCode.Escape) == true) {
			Cursor.lockState = CursorLockMode.None;
		}
		if (Input.GetMouseButton(0) == true) {
			Cursor.lockState = CursorLockMode.Locked;
		}
		if (Input.GetKeyDown(KeyCode.Tab) == true) {
			canSee = canSee != true;
			screenCover.SetActive(!canSee);
		}
		sprint = Input.GetKey(KeyCode.LeftShift);

		if (Cursor.lockState == CursorLockMode.Locked) {
			cameraObject.transform.rotation = Quaternion.Euler(cameraObject.transform.rotation.eulerAngles + new Vector3(mouseInput.y, 0f, 0f));
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, mouseInput.x, 0f));
		}

	}
}
