/* Module      : VolumeControl.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This control the volume of the game
 *
 * Date        : 2015/1/26
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeControl : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Variables to store the boundary distances from the origin
	static float volume;
	static float musicVolume;
	static float voiceVolume;

	public Slider volumeSlider;
	public Slider musicSlider;
	public Slider voiceSlider;

	public GameObject portraitAudio;
	
	void Start () {
		volume = PlayerPrefs.GetFloat ("MasterVolume", 0);
		musicVolume = PlayerPrefs.GetFloat ("MusicVolume", 0);
		voiceVolume = PlayerPrefs.GetFloat ("VoiceVolume", 0);

		audio.volume = musicVolume;
		AudioListener.volume = volume;
		portraitAudio.audio.volume = voiceVolume;

		musicSlider.value = musicVolume;
		volumeSlider.value = volume;
		voiceSlider.value = voiceVolume;
	}

	public void SetVolume (float newVolume) {

		volume = newVolume;
		AudioListener.volume = volume;

	}

	public void SetMusic (float newVolume) {

		musicVolume = newVolume;
		audio.volume = musicVolume;

	}

	public void SetVoice(float newVolume) {

		voiceVolume = newVolume;
		portraitAudio.audio.volume = voiceVolume;

	}

}
