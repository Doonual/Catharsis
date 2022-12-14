using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerMovementSettings {

	[Range(0f, 1f)]
	public float walkAcceleration;
	[Range(0f, 1f)]
	public float runAcceleration;
	[Range(0f, 1f)]
	public float walkFriction;
	[Range(0f, 1f)]
	public float lookSensitivity;
	public bool enableVerticalLooking;

}

[System.Serializable]
public struct PlayerFootstepSettings {

	public float stepSize;
	public float stepVariance;

}

[System.Serializable]
public struct PlayerAudioSettings {

	public PlayerFootstepSettings footstepSettings;

}

public class PlayerController : MonoBehaviour {

	public PlayerMovementSettings movementSettings;
	public PlayerAudioSettings audioSettings;

	bool sprint;

	bool canSee = true;

	AudioClip[] footstepAudio;
	float thisStepSize;
	float stepCount;

	Vector2 walkInput;

	RealSpace3D.RealSpace3D_AudioSource footAudio;
	GameObject screenCover;
	Rigidbody rigidBody;
	GameObject cameraObject;

	void Start() {

		footAudio = transform.GetChild(1).GetChild(0).GetComponent<RealSpace3D.RealSpace3D_AudioSource>();
		screenCover = transform.GetChild(3).GetChild(0).gameObject;
		cameraObject = transform.GetChild(0).gameObject;
		rigidBody = GetComponent<Rigidbody>();
		
		Cursor.lockState = CursorLockMode.Locked;

		thisStepSize = audioSettings.footstepSettings.stepSize + Random.Range(-audioSettings.footstepSettings.stepVariance / 2f, audioSettings.footstepSettings.stepVariance / 2f);
		stepCount = thisStepSize;

	}


	void FixedUpdate() {

		#region Movement

		walkInput = Vector2.zero;

		sprint = Input.GetKey(KeyCode.LeftShift);

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
				walkInput = walkInput.normalized * movementSettings.walkAcceleration;
			}
			else {
				walkInput = walkInput.normalized * movementSettings.runAcceleration;
			}
		}

		rigidBody.velocity += (transform.forward * walkInput.y + transform.right * walkInput.x);
		rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z) * movementSettings.walkFriction + new Vector3(0f, rigidBody.velocity.y, 0f);

		#endregion

	}

	void Update() {

		#region Looking

		Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * movementSettings.lookSensitivity;

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
		

		if (Cursor.lockState == CursorLockMode.Locked) {
			cameraObject.transform.rotation = Quaternion.Euler(cameraObject.transform.rotation.eulerAngles + new Vector3(mouseInput.y * (movementSettings.enableVerticalLooking == true ? 1f : 0f), 0f, 0f));
			if (movementSettings.enableVerticalLooking == false) {
				cameraObject.transform.rotation = Quaternion.Euler(cameraObject.transform.rotation.eulerAngles - new Vector3(cameraObject.transform.rotation.eulerAngles.x, 0f, 0f));
			}
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, mouseInput.x, 0f));
		}

		#endregion
		#region Footsteps

		Ray groundRay = new Ray(transform.position + new Vector3(0f, 0.5f, 0f), Vector3.down);
		RaycastHit rayHit;
		LayerMask mask = LayerMask.GetMask("Footsteps");
		Physics.Raycast(groundRay, out rayHit, 999f, mask.value);
		GameObject groundObj = rayHit.collider.gameObject;

		AudioProperties groundAudioProp = groundObj.GetComponent<AudioProperties>();
		if (groundAudioProp != null) {

			if (sprint == false) {
				footstepAudio = groundAudioProp.material.walkAudio;
			}
			else {
				footstepAudio = groundAudioProp.material.runAudio;
			}

		}

		stepCount -= Time.deltaTime * (rigidBody.velocity - new Vector3(0f, rigidBody.velocity.y, 0f)).magnitude;
		if (stepCount <= 0f) {
			thisStepSize = audioSettings.footstepSettings.stepSize + Random.Range(-audioSettings.footstepSettings.stepVariance / 2f, audioSettings.footstepSettings.stepVariance / 2f);
			stepCount = thisStepSize;
			footAudio.rs3d_LoadAudioClip(footstepAudio[Random.Range(0, footstepAudio.Length)]);
			footAudio.rs3d_PlaySound();
		}

		#endregion

	}
}
