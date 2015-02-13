/* Module      : Interceptor.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Interceptor
 *
 * Date        : 2015/1/22
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Interceptor :  AbstractEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//Speed in the x direction
	float xSpeed;

	//Player position
	Vector3 playerPosition;
	
	//Time before the interceptor speeds away
	public int flyOffTime;

	//Timer to keep track of when to fly off
	int timer;

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

		//Initialize base objects
		setup ();

		//Set the x speed to move to the left 
		xSpeed = -speed;
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the enemey slowly up and down and after a certain amount of time,
	 * 				Speeds off the left side of the screen.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		Vector3 newPos;

		if(timer == flyOffTime)
		{
			//The new position of the enemy after moving
			newPos = new Vector3 (transform.position.x + (speed*2), transform.position.y, transform.position.z);
		}
		else
		{
			//If the enemy is in the boundaries
			if(boundaries.inBoundaries(transform.position,1))
			{
				//Increment the timer
				timer++;
			}

			//Stop the enemy when it hits right right bound of the screen
			if (transform.position.x < boundaries.getRight() * .9f)
				xSpeed = 0;
			
			//The new position of the enemy after moving
			newPos = new Vector3 (transform.position.x +xSpeed, transform.position.y + speed, transform.position.z);
			
			//If hitting either top or bottom boundaries, reverse the direction
			if(newPos.y >= boundaries.getTop() * .9f || newPos.y <= boundaries.getBottom() * .9f)
				speed = -speed;

		}
	
		//Apply the movement
		transform.position = newPos;

		//Reload the weapon
		reload ();

		//If the enemy can shoot and is in bounds
		if(canShoot())
		{
			//Spawn the bullet and store it
			GameObject bullet = shoot (turret);

			//If the player was destroyed
			if(player == null)
			{
				//Tell the enemy to move off screen to the left
				playerPosition = transform.position+Vector3.left;
			}
			else
			{
				//Otherwise move towards the players position
				playerPosition = player.transform.position;
			}
				

			//Store the direction of the player in respect to the bullet
			Vector3 direction = playerPosition-bullet.transform.position;
		
			//Rotate the bullet towards the player
			bullet.transform.rotation = Quaternion.LookRotation(direction);

			//Rotate the bullet along the y so that it faces the camera
			bullet.transform.Rotate(0,90,0);

		}		

		//Destroy the ship if it goes off screen
		checkBoundaries (); 

	}
}