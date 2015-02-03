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

	public void LockInPilot() {

	}

	public void LockInGunner() {
		
	}

	public void LockInMechanic() {
		
	}

	public void LockInNavigator() {
		
	}

	public void SelectCharacter(int character) {
		selected = character;
	}

	

}
