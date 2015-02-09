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
	public int[] hull1Cost;
	public int[] lasers1Cost;
	public int[] lasers2Cost;

	int[][] costs;

	int[] upgradeLevel;

	string[] upgradePrefs = {"EngineUpgrade", 
		"BlasterUpgradeSpeed", "BlasterUpgradeBurst", 
		"ShieldUpgradeRecharge", "ShieldUpgradeNumber", "ShieldUpgradeHardened",
		"PowerUpgrade", 
		"MissileUpgradeLoader", "MissileUpgradePayload", 
		"CargoUpgradeMissiles", "CargoUpgradeCrystals",
		"HullUpgrade", 
		"LaserUpgradeEmplacement", "LaserUpgradeDamage"};
	
	public string[] descriptions;

	//Flags on whether to start the game
	bool startGame;
	
	//The buttons available to press
	public Button startButton;
	public Button damageButton;
	public Button purchaseButton;
	public Button shieldButton;
	public Button laserButton;
	public Button missileButton;
	public Button healthButton;

	//The dynamic text found on the screen
	public Text moneyText;
	public Text crystalText;
	public Text missileText;
	public Text statusText;
	public Text descriptionText;
	public Text CostText;

	int money;
	int missiles;
	int crystals;

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

		upgradeFunction = new Upgrade[] {UpgradeEngine1,
										   UpgradeBlaster1, UpgradeBlaster2,
										   UpgradeShields1, UpgradeShields2, UpgradeShields3,
										   UpgradePower1, 
										   UpgradeMissiles1, UpgradeMissiles2,
										   UpgradeCargo1, UpgradeCargo2,
										   UpgradeHull1,
										   UpgradeLasers1, UpgradeLasers2};

		costs = new int[][] {engine1Cost, 
							blaster1Cost, blaster2Cost, 
							shields1Cost, shields2Cost, shields3Cost, 
							power1Cost,
							missiles1Cost, missiles2Cost, 
							cargo1Cost, cargo2Cost, 
							hull1Cost, 
							lasers1Cost, lasers2Cost};

		money = PlayerPrefs.GetInt ("Money", 0);
		missiles = PlayerPrefs.GetInt ("Missiles", 0);
		crystals = PlayerPrefs.GetInt ("Crystals", 0);
		
		//Upgrades
		for (int i = 0; i < upgradePrefs.Length; i++) {
			upgradeLevel[i] = PlayerPrefs.GetInt (upgradePrefs[i], 0);
		}

		moneyText.text = (money.ToString());

		//Initialize states to not pressed
		startGame = false;

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
			statusText.text = "Upgrade purchased!";
				
			//Do the specific upgrade logic
			upgradeFunction[selected]();

		}
		else {
			statusText.text = "Not enough money!";
		}

	}

	public void Select(int select) {

		selected = select;
		if (costs.GetLength(selected) < upgradeLevel[selected]) {
			CostText.text = costs[selected][upgradeLevel[selected]].ToString();
		}
		else {
			CostText.text = "MAX";
		}
		descriptionText.text = descriptions[selected];
		statusText.text = "";

	}

	public void UpgradeEngine1() {

	}

	public void UpgradeBlaster1() {

	}

	public void UpgradeBlaster2() {

	}

	public void UpgradeShields1() {

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

	public void UpgradeHull1() {

	}

	public void UpgradeLasers1() {

	}

	public void UpgradeLasers2() {

	}

	public void PurchaseCrystal() {

		crystals++;
		money -= 4;
		moneyText.text = money.ToString();
		crystalText.text = crystals.ToString();

	}

	public void PurchaseMissile() {

		missiles++;
		money -= 5;
		moneyText.text = money.ToString();
		missileText.text = missiles.ToString();

	}

	public void SellMissile() {

		missiles--;
		money += 2;
		moneyText.text = money.ToString();
		missileText.text = missiles.ToString();

	}

	public void SellCrystal() {

		crystals--;
		money += 2;
		moneyText.text = money.ToString();
		crystalText.text = crystals.ToString();

	}

}
