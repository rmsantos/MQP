/* Module      : PlayerCollisions.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the health and the collisions for the player
 *
 * Date        : 2015/1/21
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using System.Collections;

public class PlayerCollisions : MonoBehaviour {

	int health;

	void Start () {
		health = 100;
	}
	
	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "EnemyBullets")
		{
			//Destroy the player bullet and this object
			Destroy(col.gameObject);
			health -= 5;

			print (health);
			
		}

		if(col.gameObject.tag == "Enemies" || col.gameObject.tag == "Asteroids")
		{
			//Destroy the player bullet and this object
			Destroy(col.gameObject);
			health -= 10;
			
			print (health);
			
		}

		if (health <= 0) {
			Destroy(this.gameObject);
		}
	}

}
