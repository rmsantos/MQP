/* Module      : Ambusher.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the BasicEnemy
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

public class Ambusher : MonoBehaviour, BasicEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The speed at which the enemy will move
	public float speed;
	
	//Stores the boundaries of the game
	Boundaries boundaries;

	//Invisibility counter and boolean
	int counter;
	bool invisible;
	float alpha;
	
	//Player script
	GameObject player;

	//Player position
	Vector3 playerPosition;

	//Value of destroying this enemy
	public int value;
	
	//ScoreHandler object to track players score
	public GameObject scoreObject;
	static ScoreHandler score;

	//The health of this enemy
	public int health;

	//The health per level
	public float healthPerLevel;

	//Stores the damage colliding with the player does
	public int collisionDamage;

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

		//Invisibility values
		invisible = false;
		counter = 200;
		alpha = 1f;
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");

		//Search for the ScoreHandler object for tracking score
		score = (ScoreHandler)scoreObject.GetComponent("ScoreHandler");

		//Get the portrait controller to play audio clips
		PortraitController portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();

		//Play the ambusher spawn clip
		portraitController.playAmbusherSpawn ();

		//Not quitting the application
		isQuitting = false;

		//Set the level progression modifiers
		health += (int)(healthPerLevel * ((float)PlayerPrefs.GetInt("Level", 0) - 1f));
		collisionDamage += (int)(damagePerLevel * ((float)PlayerPrefs.GetInt ("Level", 0) - 1f));
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the enemy slowly to the left and periodically shoots bullets
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

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

		//follow the player
		transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed);
		
		//If the enemy leaves the game space
		//Leave some room for the enemy to fully exit the visible screen (by multiplying 1.2)
		if (transform.position.x < (boundaries.getLeft() * 1.2))
		{
			//Destroy the enemy
			Destroy (this.gameObject);
		}

		counter -= 1;
		if (invisible && counter <= 0) {
			if (alpha < 1.0f) {
				alpha += .05f;
				var originalColour = renderer.material.color;
				renderer.material.color = new Color(originalColour.r, originalColour.g, originalColour.b, alpha);
			}
			else {
				invisible = false;
				counter = 50;
			}
		}
		else if (!invisible && counter <= 0) {
			if (alpha > .20f) {
				alpha -= .05f;
				var originalColour = renderer.material.color;
				renderer.material.color = new Color(originalColour.r, originalColour.g, originalColour.b, alpha);
			}
			else {
				invisible = true;
				counter = 150;
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
