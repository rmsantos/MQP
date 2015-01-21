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
	
	//The speed at which the missile will move
	public float speed;
		
	//Stores the boundaries of the game
	Boundaries boundaries;
	
	//Player script
	GameObject player;

	//The speed at which the missile will rotate
	public float rotationSpeed;

	//The radius of the explosion
	public float explosionRadius;

	//The damage this missile will deal
	public int damage;
	
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

		//Move the missile forwards
		transform.Translate( transform.forward * speed, Space.World);

		//Find the direction of the player
		Vector3 targetDir = player.transform.position - transform.position;

		//The the position of the player
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotationSpeed, 0);

		//Rotate towards that position
		transform.rotation = Quaternion.LookRotation(newDir);

		//Delete the bullet if it goes off screen
		if (!boundaries.inBoundaries(transform.position,1.2f))
		{
			//Destroy the enemy
			Destroy (this.gameObject);
		}
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : OnCollisionEnter(Collision col)
	 *
	 * Description : Deals with collisions between the player bullets and this missile
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
			//Draw a sphere at this position and track everything that overlaps it
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);

			//For each item that overlaps the sphere
			foreach( Collider collide in hitColliders)
			{
				//Find the component that extends BasicEnemy (the enemy script)
				BasicEnemy enemy = (BasicEnemy)collide.GetComponent(typeof(BasicEnemy));

				//If it doesnt exist then this is not an enemy
				//If so then deal damage
				if(enemy != null)
				{
					enemy.takeDamage(damage);
				}

				//Delete it if it has the appripriate tag. Asteroids and enemy bullets will be destroyed
				//in on hit.
				//This sumulates the "explosion"
				if(collide.tag == "EnemyBullets" || collide.tag == "Asteroids")
					Destroy (collide.gameObject);
			}

			//Delete the missile and the player bullet
			Destroy (col.gameObject);
			Destroy (this.gameObject);
			
		}
	}
}
