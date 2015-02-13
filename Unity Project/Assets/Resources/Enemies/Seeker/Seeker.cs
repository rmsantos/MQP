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
	public GameObject scoreObject;
	static ScoreHandler score;

	//Speed in the x direction
	float xSpeed;

	//Player script
	GameObject player;

	//Player position
	Vector3 playerPosition;

	//The health of this enemy
	public int health;
	
	//The health per level
	public float healthPerLevel;

	//Stores the damage colliding with the player does
	public int collisionDamage;

	//Stores the damage the missile does
	public int missileDamage;

	//The damage per level
	public float damagePerLevel;

	//Quitting boolean
	bool isQuitting;

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

		//save initial rotation value
		originalRotationValue = transform.rotation;

		//The enemy can shoot right when it spawns
		ready = true;
		
		//Set the shooting timer
		shootTimer = reloadTime;
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//Search for the ScoreHandler object for tracking score
		score = (ScoreHandler)scoreObject.GetComponent("ScoreHandler");

		//Set the x speed to move to the left 
		xSpeed = -speed;

		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");

		//Not quitting the application
		isQuitting = false;

		//Set the level progression modifiers
		health += (int)(healthPerLevel * ((float)PlayerPrefs.GetInt("Level", 0) - 1f));
		missileDamage += (int)(damagePerLevel * ((float)PlayerPrefs.GetInt ("Level", 0) - 1f));
		collisionDamage += (int)(damagePerLevel * ((float)PlayerPrefs.GetInt ("Level", 0) - 1f));

	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the enemy to a fixed x position then moves 
	 * 				it upwards and downwards and periodically shoots missiles
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//Stop the enemy when it hits right right bound of the screen
		if (transform.position.x < boundaries.getRight() * .9f)
			xSpeed = 0;

		//The new position of the enemy after moving
		Vector3 newPos = new Vector3 (transform.position.x +xSpeed, transform.position.y + speed, transform.position.z);

		//If hitting either top or bottom boundaries, reverse the direction
		if(newPos.y >= boundaries.getTop() * .8f || newPos.y <= boundaries.getBottom() * .8f)
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

			//Store the players position
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

			//Store the direction of the player in respect to the missile
			Vector3 direction = playerPosition-missile.transform.position;

			//Rotate the missile towards the player
			missile.transform.rotation = Quaternion.LookRotation(direction);

			//Cast to an missile type
			SeekerMissile seekerMissile = (SeekerMissile)missile.GetComponent(typeof(SeekerMissile));

			//Set the damage of the missile
			seekerMissile.setDamage(missileDamage);

			//Flag that player has just shot
			ready = false;
			
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