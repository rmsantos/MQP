/* Module      : Seeker.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Seeker
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

public class Seeker : MonoBehaviour, BasicEnemy {
	
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

	//The health of this enemy
	public int health;

	//Stores the damage colliding with the player does
	public int collisionDamage;

	//Stores the damage the missile does
	public int missileDamage;

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

		gameObject.transform.Rotate(90, 180, 0);
		
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
	 * Description : Moves the enemy to a fixed x position then moves 
	 * 				it upwards and downwards and periodically shoots missiles
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//Stop the enemy when it hits right right bound of the screen
		if (transform.position.x < boundaries.getRight())
			xSpeed = 0;

		//The new position of the enemy after moving
		Vector3 newPos = new Vector3 (transform.position.x +xSpeed, transform.position.y + speed, transform.position.z);

		//If hitting either top or bottom boundaries, reverse the direction
		if(newPos.y >= boundaries.getTop() || newPos.y <= boundaries.getBottom())
			speed = -speed;

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
			//Spawn the missle and store it
			GameObject missile = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

		
			//Store the direction of the player in respect to the missile
			Vector3 direction = player.transform.position-missile.transform.position;

			//Set the z to 0 so that it moves only in 2D
			direction.z = 0;

			//Rotate the missile towards the player
			missile.transform.rotation = Quaternion.LookRotation(direction);

			//Cast to an missile type
			SeekerMissile seekerMissile = (SeekerMissile)missile.GetComponent(typeof(SeekerMissile));

			//Set the damage of the missile
			seekerMissile.setDamage(missileDamage);

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
	void OnCollisionEnter (Collision col)
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