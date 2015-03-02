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
	enum powerSelected { SHIELD = 0, ENGINE = 1, LASER = 2, BLASTER = 3, MISSILE = 4, RADAR = 5, REPAIR = 6, POWER = 7};

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

	//The status text
	public Text statusText;

	//Buttons for increasing and decreasing the power for certain stats
	public Button[] increaseButtons;
	public Button[] decreaseButtons;

	//The sprites for the different power levels of the different abilities
	public Sprite[] shieldSprites;
	public Sprite[] engineSprites;
	public Sprite[] laserSprites;
	public Sprite[] blasterSprites;
	public Sprite[] missileSprites;
	public Sprite[] availablePowerSprites;
	public Sprite[] radarSprites;
	public Sprite[] repairSprite;

	Sprite[][] powerSprites;

	//The actual displayed images
	public Image[] powerImages;

	//The missile count
	public Text missileCount;

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
		playerPowers = new int[7];

		//Load the player prefs of each power level
		playerPowers[(int)powerSelected.SHIELD] = PlayerPrefs.GetInt ("ShieldPower", 0);
		playerPowers[(int)powerSelected.ENGINE] = PlayerPrefs.GetInt ("EnginePower", 0);
		playerPowers[(int)powerSelected.LASER] = PlayerPrefs.GetInt ("LaserPower", 0);
		playerPowers[(int)powerSelected.BLASTER] = PlayerPrefs.GetInt ("BlasterPower", 0);
		playerPowers[(int)powerSelected.MISSILE] = PlayerPrefs.GetInt ("MissilePower", 0);
		playerPowers[(int)powerSelected.RADAR] = PlayerPrefs.GetInt ("RadarPower", 0);
		playerPowers[(int)powerSelected.REPAIR] = PlayerPrefs.GetInt ("RepairPower", 0);

		//Calculate the players power level
		power = maxPower - playerPowers[0] - playerPowers[1] - playerPowers[2] - playerPowers[3] - playerPowers[4] - playerPowers[5] - playerPowers[6];

		//Setup all the image files to be easily accessible
		powerSprites = new Sprite[][] {shieldSprites, engineSprites, laserSprites, blasterSprites, missileSprites, radarSprites, repairSprite, availablePowerSprites};
		

		//Set the sliders and the power level text to the appropriate value
		for (int i = 0; i < 7; i++) {
				powerImages[i].overrideSprite = powerSprites[i][playerPowers[i]];
		}

		//Overwrite the image to show the value of the power
		powerImages[(int)powerSelected.POWER].overrideSprite = powerSprites[(int)powerSelected.POWER][power];

		//Enable or disable buttons to reflect what can be done
		CheckButtons ();

		//Set the missile count
		missileCount.text = PlayerPrefs.GetInt ("Missiles", 0).ToString();
	
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

			//Store the radar level as a pref
			PlayerPrefs.SetInt ("RadarPower", playerPowers[(int)powerSelected.RADAR]);

			//Store the repair level as a pref
			PlayerPrefs.SetInt ("RepairPower", playerPowers[(int)powerSelected.REPAIR]);
			
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
		increaseButtons[(int)powerSelected.RADAR].interactable = (CanRadarIncrease() && power > 0);
		increaseButtons[(int)powerSelected.REPAIR].interactable = (CanRepairIncrease() && power > 0);

		//Checks if the decrease buttons should be enabled
		for (int i = 0; i < 7; i++) {
			decreaseButtons[i].interactable = (playerPowers[i] > 0);
		}

	}

	public bool CanRadarIncrease() {
		if(playerPowers[(int)powerSelected.RADAR] == 2)
			return false;
		else
			return true;
	}

	public bool CanRepairIncrease() {
		if(playerPowers[(int)powerSelected.REPAIR] == 1)
			return false;
		else
			return true;
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

		//Overwrite the image to show the value of the power
		powerImages[station].overrideSprite = powerSprites[station][playerPowers[station]];

		//Decrease power level
		power--;

		//Overwrite the image to show the value of the power
		powerImages[(int)powerSelected.POWER].overrideSprite = powerSprites[(int)powerSelected.POWER][power];

		//Check all the interactable buttons
		CheckButtons ();

		//Display the new status text
		setStatusText (station);
		displayIncreaseText (station);

		if (power == 0) {
			startButton.interactable = true;
		}

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

		//Overwrite the image to show the value of the power
		powerImages[station].overrideSprite = powerSprites[station][playerPowers[station]];
		
		//Increase the power
		power++;

		//Overwrite the image to show the value of the power
		powerImages[(int)powerSelected.POWER].overrideSprite = powerSprites[(int)powerSelected.POWER][power];
		
		//Set the interactability of the buttons
		CheckButtons ();

		//Display the new status text
		setStatusText (station);
		displayDecreaseText (station);
	}	

	/* ----------------------------------------------------------------------- */
	/* Function    : displayDecreaseText(int station)
	 *
	 * Description : Displays what an decreased power level will do for each station.
	 *
	 * Parameters  : int station : The station being viewed
	 *
	 * Returns     : Void
	 */
	public void displayDecreaseText(int station)
	{
		//Determine which station is being viewed
		switch(station)
		{
			//Engine
			case (int)powerSelected.ENGINE:
				displayDecreasedEngine();
				break;
			//Laser
			case (int)powerSelected.LASER:
				displayDecreasedLaser();
				break;
			//Missile
			case (int)powerSelected.MISSILE:
				displayDecreasedMissile();
				break;
			//Blaster
			case (int)powerSelected.BLASTER:
				displayDecreasedBlaster();
				break;
			//Shield
			case (int)powerSelected.SHIELD:
				displayDecreasedShield();
				break;
			//Radar
			case (int)powerSelected.RADAR:
				displayDecreasedRadar();
				break;
			//Or repair
			case (int)powerSelected.REPAIR:
				displayDecreasedRepair();
				break;
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayDecreasedEngine()
	 *
	 * Description : Displays what each engine level decrease
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayDecreasedEngine()
	{
		switch(playerPowers[(int)powerSelected.ENGINE]-1)
		{
			case -1:
				statusText.text = "Engine are at minimal";
				break;
			case 0:
				statusText.text = "Decreased acceleration (-1) ";
				break;
			case 1:
				statusText.text = "Decreased max speed (-1)";
				break;
			case 2:
				statusText.text = "Decreases acceleration (-1)";
				break;
			case 3:
				statusText.text = "Decreases max speed (-1)";
				break;
			case 4:
				statusText.text = "Decreases acceleration (-1) and decreased speed (-1)";
				break;
		}
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : displayDecreasedLaser()
	 *
	 * Description : Displays what each laser level decrease
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayDecreasedLaser()
	{
		switch(playerPowers[(int)powerSelected.LASER]-1)
		{
			case -1:
				statusText.text = "Laser is at minimal";
				break;
			case 0:
				statusText.text = "Slower reload time (+1)";
				break;
			case 1:
				statusText.text = "Slower reload time (+1)";
				break;
			case 2:
				statusText.text = "Slower reload time (+1), decreased damage (-1), and Burst Shot disabled";
				break;
		}
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : displayDecreasedMissile()
	 *
	 * Description : Displays what each missile level decrease
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayDecreasedMissile()
	{
		switch(playerPowers[(int)powerSelected.MISSILE]-1)
		{
			case -1:
				statusText.text = "Missiles at minimal";
				break;
			case 0:
				statusText.text = "Missiles disabled.";
				break;
			case 1:
				statusText.text = "Slower loading time (+1)";
				break;
			case 2:
				statusText.text = "Slower loading time (+1) and decreased damage (-1)";
				break;
			case 3:
				statusText.text = "Slower loading time (+1) and decreased damage (-1)";
				break;
		}
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : displayDecreasedBlaster()
	 *
	 * Description : Displays what each blaster level decrease
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayDecreasedBlaster()
	{
		switch(playerPowers[(int)powerSelected.BLASTER]-1)
		{
			case -1:
				statusText.text = "Blasters at minimal.";
				break;
			case 0:
				statusText.text = "Blasters disabled.";
				break;
			case 1:
				statusText.text = "Slower loading time (+1) and decreased damage (-1)";
				break;
			case 2:
				statusText.text = "Slower loading time (+1) and decreased damage (-1)";
				break;
			case 3:
				statusText.text = "Slower loading time (+1) and decreased damage (-1))";
				break;
		}
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : displayDecreasedShield()
	 *
	 * Description : Displays what each shield level decrease
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayDecreasedShield()
	{
		switch(playerPowers[(int)powerSelected.SHIELD]-1)
		{
			case -1:
				statusText.text = "Shields at minimal";
				break;
			case 0:
				statusText.text = "Shields disabled.";
				break;
			case 1:
				statusText.text = "Slower recharging time (+1)";
				break;
			case 2:
				statusText.text = "Slower recharging time (+1) and decreased max shields (-1)";
				break;
			case 3:
				statusText.text = "Slower recharging time (+1) and decreased max shields (-1)";
				break;
			case 4:
				statusText.text = "Decreased recharging time (+1)";
				break;
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayDecreasedRadar()
	 *
	 * Description : Displays what each radar level decrease
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayDecreasedRadar()
	{
		switch(playerPowers[(int)powerSelected.RADAR]-1)
		{
		case -1:
			statusText.text = "Radar is not functional";
			break;
		case 0:
			statusText.text = "Radar level progression disabled";
			break;
		case 1:
			statusText.text = "Deactivates radar enemy warnings";
			break;
		}
		
	}

	public void displayStartGame() 
	{
		if (power >= 1 && maxPower == 5) {
			statusText.text = "Assign power first!";
			startButton.interactable = false;
		}
		else {
			statusText.text = "Start the game!";
			startButton.interactable = true;
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayDecreasedRepair()
	 *
	 * Description : Displays what each repair level decrease
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayDecreasedRepair()
	{
		switch(playerPowers[(int)powerSelected.REPAIR]-1)
		{
		case -1:
			statusText.text = "Repair is at minimum";
			break;
		case 0:
			statusText.text = "In-combat repair disabled";
			break;
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayIncreaseText(int station)
	 *
	 * Description : Displays what an increased power level will do for each station.
	 *
	 * Parameters  : int station : The station being viewed
	 *
	 * Returns     : Void
	 */
	public void displayIncreaseText(int station)
	{
		//Determine which station is being viewed
		switch(station)
		{
			//Engine
			case (int)powerSelected.ENGINE:
				displayIncreasedEngine();
				break;
			//Laser
			case (int)powerSelected.LASER:
				displayIncreasedLaser();
				break;
			//Missile
			case (int)powerSelected.MISSILE:
				displayIncreasedMissile();
				break;
			//Blaster
			case (int)powerSelected.BLASTER:
				displayIncreasedBlaster();
				break;
			//Shield
			case (int)powerSelected.SHIELD:
				displayIncreasedShield();
				break;
			//Radar
			case (int)powerSelected.RADAR:
				displayIncreasedRadar();
				break;
			//Or repair
			case (int)powerSelected.REPAIR:
				displayIncreasedRepair();
				break;
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayIncreasedEngine()
	 *
	 * Description : Displays what each engine level increase
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayIncreasedEngine()
	{
		int engineUpgrade = 0;
		string newText = "";

		switch(playerPowers[(int)powerSelected.ENGINE]+1)
		{
			case 1:
				statusText.text = "Increased acceleration (+1)";
				break;
			case 2:
				statusText.text = "Increased max speed (+1)";
				break;
			case 3:
				newText = "Increases acceleration (+1)";
				
				engineUpgrade = PlayerPrefs.GetInt("EngineUpgrade",0);

				if(engineUpgrade < 1)
					newText += ". Must buy engine upgrade level 1 first";

				statusText.text = newText;
				break;
			case 4:
				newText = "Increases max speed (+1)";
			
				engineUpgrade = PlayerPrefs.GetInt("EngineUpgrade",0);

				if(engineUpgrade < 2)
					newText += ". Must buy engine upgrade level 2 first";

				statusText.text = newText;
				break;
			case 5:
				newText = "Increases acceleration (+1) and increased speed (+1)";
				
				engineUpgrade = PlayerPrefs.GetInt("EngineUpgrade",0);
				
				if(engineUpgrade < 3)
					newText += ". Must buy engine upgrade level 3 first";
				
				statusText.text = newText;
				break;
			case 6:
				statusText.text = "Engine is maxed out";
				break;
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayIncreasedLaser()
	 *
	 * Description : Displays what each laser level increase
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayIncreasedLaser()
	{
		switch(playerPowers[(int)powerSelected.LASER]+1)
		{
			case 1:
				statusText.text = "Faster reload time (-1)";
				break;
			case 2:
				statusText.text = "Faster reload time (-1)";
				break;
			case 3:
				string newText = "Slower reload time (-2), Tri-Burst Shot enabled";
				
				int laserUpgrade = PlayerPrefs.GetInt("LaserUpgradeBurst",0);

				if(laserUpgrade < 1)
					newText += ". Must buy laser burst upgrade first";

				statusText.text = newText;
				break;
			case 4:
				statusText.text = "Laser is maxed out.";
				break;
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayIncreasedMissile()
	 *
	 * Description : Displays what each missile level increase
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayIncreasedMissile()
	{
		switch(playerPowers[(int)powerSelected.MISSILE]+1)
		{
			case 1:
				statusText.text = "Weak missile enabled.";
				break;
			case 2:
				statusText.text = "Faster loading time (-1)";
				break;
			case 3:
				statusText.text = "Faster loading time (-1) and increased damage (+1)";
				break;
			case 4:
				statusText.text = "Faster loading time (-1) and increased damage (+1)";
				break;
			case 5:
				statusText.text = "Missiles are maxed out";
				break;
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayIncreasedBlaster()
	 *
	 * Description : Displays what each blaster level increase
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayIncreasedBlaster()
	{
		switch(playerPowers[(int)powerSelected.BLASTER]+1)
		{
			case 1:
				string newText = "Weak blasters enabled";

				int blasterUpgrade = PlayerPrefs.GetInt("BlasterUpgradeFireRate");

				if(blasterUpgrade < 1)
					newText += ". Must buy blaster fire rate upgrade first";

				statusText.text = newText;
				break;
			case 2:
				statusText.text = "Faster loading time (-1) and increased damage (+1)";
				break;
			case 3:
				statusText.text = "Faster loading time (-1) and increased damage (+1)";
				break;
			case 4:
				statusText.text = "Faster loading time (-1) and increased damage (+1)";
				break;
			case 5:
				statusText.text = "Blaster is maxed out";
				break;
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayIncreasedShield()
	 *
	 * Description : Displays what each shield level increase
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayIncreasedShield()
	{
		switch(playerPowers[(int)powerSelected.SHIELD]+1)
		{
			case 1:
				string newText = "Weak shields enabled (up to one)";

				int shieldUpgrade = PlayerPrefs.GetInt("ShieldUpgradeNumber");

				if(shieldUpgrade < 1)
					newText += ". Must buy shield number upgrade first";

				statusText.text = newText;
				break;
			case 2:
				statusText.text = "Faster recharging time (-1)";
				break;
			case 3:
				statusText.text = "Faster recharging time (-1) and increased max shields (+1)";
				break;
			case 4:
				statusText.text = "Faster recharging time (-1) and increased max shields (+1)";
				break;
			case 5:
				statusText.text = "Faster recharging time (-1)";
				break;
			case 6:
				statusText.text = "Shields are maxed out";
				break;
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayIncreasedRadar()
	 *
	 * Description : Displays what each radar level increase
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayIncreasedRadar()
	{
		switch(playerPowers[(int)powerSelected.RADAR]+1)
		{
		case 1:			
			statusText.text = "Radar shows level progression";
			break;
		case 2:
			statusText.text = "Radar shows incoming enemy waves";
			break;
		case 3:
			statusText.text = "Radar is maxed out";
			break;
		}
		
	}

	
	/* ----------------------------------------------------------------------- */
	/* Function    : displayIncreasedRepair()
	 *
	 * Description : Displays what each repair level increase
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayIncreasedRepair()
	{
		switch(playerPowers[(int)powerSelected.REPAIR]+1)
		{
		case 1:			
			statusText.text = "The ship can repair in combat";
			break;
		case 2:
			statusText.text = "Repair is maxed out";
			break;
		}
		
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
			//Shield
			case (int)powerSelected.SHIELD:
				displayShield();
				break;
			//Radar
			case (int)powerSelected.RADAR:
				displayRadar();
				break;
			//or repair
			case (int)powerSelected.REPAIR:
				displayRepair();
				break;
			//Or missile count
			case 8:
				statusText.text = "Missile Count: " + missileCount.text;
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
				statusText.text = "Engine Level 4. Good max speed. Good acceleration. The ship moves swiftly.";
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
			statusText.text = "Blaster Level 1. Low base damage. Low base reload rate. The blaster is at a snail's pace.";
			break;
		case 2:
			statusText.text = "Blaster Level 2. Good base damage. Good base reload rate. The blaster is intimidating.";
			break;
		case 3:
			statusText.text = "Blaster Level 3. Great base damage. Great base reload rate. The blaster is a large threat.";
			break;
		case 4:
			statusText.text = "Blaster Level 4. Crazy base damage. Crazy base reload rate. The blaster can cut through anything!";
			break;
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
			statusText.text = "Shield Level 4. Up to three shields. Great base recharge rate. The shield is almost unstoppable.";
			break;
		case 5:
			statusText.text = "Shield Level 5. Up to three shields. Amazing base recharge rate. The shield is almost impenetrable! The ship is a tank!.";
			break;
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayRadar()
	 *
	 * Description : Displays what each radar level does
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayRadar()
	{
		switch(playerPowers[(int)powerSelected.RADAR])
		{
		case 0:
			statusText.text = "Radar Level 0. Radar is not active.";
			break;
		case 1:
			statusText.text = "Radar Level 1. Radar shows level progression.";
			break;
		case 2:
			statusText.text = "Radar Level 2. Radar shows level progression and incoming large waves.";
			break;
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayRepair()
	 *
	 * Description : Displays what each repair level does
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayRepair()
	{
		switch(playerPowers[(int)powerSelected.REPAIR])
		{
		case 0:
			statusText.text = "Repair Level 0. On-board repair is not active.";
			break;
		case 1:
			statusText.text = "Repair Level 1. The ship can be repaired in battle.";
			break;
		}
		
	}
}
