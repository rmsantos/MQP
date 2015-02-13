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

	//Upgrade management variables
	delegate void Upgrade();

	Upgrade[] upgradeFunction;

	public int[] engine1Cost;
	public int[] blaster1Cost;
	public int[] blaster2Cost;
	public int[] shields1Cost;
	public int[] shields2Cost;
	public int[] shields3Cost;
	public int[] power1Cost;
	public int[] missiles1Cost;
	public int[] missiles2Cost;
	public int[] cargo1Cost;
	public int[] cargo2Cost;
	public int[] cargo3Cost;
	public int[] hull1Cost;
	public int[] hull2Cost;
	public int[] lasers1Cost;
	public int[] lasers2Cost;
	public int[] lasers3Cost;

	public Slider playerHealthSlider;

	public Button[] selectButtons;

	int[][] costs;

	int[] upgradeLevel;

	string[] upgradePrefs = {"EngineUpgrade", 
		"BlasterUpgradeFireRate", "BlasterUpgradeDamage", 
		"ShieldUpgradeNumber", "ShieldUpgradeRecharge", "ShieldUpgradeHardened",
		"PowerUpgrade", 
		"MissileUpgradePayload", "MissileUpgradeLoader", 
		"CargoUpgradeMissiles", "CargoUpgradeCrystals", "CargoUpgradeCredits",
		"HullUpgradeReinforced", "HullUpgradeAsteroidResistance", 
		"LaserUpgradeSpeed", "LaserUpgradeDamage", "LaserUpgradeBurst"};
	
	public string[] descriptions;

	//Flags on whether to start the game
	bool startGame;
	
	//The buttons available to press
	public Button startButton;
	public Button purchaseButton;

	//The dynamic text found on the screen
	public Text moneyText;
	public Text crystalText;
	public Text missileText;
	public Text descriptionText;
	public Text scoreText;
	public Text levelText;
	public Text[] upgradeCosts;

	//Player variables that are displayed
	int money;
	int missiles;
	int crystals;
	int score;
	int playerHealth;

	//The actively selected upgrade (very important)
	int selected;

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

		//Populate the upgrade functions
		upgradeFunction = new Upgrade[] {UpgradeEngine1,
										   UpgradeBlaster1, UpgradeBlaster2,
										   UpgradeShields1, UpgradeShields2, UpgradeShields3,
										   UpgradePower1, 
										   UpgradeMissiles1, UpgradeMissiles2,
										   UpgradeCargo1, UpgradeCargo2, UpgradeCargo3,
										   UpgradeHull1, UpgradeHull2,
										   UpgradeLasers1, UpgradeLasers2, UpgradeLasers3};

		//Populate the cost arrays
		costs = new int[][] {engine1Cost, 
							blaster1Cost, blaster2Cost, 
							shields1Cost, shields2Cost, shields3Cost, 
							power1Cost,
							missiles1Cost, missiles2Cost, 
							cargo1Cost, cargo2Cost, cargo3Cost, 
							hull1Cost, hull2Cost, 
							lasers1Cost, lasers2Cost, lasers3Cost};

		//Initialize the player values from playerprefs
		money = PlayerPrefs.GetInt ("Money", 0);
		missiles = PlayerPrefs.GetInt ("Missiles", 0);
		crystals = PlayerPrefs.GetInt ("Crystals", 0);
		score = PlayerPrefs.GetInt ("Score", 0);
		playerHealth = PlayerPrefs.GetInt ("Health", 0);

		//Display the player values
		moneyText.text = money.ToString();
		scoreText.text = score.ToString();
		missileText.text = missiles.ToString();
		crystalText.text = crystals.ToString();
		playerHealthSlider.value = playerHealth;
		
		//Setup and retreive the previously bought upgrade levels
		upgradeLevel = new int[upgradePrefs.Length];
		for (int i = 0; i < upgradePrefs.Length; i++) {
			upgradeLevel[i] = PlayerPrefs.GetInt (upgradePrefs[i], 0);
			if (costs[i].GetLength(0) > upgradeLevel[i]) {
				upgradeCosts[i].text = costs[i][upgradeLevel[i]].ToString();
			}
			else {
				upgradeCosts[i].text = "MAX";
			}
		}

		//Initialize start state to false
		startGame = false;

		//Selected should be set to a value that isn't an upgrade value, so -1
		selected = -1;
		purchaseButton.interactable = false;

		//Check for special case button disables
		if (upgradeLevel[3] < 1) {
			selectButtons[4].interactable = false;
			selectButtons[5].interactable = false;
		}
		if (upgradeLevel[14] < 1) {
			selectButtons[15].interactable = false;
		}

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
			PlayerPrefs.SetInt("Money", money);
			PlayerPrefs.SetInt("Missiles", missiles);
			PlayerPrefs.SetInt("Crystals", crystals);
			PlayerPrefs.SetInt("Score", score);
			PlayerPrefs.SetInt("Health", playerHealth);
			//Load the main game
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
		startGame = start;
	}

	public void Purchase() {

		if (costs[selected][upgradeLevel[selected]] <= money) {

			//Do general "every upgrade" purchasing logic
			money -= costs[selected][upgradeLevel[selected]];
			moneyText.text = money.ToString();
			upgradeLevel[selected] = upgradeLevel[selected] + 1;
			PlayerPrefs.SetInt(upgradePrefs[selected], upgradeLevel[selected]);
			Select(selected);
			descriptionText.text = "Upgrade purchased!";
				
			//Do the specific upgrade logic
			upgradeFunction[selected]();

		}
		else {
			descriptionText.text = "Not enough money!";
		}

	}

	public void Select(int select) {

		//Set selected variable to appropriate value
		selected = select;

		//Display the appropriate cost 
		if (costs[selected].GetLength(0) > upgradeLevel[selected]) {
			upgradeCosts[selected].text = costs[selected][upgradeLevel[selected]].ToString();
			purchaseButton.interactable = true;
		}
		else {
			upgradeCosts[selected].text = "MAX";
			purchaseButton.interactable = false;
		}

		//Set descriptive texts
		descriptionText.text = descriptions[selected];
		levelText.text = "lvl: " + upgradeLevel[selected].ToString();

	}

	public void UpgradeEngine1() {

	}

	public void UpgradeBlaster1() {

	}

	public void UpgradeBlaster2() {

	}

	public void UpgradeShields1() {
		selectButtons[4].interactable = true;
		selectButtons[5].interactable = true;
	}

	public void UpgradeShields2() {

	}

	public void UpgradeShields3() {

	}

	public void UpgradePower1() {

	}

	public void UpgradeMissiles1() {

	}

	public void UpgradeMissiles2() {

	}

	public void UpgradeCargo1() {

	}

	public void UpgradeCargo2() {

	}

	public void UpgradeCargo3() {

	}

	public void UpgradeHull1() {

	}

	public void UpgradeHull2() {

	}

	public void UpgradeLasers1() {
		selectButtons[15].interactable = true;
	}

	public void UpgradeLasers2() {

	}

	public void UpgradeLasers3() {

	}

	//Used to purchase crystal
	public void PurchaseCrystal() {

		if (money >= 4) {
			crystals++;
			money -= 4;
			moneyText.text = money.ToString();
			crystalText.text = crystals.ToString();
		}

	}

	//Used to purchase missile
	public void PurchaseMissile() {

		if (money >= 5) {
			missiles++;
			money -= 5;
			moneyText.text = money.ToString ();
			missileText.text = missiles.ToString ();
		}

	}

	//Used to sell missile
	public void SellMissile() {

		if (missiles >= 1) {
			missiles--;
			money += 2;
			moneyText.text = money.ToString();
			missileText.text = missiles.ToString();
		}

	}

	//Used to sell crystal
	public void SellCrystal() {

		if (crystals >= 1) {
			crystals--;
			money += 2;
			moneyText.text = money.ToString();
			crystalText.text = crystals.ToString();
		}

	}

	//Used to increase score
	public void PurchaseScore() {

		if (money >= 25) {
			score += 100;
			money -= 25;
			moneyText.text = money.ToString();
			scoreText.text = score.ToString();
		}
		
	}

	//Used to repair hull
	public void RepairHull() {

		if (money >= 5 && playerHealth < 100) {

			money -= 5;
			playerHealth += 5;
			if (playerHealth > 100) {
				playerHealth = 100;
			}
			moneyText.text = money.ToString();
			playerHealthSlider.value = playerHealth;

		}

	}

}
