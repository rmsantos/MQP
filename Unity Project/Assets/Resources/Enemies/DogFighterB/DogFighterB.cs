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

	//Value of destroying this enemy
	public int value;
	
	//ScoreHandler object to track players score
	ScoreHandler score;

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

		//Search for the ScoreHandler object for tracking score
		score = GameObject.FindGameObjectWithTag ("ScoreHandler").GetComponent<ScoreHandler>(); 

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

		//Move to the left
		transform.position = new Vector3 (transform.position.x - (speed * 2f), transform.position.y, transform.position.z);

		if (!ready) {

			shootTimer--;

			//If the shoot timer has reached 0, reset it and flag that the enemy can shoot
			if (shootTimer <= 0) {
				
				ready = true;
				shootTimer = reloadTime;
			}
		}

		if(ready && boundaries.inBoundaries(transform.position,1))
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

	/* ----------------------------------------------------------------------- */
	/* Function    : OnCollisionEnter(Collision col)
	 *
	 * Description : Deals with collisions between the player bullets and this enemy.
	 *
	 * Parameters  : Collision col : The other object collided with
	 *
	 * Returns     : Void
	 */
	void OnCollisionEnter (Collision col)
	{
		//If this is hit by a player bullet
		if(col.gameObject.tag == "PlayerBullet")
		{
			//Destroy the player bullet and this object
			Destroy(col.gameObject);
			Destroy (this.gameObject);
			
			//Update the players score
			score.UpdateScore(value);
			
		}
	}
}
