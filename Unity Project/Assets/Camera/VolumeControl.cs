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
	static float soundEffectsVolume;

	public Slider volumeSlider;
	public Slider musicSlider;
	public Slider voiceSlider;
	public Slider soundEffectsSlider;

	public GameObject portraitAudio;
	public GameObject audioHandlerObject;
	AudioHandler audioHandler;

	void Start () {
		volume = PlayerPrefs.GetFloat ("MasterVolume", 0);
		musicVolume = PlayerPrefs.GetFloat ("MusicVolume", 0);
		voiceVolume = PlayerPrefs.GetFloat ("VoiceVolume", 0);
		soundEffectsVolume = PlayerPrefs.GetFloat ("SoundEffectsVolume", 0);

		//Music volume
		audio.volume = .05f * musicVolume;
		musicSlider.value = musicVolume;

		//Portrait Volume
		portraitAudio.audio.volume = voiceVolume;
		voiceSlider.value = voiceVolume;

		//Master volume
		AudioListener.volume = volume;
		volumeSlider.value = volume;

		//Find the audiohandler script
		audioHandler = audioHandlerObject.GetComponent<AudioHandler> ();

		//Set the audio of all sound effects
		audioHandler.setSoundEffectsVolume (.2f * soundEffectsVolume);
		soundEffectsSlider.value = soundEffectsVolume;

	}

	public void SetVolume (float newVolume) {

		volume = newVolume;
		AudioListener.volume = volume;
		PlayerPrefs.SetFloat ("MasterVolume", volume);

	}

	public void SetMusic (float newVolume) {

		musicVolume = .05f * newVolume;
		audio.volume = musicVolume;
		PlayerPrefs.SetFloat ("MusicVolume", newVolume);

	}

	public void SetVoice(float newVolume) {

		voiceVolume = newVolume;
		portraitAudio.audio.volume = voiceVolume;
		PlayerPrefs.SetFloat ("VoiceVolume", voiceVolume);

	}

	public void setSoundEffects(float newVolume) {

		//Set the audio of all sound effects
		audioHandler = audioHandlerObject.GetComponent<AudioHandler> ();
		audioHandler.setSoundEffectsVolume (.2f * newVolume);
		soundEffectsSlider.value = newVolume;
		PlayerPrefs.SetFloat ("SoundEffectsVolume", newVolume);
	}

}
