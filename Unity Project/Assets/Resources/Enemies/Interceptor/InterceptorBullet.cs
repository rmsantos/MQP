/* Module      : InterceptorBullet.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Interceptor Bullet
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

public class InterceptorBullet : MonoBehaviour, BasicBullet {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The speed the bullet will move
	public float speed;
	
	//Stores the boundaries of the game
	Boundaries boundaries;
	
	//The damage this bullet will deal
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

		transform.Rotate (0, 0, 90);

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
	
		//Move the bullet forwards
		transform.Translate( transform.forward * speed, Space.World);
		
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
	 * Description : Sets the damage this bullet will deal.
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
	public int getBulletDamage ()
	{
		return damage;
	}
}
