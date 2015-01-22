/* Module      : BasicAsteroid.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file is an abstract class for all asteroids. It 
 * 				contains their shattering functions.
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

public interface BasicAsteroid {
	
	/* ----------------------------------------------------------------------- */
	/* Function    : shatter()
	 *
	 * Description : Calls the corresponding shatter function for all asteroids
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void shatter();

	/* ----------------------------------------------------------------------- */
	/* Function    : getCollisionDamage()
	 *
	 * Description : Calls the corresponding getCollisionDamage function for all asteroids
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	int getCollisionDamage();
}
