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

	//The status text
	public Text statusText;

	//Buttons for increasing and decreasing the power for certain stats
	public Button[] increaseButtons;
	public Button[] decreaseButtons;

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
		maxPower = 5 + PlayerPrefs.GetInt ("PowerUpgrade", 0);

		//Load the player prefs of each power level
		shield = PlayerPrefs.GetInt ("ShieldPower", 0);
		engine = PlayerPrefs.GetInt ("EnginePower", 0);
		laser = PlayerPrefs.GetInt ("LaserPower", 0);
		blaster = PlayerPrefs.GetInt ("BlasterPower", 0);
		missile = PlayerPrefs.GetInt ("MissilePower", 0);

		//Calculate the players power level
		power = maxPower - shield - engine - laser - blaster - missile;

		//Display the current power levels
		powerBar.maxValue = maxPower;
		powerBar.value = power;
		shieldBar.value = shield;
		engineBar.value = engine;
		laserBar.value = laser;
		blasterBar.value = blaster;
		missileBar.value = missile;

		CheckButtons ();
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

	public void CheckButtons() {

		//Checks if the increase buttons should be enabled
		increaseButtons[(int)powerSelected.SHIELD].interactable = (CanShieldIncrease() && power > 0);
		increaseButtons[(int)powerSelected.ENGINE].interactable = (CanEngineIncrease() && power > 0);
		increaseButtons[(int)powerSelected.LASER].interactable = (CanLaserIncrease() && power > 0);
		increaseButtons[(int)powerSelected.BLASTER].interactable = (CanBlasterIncrease() && power > 0);
		increaseButtons[(int)powerSelected.MISSILE].interactable = (CanMissileIncrease() && power > 0);

		//Checks if the decrease buttons should be enabled
		decreaseButtons[(int)powerSelected.SHIELD].interactable = (shield > 0);
		decreaseButtons[(int)powerSelected.ENGINE].interactable = (engine > 0);
		decreaseButtons[(int)powerSelected.LASER].interactable = (laser > 0);
		decreaseButtons[(int)powerSelected.BLASTER].interactable = (blaster > 0);
		decreaseButtons[(int)powerSelected.MISSILE].interactable = (missile > 0);

	}

	public bool CanShieldIncrease() {

		if(shield == 0 && PlayerPrefs.GetInt("ShieldUpgradeNumber",0) < 1)
		{
			return false;
		}
		else if(shield == 2 && PlayerPrefs.GetInt("ShieldUpgradeNumber",0) < 2)
		{
			return false;
		}
		else if(shield == 3 && PlayerPrefs.GetInt("ShieldUpgradeNumber",0) < 3)
		{
			return false;
		}
		else if (shield == 5) 
		{
			return false;
		}
		else 
		{
			return true;
		}

	}

	public bool CanEngineIncrease() {

		if(engine == 2 && PlayerPrefs.GetInt("EngineUpgrade",0) < 1)
		{
			return false;
		}
		else if(engine == 3 && PlayerPrefs.GetInt("EngineUpgrade",0) < 2)
		{
			return false;
		}
		else if(engine == 4 && PlayerPrefs.GetInt("EngineUpgrade",0) < 3)
		{
			return false;
		}
		else if (engine == 5) 
		{
			return false;
		}
		else 
		{
			return true;
		}
		
	}
	
	public bool CanBlasterIncrease() {

		if(blaster == 0 && PlayerPrefs.GetInt("BlasterUpgradeFireRate",0) < 1)
		{
			return false;
		}
		else if (blaster == 4) {
			return false;
		}
		else 
		{
			return true;
		}
		
	}
	
	public bool CanLaserIncrease() {

		if(laser == 2 && PlayerPrefs.GetInt("LaserUpgradeBurst",0) != 1)
		{
			return false;
		}
		else if (laser == 3) {
			return false;
		}
		else {
			return true;
		}
		
	}
	
	public bool CanMissileIncrease() {

		if (missile == 4) 
		{
			return false;
		}
		else 
		{
			return true;
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

		//Determine what station is being increased
		switch(station)
		{
			//Shield power
			case (int)powerSelected.SHIELD:
				
				//increase shield level
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

				//increase engine level
				engine++;
				
				//Decrease power level
				power--;
				
				//Store the engine level as a pref
				PlayerPrefs.SetInt ("EnginePower", engine);
				
				//Update the engine bar to reflect
				engineBar.value = engine;
				
				//And break the case statement
				break;
			//Blaster power
			case (int)powerSelected.BLASTER:

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
			//Laser power
			case (int)powerSelected.LASER:

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
			//Missile Power
			case (int)powerSelected.MISSILE:
				
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

		//Check all the interactable buttons
		CheckButtons ();
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
		//Determine what station is being decreased
		switch(station)
		{
			//Shield power
			case (int)powerSelected.SHIELD:

				//decrease the shield
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
				
				//decrease engine level
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
				
				//decrease laser level
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
				
				//decrease blaster level
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
				
				//decrease missile level
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

		CheckButtons ();

	}	
}
