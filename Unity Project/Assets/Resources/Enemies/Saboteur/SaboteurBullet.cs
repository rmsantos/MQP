/* Module      : SaboteurBullet.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Saboteur Bullet
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

public class SaboteurBullet : MonoBehaviour {
	
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

		//Reverse the speed so it moves in the correct direction
		speed = -speed;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Moves the bullet slowly to the right. Destroys itself upon
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
		
		//Delete the bullet if it goes off screen
		if (!boundaries.inBoundaries(transform.position,1.2f))
		{
			//Destroy the bullet
			Destroy (this.gameObject);
		}
	}
}
