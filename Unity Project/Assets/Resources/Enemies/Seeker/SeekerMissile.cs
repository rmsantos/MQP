﻿/* Module      : SeekerMissile.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Seeker's missile
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

public class SeekerMissile : MonoBehaviour {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The speed at which the bullet will move
	public float speed;
		
	//Stores the boundaries of the game
	Boundaries boundaries;
	
	//Player script
	GameObject player;
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Searches for the player.
	 * 				Also stores the boundaries of the game.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Moves the missile slowly towards the player.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		transform.Translate( transform.forward * speed, Space.World);

		Vector3 targetDir = player.transform.position - transform.position;
		float step = 0.01f;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);

		transform.rotation = Quaternion.LookRotation(newDir);

		if (transform.position.x < (boundaries.getLeft() * 1.2))
		{
			//Destroy the enemy
			Destroy (this.gameObject);
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
			
		}
	}
}
