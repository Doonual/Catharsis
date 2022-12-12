using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioPropertiesPreset {

	public enum AudioMaterial {

		wood,
		woodCreaky,
		tile,
		custom,


	};

	public static AudioClip[] woodWalkFootsteps = {
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Wood_Walk1"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Wood_Walk2"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Wood_Walk3"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Wood_Walk4"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Wood_Walk5"),
	};
	public static AudioClip[] woodRunFootsteps = {
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Wood_Running1"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Wood_Running2"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Wood_Running3"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Wood_Running4"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Wood_Running5"),
	};

	public static AudioClip[] woodCreakyWalkFootsteps = {
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Walk_Creak1"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Walk_Creak2"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Walk_Creak3"),
	};
	public static AudioClip[] woodCreakyRunFootsteps = {
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Sprint_Creak1"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Sprint_Creak2"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Wood/Sprint_Creak3"),
	};


	public static AudioClip[] tileWalkFootsteps = {
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Tile/Tile_Walk1"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Tile/Tile_Walk2"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Tile/Tile_Walk3"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Tile/Tile_Walk4"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Tile/Tile_Walk5"),
	};
	public static AudioClip[] tileRunFootsteps = {
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Tile/Tile_Run1"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Tile/Tile_Run2"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Tile/Tile_Run3"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Tile/Tile_Run4"),
		Resources.Load<AudioClip>("Audio/Sound effects/Footsteps/Tile/Tile_Run5"),
	};


	public static AudioClip[] ShuffleArray(AudioClip[] array) {

		AudioClip[] tempArray = new AudioClip[array.Length];
		for (int i = 0; i < array.Length; i++) {
			tempArray[i] = array[i];
		}
		for (int i = 0; i < array.Length; i++) {
			AudioClip temp;
			temp = tempArray[i];
			int rand = Random.Range(0, array.Length);
			tempArray[i] = tempArray[rand];
			tempArray[rand] = temp;
		}

		return tempArray;

	}

}
