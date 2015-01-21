/* Module      : Saboteur.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Saboteur
 *
 * Date        : 2015/1/21
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Saboteur : MonoBehaviour {
	
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
	
	//Value of destroying this enemy
	public int value;
	
	//ScoreHandler object to track players score
	ScoreHandler score;
	
	//Speed in the x direction
	float xSpeed;
	
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
		
		//Search for the ScoreHandler object for tracking score
		score = GameObject.FindGameObjectWithTag ("ScoreHandler").GetComponent<ScoreHandler>(); 
		
		//Set the x speed to move to the left 
		xSpeed = speed;
		
		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Moves the enemy to a fixed x position then moves 
	 * 				it upwards and downwards to track the player and shoots.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */
		
		//Stop the enemy when it hits the left bound of the screen
		if (transform.position.x > boundaries.getLeft())
			xSpeed = 0;

		//Direction the enemy should move
		float direction = 0;

		//Track the movement of the player and follow it
		if(player.transform.position.y > transform.position.y)
		{
			//Move up
			direction =  Mathf.Abs(speed);
		}
		//If the within a certain threshold of the player
		else if(Mathf.Abs(transform.position.y-player.transform.position.y) < 0.1)
		{
			//Don't move
			direction = 0;
		}
		else
		{
			//Move down
			direction = -Mathf.Abs (speed);
		}

		//The new position of the enemy after moving
		Vector3 newPos = new Vector3 (transform.position.x +xSpeed, transform.position.y + direction, transform.position.z);

		//Apply the movement
		transform.position = newPos;
		
		//If the enemy is "reloading", don't decrement the timer
		if (!ready) {
			
			//Decrements the shoot timer
			shootTimer--;
			
			//If the shoot timer has reached 0, reset it and flag that the enemy can shoot
			if (shootTimer <= 0) 
			{
				ready = true;
				shootTimer = reloadTime;
			}
		}
		
		//If the enemy can shoot and is in bounds
		if(ready & boundaries.inBoundaries(transform.position,1))
		{
			//Flag that a bullet was shot
			ready = false;
			
			//Spawn the bullet and store it
			Instantiate(bulletPrefab,transform.position,Quaternion.identity);
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