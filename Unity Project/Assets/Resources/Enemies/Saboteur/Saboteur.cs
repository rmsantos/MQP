/* Module      : Saboteur.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Saboteur
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

public class Saboteur : MonoBehaviour, BasicEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The speed at which the enemy will move
	public float speed;
	
	//Is the enemy ready to shoot?
	bool ready;
	
	//Counter for reloading
	int shootTimer;
	
	//Time before the enemy can shoot again 
	public int reloadTime;
	
	//Prefab of the enemy bullet
	public GameObject bulletPrefab;
	
	//Stores the boundaries of the game
	Boundaries boundaries;
	
	//Value of destroying this enemy
	public int value;
	
	//ScoreHandler object to track players score
	public GameObject scoreObject;
	static ScoreHandler score;
	
	//Player script
	GameObject player;

	//The health of this enemy
	public int health;

	//Stores the damage colliding with the player does
	public int collisionDamage;
	
	//Stores the damage the bullet does
	public int bulletDamage;

	//Can the ship move vertically now?
	bool verticalMove;

	//Speed changes depending on the ships state
	float newSpeed;

	//Quitting boolean
	bool isQuitting;

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
		
		//The enemy can shoot right when it spawns
		ready = true;
		
		//Set the shooting timer
		shootTimer = reloadTime;
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//Search for the ScoreHandler object for tracking score
		score = (ScoreHandler)scoreObject.GetComponent("ScoreHandler");

		//The ship cannot initially move vertically
		verticalMove = false;

		//Speed changes depending on the ships state
		newSpeed = speed;

		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");

		//Not quitting the application
		isQuitting = false;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : delay()
	 *
	 * Description : Delays for a very small time then
	 * 				flags the ship to move vertically.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	IEnumerator delay()
	{
		//Wait a small amount of time so that the ship doesn't look choppy
		yield return new WaitForSeconds(0.001f);

		//Allow the ship to move vertically.
		verticalMove = true;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the enemy to a fixed x position then moves 
	 * 				it upwards and downwards to track the player and shoots.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//Direction the enemy should move
		float direction = 0;
	
		//If the ship can move move vertically
		if(verticalMove)
		{
			//If the within a certain threshold of the player
			if(player == null || (Mathf.Abs(transform.position.y-player.transform.position.y) < 0.1))
			{
				//Don't move
				direction = 0;
			}
			//Track the movement of the player and follow it
			else if(player.transform.position.y > transform.position.y)
			{
				//Move up
				direction =  Mathf.Abs(speed);
			}
			else
			{
				//Move down
				direction = -Mathf.Abs (speed);
			}
		}

		//The new position of the enemy after moving
		Vector3 newPos = new Vector3 (boundaries.getLeft() * .9f, transform.position.y + direction, transform.position.z);

		//If the ship is at the left side of the screen
		if(transform.position.x <= boundaries.getLeft()* 0.89)
		{
			//If this peice of code hasn't been run yet
			if(!verticalMove)
			{
				//Delay a short time then change verticalMove
				StartCoroutine(delay());

				//Also decrease the speed greatly
				newSpeed = speed/5;
			}
		}

		//Make the move
		transform.position = Vector3.MoveTowards(transform.position, newPos, newSpeed);
		
		//If the enemy is "reloading", don't decrement the timer
		if (!ready) {
			
			//Decrements the shoot timer
			shootTimer--;
			
			//If the shoot timer has reached 0, reset it and flag that the enemy can shoot
			if (shootTimer <= 0) 
			{
				ready = true;
				shootTimer = reloadTime;
			}
		}

		//If the enemy can shoot and is in bounds and can move vertically
		if(ready && boundaries.inBoundaries(transform.position,1.2f) && verticalMove)
		{
			//Flag that a bullet was shot
			ready = false;
			
			//Spawn the bullet and store it
			GameObject bullet = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

			//Rotate the bullet around to move towards the right side of the screen
			bullet.transform.Rotate (0,0,180);

			//Cast to a bullet type
			SimpleEnemyBullet simpleEnemyBullet = (SimpleEnemyBullet)bullet.GetComponent(typeof(SimpleEnemyBullet));
			
			//Set the damage of the bullet
			simpleEnemyBullet.setDamage(bulletDamage);
		}
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
			
			//Get the damage the player bullet will deal
			int damage = col.gameObject.GetComponent<Bullet>().getDamage();
			
			//Deal the damage to this enemy
			takeDamage(damage);
			
		}
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : takeDamage(float damage)
	 *
	 * Description : Deals damage to the enemies health
	 *
	 * Parameters  : int damage : The damage to be dealt
	 *
	 * Returns     : Void
	 */
	public void takeDamage(int damage)
	{
		
		//Subtract health from the enemy
		health -= damage;
		
		//If health hits 0, then the enemy dies
		if(health <= 0)
		{
			//Destroy the enemy
			Destroy(this.gameObject);
			
			//Update the players score
			score.UpdateScore(value);
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : getCollisionDamage()
	 *
	 * Description : Returns the collision damage for this enemy
	 *
	 * Parameters  : None.
	 *
	 * Returns     : int:  Collision damage
	 */
	public int getCollisionDamage()
	{
		return collisionDamage;
	}

	//Only called when the application is being quit. Will disable spawning in OnDestroy
	void OnApplicationQuit() {
		
		isQuitting = true;
		
	}
	
	//Used to spawn particle effects or money when destroyed
	void OnDestroy() {
		
		if (!isQuitting) {
			//Load the money prefab
			GameObject money = Resources.Load<GameObject>("Money/Money");
			
			//Load the explosion
			GameObject explosion = Resources.Load<GameObject>("Explosions/SimpleExplosion");
			
			//Position of the enemy
			var position = gameObject.transform.position;
			
			//Create the explosion at this location
			Instantiate(explosion, new Vector3(position.x, position.y, position.z), Quaternion.identity);	
			
			//Create money at this location
			Instantiate(money, new Vector3(position.x, position.y, position.z), Quaternion.identity);
			
		}
		
	}

}