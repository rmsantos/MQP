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
	bool startTutorial;

	//The start and quit buttons
	public GameObject startButton;
	public GameObject quitButton;
	public GameObject highScoreButton;
	public GameObject tutorialButton;
	
	//The options menu
	public GameObject options;

	//initial values
	public int initialHealth;
	public int initialShieldPower;
	public int initialEnginePower;
	public int initialMissilePower;
	public int initialLaserPower;
	public int initialBlasterPower;
	public int initialRadarPower;
	public int initialRepairPower;
	public int initialMoneyValue;
	public int initialCrystalValue;
	public int initialMissiles;
	public int initialCrystals;
	public int initialMoney;
	public float initialMasterVolume;
	public float initialMusicVolume;
	public float initialVoiceVolume;
	public float initialSoundEffectsVolume;

	//Game values
	public int initialLevel;
	public int initialScore;

	//upgrade values
	public int engineUpgrade;
	public int laserUpgradeSpeed;
	public int laserUpgradeDamage;
	public int laserUpgadeBurst;
	public int shieldUpgradeNumber;
	public int shieldUpgradeRecharge;
	public int shieldUpgradeHardened;
	public int missileUpgradePayload;
	public int missileUpgradeLoader;
	public int cargoUpgradeMissiles;
	public int cargoUpgradeCrystals;
	public int cargoUpgradeCredits;
	public int hullUpgradeReinforced;
	public int hullUpgradeAsteroidResistance;
	public int blasterUpgradeFireRate;
	public int blasterUpgradeDamage;
	public int powerUpgrade;

	public string[] initialHighScorers;
	public int[] initialHighScores;

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
				PlayerPrefs.SetInt("Score" + (i + 1).ToString(), initialHighScores[i]);
				PlayerPrefs.SetString("Name" + (i + 1).ToString(), initialHighScorers[i]);
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
			Application.LoadLevel (5);
		}

		//If the user clicked tutorial and the audio file is done
		if(startTutorial && !tutorialButton.audio.isPlaying)
		{
			//Load the tutorial
			Application.LoadLevel (6);
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
		PlayerPrefs.SetInt ("ShieldPower", initialShieldPower);
		PlayerPrefs.SetInt ("EnginePower", initialEnginePower);
		PlayerPrefs.SetInt ("MissilePower", initialMissilePower);
		PlayerPrefs.SetInt ("LaserPower", initialLaserPower);
		PlayerPrefs.SetInt ("BlasterPower", initialBlasterPower);
		PlayerPrefs.SetInt ("RadarPower", initialRadarPower);
		PlayerPrefs.SetInt ("RepairPower", initialRepairPower);
		PlayerPrefs.SetInt ("MoneyValue", initialMoneyValue);
		PlayerPrefs.SetInt ("CrystalValue", initialCrystalValue);
		PlayerPrefs.SetInt ("Health", initialHealth);
		PlayerPrefs.SetInt ("Missiles", initialMissiles);
		PlayerPrefs.SetInt ("Crystals", initialCrystals);
		PlayerPrefs.SetInt ("Money", initialMoney);
		PlayerPrefs.SetFloat ("MasterVolume", initialMasterVolume);
		PlayerPrefs.SetFloat ("MusicVolume", initialMusicVolume);
		PlayerPrefs.SetFloat ("VoiceVolume", initialVoiceVolume);
		PlayerPrefs.SetFloat ("SoundEffectsVolume", initialSoundEffectsVolume);

		//Upgrades
		PlayerPrefs.SetInt ("PowerUpgrade", powerUpgrade);
		PlayerPrefs.SetInt ("EngineUpgrade", engineUpgrade);
		PlayerPrefs.SetInt ("LaserUpgradeSpeed", laserUpgradeSpeed);
		PlayerPrefs.SetInt ("LaserUpgradeDamage", laserUpgradeDamage);
		PlayerPrefs.SetInt ("LaserUpgradeBurst", laserUpgadeBurst);
		PlayerPrefs.SetInt ("ShieldUpgradeRecharge", shieldUpgradeRecharge);
		PlayerPrefs.SetInt ("ShieldUpgradeNumber", shieldUpgradeNumber);
		PlayerPrefs.SetInt ("ShieldUpgradeHardened", shieldUpgradeHardened);
		PlayerPrefs.SetInt ("MissileUpgradePayload", missileUpgradePayload);
		PlayerPrefs.SetInt ("MissileUpgradeLoader", missileUpgradeLoader);
		PlayerPrefs.SetInt ("CargoUpgradeMissiles", cargoUpgradeMissiles);
		PlayerPrefs.SetInt ("CargoUpgradeCrystals", cargoUpgradeCrystals);
		PlayerPrefs.SetInt ("CargoUpgradeCredits", cargoUpgradeCredits);
		PlayerPrefs.SetInt ("HullUpgradeReinforced", hullUpgradeReinforced);
		PlayerPrefs.SetInt ("HullUpgradeAsteroidResistance", hullUpgradeAsteroidResistance);
		PlayerPrefs.SetInt ("BlasterUpgradeFireRate", blasterUpgradeFireRate);
		PlayerPrefs.SetInt ("BlasterUpgradeDamage", blasterUpgradeDamage);

		//Game values
	
		PlayerPrefs.SetInt ("Score", initialScore);
		PlayerPrefs.SetInt ("Level", initialLevel);

		startGame = start;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setTutorialStart()
	 *
	 * Description : Sets the start tutorial bool to true
	 *
	 * Parameters  : bool start : Start the tutorial?
	 *
	 * Returns     : Void
	 */
	public void setTutorialStart()
	{
		//Set the games power and upgrade levels for the tutorial
		//Power Levels
		PlayerPrefs.SetInt ("ShieldPower", 2);
		PlayerPrefs.SetInt ("EnginePower", 2);
		PlayerPrefs.SetInt ("MissilePower", 2);
		PlayerPrefs.SetInt ("LaserPower", 2);
		PlayerPrefs.SetInt ("BlasterPower", 2);
		PlayerPrefs.SetInt ("RadarPower", 0);
		PlayerPrefs.SetInt ("RepairPower", 0);
		PlayerPrefs.SetInt ("MoneyValue", 0);
		PlayerPrefs.SetInt ("CrystalValue", 0);
		PlayerPrefs.SetInt ("Health", 100);
		PlayerPrefs.SetInt ("Missiles", 5);
		PlayerPrefs.SetInt ("Crystals", 5);
		PlayerPrefs.SetInt ("Money", 0);
		PlayerPrefs.SetFloat ("MasterVolume", 1);
		PlayerPrefs.SetFloat ("MusicVolume", 1);
		PlayerPrefs.SetFloat ("VoiceVolume", 1);
		PlayerPrefs.SetFloat ("SoundEffectsVolume", 1);
		
		//Upgrades
		PlayerPrefs.SetInt ("PowerUpgrade", 0);
		PlayerPrefs.SetInt ("EngineUpgrade", 0);
		PlayerPrefs.SetInt ("LaserUpgradeSpeed", 0);
		PlayerPrefs.SetInt ("LaserUpgradeDamage", 0);
		PlayerPrefs.SetInt ("LaserUpgradeBurst", 0);
		PlayerPrefs.SetInt ("ShieldUpgradeRecharge",1);
		PlayerPrefs.SetInt ("ShieldUpgradeNumber", 1);
		PlayerPrefs.SetInt ("ShieldUpgradeHardened",0);
		PlayerPrefs.SetInt ("MissileUpgradePayload", 0);
		PlayerPrefs.SetInt ("MissileUpgradeLoader", 0);
		PlayerPrefs.SetInt ("CargoUpgradeMissiles", 0);
		PlayerPrefs.SetInt ("CargoUpgradeCrystals", 0);
		PlayerPrefs.SetInt ("CargoUpgradeCredits", 0);
		PlayerPrefs.SetInt ("HullUpgradeReinforced", 0);
		PlayerPrefs.SetInt ("HullUpgradeAsteroidResistance", 0);
		PlayerPrefs.SetInt ("BlasterUpgradeFireRate", 1);
		PlayerPrefs.SetInt ("BlasterUpgradeDamage", 0);

		startTutorial = true;
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

	public void Options(bool visible) {
		options.SetActive (visible);
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
