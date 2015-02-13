/* Module      : Grenadier.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Grenadier
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

public class Grenadier : AbstractEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Angle at which the grenadier will shoot the "shotgun"
	public float shootAngle;

	//The place to spawn bullets
	public Transform turret1;
	public Transform turret2;

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
	 * Description : Moves the enemy slowly to the left and periodically shoots bullets in a shotgun pattern
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
			//Spawn the first bullet and store it
			GameObject bullet1 = shoot (turret);

			//Spawn the second bullet and store it
			GameObject bullet2 = shoot(turret2);

			//Spawn the third bullet and store it
			GameObject bullet3 = shoot (turret1);

			//Bullet 1 will move only in a straight line

			//Bullet 2 will shoot at a +degree angle
			bullet2.transform.Rotate(0,0,shootAngle);

			//Bullet 3 will shoot at a -degree angle
			bullet3.transform.Rotate(0,0,-shootAngle);

		}	
		
		//Destroy the ship if it goes off screen
		checkBoundaries ();		
	}
}
