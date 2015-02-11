/* Module      : Gunner.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls input from the player who is currently the gunner.
 *
 * Date        : 2015/1/16
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

public class Gunner : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//The transform of the turret
	public Transform bulletTurret;

	//The transform of the missile shooter
	public Transform missileTurret;

	//The transform of the laser shooter
	public Transform laserTurret;

	//The tip of the turret
	public Transform turretEnd;

	//The prefab object for the bullet
	public GameObject bulletPrefab;

	//The prefab object for the missile
	public GameObject missilePrefab;

	//The prefab object for the laser
	public GameObject laserPrefab;

	//Is the player ready to shoot a bullet?
	bool readyBullet;

	//Is the player ready to shoot a missile?
	bool readyMissile;

	//Is the player ready to shoot a laser?
	bool readyLaser;

	//Player is going to shoot a bullet
	bool shootingBullet;

	//Player is going to shoot a missile
	bool shootingMissile;

	//Player is going to shoot a laser
	bool shootingLaser;

	//Counter for reloading bullets
	int bulletShootTimer;

	//Time before the player can shoot bullets again 
	int bulletReloadTime;

	//Time before the player can shoot lasers again 
	int laserReloadTime;

	//Counter for reloading missiles
	int missileShootTimer;

	//Counter for reloading lasers
	int laserShootTimer;
	
	//Time before the player can shoot missiles again 
	int missileReloadTime;

	//The damage bullets will deal
	int bulletDamage;

	//The damage missiles will deal
	int missileDamage;

	//The damage lasers will deal
	int laserDamage;

	//The power levels for each weapon
	int blasterPower;
	int missilePower;
	int laserPower;

	//Randomizer script
	public GameObject pauseObject;
	PauseController pauseMenu;

	//Sliders to display weapon levels
	public Slider blasterBar;
	public Slider laserBar;
	public Slider missileBar;

	//Number of missiles the player has
	int missiles;

	//Get the portrait controller to play audio clips
	PortraitController portraitController;

	//Flag for playing the missile audio clip
	bool missileCheck;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the fire rate variables
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {
		//Set the shoot timer to its reload time
		bulletShootTimer = bulletReloadTime;
		missileShootTimer = missileReloadTime;
		laserShootTimer = laserReloadTime;

		//Load the pause controller object 
		pauseMenu = (PauseController)pauseObject.GetComponent("PauseController");

		//Pull the values from player prefs
		blasterPower = PlayerPrefs.GetInt ("BlasterPower", 0);
		missilePower = PlayerPrefs.GetInt ("MissilePower", 0);
		laserPower = PlayerPrefs.GetInt ("LaserPower", 0);

		//Set all weapons based on power levels
		setBlaster (blasterPower);
		setLaser (laserPower);
		setMissile (missilePower);

		//Display the weapon levels
		blasterBar.value = blasterPower;
		laserBar.value = laserPower;
		missileBar.value = missilePower;

		//Load the players missiles
		missiles = PlayerPrefs.GetInt ("Missiles", 0);

		//Find the portrait controller script
		portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();

		//The audio file should not defaultly play
		missileCheck = false;
	}
	

	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Spawns a bullet object, rotates it towards the mouse click location.
	 * 				Also sends the mouse click location to the bullet object.
	 * 				Also deals with firing rates and reloading time.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {

		//Play the missile clip only when the missiles go under 5 for the first time.
		//Dont play it again until it goes over 5 then back under
		if(!missileCheck && missiles < 2)
		{
			missileCheck = true;
			portraitController.playMissilesLow();
		}
		else if(missiles >= 2)
		{
			missileCheck = false;
		}
			
			
		//If the player is "reloading" a bullet, don't decrement the timer
		if (!readyBullet) {
			
			//Decrements the shoot timer
			bulletShootTimer--;
			
			//If the shoot timer has reached 0, reset it and flag that the player can shoot
			if (bulletShootTimer <= 0) {
				
				readyBullet = true;
				bulletShootTimer = bulletReloadTime;
			}
		}

		//If the player is "reloading" a missile, don't decrement the timer
		if (!readyMissile) {
			
			//Decrements the shoot timer
			missileShootTimer--;
			
			//If the shoot timer has reached 0, reset it and flag that the player can shoot
			if (missileShootTimer <= 0) {
				
				readyMissile = true;
				missileShootTimer = missileReloadTime;
			}
		}

		//If the player is "reloading" a laser, don't decrement the timer
		if (!readyLaser) {
			
			//Decrements the shoot timer
			laserShootTimer--;
			
			//If the shoot timer has reached 0, reset it and flag that the player can shoot
			if (laserShootTimer <= 0) {
				
				readyLaser = true;
				laserShootTimer = laserReloadTime;
			}
		}

		//Read the mouse location in pixels
		Vector3 mousePos = Input.mousePosition;
		
		//Set the z offset since the camera is at -10z
		mousePos.z = 10;
		
		//Store the mouse's position in world coordinates
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (mousePos);

		//Store the direction of the player in respect to the bullet
		Vector3 turretDirection = mouseWorldPos-bulletTurret.position;
	
		//Rotate the turret towards the mouse
		bulletTurret.rotation = Quaternion.LookRotation(turretDirection, bulletTurret.up);
		laserTurret.rotation = Quaternion.LookRotation(turretDirection, laserTurret.up);

		//If the user clicked the left mouse button
		if (shootingBullet && readyBullet) {

			//Flag that player has just shot
			readyBullet = false;

			//Instantiate a bullet with bulletPrefab at the players turret
			GameObject bullet = (GameObject)Instantiate(bulletPrefab,turretEnd.position, Quaternion.identity);
						
			//Store the direction of the player in respect to the bullet
			Vector3 direction = mouseWorldPos-bullet.transform.position;

			//Rotate the bullet towards the player
			bullet.transform.rotation = Quaternion.LookRotation(direction);
			
			//Rotate the bullet along the y so that it faces the mouse point
			bullet.transform.Rotate(0,90,0);

			//Send the damage the bullet will deal to the bullet
			bullet.GetComponent<Bullet>().setDamage(bulletDamage);

			//If power level 3, then enable tri-shot
			if(PlayerPrefs.GetInt("BlasterPower",0) == 3)
			{
				//SECOND BULLET
				//Instantiate a bullet with bulletPrefab at the players turret
				GameObject bullet2 = (GameObject)Instantiate(bulletPrefab,turretEnd.position, Quaternion.identity);
				
				//Store the direction of the player in respect to the bullet
				Vector3 direction2 = mouseWorldPos-bullet2.transform.position;
				
				//Rotate the bullet towards the player
				bullet2.transform.rotation = Quaternion.LookRotation(direction2);
				
				//Rotate the bullet along the y so that it faces the mouse point
				bullet2.transform.Rotate(0,90,5);
				
				//Send the damage the bullet will deal to the bullet
				bullet2.GetComponent<Bullet>().setDamage(bulletDamage);


				//THIRD BULLET
				//Instantiate a bullet with bulletPrefab at the players turret
				GameObject bullet3 = (GameObject)Instantiate(bulletPrefab,turretEnd.position, Quaternion.identity);
				
				//Store the direction of the player in respect to the bullet
				Vector3 direction3 = mouseWorldPos-bullet3.transform.position;
				
				//Rotate the bullet towards the player
				bullet3.transform.rotation = Quaternion.LookRotation(direction3);
				
				//Rotate the bullet along the y so that it faces the mouse point
				bullet3.transform.Rotate(0,90,-5);
				
				//Send the damage the bullet will deal to the bullet
				bullet3.GetComponent<Bullet>().setDamage(bulletDamage);
			}
		
		}

		//If the user clicked the right mouse button
		if (shootingMissile) {

			//Subtract the number of missiles
			missiles--;

			//Store that pref
			PlayerPrefs.SetInt("Missiles",missiles);

			//Flag that player has just shot
			readyMissile = false;
			shootingMissile = false;
			
			//Instantiate a bullet with bulletPrefab at the players current location
			GameObject missile = (GameObject)Instantiate(missilePrefab,missileTurret.position,Quaternion.identity);
			
			//Store the direction of the player in respect to the bullet
			Vector3 direction = mouseWorldPos-missile.transform.position;
			
			//Rotate the bullet towards the player
			missile.transform.rotation = Quaternion.LookRotation(direction);

			//Send the damage the bullet will deal to the bullet
			missile.GetComponent<Missile>().setDamage(missileDamage);
			
		}

		//If the usesr has clicked the middle mouse button
		if(shootingLaser)
		{
			//Create the laser object and store it
			GameObject laser = (GameObject)Instantiate(laserPrefab,laserTurret.position,Quaternion.identity);

			//Transfer the damage to the laser object
			laser.GetComponent<Laser>().setDamage(laserDamage);

			//Flag that the laser was shot
			readyLaser = false;
			shootingLaser = false;

		}
	
	}

	void Update() {

		//If the game is paused, don't do anything
		if (!pauseMenu.IsPaused()) 
		{
			//If the player tries to shoot a bullet and can
			if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {

				//Flag the shoot
				shootingBullet = true;

			}
			else if(Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) {
				
				//Flag the shoot
				shootingBullet = false;

			}

			//If the player tries to shoot a missile and can
			if(readyMissile && (Input.GetMouseButtonDown(1) || Input.GetKeyDown("m")) && missilePower > 0 && missiles > 0) {
				//Flag the shoot
				shootingMissile= true;

			}

			//If the player tries to shoot a laser and can
			if(readyLaser && (Input.GetMouseButtonDown(2) || Input.GetKeyDown("l")) && laserPower > 0) {

				//Flag the shoot
				shootingLaser= true;
				
			}
		}

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setBlaser(int level)
	 *
	 * Description : Used to set the appropriate values based on blaster power level
	 *
	 * Parameters  : int level : The blaster power level
	 *
	 * Returns     : Void
	 */
	void setBlaster(int level)
	{
		//Set the reload and damage stats based on power level
		switch (level)
		{
		case 0:
			bulletDamage = 1;
			bulletReloadTime = 22;
			break;
		case 1:
			bulletDamage = 1;
			bulletReloadTime = 18;
			break;
		case 2:
			bulletDamage = 1;
			bulletReloadTime = 14;
			break;
		case 3:
			bulletDamage = 2;
			bulletReloadTime = 14;
			break;
		}

		//If the player has bought the bullet speed upgrade
		//Then reduce reload time
		bulletReloadTime -= PlayerPrefs.GetInt("BlasterUpgradeSpeed")*2;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : setLaser(int level)
	 *
	 * Description : Used to set the appropriate values based on laser power level
	 *
	 * Parameters  : int level : The laser power level
	 *
	 * Returns     : Void
	 */
	void setLaser(int level)
	{
		//Set the reload and damage stats based on power level
		switch (level)
		{
		case 0:
			laserDamage = 0;
			laserReloadTime = 9999;
			break;
		case 1:
			laserDamage = 4;
			laserReloadTime = 600;
			break;
		case 2:
			laserDamage = 8;
			laserReloadTime = 570;
			break;
		case 3:
			laserDamage = 12;
			laserReloadTime = 540;
			break;
		case 4:
			laserDamage = 16;
			laserReloadTime = 510;
			break;
		}

		//Subtract reload time based on laser upgrade
		laserReloadTime -= PlayerPrefs.GetInt ("LaserUpgradeFireRate", 0) * 60;

		//If the player has bought the laser damage upgrade
		//Then increase damage
		laserDamage += PlayerPrefs.GetInt("LaserUpgradeDamage",0) * 4;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : setMissile(int level)
	 *
	 * Description : Used to set the appropriate values based on missile power level
	 *
	 * Parameters  : int level : The missile power level
	 *
	 * Returns     : Void
	 */
	void setMissile(int level)
	{
		//Set the reload and damage stats based on power level
		switch (level)
		{
		case 0:
			missileDamage = 0;
			missileReloadTime = 99999;
			break;
		case 1:
			missileDamage = 2;
			missileReloadTime = 720;
			break;
		case 2:
			missileDamage = 2;
			missileReloadTime = 600;
			break;
		case 3:
			missileDamage = 3;
			missileReloadTime = 480;
			break;
		case 4:
			missileDamage = 3;
			missileReloadTime = 360;
			break;
		}

		//Subtract reload time based on missile upgrade
		missileReloadTime -= PlayerPrefs.GetInt ("MissileUpgradeLoader", 0) * 20;
		
		//If the player has bought the missile payload upgrade
		//Then increase damage
		missileDamage += PlayerPrefs.GetInt("MissileUpgradePayload",0) * 2;
	}

}
