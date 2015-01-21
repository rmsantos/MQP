﻿/* Module      : Ambusher.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the BasicEnemy
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

public class Ambusher : MonoBehaviour {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The speed at which the enemy will move
	public float speed;
	
	//Stores the boundaries of the game
	Boundaries boundaries;

	//Invisibility counter and boolean
	int counter;
	bool invisible;
	float alpha;
	
	//Player script
	GameObject player;
	
	//Value of destroying this enemy
	public int value;
	
	//ScoreHandler object to track players score
	ScoreHandler score;
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the firing rate variables.
	 * 				Also stores the boundaries of the game.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {
		
		//gameObject.transform.Rotate(90, 180, 0);

		invisible = false;
		counter = 200;
		alpha = 1f;
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");
		
		//Search for the ScoreHandler object for tracking score
		score = GameObject.FindGameObjectWithTag ("ScoreHandler").GetComponent<ScoreHandler>(); 
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Moves the enemy slowly to the left and periodically shoots bullets
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */
		
		//follow the player
		transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
		
		//If the enemy leaves the game space
		//Leave some room for the enemy to fully exit the visible screen (by multiplying 1.2)
		if (transform.position.x < (boundaries.getLeft() * 1.2))
		{
			//Destroy the enemy
			Destroy (this.gameObject);
		}

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
	
	/* ----------------------------------------------------------------------- */
	/* Function    : OnCollisionEnter(Collision col)
	 *
	 * Description : Deals with collisions between the player bullets and this enemy.
	 *
	 * Parameters  : Collision col : The other object collided with
	 *
	 * Returns     : Void
	 */
	void OnCollisionEnter (Collision col)
	{
		//If this is hit by a player bullet
		if(col.gameObject.tag == "PlayerBullet")
		{
			//Destroy the player bullet and this object
			Destroy(col.gameObject);
			Destroy (this.gameObject);
			
			//Update the players score
			score.UpdateScore(value);
			
		}
	}
}
