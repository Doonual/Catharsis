using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioMaterial", menuName = "ScriptableObjects/AudioMaterial")]
public class AudioMaterial : ScriptableObject {

	public string audioName;
	public AudioClip[] walkAudio;
	public AudioClip[] runAudio;
	public AudioClip[] sprintStopAudio;

}
