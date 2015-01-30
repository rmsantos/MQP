/* Module      : UpgradeMenu.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the upgrade menu
 *
 * Date        : 2015/1/30
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

public class UpgradeMenu : MonoBehaviour {

	//Flags on whether to start the game
	bool startGame;

	//The start button
	public GameObject startButton;
	public GameObject damageButton;

	public Text moneyText;
	
	int damage;
	int money;

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

		damage = PlayerPrefs.GetInt ("Damage", 1);
		money = PlayerPrefs.GetInt ("Money", 0);

		moneyText.text = ("Money: " + money.ToString());

		//Initialize states to not pressed
		startGame = false;
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
			Application.LoadLevel (1);
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

	public void UpgradeDamage() {
		if (money >= 1) {
			money--;
			damage++;
			PlayerPrefs.SetInt("Money", money);
			PlayerPrefs.SetInt("Damage", damage);
			moneyText.text = ("Money: " + money.ToString());
		}
	}

}
