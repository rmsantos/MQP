/* Module      : CharacterSelectMenu.cs
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

	public Sprite[] characterImages;
	
	int pilot;
	int gunner;
	int navigator;
	int mechanic;

	int selected;

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

		selected = 0;

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

		//If the menu theme isnt playing
		if(!audio.isPlaying)
		{
			//Play it!
			audio.Play();
		}

		//If the user clicked start and the audio file is done
		if(startGame && !startButton.audio.isPlaying)
		{
			PlayerPrefs.SetInt("Portrait1", pilot);
			PlayerPrefs.SetInt("Portrait2", gunner);
			PlayerPrefs.SetInt("Portrait3", mechanic);
			PlayerPrefs.SetInt("Portrait4", navigator);
			//Load the main game
			Application.LoadLevel (1);
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
		startGame = start;
	}

	public bool HasNotBeenSelected() {
		return (pilot != selected && gunner != selected && mechanic != selected && navigator != selected);
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
		selected = character;
		selectedImage.overrideSprite = characterImages[character];
	}

	

}
