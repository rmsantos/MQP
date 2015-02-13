/* Module      : Ambusher.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the ambusher
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

public class Ambusher : AbstractEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Invisibility counter and boolean
	int counter;
	bool invisible;
	float alpha;

	//Player position
	Vector3 playerPosition;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes enemy variables
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {
	
		//Finds useful gameobjects
		setup ();

		//Invisibility values
		invisible = false;
		counter = 200;
		alpha = 1f;

		//Play the ambusher spawn clip
		portraitController.playAmbusherSpawn ();

	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the enemy slowly to the left and periodically shoots bullets
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//Store the players position
		//If the player was destroyed
		if(player == null)
		{
			//Tell the enemy to move off screen to the left
			playerPosition = transform.position+Vector3.left;
		}
		else
		{
			//Otherwise move towards the players position
			playerPosition = player.transform.position;
		}

		//follow the player
		transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed);

		//Destroy the ship if it goes off screen
		checkBoundaries ();

		counter -= 1;
		if (invisible && counter <= 0) {
			if (alpha < 1.0f) {
				alpha += .05f;
				var originalColour = renderer.material.color;
				renderer.material.color = new Color(originalColour.r, originalColour.g, originalColour.b, alpha);
			}
			else {
				invisible = false;
				counter = 50;
			}
		}
		else if (!invisible && counter <= 0) {
			if (alpha > .20f) {
				alpha -= .05f;
				var originalColour = renderer.material.color;
				renderer.material.color = new Color(originalColour.r, originalColour.g, originalColour.b, alpha);
			}
			else {
				invisible = true;
				counter = 150;
			}
		}		
	}

}
