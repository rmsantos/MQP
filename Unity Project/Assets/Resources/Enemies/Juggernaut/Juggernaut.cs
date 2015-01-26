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

	//Store the transform of the first shield of this enemy
	public Transform shield1;

	//Store the transform of the second shield of this enemy
	public Transform shield2;

	//Angle at which the juggernaut will shoot the "shotgun"
	public float shootAngle;

	//Rotation speed for the shields
	public float rotationSpeed;

	//Shield rotation
	Transform shieldRotation;
	

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

		//Store the rotation of the shield
		shieldRotation = shield1.transform;

		//Set the collision damage that the shields will do
		shield1.GetComponentInParent<JuggernautShield> ().setCollisionDamage (collisionDamage/2);
		shield2.GetComponentInParent<JuggernautShield> ().setCollisionDamage (collisionDamage/2);
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
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
		shield1.RotateAround (transform.position, Vector3.forward, rotationSpeed);
		shield2.RotateAround (transform.position, Vector3.forward, rotationSpeed);

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

			//Bullet 2 shoots perpendicular and shootAngle to the shield
			bullet2.transform.rotation = shieldRotation.rotation;
			bullet2.transform.Rotate(new Vector3(0,0,shootAngle));

			//Bullet 3 shoots perpendicular and -shootAngle to the shield
			bullet3.transform.rotation = shieldRotation.rotation;
			bullet3.transform.Rotate(new Vector3(0,0,-shootAngle));

			//Bullet 4 shoots -perpendicular to the shield
			bullet4.transform.rotation = shieldRotation.rotation;
			bullet4.transform.Rotate(new Vector3(0,180,0));

			//Bullet 5 shoots -perpendicular and shootAngle to the shield
			bullet5.transform.rotation = shieldRotation.rotation;
			bullet5.transform.Rotate(new Vector3(0,180,shootAngle));

			//Bullet 6 shoots -perpendicular and -shootAngle to the shield
			bullet6.transform.rotation = shieldRotation.rotation;
			bullet6.transform.Rotate(new Vector3(0,180,-shootAngle));
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
