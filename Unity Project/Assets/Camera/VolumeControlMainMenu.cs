/* Module      : VolumeControl.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This control the volume of the game
 *
 * Date        : 2015/3/4
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeControlMainMenu: MonoBehaviour {

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

	public Text controlVolumeButtonText;
	public Text controlVolumeText;
	public Text controlsText;

	public Text master;
	public Text music;
	public Text voice;
	public Text effects;

	public GameObject audioSource;

	//The scale that affects the music volume
	float musicScale;

	bool controls;

	void Start () {
		//Start the scale at .05f
		musicScale = .05f;

		volume = PlayerPrefs.GetFloat ("MasterVolume", 0);
		musicVolume = PlayerPrefs.GetFloat ("MusicVolume", 0);
		voiceVolume = PlayerPrefs.GetFloat ("VoiceVolume", 0);
		soundEffectsVolume = PlayerPrefs.GetFloat ("SoundEffectsVolume", 0);

		//Music volume
		audioSource.audio.volume = musicScale * musicVolume;
		musicSlider.value = musicVolume;

		//Portrait Volume
		voiceSlider.value = voiceVolume;

		//Master volume
		AudioListener.volume = volume;
		volumeSlider.value = volume;

		//Set the audio of all sound effects
		soundEffectsSlider.value = soundEffectsVolume;

		controls = false;

	}

	public void SetVolume (float newVolume) {

		volume = newVolume;
		AudioListener.volume = volume;
		PlayerPrefs.SetFloat ("MasterVolume", volume);

	}

	public void SetMusic (float newVolume) {

		musicVolume = musicScale * newVolume;
		audioSource.audio.volume = musicVolume;
		PlayerPrefs.SetFloat ("MusicVolume", newVolume);

	}

	public void SetVoice(float newVolume) {

		voiceVolume = newVolume;
		PlayerPrefs.SetFloat ("VoiceVolume", voiceVolume);

	}

	public void setSoundEffects(float newVolume) {
		
		soundEffectsSlider.value = newVolume;
		PlayerPrefs.SetFloat ("SoundEffectsVolume", newVolume);
	}

	public void switchControls() {
		controls = !controls;
		if (controls) {
			volumeSlider.active = false;
			soundEffectsSlider.active = false;
			voiceSlider.active = false;
			musicSlider.active = false;
			master.active = false;
			music.active = false;
			voice.active = false;
			effects.active = false;
			controlsText.active = true;
			controlVolumeText.text = "Controls";
			controlVolumeButtonText.text = "Volume Options";
		}
		else {
			volumeSlider.active = true;
			soundEffectsSlider.active = true;
			voiceSlider.active = true;
			musicSlider.active = true;
			master.active = true;
			music.active = true;
			voice.active = true;
			effects.active = true;
			controlsText.active = false;
			controlVolumeText.text = "Volume Options";
			controlVolumeButtonText.text = "Controls";
		}
	}

}
