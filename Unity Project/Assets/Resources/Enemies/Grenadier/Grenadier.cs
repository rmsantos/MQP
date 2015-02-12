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

public class Grenadier : MonoBehaviour, BasicEnemy {
	
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

	//Angle at which the grenadier will shoot the "shotgun"
	public float shootAngle;

	//The health of this enemy
	public int health;

	//Stores the damage colliding with the player does
	public int collisionDamage;
	
	//Stores the damage the bullet does
	public int bulletDamage;

	//Get the portrait controller to play audio clips
	PortraitController portraitController;

	//Quitting boolean
	bool isQuitting;

	//The place to spawn bullets
	public Transform turret1;
	public Transform turret2;
	public Transform turret3;

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

		//The enemy can shoot right when it spawns
		ready = true;

		//save initial rotation value
		originalRotationValue = transform.rotation;
		
		//Set the shooting timer
		shootTimer = reloadTime;
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//Search for the ScoreHandler object for tracking score
		score = (ScoreHandler)scoreObject.GetComponent("ScoreHandler");

		//Find the portrait controller script
		portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();

		//Not quitting the application
		isQuitting = false;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the enemy slowly to the left and periodically shoots bullets in a shotgun pattern
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
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
			GameObject bullet1 = (GameObject)Instantiate(bulletPrefab,turret3.position,Quaternion.identity);

			//Spawn the second bullet and store it
			GameObject bullet2 = (GameObject)Instantiate(bulletPrefab,turret2.position,Quaternion.identity);

			//Spawn the third bullet and store it
			GameObject bullet3 = (GameObject)Instantiate(bulletPrefab,turret1.position,Quaternion.identity);

			//Cast the first bullet to a bullet type
			SimpleEnemyBullet simpleEnemyBullet1 = (SimpleEnemyBullet)bullet1.GetComponent(typeof(SimpleEnemyBullet));
			
			//Set the damage of the bullet
			simpleEnemyBullet1.setDamage(bulletDamage);

			//Cast the second bullet to a bullet type
			SimpleEnemyBullet simpleEnemyBullet2 = (SimpleEnemyBullet)bullet2.GetComponent(typeof(SimpleEnemyBullet));
			
			//Set the damage of the bullet
			simpleEnemyBullet2.setDamage(bulletDamage);

			//Cast the third bullet to a bullet type
			SimpleEnemyBullet simpleEnemyBullet3 = (SimpleEnemyBullet)bullet3.GetComponent(typeof(SimpleEnemyBullet));
			
			//Set the damage of the bullet
			simpleEnemyBullet3.setDamage(bulletDamage);

			//Bullet 1 will move only in a straight line

			//Bullet 2 will shoot at a +degree angle
			bullet2.transform.Rotate(0,0,shootAngle);

			//Bullet 3 will shoot at a -degree angle
			bullet3.transform.Rotate(0,0,-shootAngle);

		}	
		
		//If the enemy leaves the game space
		//Leave some room for the enemy to fully exit the visible screen (by multiplying 1.2)
		if (transform.position.x < (boundaries.getLeft() * 1.2))
		{
			//Destroy the enemy
			Destroy (this.gameObject);
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
			//Play the sound effect upon this enemy being destroyed
			portraitController.playLargeEnemyDestroyed();

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
