using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EthanolBehaviourSettings {

	public float minSpeed;
	public float maxSpeed;

	[Space(20)]
	public float minWalkTime;
	public float maxWalkTime;
	public float maxWanderDistance;

	[Space(20)]
	public float minPauseTime;
	public float maxPauseTime;

	[Space(20)]
	public float frenzyDist;
	public float frenzyTime;
	public float frenzySpeed;

}
[System.Serializable]
public struct EthanolAudioSettings {

	public float minIdleAudioInterval;
	public float maxIdleAudioInterval;

	[Space(20)]
	public float minAlertVolume;
	public float maxAlertVolume;

	[Space(20)]
	public float stepSize;
	public float stepVariance;

	[Space(20)]
	public float voicePitchVariation;
	public float footstepPitchVariation;

	[Space(20)]
	public EthanolAudioClips audioClips;

}
[System.Serializable]
public struct EthanolAudioClips {

	public AudioClip[] idleAudio;
	public AudioClip[] footstepAudio;
	public AudioClip[] alertAudio;
	public AudioClip[] attackAudio;
	public AudioClip[] frenzyAudio;

}

public class EthanolController : MonoBehaviour {

	public EthanolBehaviourSettings behaviourSettings;
	public EthanolAudioSettings audioSettings;

	Vector2 walkDir;

	
	float walkTimer;

	
	float thisPauseTime;

	bool frenzyStarted;
	float frenzyCount;
	Vector2 frenzyVel;


	
	float cooldownCounter;

	
	Vector3 startPos;
	int mode; // 0 - Cooldown | 1 - wander | 2 - frenzy


	float idleAudioCount;
	float stepCount;

	GameObject player;
	Rigidbody rigidBody;

	RealSpace3D.RealSpace3D_AudioSource idleAudioSource;
	RealSpace3D.RealSpace3D_AudioSource alertAudioSource;
	RealSpace3D.RealSpace3D_AudioSource frenzyAudioSource;
	RealSpace3D.RealSpace3D_AudioSource footAudioSource;


	void Start() {

		player = GameObject.FindGameObjectWithTag("Player");
		rigidBody = GetComponent<Rigidbody>();

		stepCount = audioSettings.stepSize + Random.Range(-audioSettings.stepVariance / 2f, audioSettings.stepVariance / 2f);
		mode = 1;
		startPos = transform.position;
		walkTimer = Random.Range(behaviourSettings.minWalkTime, behaviourSettings.maxWalkTime);
		thisPauseTime = Random.Range(behaviourSettings.minPauseTime, behaviourSettings.maxPauseTime);
		walkDir = Random.insideUnitCircle * Random.Range(behaviourSettings.minSpeed, behaviourSettings.maxSpeed);
		idleAudioSource = transform.GetChild(1).GetComponent<RealSpace3D.RealSpace3D_AudioSource>();
		alertAudioSource = transform.GetChild(2).GetComponent<RealSpace3D.RealSpace3D_AudioSource>();
		frenzyAudioSource = transform.GetChild(3).GetComponent<RealSpace3D.RealSpace3D_AudioSource>();
		footAudioSource = transform.GetChild(4).GetComponent<RealSpace3D.RealSpace3D_AudioSource>();

		frenzyCount = 0f;

		idleAudioCount = Random.Range(audioSettings.minIdleAudioInterval, audioSettings.maxIdleAudioInterval);
	}

	private void OnDrawGizmos() {

//		Gizmos.DrawWireSphere(startPos, behaviourSettings.maxWanderDistance);

	}

	void Update() {

		#region Behaviour

		#region Cooldown

		if (mode == 0) {
			cooldownCounter -= Time.deltaTime;
			if (cooldownCounter <= 0f) {
				mode = 1;
				Debug.Log("Switched to wander");
			}
		}

		#endregion
		#region Wander

		if (mode == 1) {

			walkTimer -= Time.deltaTime;

			if (walkTimer <= 0f) {

				walkDir = Vector2.zero;

			}

			if (walkTimer <= -thisPauseTime) {
				walkTimer = Random.Range(behaviourSettings.minWalkTime, behaviourSettings.maxWalkTime);
				thisPauseTime = Random.Range(behaviourSettings.minPauseTime, behaviourSettings.maxPauseTime);
				walkDir = (Random.insideUnitCircle * behaviourSettings.maxWanderDistance + new Vector2(startPos.x - transform.position.x, startPos.z - transform.position.z)).normalized * Random.Range(behaviourSettings.minSpeed, behaviourSettings.maxSpeed);
			}

			rigidBody.velocity = new Vector3(walkDir.x, 0f, walkDir.y);

		}

		#endregion
		#region Frenzy

		if (mode == 1) {

			float playerDist = (player.transform.position - transform.position).magnitude;
			float normalizedExposure = Mathf.Clamp(((behaviourSettings.frenzyDist - playerDist) / behaviourSettings.frenzyDist), -0.1f, 1f);

			frenzyCount += normalizedExposure * Time.deltaTime * 10f;
			frenzyCount = Mathf.Max(frenzyCount, 0f);

			if (frenzyCount > behaviourSettings.frenzyTime) {
				mode = 2;

				Vector3 normalizedDir = (player.transform.position - transform.position).normalized;
				frenzyVel = new Vector2(normalizedDir.x, normalizedDir.z) * behaviourSettings.frenzySpeed;

				frenzyCount = 0f;

			}

		}

		if (mode == 2) {

			rigidBody.velocity = new Vector3(frenzyVel.x, 0f, frenzyVel.y);
			Debug.Log(frenzyVel);
			if (frenzyAudioSource.rs3d_IsPlaying() == false && frenzyStarted == true) {
				mode = 1;
				frenzyStarted = false;
			}


			if (frenzyVel != Vector2.zero) {
				Ray frenzyRay = new Ray(transform.position, new Vector3(frenzyVel.x, 0f, frenzyVel.y));
				RaycastHit frenzyHit;
				Physics.Raycast(frenzyRay, out frenzyHit, 0.4f);

				if (frenzyHit.transform != null) {
					frenzyVel = Vector2.zero;
				}
			}

			
		}

		#endregion

		#endregion
		#region Audio

		if (mode == 0) {
			if (frenzyAudioSource.rs3d_IsPlaying() == true) {
				frenzyAudioSource.rs3d_StopSound();
			}
		}
		if (mode == 1) {

			if (frenzyCount == 0f) {

				if (alertAudioSource.rs3d_IsPlaying() == true) {
					alertAudioSource.rs3d_StopSound();
				}

				if (idleAudioSource.rs3d_IsPlaying() == false) {
					idleAudioCount -= Time.deltaTime;
				}
				if (idleAudioCount <= 0f) {
					idleAudioCount = Random.Range(audioSettings.minIdleAudioInterval, audioSettings.maxIdleAudioInterval);
					if (audioSettings.audioClips.idleAudio.Length != 0) {
						idleAudioSource.rs3d_LoadAudioClip(audioSettings.audioClips.idleAudio[Random.Range(0, audioSettings.audioClips.idleAudio.Length)]);
						idleAudioSource.rs3d_AdjustPitch(1f * Mathf.Pow(2f, Random.Range(-audioSettings.voicePitchVariation / 2f, audioSettings.voicePitchVariation / 2f)));
						idleAudioSource.rs3d_PlaySound();
					}
				}

			}

			if (frenzyCount > 0f) {

				if (alertAudioSource.rs3d_IsPlaying() == false) {
					if (audioSettings.audioClips.alertAudio.Length != 0) {
						alertAudioSource.rs3d_LoadAudioClip(audioSettings.audioClips.alertAudio[Random.Range(0, audioSettings.audioClips.alertAudio.Length)]);
					}
					alertAudioSource.rs3d_AdjustPitch(1f * Mathf.Pow(2f, Random.Range(-audioSettings.voicePitchVariation / 2f, audioSettings.voicePitchVariation / 2f)));
					alertAudioSource.rs3d_PlaySound();
				}

				alertAudioSource.rs3d_AdjustVolume(Mathf.Lerp(audioSettings.minAlertVolume, audioSettings.maxAlertVolume, frenzyCount / behaviourSettings.frenzyDist));

			}

		}
		if (mode == 2) {

			if (alertAudioSource.rs3d_IsPlaying() == true) {
				alertAudioSource.rs3d_StopSound();
			}
			if (frenzyAudioSource.rs3d_IsPlaying() == false) {
				if (audioSettings.audioClips.frenzyAudio.Length != 0) {
					frenzyAudioSource.rs3d_LoadAudioClip(audioSettings.audioClips.frenzyAudio[Random.Range(0, audioSettings.audioClips.frenzyAudio.Length)]);
					frenzyAudioSource.rs3d_AdjustPitch(1f * Mathf.Pow(2f, Random.Range(-audioSettings.voicePitchVariation / 2f, audioSettings.voicePitchVariation / 2f)));
					frenzyAudioSource.rs3d_PlaySound();
					frenzyStarted = true;
				}
			}
			

		}

		#region Footsteps

		stepCount -= Time.deltaTime * (rigidBody.velocity - new Vector3(0f, rigidBody.velocity.y, 0f)).magnitude;
		if (stepCount <= 0f) {
			stepCount = audioSettings.stepSize + Random.Range(-audioSettings.stepVariance / 2f, audioSettings.stepVariance / 2f);
			if (audioSettings.audioClips.footstepAudio.Length != 0) {
				footAudioSource.rs3d_LoadAudioClip(audioSettings.audioClips.footstepAudio[Random.Range(0, audioSettings.audioClips.footstepAudio.Length)]);
			}
			footAudioSource.rs3d_AdjustPitch(1f * Mathf.Pow(2f, Random.Range(-audioSettings.footstepPitchVariation / 2f, audioSettings.footstepPitchVariation / 2f)));
			footAudioSource.rs3d_PlaySound();

		}


		#endregion

		#endregion


	}

}