/* Module      : Juggernaut.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Juggernaut
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

public class Juggernaut :  MonoBehaviour, BasicEnemy {
	
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
	
	//The health of this enemy
	public int health;
	
	//Stores the damage colliding with the player does
	public int collisionDamage;
	
	//Stores the damage the bullet does
	public int bulletDamage;

	//Store the transform of the shield of this enemy
	public Transform shield;

	//Angle at which the juggernaut will shoot the "shotgun"
	public float shootAngle;

	//Rotation speed for the shields
	public float rotationSpeed;

	//Shield rotation
	Transform shieldRotation;

	//Get the portrait controller to play audio clips
	PortraitController portraitController;

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

		//Store the rotation of the shield
		shieldRotation = shield.transform;

		//Set the collision damage that the shields will do
		shield.GetComponentInParent<JuggernautShield> ().setCollisionDamage (collisionDamage/2);

		//Find the portrait controller script
		portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();

		//Not quitting the application
		isQuitting = false;
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

		//Rotate the shields around the enemy
		shield.RotateAround (transform.position, Vector3.forward, rotationSpeed);

		//The new position of the enemy after moving
		Vector3 newPos = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);
		
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
			//Flag that a bullet was shot
			ready = false;

			//Spawn the first bullet and store it
			GameObject bullet1 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
			
			//Spawn the second bullet and store it
			GameObject bullet2 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
			
			//Spawn the third bullet and store it
			GameObject bullet3 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

			//Spawn the fourth bullet and store it
			GameObject bullet4 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
			
			//Spawn the fifth bullet and store it
			GameObject bullet5 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
			
			//Spawn the sixths bullet and store it
			GameObject bullet6 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

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

			//Cast the fourth bullet to a bullet type
			SimpleEnemyBullet simpleEnemyBullet4 = (SimpleEnemyBullet)bullet4.GetComponent(typeof(SimpleEnemyBullet));
			
			//Set the damage of the bullet
			simpleEnemyBullet4.setDamage(bulletDamage);

			//Cast the fifth bullet to a bullet type
			SimpleEnemyBullet simpleEnemyBullet5 = (SimpleEnemyBullet)bullet5.GetComponent(typeof(SimpleEnemyBullet));
			
			//Set the damage of the bullet
			simpleEnemyBullet5.setDamage(bulletDamage);

			//Cast the sixth bullet to a bullet type
			SimpleEnemyBullet simpleEnemyBullet6 = (SimpleEnemyBullet)bullet6.GetComponent(typeof(SimpleEnemyBullet));
			
			//Set the damage of the bullet
			simpleEnemyBullet6.setDamage(bulletDamage);

			//Bullet 1 shoots perpindicular to the shield
			bullet1.transform.rotation = shieldRotation.rotation;
			bullet1.transform.Rotate (0,0,75);

			//Bullet 2 shoots perpendicular and shootAngle to the shield
			bullet2.transform.rotation = shieldRotation.rotation;
			bullet2.transform.Rotate(new Vector3(0,0,shootAngle+75));

			//Bullet 3 shoots perpendicular and -shootAngle to the shield
			bullet3.transform.rotation = shieldRotation.rotation;
			bullet3.transform.Rotate(new Vector3(0,0,-shootAngle+75));

			//Bullet 4 shoots -perpendicular to the shield
			bullet4.transform.rotation = shieldRotation.rotation;
			bullet4.transform.Rotate(new Vector3(0,180,-75));

			//Bullet 5 shoots -perpendicular and shootAngle to the shield
			bullet5.transform.rotation = shieldRotation.rotation;
			bullet5.transform.Rotate(new Vector3(0,180,shootAngle-75));

			//Bullet 6 shoots -perpendicular and -shootAngle to the shield
			bullet6.transform.rotation = shieldRotation.rotation;
			bullet6.transform.Rotate(new Vector3(0,180,-shootAngle-75));
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
	/* Function    : OnCollisionEnter2D (Collision2D col)
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
			print ("YEOUCH");

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
