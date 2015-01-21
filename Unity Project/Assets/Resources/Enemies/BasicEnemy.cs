/* Module      : BasicEnemy.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file is an abstract class for all enemies. It 
 * 				contains their damage functions and variables
 *
 * Date        : 2015/1/20
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public abstract class BasicEnemy : MonoBehaviour {

	/* ----------------------------------------------------------------------- */
	/* Function    : takeDamage()
	 *
	 * Description : Calls the corresponding takeDamage function for all enemies
	 *
	 * Parameters  : float damage : The damage that will be dealt
	 *
	 * Returns     : Void
	 */
	public abstract void takeDamage(int damage);
}
