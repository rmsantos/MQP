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

	public enum upgradeSelected { DAMAGE = 0, HEALTH = 1, MISSILE = 2, LASER = 3, SHIELD = 4}

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

	public int[] damageCost;
	public int[] healthCost;
	public int[] missileCost;
	public int[] laserCost;
	public int[] shieldCost;

	public Text moneyText;
	public Text statusText;
	public Text CostText;
	
	int damage;
	int health;
	int money;

	int damageUpgrade;
	int healthUpgrade;
	int missileUpgrade;
	int laserUpgrade;
	int shieldUpgrade;
	
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

		damage = PlayerPrefs.GetInt ("Damage", 1);
		money = PlayerPrefs.GetInt ("Money", 0);

		damageUpgrade = PlayerPrefs.GetInt ("DamageUpgrade", 0);
		healthUpgrade = PlayerPrefs.GetInt ("HealthUpgrade", 0);
		missileUpgrade = PlayerPrefs.GetInt ("MissileUpgrade", 0);
		laserUpgrade = PlayerPrefs.GetInt ("LaserUpgrade", 0);
		shieldUpgrade = PlayerPrefs.GetInt ("ShieldUpgrade", 0);

		moneyText.text = ("Money: " + money.ToString());

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

	void UpdateUpgrades() {

		//DAMAGE
		if (damageUpgrade >= damageCost.Length) {
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
		if (shieldUpgrade >= shieldCost.Length) {
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
	}

	public void purchase() {

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

	}

	public void SelectDamage() {

		selected = (int) upgradeSelected.DAMAGE;
		CostText.text = damageCost[damageUpgrade].ToString();

	}

	public void SelectHealth() {

		selected = (int) upgradeSelected.HEALTH;
		CostText.text = healthCost[healthUpgrade].ToString();

	}

	public void SelectMissiles() {

		selected = (int) upgradeSelected.MISSILE;
		CostText.text = missileCost[missileUpgrade].ToString();

	}

	public void SelectShields() {

		selected = (int) upgradeSelected.SHIELD;
		CostText.text = shieldCost[shieldUpgrade].ToString();

	}

	public void SelectLaser() {

		selected = (int) upgradeSelected.LASER;
		CostText.text = laserCost[laserUpgrade].ToString();

	}

	public void UpgradeDamage() {
		if (money >= damageCost[damageUpgrade]) {
			money -= damageCost[damageUpgrade];
			damage++;
			damageUpgrade++;
			PlayerPrefs.SetInt("Money", money);
			PlayerPrefs.SetInt("Damage", damage);
			PlayerPrefs.SetInt("DamageUpgrade", damageUpgrade);
			moneyText.text = "Money: " + money.ToString();
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
			moneyText.text = "Money: " + money.ToString();
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
			moneyText.text = "Money: " + money.ToString();
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
			moneyText.text = "Money: " + money.ToString();
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
			moneyText.text = "Money: " + money.ToString();
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

}
