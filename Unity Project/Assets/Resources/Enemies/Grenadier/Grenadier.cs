/* Module      : Grenadier.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Grenadier
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

public class Grenadier : BasicEnemy {
	
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

	//Angle at which the grenadier will shoot the "shotgun"
	public float angle;

	//The health of this enemy
	public int health;

	//Stores the damage colliding with the player does
	public int collisionDamage;
	
	//Stores the damage the bullet does
	public int bulletDamage;

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
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Moves the enemy slowly to the left and periodically shoots bullets in a shotgun pattern
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */
		
		//The new position of the enemy after moving
		Vector3 newPos = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);
		
		//Apply the movement
		transform.position = newPos;
		
		//If the enemy is "reloading", don't decrement the timer
		if (!ready) {
			
			//Decrements the shoot timer
			shootTimer--;
			
			//If the shoot timer has reached 0, reset it and flag that the enemy can shoot
			if (shootTimer <= 0) {
				
				ready = true;
				shootTimer = reloadTime;
			}
		}
		
		//If the enemy can shoot and is in bounds
		if(ready & boundaries.inBoundaries(transform.position,1))
		{
			//Flag that a bullet was shot
			ready = false;
			
			//Spawn the first bullet and store it
			GameObject bullet1 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

			//Spawn the second bullet and store it
			GameObject bullet2 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

			//Spawn the third bullet and store it
			GameObject bullet3 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

			//Cast the first bullet to a bullet type
			GrenadierBullet grenadierBullet1 = (GrenadierBullet)bullet1.GetComponent(typeof(GrenadierBullet));
						
			//Set the damage of the bullet
			grenadierBullet1.setDamage(bulletDamage);

			//Cast the second bullet to a bullet type
			GrenadierBullet grenadierBullet2 = (GrenadierBullet)bullet2.GetComponent(typeof(GrenadierBullet));
			
			//Set the damage of the bullet
			grenadierBullet2.setDamage(bulletDamage);

			//Cast the third bullet to a bullet type
			GrenadierBullet grenadierBullet3 = (GrenadierBullet)bullet3.GetComponent(typeof(GrenadierBullet));
			
			//Set the damage of the bullet
			grenadierBullet3.setDamage(bulletDamage);

			//Create a Quaternion that points directly to the left side of the screen (-x direction)
			Quaternion leftAlign = Quaternion.LookRotation(new Vector3(-1,0,0));

			//Set all bullets to look towards leftAlign
			bullet1.transform.rotation = leftAlign;
			bullet2.transform.rotation = leftAlign;
			bullet3.transform.rotation = leftAlign;

			//Bullet 1 will move only in a straight line

			//Bullet 2 will shoot at a +30 degree angle
			bullet2.transform.Rotate(angle,0,0);

			//Bullet 3 will shoot at a -30 degree angle
			bullet3.transform.Rotate(-angle,0,0);

		}	
		
		//If the enemy leaves the game space
		//Leave some room for the enemy to fully exit the visible screen (by multiplying 1.2)
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
	public override void takeDamage(int damage)
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
	public override int getCollisionDamage()
	{
		return collisionDamage;
	}
}
