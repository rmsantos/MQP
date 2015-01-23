/* Module      : Interceptor.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Interceptor
 *
 * Date        : 2015/1/22
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Interceptor :  MonoBehaviour, BasicEnemy {
	
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
	ScoreHandler score;
	
	//Speed in the x direction
	float xSpeed;
	
	//Player script
	GameObject player;

	//Player position
	Vector3 playerPosition;
	
	//The health of this enemy
	public int health;
	
	//Stores the damage colliding with the player does
	public int collisionDamage;
	
	//Stores the damage the bullet does
	public int bulletDamage;
	
	//Time before the interceptor speeds away
	public int flyOffTime;

	//Timer to keep track of when to fly off
	int timer;

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
		score = GameObject.FindGameObjectWithTag ("ScoreHandler").GetComponent<ScoreHandler>(); 
		
		//Set the x speed to move to the left 
		xSpeed = -speed;
		
		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Moves the enemey slowly up and down and after a certain amount of time,
	 * 				Speeds off the left side of the screen.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		Vector3 newPos;

		if(timer == flyOffTime)
		{
			//The new position of the enemy after moving
			newPos = new Vector3 (transform.position.x + (speed*2), transform.position.y, transform.position.z);
		}
		else
		{
			//If the enemy is in the boundaries
			if(boundaries.inBoundaries(transform.position,1))
			{
				//Increment the timer
				timer++;
			}

			//Stop the enemy when it hits right right bound of the screen
			if (transform.position.x < boundaries.getRight())
				xSpeed = 0;
			
			//The new position of the enemy after moving
			newPos = new Vector3 (transform.position.x +xSpeed, transform.position.y + speed, transform.position.z);
			
			//If hitting either top or bottom boundaries, reverse the direction
			if(newPos.y >= boundaries.getTop() || newPos.y <= boundaries.getBottom())
				speed = -speed;

		}
	
		//Apply the movement
		transform.position = newPos;

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

		//If the enemy can shoot and is in bounds
		if(ready & boundaries.inBoundaries(transform.position,1))
		{
			//Spawn the bullet and store it
			GameObject bullet = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

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
				

			//Store the direction of the player in respect to the bullet
			Vector3 direction = playerPosition-bullet.transform.position;
		
			//Rotate the bullet towards the player
			bullet.transform.rotation = Quaternion.LookRotation(direction);

			//Rotate the bullet along the y so that it faces the camera
			bullet.transform.Rotate(0,90,0);

			//Cast to a bullet type
			SimpleEnemyBullet simpleEnemyBullet = (SimpleEnemyBullet)bullet.GetComponent(typeof(SimpleEnemyBullet));
			
			//Set the damage of the bullet
			simpleEnemyBullet.setDamage(bulletDamage);
			
			//Flag that player has just shot
			ready = false;

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
			Destroy (this.gameObject);	
			
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
}