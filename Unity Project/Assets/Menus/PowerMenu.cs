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
	enum powerSelected { SHIELD = 0, ENGINE = 1, LASER = 2, BLASTER = 3, MISSILE = 4, POWER = 5};

	//Flags on whether to start the game
	bool startGame;
	
	//The start button
	public Button startButton;

	//The current power level of the player
	int power;

	//The current powers of the player
	int[] playerPowers;

	//The maximum amount of power (with upgrades)
	int maxPower;

	//The slider displaying power
	public Slider powerBar;

	//The slider display of the different player powers
	public Slider[] playerPowerSliders;

	//The status text
	public Text statusText;

	//Buttons for increasing and decreasing the power for certain stats
	public Button[] increaseButtons;
	public Button[] decreaseButtons;

	//Texts displaying the power levels.
	public Text[] powerLevels;

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

		//Initialize array
		playerPowers = new int[5];

		//Load the player prefs of each power level
		playerPowers[(int)powerSelected.SHIELD] = PlayerPrefs.GetInt ("ShieldPower", 0);
		playerPowers[(int)powerSelected.ENGINE] = PlayerPrefs.GetInt ("EnginePower", 0);
		playerPowers[(int)powerSelected.LASER] = PlayerPrefs.GetInt ("LaserPower", 0);
		playerPowers[(int)powerSelected.BLASTER] = PlayerPrefs.GetInt ("BlasterPower", 0);
		playerPowers[(int)powerSelected.MISSILE] = PlayerPrefs.GetInt ("MissilePower", 0);

		//Calculate the players power level
		power = maxPower - playerPowers[0] - playerPowers[1] - playerPowers[2] - playerPowers[3] - playerPowers[4];

		//Display the current power levels
		powerBar.maxValue = maxPower;
		powerBar.value = power;

		//Set the sliders and the power level text to the appropriate value
		for (int i = 0; i < 5; i++) {
			playerPowerSliders[i].value = playerPowers[i];
			powerLevels[i].text = playerPowers[i].ToString();
		}

		//Enable or disable buttons to reflect what can be done
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
			//Store the shield level as a pref
			PlayerPrefs.SetInt ("ShieldPower", playerPowers[(int)powerSelected.SHIELD]);

			//Store the engine level as a pref
			PlayerPrefs.SetInt ("EnginePower", playerPowers[(int)powerSelected.ENGINE]);

			//Store the blaster level as a pref
			PlayerPrefs.SetInt ("BlasterPower", playerPowers[(int)powerSelected.BLASTER]);
			
			//Store the laser level as a pref
			PlayerPrefs.SetInt ("LaserPower", playerPowers[(int)powerSelected.LASER]);

			//Store the missile level as a pref
			PlayerPrefs.SetInt ("MissilePower", playerPowers[(int)powerSelected.MISSILE]);
			
			//Store the new power level in player prefs
			PlayerPrefs.SetInt ("Power", power);
			
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
		decreaseButtons[(int)powerSelected.SHIELD].interactable = (playerPowers[(int)powerSelected.SHIELD] > 0);
		decreaseButtons[(int)powerSelected.ENGINE].interactable = (playerPowers[(int)powerSelected.ENGINE] > 0);
		decreaseButtons[(int)powerSelected.LASER].interactable = (playerPowers[(int)powerSelected.LASER] > 0);
		decreaseButtons[(int)powerSelected.BLASTER].interactable = (playerPowers[(int)powerSelected.BLASTER] > 0);
		decreaseButtons[(int)powerSelected.MISSILE].interactable = (playerPowers[(int)powerSelected.MISSILE] > 0);

	}

	public bool CanShieldIncrease() {

		if(playerPowers[(int)powerSelected.SHIELD] == 0 && PlayerPrefs.GetInt("ShieldUpgradeNumber",0) < 1)
		{
			return false;
		}
		else if(playerPowers[(int)powerSelected.SHIELD] == 2 && PlayerPrefs.GetInt("ShieldUpgradeNumber",0) < 2)
		{
			return false;
		}
		else if(playerPowers[(int)powerSelected.SHIELD] == 3 && PlayerPrefs.GetInt("ShieldUpgradeNumber",0) < 3)
		{
			return false;
		}
		else if (playerPowers[(int)powerSelected.SHIELD] == 5) 
		{
			return false;
		}
		else 
		{
			return true;
		}

	}

	public bool CanEngineIncrease() {

		if(playerPowers[(int)powerSelected.ENGINE] == 2 && PlayerPrefs.GetInt("EngineUpgrade",0) < 1)
		{
			return false;
		}
		else if(playerPowers[(int)powerSelected.ENGINE] == 3 && PlayerPrefs.GetInt("EngineUpgrade",0) < 2)
		{
			return false;
		}
		else if(playerPowers[(int)powerSelected.ENGINE] == 4 && PlayerPrefs.GetInt("EngineUpgrade",0) < 3)
		{
			return false;
		}
		else if (playerPowers[(int)powerSelected.ENGINE] == 5) 
		{
			return false;
		}
		else 
		{
			return true;
		}
		
	}
	
	public bool CanBlasterIncrease() {

		if(playerPowers[(int)powerSelected.BLASTER] == 0 && PlayerPrefs.GetInt("BlasterUpgradeFireRate",0) < 1)
		{
			return false;
		}
		else if (playerPowers[(int)powerSelected.BLASTER] == 4) {
			return false;
		}
		else 
		{
			return true;
		}
		
	}
	
	public bool CanLaserIncrease() {

		if(playerPowers[(int)powerSelected.LASER] == 2 && PlayerPrefs.GetInt("LaserUpgradeBurst",0) != 1)
		{
			return false;
		}
		else if (playerPowers[(int)powerSelected.LASER] == 3) {
			return false;
		}
		else {
			return true;
		}
		
	}
	
	public bool CanMissileIncrease() {

		if (playerPowers[(int)powerSelected.MISSILE] == 4) 
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

		//Increase the appropriate power level
		playerPowers[station]++;

		//Set the appropriate slider to that value
		playerPowerSliders[station].value = playerPowers[station];

		//And show the value of the slider here
		powerLevels[station].text = playerPowers[station].ToString();

		//Decrease power level
		power--;
		
		//And update the power bar to reflect
		powerBar.value = power;

		//And show the value of the slider here
		powerLevels[(int)powerSelected.POWER].text = power.ToString();

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

		//Decrease the appropriate power level
		playerPowers[station]--;
		
		//Set the appropriate slider to that value
		playerPowerSliders[station].value = playerPowers[station];

		//And show the value of the slider here
		powerLevels[station].text = playerPowers[station].ToString();
		
		//Increase the power
		power++;
		
		//And update the power bar to reflect
		powerBar.value = power;

		//And show the value of the slider here
		powerLevels[(int)powerSelected.POWER].text = power.ToString();
		
		//Set the interactability of the buttons
		CheckButtons ();

	}	
}
