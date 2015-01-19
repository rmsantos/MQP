/* Module      : BasicEnemy.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the BasicEnemy
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

public class DogFighterB : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//The speed at which the enemy will move
	public float speed;

	//Is the enemy ready to shoot?
	bool ready;

	//Counter for reloading
	int shootTimer;
	
	//Time before the enemy can shoot again 
	public int reloadTime;

	//Prefab of the enemy bullet
	public GameObject bulletPrefab;

	//Stores the boundaries of the game
	Boundaries boundaries;

	//Player script
	GameObject player;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the firing rate variables.
	 * 				Also stores the boundaries of the game.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//The enemy can shoot right when it spawns
		ready = true;

		//Set the shooting timer
		shootTimer = reloadTime;

		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();

		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");

	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Moves the enemy slowly to the left and periodically shoots bullets
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {

		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//follow the player
		transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);

		if (!ready) {

			shootTimer--;

			//If the shoot timer has reached 0, reset it and flag that the enemy can shoot
			if (shootTimer <= 0) {
				
				ready = true;
				shootTimer = reloadTime;
			}
		}

		if(ready)
		{
			ready = false;
			Instantiate(bulletPrefab,transform.position,Quaternion.identity);
		}

		//If the enemy leaves the game space
		//Leave some room for the enemy to fully exit the visible screen (by multiplying 1.2)
		if (transform.position.x < (boundaries.getLeft() * 1.2))
		{
			//Destroy the enemy
			Destroy (this.gameObject);
		}

	}
}
