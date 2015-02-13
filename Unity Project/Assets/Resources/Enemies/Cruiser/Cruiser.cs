/* Module      : Cruiser.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Cruiser
 *
 * Date        : 2015/1/28
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Cruiser :  AbstractEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The amplitude of the wave that this ship will move in
	public int amplitude;

	//Counter to determine amplitude of the wave
	int waveCounter;

	//The direction the ship is moving (on the y axis)
	public float direction;

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

		//Initialize the counter to be 0
		waveCounter = 0;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the enemy slowly to the left and vertically in a wave pattern
	 * 				and periodically shoots bullets
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//Increment the wave counter to count until amplitude
		waveCounter++;

		//When reaching the wave amplitude, reverse the direction
		if(waveCounter == amplitude)
		{
			waveCounter = 0;
			direction = - direction;
		}

		//The new position of the enemy after moving
		Vector3 newPos = new Vector3 (transform.position.x - speed, transform.position.y + direction, transform.position.z);
		
		//Apply the movement
		transform.position = newPos;

		//Counter to reload
		reload ();
		
		//If the enemy can shoot and is in bounds
		if(canShoot())
		{
			//Shoot the bullet
			shoot(turret);
		}	
		
		//Destroy the ship if it goes off screen
		checkBoundaries ();
		
	}
}
