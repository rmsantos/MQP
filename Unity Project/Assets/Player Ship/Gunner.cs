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
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Gunner : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//The prefab object for the bullet
	public GameObject bulletPrefab;

	//The prefab object for the missile
	public GameObject missilePrefab;

	//Is the player ready to shoot a bullet?
	bool readyBullet;

	//Is the player ready to shoot a missile?
	bool readyMissile;

	//Player is going to shoot a bullet
	bool shootingBullet;

	//Player is going to shoot a missile
	bool shootingMissile;

	//Counter for reloading bullets
	int bulletShootTimer;

	//Time before the player can shoot bullets again 
	int bulletReloadTime;

	//Counter for reloading issiles
	int missileShootTimer;
	
	//Time before the player can shoot missiles again 
	int missileReloadTime;

	//The damage bullets will deal
	int bulletDamage;

	//The damage missiles will deal
	int missileDamage;

	//Randomizer script
	public GameObject pauseObject;
	PauseController pauseMenu;

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
		//Defaultly be ready to shoot
		readyBullet = true;
		readyMissile = true;

		//Set the shoot timer to its reload time
		bulletShootTimer = bulletReloadTime;
		missileShootTimer = missileReloadTime;

		pauseMenu = (PauseController)pauseObject.GetComponent("PauseController");

		//Pull the values from player prefs
		bulletDamage = PlayerPrefs.GetInt ("Damage", 100);
		bulletReloadTime = PlayerPrefs.GetInt ("Reload", 1);

		missileDamage = 5;
		missileReloadTime = 50;

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

		//If the user clicked the left mouse button
		if (shootingBullet) {

			//Flag that player has just shot
			readyBullet = false;
			shootingBullet = false;

			//Instantiate a bullet with bulletPrefab at the players current location
			GameObject bullet = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

			//Read the mouse location in pixels
			Vector3 mousePos = Input.mousePosition;

			//Set the z offset since the camera is at -10z
			mousePos.z = 10;

			//Store the mouse's position in world coordinates
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (mousePos);

			//Store the direction of the player in respect to the bullet
			Vector3 direction = mouseWorldPos-bullet.transform.position;
			
			//Rotate the bullet towards the player
			bullet.transform.rotation = Quaternion.LookRotation(direction);
			
			//Rotate the bullet along the y so that it faces the mouse point
			bullet.transform.Rotate(0,90,0);

			//Send the damage the bullet will deal to the bullet
			bullet.GetComponent<Bullet>().setDamage(bulletDamage);
		
		}

		//If the user clicked the right mouse button
		if (shootingMissile) {
			
			//Flag that player has just shot
			readyMissile = false;
			shootingMissile = false;
			
			//Instantiate a bullet with bulletPrefab at the players current location
			GameObject missile = (GameObject)Instantiate(missilePrefab,transform.position,Quaternion.identity);
			
			//Read the mouse location in pixels
			Vector3 mousePos = Input.mousePosition;
			
			//Set the z offset since the camera is at -10z
			mousePos.z = 10;
			
			//Store the mouse's position in world coordinates
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (mousePos);
			
			//Store the direction of the player in respect to the bullet
			Vector3 direction = mouseWorldPos-missile.transform.position;
			
			//Rotate the bullet towards the player
			missile.transform.rotation = Quaternion.LookRotation(direction);
			
			//Send the damage the bullet will deal to the bullet
			missile.GetComponent<Missile>().setDamage(missileDamage);
			
		}
	
	}

	void Update() {

		//If the game is paused, don't do anything
		if (!pauseMenu.IsPaused()) 
		{
			//If the player tries to shoot a bullet and can
			if(readyBullet && Input.GetMouseButtonDown(0)) {
				//Flag the shoot
				shootingBullet = true;

			}

			//If the player tries to shoot a missile and can
			if(readyMissile && Input.GetMouseButtonDown(1)) {
				//Flag the shoot
				shootingMissile= true;

			}
		}

	}

}
