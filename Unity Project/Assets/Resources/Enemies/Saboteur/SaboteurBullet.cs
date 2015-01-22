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

public class SaboteurBullet : BasicBullet {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The speed the bullet will move
	public float speed;
	
	//Stores the boundaries of the game
	Boundaries boundaries;

	//The damage this missile will deal
	int damage;
	
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

		gameObject.transform.Rotate(90, 180, 0);

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

	/* ----------------------------------------------------------------------- */
	/* Function    : setDamage(int newDamage)
	 *
	 * Description : Sets the damage this missile will deal.
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
	/* Function    : getBulletDamage()
	 *
	 * Description : Returns the bullet damage for this enemy
	 *
	 * Parameters  : None.
	 *
	 * Returns     : int:  Bullet damage
	 */
	public override int getBulletDamage ()
	{
		return damage;
	}
}
