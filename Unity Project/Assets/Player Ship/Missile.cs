﻿/* Module      : Missile.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the players missile
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

public class Missile : MonoBehaviour {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The speed at which the missile will move
	public float speed;
	
	//Stores the boundaries of the game
	Boundaries boundaries;
	
	//The speed at which the missile will rotate
	public float rotationSpeed;
	
	//The radius of the explosion
	public float explosionRadius;
	
	//The time before the missile blows up on its own
	public int timeToExplosion;
	
	//Timer to count to explosion
	int timer;
	
	//The damage this missile will deal
	int damage;
	
	//Is true if the missile is currently exploding
	//Prevents infinite looks of exploding missiles
	bool isExploding;

	//The radius at which the missile will detect enemies
	public float detectRadius;
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Also stores the boundaries of the game.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//The missile is not exploding
		isExploding = false;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the missile forward slowly and locks on to enemies closest to it
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */
		
		//Increment the explosion timer
		timer++;
		
		//If enough time has passed
		if(timer == timeToExplosion)
		{
			//Explode
			explode ();
		}
		
		//Move the missile forwards
		transform.Translate( transform.forward * speed, Space.World);
	
		//Draw a circle with detectRadius and store everything inside it
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectRadius);

		//Min distance is set to some very large number
		float minDistance = 99999;

		//And closest enemy is set to null for now
		GameObject closestEnemy = null;

		//For each item that overlaps the cirlce
		foreach( Collider2D collide in hitColliders)
		{
			//Check if it is an enemy
			if(collide.tag == "Enemies" || collide.tag == "Boss")
			{
				//Get the distance between the missile and that enemy
				float distance = Vector2.Distance(transform.position,collide.gameObject.transform.position);

				//If thats the current smallest distance
				if(distance < minDistance)
				{
					//Store the distance and enemy gameObject
					minDistance = distance;
					closestEnemy = collide.gameObject;
				}
			}
		}

		//If there is an enemy in range
		if(closestEnemy != null)
		{
			//Find the direction of the enemy
			Vector3 targetDir = closestEnemy.transform.position - transform.position;
			
			//The the position of the enemy
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotationSpeed, 0);
			
			//Rotate towards that position
			transform.rotation = Quaternion.LookRotation(newDir);
		}

		//Delete the bullet if it goes off screen
		if (!boundaries.inBoundaries(transform.position,1.2f))
		{
			//Destroy the enemy
			Destroy (this.gameObject);
		}
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : OnCollisionEnter2D(Collision2D col)
	 *
	 * Description : Deals with collisions between missiles and enemies/asteroids
	 *
	 * Parameters  : Collision2D col : The other object collided with
	 *
	 * Returns     : Void
	 */
	void OnCollisionEnter2D (Collision2D col)
	{
		//If the missile hits an enemy or asteroid
		if(col.gameObject.tag == "Enemies" || col.gameObject.tag == "Asteroids" || 
		   col.gameObject.tag == "EnemeyShield" || col.gameObject.tag == "Boss" 
		   || col.gameObject.tag == "Missile")
		{
			//explode this missile
			explode();
		}
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : explode()
	 *
	 * Description : Explodes the missile and damages enemies in the area
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void explode()
	{
		//Only explode if this missile isnt currently in the process of exploding
		//Prevents infinite loops
		if(!isExploding)
		{
			//Flag that the missile is exploding
			isExploding = true;
			
			//Draw a sphere at this position and track everything that overlaps it
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1);
			
			//For each item that overlaps the sphere
			foreach( Collider2D collide in hitColliders)
			{
				//If the collision was with an enemy or boss
				if(collide.tag == "Enemies" || collide.tag == "Boss")
				{
					//Find the component that extends BasicEnemy (the enemy script)
					BasicEnemy enemy = (BasicEnemy)collide.GetComponent(typeof(BasicEnemy));
					
					//Deal damage to that enemy
					enemy.takeDamage(damage);
				}
				
				//Delete the object if it is an enemy or player bullet
				if(collide.tag == "EnemyBullets" || collide.tag == "PlayerBullet")
					Destroy (collide.gameObject);
				
				//If the object is an asteroid
				if(collide.tag == "Asteroids")
				{
					//Cast to an asteroid type
					BasicAsteroid asteroid = (BasicAsteroid)collide.GetComponent(typeof(BasicAsteroid));
					
					//And shatter the asteroid
					asteroid.shatter();
					
				}
				
				//If the object is an enemy missile
				if(collide.tag == "EnemyMissile")
				{
					//Cast to an asteroid type
					SeekerMissile seekerMissile = (SeekerMissile)collide.GetComponent(typeof(SeekerMissile));
					
					//And explode the missile
					seekerMissile.explode();
					
				}

				//If the object is another missile
				if(collide.tag == "Player")
				{
					//Cast to the player collisions type
					PlayerCollisions player = (PlayerCollisions)collide.GetComponent(typeof(PlayerCollisions));
					
					//The player takes damage
					player.takeDamage(damage);
					
				}
			}
			
			//Delete the missile 
			Destroy (this.gameObject);
		}
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : setDamage(int newDamage)
	 *
	 * Description : Sets the damage this missile will deal.
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
