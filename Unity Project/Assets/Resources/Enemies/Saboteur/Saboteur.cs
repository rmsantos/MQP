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
	
	//The health per level
	public float healthPerLevel;

	//Stores the damage colliding with the player does
	public int collisionDamage;
	
	//Stores the damage the bullet does
	public int bulletDamage;
	
	//The damage per level
	public float damagePerLevel;

	//The state of the saboteur
	//State 0 is moving to the left side of the screen (and off screen)
	//State 1 is turning around and moving to the left bound of the screen
	//State 2 is Moving vertically and shooting at the player
	int state;

	//Quitting boolean
	bool isQuitting;

	//The place to spawn bullets
	public Transform turret;

	//Randomizer script
	GameObject randomizer;
	Randomizer random;
	
	//Money drop rate 
	public int moneyDropRate;

	//Used as a reference for rotating back to normal
	public Quaternion originalRotationValue;

	//Rotates back to normal at this speed
	public float rotationResetSpeed;

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

		//Get the randomizer script
		randomizer = GameObject.FindGameObjectWithTag ("Randomizer");
		random = (Randomizer)randomizer.GetComponent("Randomizer");

		//Rotate the Saboteur at the start to align it properly and save it
		transform.Rotate (0, 180, 0);
		originalRotationValue = transform.rotation;

		//Start at state 0
		state = 0;

		//The enemy can shoot right when it spawns
		ready = true;
		
		//Set the shooting timer
		shootTimer = reloadTime;
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//Search for the ScoreHandler object for tracking score
		score = (ScoreHandler)scoreObject.GetComponent("ScoreHandler");

		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");

		//Not quitting the application
		isQuitting = false;

		//Set the level progression modifiers
		health += (int)(healthPerLevel * ((float)PlayerPrefs.GetInt("Level", 0) - 1f));
		bulletDamage += (int)(damagePerLevel * ((float)PlayerPrefs.GetInt ("Level", 0) - 1f));
		collisionDamage += (int)(damagePerLevel * ((float)PlayerPrefs.GetInt ("Level", 0) - 1f));
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

		//Move to the left side of the screen (off it)
		if(state == 0)
		{
			//The new position of the enemy after moving
			Vector3 newPos = new Vector3 (boundaries.getLeft() * 1.2f, transform.position.y, transform.position.z);

			//Make the move
			transform.position = Vector3.MoveTowards(transform.position, newPos, speed);

			//If the saboteur hits its mark
			if(transform.position.x <= boundaries.getLeft() * 1.1f)
			{
				//Rotate the sprite 180
				transform.Rotate(0,180,0);
				originalRotationValue = transform.rotation;

				//Switch states
				state = 1;

				//Lower the speed
				speed = speed/5;
			}
		}
		//Move to the left bound of the screen (back on screen)
		else if(state == 1)
		{
			//The new position of the enemy after moving
			Vector3 newPos = new Vector3 (boundaries.getLeft()*0.9f, transform.position.y, transform.position.z);
			
			//Make the move
			transform.position = Vector3.MoveTowards(transform.position, newPos, speed);

			//If the ship is locked into its horizontal position
			if(transform.position.x >= boundaries.getLeft()*0.9f)
			{
				//Switch to the next state
				state = 2;
			}
		}
		//State 2 slowly tracks the player
		else
		{
			//Direction the enemy should move
			float direction = 0;

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

			//The new position of the enemy after moving
			Vector3 newPos = new Vector3 (boundaries.getLeft()*0.9f, transform.position.y+direction, transform.position.z);

			//Make the move
			transform.position = Vector3.MoveTowards(transform.position, newPos, speed);

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

			if(ready)
			{
				//Flag that a bullet was shot
				ready = false;
				
				//Spawn the bullet and store it
				GameObject bullet = (GameObject)Instantiate(bulletPrefab,turret.position,Quaternion.identity);
				
				//Rotate the bullet around to move towards the right side of the screen
				bullet.transform.Rotate (0,0,180);
				
				//Cast to a bullet type
				SimpleEnemyBullet simpleEnemyBullet = (SimpleEnemyBullet)bullet.GetComponent(typeof(SimpleEnemyBullet));
				
				//Set the damage of the bullet
				simpleEnemyBullet.setDamage(bulletDamage);
			}

		}
		//Always try to rotate to the correct facing direction
		transform.rotation = Quaternion.Slerp(transform.rotation, originalRotationValue, rotationResetSpeed); 
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
			int damage = col.gameObject.GetComponent<Laser>().getDamage();
			
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

			//Load the explosion
			GameObject explosion = Resources.Load<GameObject>("Explosions/SimpleExplosion");
			
			//Position of the enemy
			var position = gameObject.transform.position;
			
			//Create the explosion at this location
			Instantiate(explosion, new Vector3(position.x, position.y, position.z), Quaternion.identity);	
			
			//Spawn money with a certain chance
			if(random.GetRandom(100) < moneyDropRate)
			{
				//Load the money prefab
				GameObject money = Resources.Load<GameObject>("Money/Money");
				
				//Create money at this location
				Instantiate(money, new Vector3(position.x, position.y, position.z), Quaternion.identity);
				
			}
			
		}
		
	}

}