/* Module      : Laser.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the laser after spawning.
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

public class Laser : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Stores the boundaries of the game
	Boundaries boundaries;

	//The speed of the lasers
	public float speed;

	//The damage this laser deals
	int damage;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Stores the start position of this laser instance.
	 * 				Also stores the boundaries of the game.
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
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the laser towards the mouse clicked position at a constant rate.
	 * 				Destroys the laser when it moves out of the game space
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {

		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//Move the laser to the right 
		transform.Translate( -transform.right * speed, Space.World);

		//If the laser leaves the game space
		//Leave some room for the laser to fully exit the visible screen (by multiplying 1.2)
		if (!boundaries.inBoundaries(transform.position, 1.2f))
		{
			//Destroy the laser
			Destroy (this.gameObject);
		}

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setDamage()
	 *
	 * Description : Used to store the damage the gunner will deal.
	 * 				Called from Gunner.cs.
	 *
	 * Parameters  : int newDamage : The new damage amount
	 *
	 * Returns     : Void
	 */
	public void setDamage(int newDamage)
	{
		damage = newDamage;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : getDamage()
	 *
	 * Description : Used to retrieve the damage the gunner will deal.
	 * 				Called from Gunner.cs.
	 *
	 * Parameters  : None
	 *
	 * Returns     : int : The damage the laser will deal
	 */
	public int getDamage()
	{
		return damage;
	}
}
