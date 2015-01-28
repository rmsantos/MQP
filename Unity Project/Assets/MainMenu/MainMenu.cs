/* Module      : MainMenu.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the main menu
 *
 * Date        : 2015/1/28
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class MainMenu : MonoBehaviour {

	//Flags on whether to start or quit the game
	bool startGame;
	bool quitGame;

	//The start and quit buttons
	public GameObject startButton;
	public GameObject quitButton;

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
		quitGame = false;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Starts and quits the game when the button is pressed
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
			//Load the main game
			Application.LoadLevel (1);
		}

		//If the user clicked quit and the audio file is done
		if(quitGame && !quitButton.audio.isPlaying)
		{
			//Quit the game
			Application.Quit();
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setStart()
	 *
	 * Description : Sets the start bool to true
	 *
	 * Parameters  : bool start : Start the game?
	 *
	 * Returns     : Void
	 */
	public void setStart(bool start)
	{
		startGame = start;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setQuit()
	 *
	 * Description : Sets the quit bool to true
	 *
	 * Parameters  : bool quit : Start the game?
	 *
	 * Returns     : Void
	 */
	public void setQuit(bool quit)
	{
		quitGame = quit;
	}
}
