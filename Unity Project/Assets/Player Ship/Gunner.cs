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

	//Is the player ready to shoot?
	bool ready;

	//Player is going to shoot
	bool shooting;

	//Counter for reloading
	int shootTimer;

	//Time before the player can shoot again 
	public int reloadTime;

	//The damage the gunner will deal
	public int damage;

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
		//Defualtly be ready to shoot
		ready = true;

		//Set the shoot timer to its reload time
		shootTimer = reloadTime;

		pauseMenu = (PauseController)pauseObject.GetComponent("PauseController");
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
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

		//If the player is "reloading", don't decrement the timer
		if (!ready) {
			
			//Decrements the shoot timer
			shootTimer--;
			
			//If the shoot timer has reached 0, reset it and flag that the player can shoot
			if (shootTimer <= 0) {
				
				ready = true;
				shootTimer = reloadTime;
			}
		}

		//If the user clicked the left mouse button
		if (shooting) {

			//Flag that player has just shot
			ready = false;
			shooting = false;

			//Instantiate a bullet with bulletPrefab at the players current location
			GameObject bullet = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

			//Read the mouse location in pixels
			Vector3 mousePos = Input.mousePosition;

			//Set the z offset since the camera is at -10z
			mousePos.z = 10;

			//Store the mouse's position in world coordinates
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (mousePos);

			//Send the mouse position in world space to the bullet
			bullet.GetComponent<Bullet>().setMousePosition(mouseWorldPos);

			//Send the damage the bullet will dela to the bullet
			bullet.GetComponent<Bullet>().setDamage(damage);
		
		}
	
	}

	void Update() {

		if (!pauseMenu.IsPaused() && ready && Input.GetMouseButtonDown(0)) {
			shooting = true;
		}

	}

}
