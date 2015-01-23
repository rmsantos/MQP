﻿using UnityEngine;
using System.Collections;

public class JuggernautShield : MonoBehaviour {
	
	/* ----------------------------------------------------------------------- */
	/* Function    : OnCollisionEnter2D (Collision2D col)
	 *
	 * Description : Deals with collisions between the player bullets and this shield
	 *
	 * Parameters  : Collision col : The other object collided with
	 *
	 * Returns     : Void
	 */
	void OnCollisionEnter2D (Collision2D col)
	{
		print ("Meep?");

		//If this is hit by a player bullet
		if(col.gameObject.tag == "PlayerBullet")
		{
			print ("DESTROYD!");

			//Destroy the player bullet
			Destroy (col.gameObject);
		}
	}
}
