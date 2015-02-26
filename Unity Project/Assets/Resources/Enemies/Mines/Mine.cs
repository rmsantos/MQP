/* Module      : Mine.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Mine
 *
 * Date        : 2015/2/13
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

using UnityEngine;
using System.Collections;

public class Mine : AbstractEnemy {

	//Is true if the mine is currently exploding
	//Prevents infinite loops of exploding mines
	bool isExploding;

	//The raduis to explode
	public float explodeRadius;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes references
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {
		
		//Initialize base objects
		setup ();
		speed = Random.Range(speed, speed * 1.5f);

		//Play the minefield audio
		portraitController.playMinefield ();

		//The mine is not exploding
		isExploding = false;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the astroid to the left and rotating it
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//The new position of the mine after moving
		Vector3 newPos = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);
		
		//Apply the movement
		transform.position = newPos;

		//Draw a sphere at this position and track everything that overlaps it
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explodeRadius);

		//For each item that overlaps the sphere
		foreach( Collider2D collide in hitColliders)
		{
			//If the mine gets close enough to the player
			if(collide.tag == "Player")
			{
				//Then explode
				explode();
			}
		}

		//Destroy the ship if it goes off screen
		checkBoundaries ();
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : OnCollisionEnter2D(Collision2D col)
	 *
	 * Description : Deals with collisions between the player bullets and this enemy.
	 *
	 * Parameters  : Collision2D col : The other object collided with
	 *
	 * Returns     : Void
	 */
	void OnCollisionEnter2D (Collision2D col)
	{
		//If this is hit by a player bullet
		if(col.gameObject.tag == "PlayerBullet")
		{
			//Destroy the player bullet and this object
			Destroy(col.gameObject);
			
			//explode
			explode();
			
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : explode()
	 *
	 * Description : Explodes the mine and damages enemies in the area
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
				if(collide.tag == "Enemies")
				{
					//Find the component that extends AbstractEnemy (the enemy script)
					AbstractEnemy enemy = (AbstractEnemy)collide.GetComponent(typeof(AbstractEnemy));
					
					//Deal damage to that enemy
					enemy.takeDamage(collisionDamage);
				}
				
				//Delete the object if it is an enemy or player laser
				if(collide.tag == "EnemyLaser" || collide.tag == "PlayerLaser")
					Destroy (collide.gameObject);
				
				//If the object is an asteroid
				if(collide.tag == "Asteroids")
				{
					//Cast to an asteroid type
					AbstractAsteroid asteroid = (AbstractAsteroid)collide.GetComponent(typeof(AbstractAsteroid));
					
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
				
				//If the object is a missile
				if(collide.tag == "Player")
				{
					//Cast to the player collisions type
					PlayerCollisions player = (PlayerCollisions)collide.GetComponent(typeof(PlayerCollisions));
					
					//The player takes damage
					player.takeDamage(collisionDamage);
					
				}

				//If the object is another mine
				if(collide.tag == "Mine")
				{
					//Cast to a mine type and explode
					Mine mine = (Mine)collide.GetComponent(typeof(Mine));
					
					//The player takes damage
					mine.explode();
				}
			}

			//Play the explosion sound
			audioHandler.playMediumEnemyExplosion();

			//Delete the mine
			Destroy (this.gameObject);
		}
	}

}
