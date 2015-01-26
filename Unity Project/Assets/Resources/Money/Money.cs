/* Module      : Money.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the MONEY
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

public class Money : MonoBehaviour {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The speed at which the enemy will move
	public float speed;
	
	//Stores the boundaries of the game
	Boundaries boundaries;
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the boundaries
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
	 * Description : Moves the money
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
		
		//If the money leaves the game space
		//Leave some room for the enemy to fully exit the visible screen (by multiplying 1.2)
		if (transform.position.x < (boundaries.getLeft() * 1.2))
		{
			//Destroy the enemy
			Destroy (this.gameObject);
		}
		
	}

}
