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

	public enum upgradeSelected {ENGINE1 = 0, 
								 BLASTER1 = 1, BLASTER2 = 2, 
								 SHIELDS1 = 3, SHIELDS2 = 4, SHIELDS3 = 5, 
								 POWER1 = 6, POWER2 = 7, 
								 MISSILES1 = 8, MISSILES2 = 9, 
								 CARGO1 = 10, GARGO2 = 11, 
								 HULL1 = 12, 
								 LASERS1 = 13, LASERS2 = 14};

	delegate void Upgrade();

	Upgrade[] upgradeFunction;

	//Flags on whether to start the game
	bool startGame;

	//The start button
	public Button startButton;
	public Button damageButton;
	public Button purchaseButton;
	public Button shieldButton;
	public Button laserButton;
	public Button missileButton;
	public Button healthButton;

	public int[] engine1Cost;
	public int[] blaster1Cost;
	public int[] blaster2Cost;
	public int[] shields1Cost;
	public int[] shields2Cost;
	public int[] shields3Cost;
	public int[] power1Cost;
	public int[] power2Cost;
	public int[] missiles1Cost;
	public int[] missiles2Cost;
	public int[] cargo1Cost;
	public int[] cargo2Cost;
	public int[] hull1Cost;
	public int[] lasers1Cost;
	public int[] lasers2Cost;

	int[][] costs;

	public Text moneyText;
	public Text crystalText;
	public Text missileText;
	public Text statusText;
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
										   UpgradePower1, UpgradePower2,
										   UpgradeMissiles1, UpgradeMissiles2,
										   UpgradeCargo1, UpgradeCargo2,
										   UpgradeHull1,
										   UpgradeLasers1, UpgradeLasers2};

		costs = new int[][] {engine1Cost, 
							blaster1Cost, blaster2Cost, 
							shields1Cost, shields2Cost, shields3Cost, 
							power1Cost, power2Cost, 
							missiles1Cost, missiles2Cost, 
							cargo1Cost, cargo2Cost, 
							hull1Cost, 
							lasers1Cost, lasers2Cost};

		money = PlayerPrefs.GetInt ("Money", 0);
		missiles = PlayerPrefs.GetInt ("Missiles", 0);
		crystals = PlayerPrefs.GetInt ("Crystals", 0);

		/*
		damageUpgrade = PlayerPrefs.GetInt ("DamageUpgrade", 0);
		healthUpgrade = PlayerPrefs.GetInt ("HealthUpgrade", 0);
		missileUpgrade = PlayerPrefs.GetInt ("MissileUpgrade", 0);
		laserUpgrade = PlayerPrefs.GetInt ("LaserUpgrade", 0);
		shieldUpgrade = PlayerPrefs.GetInt ("ShieldUpgrade", 0);
		*/

		moneyText.text = (money.ToString());

		//Initialize states to not pressed
		startGame = false;

		UpdateUpgrades();
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

	void UpdateUpgrades() {

		/*
		//DAMAGE
		if (PlayerPrefs.GetInt("DamageUpgrade", 0) >= damageCost.Length) {
			damageButton.interactable = false;
			if (selected == (int) upgradeSelected.DAMAGE) {
				CostText.text = "MAX";
			}
		}
		//MISSILES
		if (missileUpgrade >= missileCost.Length) {
			missileButton.interactable = false;
			if (selected == (int) upgradeSelected.MISSILE) {
				CostText.text = "MAX";
			}
		}
		//SHIELDS
		if (PlayerPrefs.GetInt("ShieldUpgrade", 0) >= shieldCost.Length) {
			shieldButton.interactable = false;
			if (selected == (int) upgradeSelected.SHIELD) {
				CostText.text = "MAX";
			}
		}
		//HEALTH
		if (healthUpgrade >= healthCost.Length) {
			healthButton.interactable = false;
			if (selected == (int) upgradeSelected.HEALTH) {
				CostText.text = "MAX";
			}
		}
		//LASER
		if (laserUpgrade >= laserCost.Length) {
			laserButton.interactable = false;
			if (selected == (int) upgradeSelected.LASER) {
				CostText.text = "MAX";
			}
		}
		*/
	}

	public void purchase() {

		/*
		switch (selected) {

		case (int)upgradeSelected.DAMAGE:
			UpgradeDamage();
			break;

		case (int)upgradeSelected.HEALTH:
			UpgradeHealth();
			break;

		case (int)upgradeSelected.LASER:
			UpgradeLaser();
			break;

		case (int)upgradeSelected.SHIELD:
			UpgradeShield();
			break;

		case (int)upgradeSelected.MISSILE:
			UpgradeMissile();
			break;

		default:
			break;
		}

		*/

	}

	public void Select(int select) {

		selected = (int) select;
		CostText.text = costs[selected][0].ToString();

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

	public void UpgradePower2() {

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
		PlayerPrefs.SetInt("Money", money);
		PlayerPrefs.SetInt("Crystals", crystals);
		moneyText.text = money.ToString();
		crystalText.text = crystals.ToString();

	}

	public void PurchaseMissile() {

		missiles++;
		money -= 5;
		PlayerPrefs.SetInt("Money", money);
		PlayerPrefs.SetInt("Missiles", missiles);
		moneyText.text = money.ToString();
		missileText.text = missiles.ToString();

	}

	public void SellMissile() {

		missiles--;
		money += 2;
		PlayerPrefs.SetInt("Money", money);
		PlayerPrefs.SetInt("Missiles", missiles);
		moneyText.text = money.ToString();
		missileText.text = missiles.ToString();

	}

	public void SellCrystal() {

		crystals--;
		money += 2;
		PlayerPrefs.SetInt("Money", money);
		PlayerPrefs.SetInt("Crystals", crystals);
		moneyText.text = money.ToString();
		crystalText.text = crystals.ToString();

	}

	/*
	public void UpgradeDamage() {
		if (money >= damageCost[damageUpgrade]) {
			money -= damageCost[damageUpgrade];
			damage++;
			damageUpgrade++;
			PlayerPrefs.SetInt("Money", money);
			PlayerPrefs.SetInt("Damage", damage);
			PlayerPrefs.SetInt("DamageUpgrade", damageUpgrade);
			moneyText.text = money.ToString();
			statusText.text = "Damage upgraded!";
			UpdateUpgrades();
			if (damageUpgrade < damageCost.Length) {
				CostText.text = damageCost[damageUpgrade].ToString();
			}
		}
		else {
			statusText.text = "Not enough money!";
		}
	}

	public void UpgradeShield() {
		if (money >= shieldCost[shieldUpgrade]) {
			money -= shieldCost[shieldUpgrade];
			shieldUpgrade++;
			PlayerPrefs.SetInt("Money", money);
			PlayerPrefs.SetInt("ShieldUpgrade", shieldUpgrade);
			moneyText.text = money.ToString();
			statusText.text = "Shield upgraded!";
			UpdateUpgrades();
			if (shieldUpgrade < shieldCost.Length) {
				CostText.text = shieldCost[shieldUpgrade].ToString();
			}
		}
		else {
			statusText.text = "Not enough money!";
		}
	}

	public void UpgradeLaser() {
		if (money >= laserCost[laserUpgrade]) {
			money -= laserCost[laserUpgrade];
			laserUpgrade++;
			PlayerPrefs.SetInt("Money", money);
			PlayerPrefs.SetInt("LaserUpgrade", laserUpgrade);
			moneyText.text = money.ToString();
			statusText.text = "Laser upgraded!";
			UpdateUpgrades();
			if (laserUpgrade < laserCost.Length) {
				CostText.text = laserCost[laserUpgrade].ToString();
			}
		}
		else {
			statusText.text = "Not enough money!";
		}
	}

	public void UpgradeMissile() {
		if (money >= missileCost[missileUpgrade]) {
			money -= missileCost[missileUpgrade];
			missileUpgrade++;
			PlayerPrefs.SetInt("Money", money);
			PlayerPrefs.SetInt("MissileUpgrade", missileUpgrade);
			moneyText.text = money.ToString();
			statusText.text = "Missile upgraded!";
			UpdateUpgrades();
			if (missileUpgrade < missileCost.Length) {
				CostText.text = missileCost[missileUpgrade].ToString();
			}
		}
		else {
			statusText.text = "Not enough money!";
		}
	}

	public void UpgradeHealth() {
		if (money >= healthCost[healthUpgrade]) {
			money -= healthCost[healthUpgrade];
			healthUpgrade++;
			PlayerPrefs.SetInt("Money", money);
			PlayerPrefs.SetInt("HealthUpgrade", healthUpgrade);
			moneyText.text = money.ToString();
			statusText.text = "Health upgraded!";
			UpdateUpgrades();
			if (healthUpgrade < healthCost.Length) {
				CostText.text = healthCost[healthUpgrade].ToString();
			}
		}
		else {
			statusText.text = "Not enough money!";
		}
	}

	public void UpgradePower()
	{
		//Stuff goes here
		PlayerPrefs.SetInt("PowerUpgrade",1+PlayerPrefs.GetInt("PowerUpgrade",0));
		PlayerPrefs.SetInt("Power",1+PlayerPrefs.GetInt("Power",10));
	}

*/

}
