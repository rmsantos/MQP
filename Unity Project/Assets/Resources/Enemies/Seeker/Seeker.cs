/* Module      : Seeker.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Seeker
 *
 * Date        : 2015/1/20
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Seeker : AbstractEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Speed in the x direction
	float xSpeed;

	//Player position
	Vector3 playerPosition;

	//Stores the damage the missile does
	public int missileDamage;

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

		//Set the x speed to move to the left 
		xSpeed = -speed;

	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the enemy to a fixed x position then moves 
	 * 				it upwards and downwards and periodically shoots missiles
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//Stop the enemy when it hits right right bound of the screen
		if (transform.position.x < boundaries.getRight() * .9f)
			xSpeed = 0;

		//The new position of the enemy after moving
		Vector3 newPos = new Vector3 (transform.position.x +xSpeed, transform.position.y + speed, transform.position.z);

		//If hitting either top or bottom boundaries, reverse the direction
		if(newPos.y >= boundaries.getTop() * .8f || newPos.y <= boundaries.getBottom() * .8f)
			speed = -speed;

		//Apply the movement
		transform.position = newPos;

		//Reload the weapon
		reload ();
		
		//If the enemy can shoot and is in bounds
		if(canShoot())
		{
			//Spawn the missle and store it
			GameObject missile = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

			//Store the players position
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

			//Store the direction of the player in respect to the missile
			Vector3 direction = playerPosition-missile.transform.position;

			//Rotate the missile towards the player
			missile.transform.rotation = Quaternion.LookRotation(direction);

			//Cast to an missile type
			SeekerMissile seekerMissile = (SeekerMissile)missile.GetComponent(typeof(SeekerMissile));

			//Set the damage of the missile
			seekerMissile.setDamage(missileDamage);

			//Flag that player has just shot
			ready = false;
			
		}		

		//Destroy the ship if it goes off screen
		checkBoundaries ();

	}


}