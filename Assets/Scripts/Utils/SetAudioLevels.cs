using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetAudioLevels : MonoBehaviour {
	
	public AudioMixer mainMixer;
	public static float volumeSize;

	public void Start(){

		if (PlayerPrefs.HasKey ("Volume")) {

			volumeSize = PlayerPrefs.GetFloat ("Volume");
			mainMixer.SetFloat("musicVol", volumeSize);

		} else {
			volumeSize = 0;
		}

	}

	public void SetMusicLevel(float musicLvl)
	{
		mainMixer.SetFloat("musicVol", musicLvl);
	}
}