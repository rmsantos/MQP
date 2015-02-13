/* Module      : DogFighterB.cs
 * Author      : Josh Morse
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the DogFighterB
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

public class DogFighterB : AbstractEnemy {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Vertical speed
	public float verticalSpeed;


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

		//follow the player
		if(player != null)
			transform.position = Vector3.MoveTowards(transform.position, player.transform.position, verticalSpeed);

		//Move to the left
		transform.position = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);

		//Destroy the ship if it goes off screen
		checkBoundaries ();

	}
}
