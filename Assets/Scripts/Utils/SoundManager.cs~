using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static bool musicOff = false;
	public AudioSource musicSource;
	public AudioClip[] musicClips;

	public void Start(){
	
		if (true) {
			if (PlayerPrefs.HasKey ("soundoff")) {
			
				if (PlayerPrefs.GetInt ("soundoff") == 1) {
					musicOff = true;
					musicSource.mute = musicOff;
				} else {
					musicOff = false;
					musicSource.mute = musicOff;
				}
			} else {
				musicOff = musicSource.mute;
				//musicOff = false;
				//musicSource.mute = musicOff;
			}
		}
	}

	public static void MusicOFF(bool state){
		GameObject.Find ("MusicManager").GetComponent<SoundManager> ().musicSource.mute = state;
	}

	public void OnMuteButtonClick(){

		musicSource.mute = !musicSource.mute;
		musicOff = !musicOff;

		if(musicOff)
			PlayerPrefs.SetInt ("soundoff", 	1 );
		else
			PlayerPrefs.SetInt ("soundoff", 	0 );

	}

	public static bool getState(){
		if (PlayerPrefs.HasKey ("soundoff")) {
						
			if (PlayerPrefs.GetInt ("soundoff") == 1){
				return true;
				//musicOff = true;
				//musicSource.mute = musicOff;
			}
			else{
				return false;
				//musicOff = false;
				//musicSource.mute = musicOff;
			}
		} else {
			return false;
			//musicOff = false;
			//musicSource.mute = musicOff;
		}
	}

	public static void InitMuteState(Toggle muteButton){

		muteButton.gameObject.SetActive (false);
		muteButton.isOn = !SoundManager.getState();
		muteButton.gameObject.SetActive (true);

	}

	public void PlaySelectedMusic(int musicChoice)
	{
		//Play the music clip at the array index musicChoice
		musicSource.clip = musicClips [musicChoice];
		
		//Play the selected clip
		musicSource.Play ();
	}

	public static void ChoosePlayMusic(int musicChoice)
	{
		GameObject.Find ("MusicManager").GetComponent<SoundManager> ().PlaySelectedMusic (musicChoice);
	}
}
