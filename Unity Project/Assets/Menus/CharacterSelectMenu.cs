﻿/* Module      : CharacterSelectMenu.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the character select menu
 *
 * Date        : 2015/2/3
 *  
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelectMenu : MonoBehaviour {

	//Flags on whether to start the game
	bool startGame;

	//The buttons
	public Button startButton;
	public Button[] charButtons;
	public Button lockPilotButton;
	public Button lockGunnerButton;
	public Button lockNavigatorButton;
	public Button lockMechanicButton;

	public Text descriptionText;
	public Text nameText;

	public Image pilotImage;
	public Image gunnerImage;
	public Image navigatorImage;
	public Image mechanicImage;

	public Image selectedImage;

	public GameObject namePanel;

	public Sprite[] characterImages;

	public string[] characterDescriptions;
	public string[] characterNames;
	
	int pilot;
	int gunner;
	int navigator;
	int mechanic;

	int selected;
	
	static string name;

	//Catch phrases of each character
	public AudioClip[] catchPhrases = new AudioClip[22];

	//AudioSources of each station
	public AudioSource pilotSource;
	public AudioSource gunnerSource;
	public AudioSource mechanicSource;
	public AudioSource navigatorSource;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the start and quit bools
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Initialize states to not pressed
		startGame = false;

		pilot = -1;
		gunner = -1;
		navigator = -1;
		mechanic = -1;

		selected = -1;

		name = "";


	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Starts the game when the button is pressed
	 * 				and the sound clip stops playing.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {

		//If the user clicked start and the audio file is done
		if(startGame && !startButton.audio.isPlaying)
		{
			PlayerPrefs.SetInt("Portrait1", pilot);
			PlayerPrefs.SetInt("Portrait2", gunner);
			PlayerPrefs.SetInt("Portrait3", mechanic);
			PlayerPrefs.SetInt("Portrait4", navigator);
			PlayerPrefs.SetString("Name", name);
			//Load the main game
			Application.LoadLevel (4);
		}


	
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playButtonClick()
	 *
	 * Description : Plays the button click sound effect if the user has a portrait
	 * 				selected (that hasn't been selected already)
	 *
	 * Parameters  : int position : The position being locked into
	 *
	 * Returns     : Void
	 */
	public void playButtonClick(int position)
	{
		//If a portrait is selected
		if(selected >= 0)
		{
			//Check if the current selected hasnt been selected already
			//If not then play the sound effect
			if(position == 0)
			{
				if(gunner != selected && mechanic != selected && navigator != selected)
					pilotSource.Play();
			}
			else if(position == 1)
			{
				if(pilot != selected && mechanic != selected && navigator != selected)
					gunnerSource.Play();
			}
			else if(position == 2)
			{
				if(pilot != selected && gunner != selected && navigator != selected)
					mechanicSource.Play();
			}
			else if(pilot!= selected && gunner != selected && mechanic != selected)
				navigatorSource.Play ();
		}
			
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : SetStart()
	 *
	 * Description : Sets the start bool to true
	 *
	 * Parameters  : bool start : Start the game?
	 *
	 * Returns     : Void
	 */
	public void SetStart(bool start)
	{
		if (pilot != -1 && gunner != -1 && navigator != -1 && mechanic != -1) {
			startGame = start;
		}
	}

	public bool HasNotBeenSelected() {
		return (selected != -1 && pilot != selected && gunner != selected && mechanic != selected && navigator != selected);
	}

	public void LockInPilot() {
		if (HasNotBeenSelected()) {
			pilotImage.overrideSprite = characterImages[selected];
			pilot = selected;
		}
	}

	public void LockInGunner() {
		if (HasNotBeenSelected()) {
			gunnerImage.overrideSprite = characterImages[selected];
			gunner = selected;
		}
	}

	public void LockInMechanic() {
		if (HasNotBeenSelected()) {
			mechanicImage.overrideSprite = characterImages[selected];
			mechanic = selected;
		}
	}

	public void LockInNavigator() {
		if (HasNotBeenSelected()) {
			navigatorImage.overrideSprite = characterImages[selected];
			navigator = selected;
		}
	}

	public void SelectCharacter(int character) {

		//Play the character catch phrase
		audio.clip = catchPhrases [character];
		audio.Play ();

		//Select the character
		selected = character;
		selectedImage.overrideSprite = characterImages[character];
		descriptionText.text = characterDescriptions[character];
		nameText.text = characterNames [character];
	}

	public void SetName(string enteredName) {

		name = enteredName;

	}

	public void StartSelecting() {

		namePanel.active = false;

	}

}
