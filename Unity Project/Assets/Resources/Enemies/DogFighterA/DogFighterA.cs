/* Module      : DogFighterA.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the DogFighterA
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

public class DogFighterA :  AbstractEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
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
		
		//The new position of the enemy after moving
		Vector3 newPos = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);
		
		//Apply the movement
		transform.position = newPos;

		//Reload the weapon
		reload ();
		
		//If the enemy can shoot and is in bounds
		if(canShoot())
		{
			//Shoot the weapon
			shoot (turret);
		}	
		
		//Destroy the ship if it goes off screen
		checkBoundaries ();

	}
}
