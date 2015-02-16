/* Module      : AbstractEnemy.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file contains variables and functions that all enemies share
 *
 * Date        : 2015/2/13
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

public abstract class AbstractEnemy : MonoBehaviour {

	//Stores the boundaries of the game
	protected Boundaries boundaries;

	//Player script
	protected GameObject player;

	//The audiohandler
	AudioHandler audioHandler;

	//Randomizer script
	Randomizer random;

	//ScoreHandler object to track players score
	static protected ScoreHandler score;

	//Portrait controller
	protected PortraitController portraitController;

	//The speed at which the enemy will move
	public float speed;

	//Value of destroying this enemy
	public int value;

	//The health of this enemy
	public int health;

	//Is the enemy ready to shoot?
	protected bool ready;
	
	//Counter for reloading
	int shootTimer;
	
	//Time before the enemy can shoot again 
	public int reloadTime;

	//Prefab of the enemy bullet
	public GameObject bulletPrefab;
	
	//The health per level
	public float healthPerLevel;
	
	//Stores the damage colliding with the player does
	public int collisionDamage;

	//Damage done from bullets
	public int bulletDamage;

	//The damage per level
	public float damagePerLevel;
	
	//Quitting boolean
	bool isQuitting;

	//Money drop rate 
	public int moneyDropRate;

	//Used as a reference for rotating back to normal
	protected Quaternion originalRotationValue;
	
	//Rotates back to normal at this speed
	public float rotationResetSpeed;

	//The place to spawn bullets
	public Transform turret;
	
	/* ----------------------------------------------------------------------- */
	/* Function    : setup()
	 *
	 * Description : Finds all of the useful game objects
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void setup()
	{
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();

		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");

		//Search for the audioHandler
		audioHandler = GameObject.FindGameObjectWithTag ("AudioHandler").GetComponent<AudioHandler> ();

		//Get the randomizer script
		random = GameObject.FindGameObjectWithTag ("Randomizer").GetComponent<Randomizer>();

		//Search for the ScoreHandler object for tracking score
		score = GameObject.FindGameObjectWithTag("ScoreHandler").GetComponent<ScoreHandler>();

		//Get the portrait controller to play audio clips
		portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();

		//Not quitting the application
		isQuitting = false;

		//Set the level progression modifiers
		health += (int)(healthPerLevel * ((float)PlayerPrefs.GetInt("Level", 0) - 1f));
		collisionDamage += (int)(damagePerLevel * ((float)PlayerPrefs.GetInt ("Level", 0) - 1f));
		bulletDamage += (int)(damagePerLevel * ((float)PlayerPrefs.GetInt ("Level", 0) - 1f));

		//save initial rotation value
		originalRotationValue = transform.rotation;

		//The enemy can shoot right when it spawns
		ready = true;
		
		//Set the shooting timer
		shootTimer = reloadTime;
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
		
		//Show the hit fader effect
		GetComponent<HitFader>().BeenHit();
		
		//If health hits 0, then the enemy dies
		if(health <= 0)
		{
			//Destroy the enemy
			Destroy(this.gameObject);
			
			//Update the players score
			score.UpdateScore(value);

			//Play the appropriate explosions sound
			switch(gameObject.name)
			{
				case "Ambusher":
				case "DogFighterA":
				case "DogFighterB":
				case "Cruiser":
				case "Interceptor":
				case "Saboteur":
				case "Seeker":
					audioHandler.playSmallEnemyExplosion();
					break;
				case "Grenadier":
				case "Juggernaut":
					audioHandler.playMediumEnemyExplosion();
					portraitController.playLargeEnemyDestroyed();
					break;
			}

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

	/* ----------------------------------------------------------------------- */
	/* Function    : checkBoundaries()
	 *
	 * Description : Destroys the enemy if it goes off screen to the left
	 *
	 * Parameters  : None.
	 *
	 * Returns     : None.
	 */
	public void checkBoundaries()
	{
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
	/* Function    : reload()
	 *
	 * Description : Called to increment the reload timers
	 *
	 * Parameters  : None.
	 *
	 * Returns     : None.
	 */
	public void reload()
	{
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
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : canShoot()
	 *
	 * Description : Is the enemy reloaded and in bounds?
	 *
	 * Parameters  : None.
	 *
	 * Returns     : bool : True if above, false otherwise.
	 */
	public bool canShoot()
	{
		return ready && boundaries.inBoundaries (transform.position, 1);
	}


	/* ----------------------------------------------------------------------- */
	/* Function    : reload()
	 *
	 * Description : Spawns a bullet and sets its damage
	 *
	 * Parameters  : Transform location : The position to spawn
	 *
	 * Returns     : GameObject : The bullet spawned
	 */
	public GameObject shoot(Transform location)
	{
		//Just fired
		ready = false;

		//Spawn the bullet and store it
		GameObject bullet = (GameObject)Instantiate(bulletPrefab,location.position,Quaternion.identity);
		
		//Cast to an bullet type
		SimpleEnemyBullet simpleEnemyBullet = (SimpleEnemyBullet)bullet.GetComponent(typeof(SimpleEnemyBullet));
		
		//Set the damage of the bullet
		simpleEnemyBullet.setDamage(bulletDamage);

		//Return this bullet
		return bullet;
	}
}