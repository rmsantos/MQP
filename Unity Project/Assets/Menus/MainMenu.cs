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
	bool highScores;

	//The start and quit buttons
	public GameObject startButton;
	public GameObject quitButton;
	public GameObject highScoreButton;

	public int initialHealth;
	public int initialDamage;
	public int initialShieldPower;
	public int initialEnginePower;
	public int initialMissilePower;
	public int initialLaserPower;
	public int initialMoneyValue;
	public int initialPowerValue;

	public int initialReload;


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
		highScores = false;

		if (!PlayerPrefs.HasKey ("HighScores")) {
			PlayerPrefs.SetString("HighScores", "true");
			for (int i = 0; i < 10; i++) {
				PlayerPrefs.SetInt("Score" + (i + 1).ToString(), 1000 - (i * 100));
				PlayerPrefs.SetString("Name" + (i + 1).ToString(), "AAA");
			}
		}
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
			Application.LoadLevel (3);
		}

		//If the user clicked quit and the audio file is done
		if(quitGame && !quitButton.audio.isPlaying)
		{
			//Quit the game
			Application.Quit();
		}

		//If the user clicked high scores and the audio file is done
		if(highScores && !highScoreButton.audio.isPlaying)
		{
			//Load the high scores
			Application.LoadLevel (4);
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
		//Here is where we set all the upgrade values. Player0 is our first unsaved player
		PlayerPrefs.SetString("Player", "Player0");
		PlayerPrefs.SetInt ("Damage", initialDamage);
		PlayerPrefs.SetInt ("ShieldPower", initialShieldPower);
		PlayerPrefs.SetInt ("Power", initialPowerValue);
		PlayerPrefs.SetInt ("EnginePower", initialEnginePower);
		PlayerPrefs.SetInt ("MissilePower", initialMissilePower);
		PlayerPrefs.SetInt ("LaserPower", initialLaserPower);
		PlayerPrefs.SetInt ("MoneyValue", initialMoneyValue);
		PlayerPrefs.SetInt ("Health", initialHealth);

		PlayerPrefs.SetInt ("Reload", initialReload);


		//Upgrades
		PlayerPrefs.SetInt ("DamageUpgrade", 0);
		PlayerPrefs.SetInt ("ShieldUpgrade", 0);
		PlayerPrefs.SetInt ("MissileUpgrade", 0);
		PlayerPrefs.SetInt ("LaserUpgrade", 0);
		PlayerPrefs.SetInt ("HealthUpgrade", 0);
		PlayerPrefs.SetInt ("PowerUpgrade", 0);

		//Game values
		PlayerPrefs.SetInt ("Money", 0);
		PlayerPrefs.SetInt ("Score", 0);
		PlayerPrefs.SetInt ("Level", 1);

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

	/* ----------------------------------------------------------------------- */
	/* Function    : setHighScores()
	 *
	 * Description : Sets the highScores bool to true
	 *
	 * Parameters  : bool highScores : display the high scores?
	 *
	 * Returns     : Void
	 */
	public void setHighScores(bool clicked)
	{
		highScores = clicked;
	}
}
