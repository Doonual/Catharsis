using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour {

	public RealSpace3D.RealSpace3D_AudioSource leftFootAudio;
	public RealSpace3D.RealSpace3D_AudioSource rightFootAudio;

	AudioClip[] footstepAudio;

	public float stepSize;
	public float stepVariance;
	float thisStepSize;
	float stepCount;
	bool otherDone;

	Rigidbody rigidBody;
	PlayerController playerController;

	void Start() {
		thisStepSize = stepSize + Random.Range(-stepVariance / 2f, stepVariance / 2f);
		stepCount = thisStepSize;
		otherDone = false;
		rigidBody = GetComponent<Rigidbody>();
		playerController = GetComponent<PlayerController>();
	}


	void Update() {

		Ray groundRay = new Ray(transform.position + new Vector3(0f, 0.5f, 0f), Vector3.down);
		RaycastHit rayHit;
		LayerMask mask = LayerMask.GetMask("Footsteps");
		Physics.Raycast(groundRay, out rayHit, 999f, mask.value);
		GameObject groundObj = rayHit.collider.gameObject;
		
		AudioProperties groundAudioProp = groundObj.GetComponent<AudioProperties>();
		if (groundAudioProp != null) {

			if (groundAudioProp.material == AudioPropertiesPreset.AudioMaterial.wood) {
				if (playerController.sprint == false) {
					footstepAudio = AudioPropertiesPreset.woodWalkFootsteps;
				}
				else {
					footstepAudio = AudioPropertiesPreset.woodRunFootsteps;
				}
			}

			if (groundAudioProp.material == AudioPropertiesPreset.AudioMaterial.woodCreaky) {
				if (playerController.sprint == false) {
					footstepAudio = AudioPropertiesPreset.woodCreakyWalkFootsteps;
				}
				else {
					footstepAudio = AudioPropertiesPreset.woodCreakyRunFootsteps;
				}
			}

			if (groundAudioProp.material == AudioPropertiesPreset.AudioMaterial.tile) {
				if (playerController.sprint == false) {
					footstepAudio = AudioPropertiesPreset.tileWalkFootsteps;
				}
				else {
					footstepAudio = AudioPropertiesPreset.tileRunFootsteps;
				}
			}
		}

		stepCount -= Time.deltaTime * (rigidBody.velocity - new Vector3(0f, rigidBody.velocity.y, 0f)).magnitude;
		if (stepCount <= thisStepSize / 2f && otherDone == false) {
			otherDone = true;
			rightFootAudio.rs3d_LoadAudioClip(footstepAudio[Random.Range(0, footstepAudio.Length)]);
			rightFootAudio.rs3d_PlaySound();
		}
		if (stepCount <= 0f) {
			otherDone = false;
			thisStepSize = stepSize + Random.Range(-stepVariance / 2f, stepVariance / 2f);
			stepCount = thisStepSize;
			leftFootAudio.rs3d_LoadAudioClip(footstepAudio[Random.Range(0, footstepAudio.Length)]);
			leftFootAudio.rs3d_PlaySound();
		}

	}
}
