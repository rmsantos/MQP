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
	public Image startButton;

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

	//The sprites for the different power levels of the different abilities
	public Sprite[] shieldSprites;
	public Sprite[] engineSprites;
	public Sprite[] laserSprites;
	public Sprite[] blasterSprites;
	public Sprite[] missileSprites;
	public Sprite[] availablePowerSprites;
	Sprite[][] powerSprites;

	//The actual displayed images
	public Image[] powerImages;

	//The Descriptions
	public string[] descriptions;

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

		//Setup all the image files to be easily accessible
		powerSprites = new Sprite[][] {shieldSprites, engineSprites, laserSprites, blasterSprites, missileSprites, availablePowerSprites};
		

		//Set the sliders and the power level text to the appropriate value
		for (int i = 0; i < 5; i++) {
			playerPowerSliders[i].value = playerPowers[i];
			powerLevels[i].text = playerPowers[i].ToString();
			powerImages[i].overrideSprite = powerSprites[i][playerPowers[i]];
		}

		//Overwrite the image to show the value of the power
		powerImages[(int)powerSelected.POWER].overrideSprite = powerSprites[(int)powerSelected.POWER][power];

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
		for (int i = 0; i < 5; i++) {
			decreaseButtons[i].interactable = (playerPowers[i] > 0);
		}

	}

	public void ViewDescription(int description) {

		statusText.text = descriptions[description];

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

		//Overwrite the image to show the value of the power
		powerImages[station].overrideSprite = powerSprites[station][playerPowers[station]];

		//Decrease power level
		power--;
		
		//And update the power bar to reflect
		powerBar.value = power;

		//And show the value of the slider here
		powerLevels[(int)powerSelected.POWER].text = power.ToString();

		//Overwrite the image to show the value of the power
		powerImages[(int)powerSelected.POWER].overrideSprite = powerSprites[(int)powerSelected.POWER][power];

		//Check all the interactable buttons
		CheckButtons ();

		//Display the new status text
		setStatusText (station);
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

		//Overwrite the image to show the value of the power
		powerImages[station].overrideSprite = powerSprites[station][playerPowers[station]];
		
		//Increase the power
		power++;
		
		//And update the power bar to reflect
		powerBar.value = power;

		//And show the value of the slider here
		powerLevels[(int)powerSelected.POWER].text = power.ToString();

		//Overwrite the image to show the value of the power
		powerImages[(int)powerSelected.POWER].overrideSprite = powerSprites[(int)powerSelected.POWER][power];
		
		//Set the interactability of the buttons
		CheckButtons ();

		//Display the new status text
		setStatusText (station);
	}	

	/* ----------------------------------------------------------------------- */
	/* Function    : setStatusText(int station)
	 *
	 * Description : Displays what each power level does for each station
	 *
	 * Parameters  : int station : The station being viewed
	 *
	 * Returns     : Void
	 */
	public void setStatusText(int station)
	{
		//Determine which station is being viewed
		switch(station)
		{
			//Power
			case (int)powerSelected.POWER:
				displayPower();
				break;
			//Engine
			case (int)powerSelected.ENGINE:
				displayEngine();
				break;
			//Laser
			case (int)powerSelected.LASER:
				displayLaser();
				break;
			//Missile
			case (int)powerSelected.MISSILE:
				displayMissile();
				break;
			//Blaster
			case (int)powerSelected.BLASTER:
				displayBlaster();
				break;
			//Or shield
			case (int)powerSelected.SHIELD:
				displayShield();
				break;
		}

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayPower()
	 *
	 * Description : Displays the current power level
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayPower()
	{
		statusText.text = "Power Level: " + power;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayEngine()
	 *
	 * Description : Displays what each engine level does
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayEngine()
	{
		switch(playerPowers[(int)powerSelected.ENGINE])
		{
			case 0:
				statusText.text = "Engine Level 0. Basic max speed. Basic acceleration. The ship moves very slowly.";
				break;
			case 1:
				statusText.text = "Engine Level 1. Basic max speed. Decent acceleration. The ship moves slowly.";
				break;
			case 2:
				statusText.text = "Engine Level 2. Decent max speed. Decent acceleration. The ship moves modestly.";
				break;
			case 3:
				statusText.text = "Engine Level 3. Decent max speed. Good acceleration. The ship moves well.";
				break;
			case 4:
				statusText.text = "Enigne Level 4. Good max speed. Good acceleration. The ship moves swiftly.";
				break;
			case 5:
			statusText.text = "Engine Level 5. Excellent max speed. Excellent acceleration. The ship is a comet!";
				break;
		}

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayLaser()
	 *
	 * Description : Displays what each laser level does
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayLaser()
	{
		switch(playerPowers[(int)powerSelected.LASER])
		{
		case 0:
			statusText.text = "Laser Level 0. Low base damage. Slow base reload rate. The laser is clunky.";
			break;
		case 1:
			statusText.text = "Laser Level 1. Low base damage. Decent base reload rate. The laser is acceptable.";
			break;
		case 2:
			statusText.text = "Laser Level 2. Low base damage. Good base reload rate. The laser is swift.";
			break;
		case 3:
			statusText.text = "Laser Level 3. Better base damage. Better reload rate. Burst Shot enabled! The laser is unstoppable!";
			break;
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayMissile()
	 *
	 * Description : Displays what each missile level does
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayMissile()
	{
		switch(playerPowers[(int)powerSelected.MISSILE])
		{
		case 0:
			statusText.text = "Missile Level 0. Missiles disabled.";
			break;
		case 1:
			statusText.text = "Missile Level 1. Low base damage. Low base reload rate. The missile is slow.";
			break;
		case 2:
			statusText.text = "Missile Level 2. Low base damage. Good base reload rate. The missile is dangerous.";
			break;
		case 3:
			statusText.text = "Missile Level 3. Better base damage. Great base reload rate. The missile is deadly.";
			break;
		case 4:
			statusText.text = "Missile Level 4. Enhanced base damage. Enhanced reload rate. The missile is almost nuclear!";
			break;
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayBlaster()
	 *
	 * Description : Displays what each blaster level does
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayBlaster()
	{
		switch(playerPowers[(int)powerSelected.BLASTER])
		{
		case 0:
			statusText.text = "Blaster Level 0. Blaster disabled.";
			break;
		case 1:
			statusText.text = "Blaster Level 1. Low base damage. Low base reload rate. The blaster is a snail's pace.";
			break;
		case 2:
			statusText.text = "Blaster Level 2. Good base damage. Good base reload rate. The blaster is a intimidating.";
			break;
		case 3:
			statusText.text = "Blaster Level 3. Great base damage. Great base reload rate. The blaster is a large threat.";
			break;
		case 4:
			break;
			statusText.text = "Blaster Level 4. Crazy base damage. Crazy base reload rate. The blaster can cut through anything!";
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayShield()
	 *
	 * Description : Displays what each shield level does
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayShield()
	{
		switch(playerPowers[(int)powerSelected.SHIELD])
		{
		case 0:
			statusText.text = "Shield Level 0. The ship has no shields.";
			break;
		case 1:
			statusText.text = "Shield Level 1. Up to one shield. Low base recharge rate. The shield is...there.";
			break;
		case 2:
			statusText.text = "Shield Level 2. Up to one shield. Decent base recharge rate. The shield is useful.";
			break;
		case 3:
			statusText.text = "Shield Level 3. Up to two shields. Good base recharge rate. The shield is effective.";
			break;
		case 4:
			break;
			statusText.text = "Shield Level 4. Up to three shields. Great base recharge rate. The shield is almost unstoppable.";
		case 5:
			break;
			statusText.text = "Shield Level 6. Up to three shields. Amazing base recharge rate. The shield is almost impenetrable! The ship is a tank!.";
		}
		
	}
}
