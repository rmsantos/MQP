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
			CostText.text = "MAX";
		}
		else {
			CostText.text = damageCost[damageUpgrade].ToString();
		}

	}

	public void purchase() {

		switch (selected) {

			default:
				break;
		}

	}

	public void SelectDamage() {

		selected = (int) upgradeSelected.DAMAGE;

	}

	public void SelectHealth() {

		selected = (int) upgradeSelected.HEALTH;

	}

	public void SelectMissiles() {

		selected = (int) upgradeSelected.MISSILE;

	}

	public void SelectShields() {

		selected = (int) upgradeSelected.SHIELD;

	}

	public void SelectLaser() {

		selected = (int) upgradeSelected.LASER;

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
		}
		else {
			statusText.text = "Not enough money!";
		}
	}

}
