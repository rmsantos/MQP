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

	//Counter for reloading
	int shootTimer;

	//Time before the player can shoot again 
	public int reloadTime;

	//The damage the gunner will deal
	public int damage;

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
	void Update () {

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
		if(ready && Input.GetMouseButtonDown(0))
		{

			/* -- LOCAL VARIABLES  --------------------------------------------------- */

			//Instantiate a bullet with bulletPrefab at the players current location
			GameObject bullet = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

			//Read the mouse location in pixels
			Vector3 mousePos = Input.mousePosition;

			//Set the z offset since the camera is at -10z
			mousePos.z = 10;

			//Store the mouse's position in world coordinates
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (mousePos);

			//Store the direction of the mouse click
			Vector3 direction = mouseWorldPos-bullet.transform.position;

			//Look at the mouse click 
			//bullet.transform.LookAt(direction,Vector3.up);

			//Rotate 90 degrees along the x so that the cylinder is facing the target (like a bullet)
			//bullet.transform.Rotate(90,0,0);

			//Send the mouse position in world space to the bullet
			bullet.GetComponent<Bullet>().setMousePosition(mouseWorldPos);

			//Send the damage the bullet will dela to the bullet
			bullet.GetComponent<Bullet>().setDamage(damage);

			//Flag that player has just shot
			ready = false;
		
		}
	
	}
}
