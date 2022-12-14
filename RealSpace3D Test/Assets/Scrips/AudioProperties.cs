using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioProperties : MonoBehaviour {

	public AudioMaterial material;

	private void Awake() {
		gameObject.layer = 11;
	}

}
