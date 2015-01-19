/* Module      : BasicEnemy.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the BasicEnemy
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

public class BasicEnemyBullet : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//The speed the bullet will move
	public float speed;

	//Stores the boundaries of the game
	Boundaries boundaries;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Stores the boundaries of the game.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>(); 
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Moves the bullet slowly to the left. Destroys itself upon
	 * 				leaving the game space.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {

		/* -- LOCAL VARIABLES ---------------------------------------------------- */
		
		//The new position of the enemy after moving
		Vector3 newPos = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);

		//Apply the movement
		transform.position = newPos;

		//If the bullet leaves the game space
		//Leave some room for the bullet to fully exit the visible screen (by multiplying 1.2)
		if (transform.position.x < (boundaries.getLeft() * 1.2))
		{
			//Destroy the bullet
			Destroy (this.gameObject);
		}
	}
}
