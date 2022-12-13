using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EthanolController : MonoBehaviour {

	public float minSpeed;
	public float maxSpeed;
	Vector2 walkDir;

	[Space(20)]
	public float minWalkTime;
	public float maxWalkTime;
	float walkTimer;

	[Space(20)]
	public float minPauseTime;
	public float maxPauseTime;
	float thisPauseTime;

	[Space(20)]
	public float frenzyDist;
	public float frenzyTime;
	public float frenzySpeed;
	public float frenzyCount;
	Vector2 frenzyVel;


	[Space(20)]
	public float cooldownTime;
	float cooldownCounter;

	
	Vector3 startPos;
	int mode; // 0 - Cooldown | 1 - wander | 2 - frenzy

	public GameObject player;
	Rigidbody rigidBody;

    void Start() {

		player = GameObject.FindGameObjectWithTag("Player");
		rigidBody = GetComponent<Rigidbody>();

		mode = 1;
		startPos = transform.position;
		walkTimer = Random.Range(minWalkTime, maxWalkTime);
		thisPauseTime = Random.Range(minPauseTime, maxPauseTime);
		walkDir = Random.insideUnitCircle * Random.Range(minSpeed, maxSpeed);

		frenzyCount = 0f;

	}

    
    void Update() {

		#region Cooldown
		
		if (mode == 0) {
			cooldownCounter -= Time.deltaTime;
			if (cooldownCounter <= 0f) {
				mode = 1;
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
				walkTimer = Random.Range(minWalkTime, maxWalkTime);
				thisPauseTime = Random.Range(minPauseTime, maxPauseTime);
				walkDir = Random.insideUnitCircle.normalized * Random.Range(minSpeed, maxSpeed) + Random.Range(0f, minSpeed) * new Vector2(startPos.x - transform.position.x, startPos.z - transform.position.z);
			}

			rigidBody.velocity = new Vector3(walkDir.x, 0f, walkDir.y);

		}

		#endregion
		#region Frenzy

		if (mode == 1) {

			float playerDist = (player.transform.position - transform.position).magnitude;
			float normalizedExposure = Mathf.Clamp((frenzyDist - playerDist) / frenzyDist, 0f, 1f) * 2f - 1f;

			frenzyCount += normalizedExposure * Time.deltaTime * 10f;
			frenzyCount = Mathf.Max(frenzyCount, 0f);

			if (frenzyCount > frenzyTime) {
				mode = 2;

				Vector3 normalizedDir = (player.transform.position - transform.position).normalized;
				frenzyVel = new Vector2(normalizedDir.x, normalizedDir.z) * frenzySpeed;

			}

		}

		if (mode == 2) {

			rigidBody.velocity = new Vector3(frenzyVel.x, 0f, frenzyVel.y);

			Ray frenzyRay = new Ray(transform.position, new Vector3(frenzyVel.x, 0f, frenzyVel.y));
			RaycastHit frenzyHit;
			Physics.Raycast(frenzyRay, out frenzyHit, 0.5f);

			if (frenzyHit.transform != null) {
				if (frenzyHit.transform.tag != "Player") {
					mode = 0;
					cooldownCounter = cooldownTime;
					frenzyCount = 0f;
				}
			}


		}

		#endregion

	}
}
