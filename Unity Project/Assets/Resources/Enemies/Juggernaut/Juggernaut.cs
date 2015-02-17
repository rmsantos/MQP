/* Module      : Juggernaut.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Juggernaut
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

public class Juggernaut :  AbstractEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//Store the transform of the shield of this enemy
	public Transform shield;

	//Angle at which the juggernaut will shoot the "shotgun"
	public float shootAngle;

	//Rotation speed for the shields
	public float rotationSpeed;

	//Shield rotation
	Transform shieldRotation;

	//State of this juggernaut
	int state;

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

		//Store the rotation of the shield
		shieldRotation = shield.transform;

		//Set the collision damage that the shields will do
		shield.GetComponentInParent<JuggernautShield> ().setCollisionDamage (collisionDamage/2);

		//Initialize the state
		state = 0;
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

		//Rotate the shields around the enemy
		shield.RotateAround (transform.position, Vector3.forward, rotationSpeed);

		//State 0 is to speed up to move on screen
		if(state == 0)
		{
			//The new position of the enemy after moving
			Vector3 newPos = new Vector3 (transform.position.x - speed*5, transform.position.y, transform.position.z);
			
			//Apply the movement
			transform.position = newPos;

			//When fully on screen, then switch states
			if(transform.position.x < boundaries.getRight() * 0.8f)
			{
				state = 1;
			}

		}
		//Else start firing and such
		else
		{
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
				GameObject bullet2 = shoot (turret);
				
				//Spawn the third bullet and store it
				GameObject bullet3 = shoot (turret);

				//Spawn the fourth bullet and store it
				GameObject bullet4 = shoot (turret);
				
				//Spawn the fifth bullet and store it
				GameObject bullet5 = shoot (turret);
				
				//Spawn the sixths bullet and store it
				GameObject bullet6 = shoot (turret);

				//Bullet 1 shoots perpindicular to the shield
				bullet1.transform.rotation = shieldRotation.rotation;
				bullet1.transform.Rotate (0,0,75);

				//Bullet 2 shoots perpendicular and shootAngle to the shield
				bullet2.transform.rotation = shieldRotation.rotation;
				bullet2.transform.Rotate(new Vector3(0,0,shootAngle+75));

				//Bullet 3 shoots perpendicular and -shootAngle to the shield
				bullet3.transform.rotation = shieldRotation.rotation;
				bullet3.transform.Rotate(new Vector3(0,0,-shootAngle+75));

				//Bullet 4 shoots -perpendicular to the shield
				bullet4.transform.rotation = shieldRotation.rotation;
				bullet4.transform.Rotate(new Vector3(0,180,-75));

				//Bullet 5 shoots -perpendicular and shootAngle to the shield
				bullet5.transform.rotation = shieldRotation.rotation;
				bullet5.transform.Rotate(new Vector3(0,180,shootAngle-75));

				//Bullet 6 shoots -perpendicular and -shootAngle to the shield
				bullet6.transform.rotation = shieldRotation.rotation;
				bullet6.transform.Rotate(new Vector3(0,180,-shootAngle-75));
			}
		}
		
		//Destroy the ship if it goes off screen
		checkBoundaries ();		
	}
}
