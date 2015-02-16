/* Module      : SeekerMissile.cs
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

	//Player position
	Vector3 playerPosition;

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

	//Quitting boolean
	bool isQuitting;
	
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

		//gameObject.transform.Rotate(90, 180, 0);
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");

		//The missile is not exploding
		isExploding = false;

		//Not quitting the application
		isQuitting = false;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the missile slowly towards the player.
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

		//Store the players position
		//If the player was destroyed
		if(player == null)
		{
			//Tell the missile to move off screen to the left
			playerPosition = transform.position+Vector3.left;
		}
		else
		{
			//Otherwise move towards the players position
			playerPosition = player.transform.position;
		}

		//Find the direction of the player
		Vector3 targetDir = playerPosition - transform.position;

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
	/* Function    : OnCollisionEnter2D(Collision2D col)
	 *
	 * Description : Deals with collisions between the player bullets and this missile
	 *
	 * Parameters  : Collision2D col : The other object collided with
	 *
	 * Returns     : Void
	 */
	void OnCollisionEnter2D (Collision2D col)
	{
		//If this is hit by a player bullet
		if(col.gameObject.tag == "PlayerBullet" || col.gameObject.tag == "Player" 
		   || col.gameObject.tag == "Missile" || col.gameObject.tag == "SeekerMissile")
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
					//Find the component that extends AbstractEnemy (the enemy script)
					AbstractEnemy enemy = (AbstractEnemy)collide.GetComponent(typeof(AbstractEnemy));

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
					AbstractAsteroid asteroid = (AbstractAsteroid)collide.GetComponent(typeof(AbstractAsteroid));
					
					//And shatter the asteroid
					asteroid.shatter();
					
				}

				//If the object is another missile
				if(collide.tag == "EnemyMissile")
				{
					//Cast to an asteroid type
					SeekerMissile seekerMissile = (SeekerMissile)collide.GetComponent(typeof(SeekerMissile));
					
					//And explode the missile
					seekerMissile.explode();
					
				}

				//If the object is another missile
				if(collide.tag == "Missile")
				{
					//Cast to an asteroid type
					Missile missile = (Missile)collide.GetComponent(typeof(Missile));
					
					//And explode the missile
					missile.explode();
					
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

	//Only called when the application is being quit. Will disable spawning in OnDestroy
	void OnApplicationQuit() {
		
		isQuitting = true;
		
	}
	
	//Used to spawn particle effects or money when destroyed
	void OnDestroy() {
		
		if (!isQuitting) {
			
			//Load the explosion
			GameObject explosion = Resources.Load<GameObject>("Explosions/SimpleExplosion");
			
			//Position of the enemy
			var position = gameObject.transform.position;
			
			//Create the explosion at this location
			Instantiate(explosion, new Vector3(position.x, position.y, position.z), Quaternion.identity);	
			
		}
		
	}
}
