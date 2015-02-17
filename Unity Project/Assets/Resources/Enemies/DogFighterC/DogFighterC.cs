/* Module      : DogFighterC.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the DogFighterC
 *
 * Date        : 2015/2/17
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class DogFighterC :  AbstractEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//State in which this enemy is in
	int state;

	//X and Y positions to travel to
	float xPos;
	float yPos;

	//Target position to move to
	Vector3 targetPos;

	//Anchor point
	Vector3 anchorPos;

	//Position of the player
	Vector3 playerPosition;

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

		//Initialize the state
		state = 0;

		//Initialize the positions
		targetPos = Vector3.zero;
		anchorPos = targetPos;
		playerPosition = Vector3.zero;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the enemy slowly to the left and periodically shoots bullets
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//If in an initial state then pick a position
		if(state == 0)
		{
			//Pick a random x and y position to travel to based on the game boundaries
			xPos = Random.Range (boundaries.getLeft()/2f, boundaries.getRight ()*.8f);
			yPos = Random.Range (boundaries.getBottom ()*.8f, boundaries.getTop ()*.8f);

			//Switch the state
			state = 1;
		}
		//In state 2, move towards that position
		else if (state == 1)
		{
			//The move target
			targetPos = new Vector3(xPos,yPos,0);

			//Move towards the player
			transform.position = Vector3.MoveTowards(transform.position,targetPos,speed);

			//If reached the target, then switch states
			if(transform.position == targetPos)
			{
				state = 2;
				anchorPos = targetPos;
			}
		}
		else if(state == 2)
		{
			//Reload the weapon
			reload ();
			
			//If the enemy can shoot and is in bounds
			if(canShoot())
			{
				//Shoot the weapon
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

			//Select a direction to move towards in relation to the current position
			Vector2 circle = Random.insideUnitCircle;
			Vector3 movePosition = anchorPos + new Vector3(circle.x,circle.y,0);

			//Find the direction of the movment
			Vector3 forceDirection = movePosition - transform.position;

			//Add normalized force
			rigidbody2D.AddForce(forceDirection.normalized*2);
		
		}
		
		//Destroy the ship if it goes off screen
		checkBoundaries ();
		
	}
}
