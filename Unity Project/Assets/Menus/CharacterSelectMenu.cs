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
	public Button lockPilotButton2;
	public Button lockGunnerButton2;
	public Button lockNavigatorButton2;
	public Button lockMechanicButton2;

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

		//Don't allow the player to click the start button unless the crew is locked in
		if(pilot == -1 || gunner == -1 || mechanic == -1 || navigator == -1)
			startButton.interactable = false;
		else
			startButton.interactable = true;

		//If there is no character selected or the character has already been selected, then disable the lock buttons
		if(selected == -1 || pilot == selected || gunner == selected || mechanic == selected || navigator == selected)
		{
			lockNavigatorButton.interactable = false;
			lockGunnerButton.interactable = false;
			lockPilotButton.interactable = false;
			lockMechanicButton.interactable = false;
			lockNavigatorButton2.interactable = false;
			lockGunnerButton2.interactable = false;
			lockPilotButton2.interactable = false;
			lockMechanicButton2.interactable = false;
		}
		else
		{
			//Else enable all the buttons
			lockNavigatorButton.interactable = true;
			lockGunnerButton.interactable = true;
			lockPilotButton.interactable = true;
			lockMechanicButton.interactable = true;
			lockNavigatorButton2.interactable = true;
			lockGunnerButton2.interactable = true;
			lockPilotButton2.interactable = true;
			lockMechanicButton2.interactable = true;
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
