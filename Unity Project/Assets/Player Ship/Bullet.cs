/* Module      : Bullet.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the bullet after spawning.
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

public class Bullet : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//The mouse position where the player wants to fire the gun
	Vector3 mousePos;

	//The spawn point of the bullet
	Vector3 startPos;

	//Stores the boundaries of the game
	Boundaries boundaries;

	//The speed of the bullets
	public float speed;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Stores the start position of this bullet instance.
	 * 				Also stores the boundaries of the game.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {
		//Store the initial position of the bullet
		startPos = transform.position;

		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>(); 
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Moves the bullet towards the mouse clicked position at a constant rate.
	 * 				Destroys the bullet when it moves out of the game space
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {

		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//Calculate the direction of the mouse position relative to the current position.
		Vector3 direction = mousePos - startPos;

		//Calculate the distance to the mouse position.
		float distance = Vector3.Distance (mousePos, startPos);

		//Move the bullet in the direction of the mouse, at a rate of speed * Time.deltaTime
		//over the distance. This ensures that all bullets move at the same speed.
		//Everything is in relation to Space.world.
		transform.Translate( direction * speed * Time.deltaTime / distance , Space.World);

		//If the bullet leaves the game space
		//Leave some room for the bullet to fully exit the visible screen (by multiplying 1.2)
		if (transform.position.x > (boundaries.getRight() * 1.2) ||
			transform.position.x < (boundaries.getLeft() * 1.2) ||
			transform.position.y > (boundaries.getTop() * 1.2) ||
			transform.position.y < (boundaries.getBottom() * 1.2))
		{
			//Destroy the bullet
			Destroy (this.gameObject);
		}

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setMousePosition()
	 *
	 * Description : Used to store the position of the mouse when the user clicked.
	 * 				Called from Gunner.cs.
	 *
	 * Parameters  : Vector3 newPos : The position the user clicked.
	 *
	 * Returns     : Void
	 */
	public void setMousePosition(Vector3 newPos)
	{
		//Store the position of the mouse
		mousePos = newPos;
	}
}
