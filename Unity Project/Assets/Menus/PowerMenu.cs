/* Module      : PowerMenu.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the power menu
 *
 * Date        : 2015/2/5
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

public class PowerMenu : MonoBehaviour {
		
	//Enums for all the player stations
	enum powerSelected { SHIELD = 0, ENGINE = 1, LASER = 2, BLASTER = 3, MISSILE = 4};

	//Flags on whether to start the game
	bool startGame;
	
	//The start button
	public Button startButton;

	//The current power level of the player
	int power;

	//The current shield level of the player
	int shield;

	//The current engine level of the player
	int engine;

	//The current laser level of the player
	int laser;

	//The current blaster level of the player
	int blaster;

	//The current missile level of the player
	int missile;

	//The maximum amount of power (with upgrades)
	int maxPower;

	//The slider displaying power
	public Slider powerBar;

	//The slider display shield level
	public Slider shieldBar;

	//The slider display engine level
	public Slider engineBar;

	//The slider display laser level
	public Slider laserBar;

	//The slider display blaser level
	public Slider blasterBar;

	//The slider display missile level
	public Slider missileBar;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the power levels
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {
		
		//Initialize states to not pressed
		startGame = false;

		//Load the players current power and max power from player prefs
		power = PlayerPrefs.GetInt ("Power", 10);
		maxPower = 10 + PlayerPrefs.GetInt ("PowerUpgrade", 0);

		//Load the player prefs of each power level
		shield = PlayerPrefs.GetInt ("ShieldPower", 0);
		engine = PlayerPrefs.GetInt ("EnginePower", 0);
		laser = PlayerPrefs.GetInt ("LaserPower", 0);
		blaster = PlayerPrefs.GetInt ("BlasterPower", 0);
		missile = PlayerPrefs.GetInt ("MissilePower", 0);


		//Display the current power levels
		powerBar.value = power;
		shieldBar.value = shield;
		engineBar.value = engine;
		laserBar.value = laser;
		blasterBar.value = blaster;
		missileBar.value = missile;
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
	/* Function    : increasePower(int station)
	 *
	 * Description : Increase power to station
	 *
	 * Parameters  : int station : The station to give power to
	 *
	 * Returns     : Void
	 */
	public void increasePower(int station)
	{
		//If there is no power left, then exit
		if(power == 0)
			return;

		//Determine what station is being increased
		switch(station)
		{
			//Shield power
			case (int)powerSelected.SHIELD:
				//If its max, then exit
				if(shield == 5)
					return;

				//Else increase shield level
				shield++;

				//Decrease power level
				power--;

				//Store the shield level as a pref
				PlayerPrefs.SetInt ("ShieldPower", shield);

				//Update the shield bar to reflect
				shieldBar.value = shield;
				
				//And break the case statement
				break;
			//Engine power
			case (int)powerSelected.ENGINE:
				//If its max, then exit
				if(engine == 5)
					return;
				
				//Else increase engine level
				engine++;
				
				//Decrease power level
				power--;
				
				//Store the engine level as a pref
				PlayerPrefs.SetInt ("EnginePower", engine);
				
				//Update the engine bar to reflect
				engineBar.value = engine;
				
				//And break the case statement
				break;
			//Laser power
			case (int)powerSelected.LASER:
				//If its max, then exit
				if(laser == 4)
					return;
				
				//Else increase laser level
				laser++;
				
				//Decrease power level
				power--;
				
				//Store the laser level as a pref
				PlayerPrefs.SetInt ("LaserPower", laser);
				
				//Update the laser bar to reflect
				laserBar.value = laser;
				
				//And break the case statement
				break;
			//Blaster power
			case (int)powerSelected.BLASTER:
				//If its max, then exit
				if(blaster == 3)
					return;
				
				//Else increase blaster level
				blaster++;
				
				//Decrease power level
				power--;
				
				//Store the blaster level as a pref
				PlayerPrefs.SetInt ("BlasterPower", blaster);
				
				//Update the blaster bar to reflect
				blasterBar.value = blaster;
				
				//And break the case statement
				break;
			//Missile Power
			case (int)powerSelected.MISSILE:
				//If its max, then exit
				if(missile == 4)
					return;
				
				//Else increase missile level
				missile++;
				
				//Decrease power level
				power--;
				
				//Store the missilelevel as a pref
				PlayerPrefs.SetInt ("MissilePower", missile);
				
				//Update the missile bar to reflect
				missileBar.value = missile;
				
				//And break the case statement
				break;
		}

		//Store the new power level in player prefs
		PlayerPrefs.SetInt ("Power", power);

		//And update the power bar to reflect
		powerBar.value = power;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : decreasePower(int station)
	 *
	 * Description : Decrease power to station
	 *
	 * Parameters  : int station : The station to take power from
	 *
	 * Returns     : Void
	 */
	public void decreasePower(int station)
	{
		//If power is max, don't overload it
		if(power == maxPower)
			return;

		//Determine what station is being decreased
		switch(station)
		{
			//Shield power
			case (int)powerSelected.SHIELD:
				//If the shield is none, then exit
				if(shield == 0)
					return;

				//Otherwise decrease the shield
				shield--;

				//Increase the power
				power++;

				//Store the shield level as a pref
				PlayerPrefs.SetInt ("ShieldPower", shield);

				//And update the slide
				shieldBar.value = shield;

				//Break the case statement
				break;
			//Engine power
			case (int)powerSelected.ENGINE:
				//If its none, then exit
				if(engine == 0)
					return;
				
				//Else decrease engine level
				engine--;
				
				//Increase power level
				power++;
				
				//Store the engine level as a pref
				PlayerPrefs.SetInt ("EnginePower", engine);
				
				//Update the engine bar to reflect
				engineBar.value = engine;
				
				//And break the case statement
				break;
			//Laser power
			case (int)powerSelected.LASER:
				//If its none, then exit
				if(laser == 0)
					return;
				
				//Else decrease laser level
				laser--;
				
				//Increase power level
				power++;
				
				//Store the laser level as a pref
				PlayerPrefs.SetInt ("LaserPower", laser);
				
				//Update the laser bar to reflect
				laserBar.value = laser;
				
				//And break the case statement
				break;
			//Blaster power
			case (int)powerSelected.BLASTER:
				//If its none, then exit
				if(blaster == 0)
					return;
				
				//Else decrease blaster level
				blaster--;
				
				//Increase power level
				power++;
				
				//Store the blaster level as a pref
				PlayerPrefs.SetInt ("BlasterPower", blaster);
				
				//Update the blaster bar to reflect
				blasterBar.value = blaster;
				
				//And break the case statement
				break;
			//Missile power
			case (int)powerSelected.MISSILE:
				//If its none, then exit
				if(missile== 0)
					return;
				
				//Else decrease missile level
				missile--;
				
				//Increase power level
				power++;
				
				//Store the missile level as a pref
				PlayerPrefs.SetInt ("MissilePower", missile);
				
				//Update the missile bar to reflect
				missileBar.value = missile;
				
				//And break the case statement
				break;
		}

		//Store the new power level in player prefs
		PlayerPrefs.SetInt ("Power", power);
		
		//And update the power bar to reflect
		powerBar.value = power;

	}	
}
