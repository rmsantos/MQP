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
using UnityEngine.UI;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class HighScoreMenu : MonoBehaviour {

	//Flags on whether to start or quit the game
	bool saveScore;

	//The save button
	public GameObject saveButton;
	
	int[] score;
	string[] name;
	public Text[] scoreListings;
	public Text[] scoreNames;

	int scorePlacement;

	int currentScore;
	
	bool highScore;

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
		saveScore = false;

		//dummy initialize arrays
		score = new int[] {0,0,0,0,0,0,0,0,0,0};
		name = new string[] {"","","","","","","","","",""};

		for (int i = 0; i < 10; i++) {
			score[i] = PlayerPrefs.GetInt("Score" + (i + 1).ToString(), 1000 - (i * 100));
			name[i] = PlayerPrefs.GetString("Name" + (i + 1).ToString(), "AAA");
		}

		currentScore = PlayerPrefs.GetInt ("Score", 0);

		highScore = false;

		if (currentScore > score[0]) {
			highScore = true;
			scorePlacement = 0;
		}
		else if (currentScore > score[9]) {
			highScore = true;
			
			for (int i = 9; i >= 0; i--) {
				if (currentScore < score[i]) {
					scorePlacement = i + 1;
					break;
				}
			}
		}

		if (highScore) {
			for (int i = 8; i >= scorePlacement; i--) {
				score[i+1] = score[i];
				name[i+1] = name[i];
			}
			score[scorePlacement] = currentScore;
			name[scorePlacement] = PlayerPrefs.GetString("Name", "AAA");
		}

		for (int i = 0; i < 10; i++) {
			scoreListings[i].text = name[i];
			scoreNames[i].text = score[i].ToString();
		}

		//Play the audio from where it left off
		Camera.main.audio.time = PlayerPrefs.GetFloat ("MainMenuLocation", 0);
		Camera.main.audio.Play ();

		AudioListener.volume = PlayerPrefs.GetFloat ("MasterVolume", 0);
		Camera.main.audio.volume = PlayerPrefs.GetFloat ("MusicVolume", 0) * .3f;
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
	
		//If the user clicked save and the audio file is done
		if(saveScore && !saveButton.audio.isPlaying)
		{
			//Set the location of the music
			PlayerPrefs.SetFloat("MainMenuLocation",Camera.main.audio.time);

			//Load the main game
			Application.LoadLevel (1);
		}

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : Save()
	 *
	 * Description : Sets the save bool to true
	 *
	 * Parameters  : bool save : save the high score
	 *
	 * Returns     : Void
	 */
	public void Save(bool save)
	{
		//Here is where we save all the high scores back again
		for (int i = 0; i < 10; i++) {
			PlayerPrefs.SetInt("Score" + (i + 1).ToString(), score[i]);
			PlayerPrefs.SetString("Name" + (i + 1).ToString(), name[i]);
		}
		saveScore = save;
		PlayerPrefs.SetInt ("Score", 0);
	}
	
}
