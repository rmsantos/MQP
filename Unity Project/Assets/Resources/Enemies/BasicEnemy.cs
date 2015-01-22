/* Module      : BasicEnemy.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file is an abstract class for all enemies. It 
 * 				contains their damage functions and variables
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

public interface BasicEnemy {

	/* ----------------------------------------------------------------------- */
	/* Function    : takeDamage(int damage)
	 *
	 * Description : Calls the corresponding takeDamage function for all enemies
	 *
	 * Parameters  : int damage : The damage that will be dealt
	 *
	 * Returns     : Void
	 */
	void takeDamage(int damage);


	/* ----------------------------------------------------------------------- */
	/* Function    : getCollisionDamage()
	 *
	 * Description : Returns the collision damage for that enemy
	 *
	 * Parameters  : None.
	 *
	 * Returns     : int:  Collision damage
	 */
	int getCollisionDamage();

}
