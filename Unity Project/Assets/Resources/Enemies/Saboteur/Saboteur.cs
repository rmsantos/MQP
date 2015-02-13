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

public class Saboteur : AbstractEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//The state of the saboteur
	//State 0 is moving to the left side of the screen (and off screen)
	//State 1 is turning around and moving to the left bound of the screen
	//State 2 is Moving vertically and shooting at the player
	int state;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes enemy variables
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Initialize base objects
		setup ();

		//Rotate the Saboteur at the start to align it properly and save it
		transform.Rotate (0, 180, 0);

		//Start at state 0
		state = 0;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the enemy to a fixed x position then moves 
	 * 				it upwards and downwards to track the player and shoots.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//Move to the left side of the screen (off it)
		if(state == 0)
		{
			//The new position of the enemy after moving
			Vector3 newPos = new Vector3 (boundaries.getLeft() * 1.2f, transform.position.y, transform.position.z);

			//Make the move
			transform.position = Vector3.MoveTowards(transform.position, newPos, speed);

			//If the saboteur hits its mark
			if(transform.position.x <= boundaries.getLeft() * 1.1f)
			{
				//Rotate the sprite 180
				transform.Rotate(0,180,0);
				originalRotationValue = transform.rotation;

				//Switch states
				state = 1;

				//Lower the speed
				speed = speed/5;
			}
		}
		//Move to the left bound of the screen (back on screen)
		else if(state == 1)
		{
			//The new position of the enemy after moving
			Vector3 newPos = new Vector3 (boundaries.getLeft()*0.9f, transform.position.y, transform.position.z);
			
			//Make the move
			transform.position = Vector3.MoveTowards(transform.position, newPos, speed);

			//If the ship is locked into its horizontal position
			if(transform.position.x >= boundaries.getLeft()*0.9f)
			{
				//Switch to the next state
				state = 2;
			}
		}
		//State 2 slowly tracks the player
		else
		{
			//Direction the enemy should move
			float direction = 0;

			//If the within a certain threshold of the player
			if(player == null || (Mathf.Abs(transform.position.y-player.transform.position.y) < 0.1))
			{
				//Don't move
				direction = 0;
			}
			//Track the movement of the player and follow it
			else if(player.transform.position.y > transform.position.y)
			{
				//Move up
				direction =  Mathf.Abs(speed);
			}
			else
			{
				//Move down
				direction = -Mathf.Abs (speed);
			}

			//The new position of the enemy after moving
			Vector3 newPos = new Vector3 (boundaries.getLeft()*0.9f, transform.position.y+direction, transform.position.z);

			//Make the move
			transform.position = Vector3.MoveTowards(transform.position, newPos, speed);

			//Reload the weapon
			reload ();

			//If the player can shoot
			if(ready)
			{				
				//Spawn the bullet and store it
				GameObject bullet = shoot(turret);
				
				//Rotate the bullet around to move towards the right side of the screen
				bullet.transform.Rotate (0,0,180);

			}

		}
		//Always try to rotate to the correct facing direction
		transform.rotation = Quaternion.Slerp(transform.rotation, originalRotationValue, rotationResetSpeed); 
	}
}