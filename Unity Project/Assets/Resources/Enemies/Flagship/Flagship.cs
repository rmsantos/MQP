/* Module      : Flagship.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the flagship boss
 *
 * Date        : 2015/1/23
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Flagship :  MonoBehaviour, BasicEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The speed at which the enemy will move
	public float speed;

	float moveSpeed;

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

	//The health of this enemy
	public int health;

	//Stores the damage colliding with the player does
	public int collisionDamage;
	
	//Stores the damage the bullet does
	public int bulletDamage;

	//The Boss Instance Object
	public GameObject boss1;
	Boss1 bossInstance;

	int rotateCount;

	int phase;

	int secondShoot;


	public GameObject missilePrefab;

	public GameObject ambusherPrefab;

	int movePhase;

	Vector3 lastPos;

	public int missileDamage;

	int startingHealth;

	//Player script
	GameObject player;

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

		//Get the script that created this boss
		bossInstance = (Boss1) boss1.GetComponent("Boss1");

		phase = 0;
		movePhase = 0;
		moveSpeed = speed;
		secondShoot = 0;


		startingHealth = health;

		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Moves the enemy slowly to the left
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		if(phase == 0)
		{
			transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed);

			if(transform.position == Vector3.zero)
			{
				phase = 1;
			}


		}

		//Spinny bullet hell mode
		if(phase == 1)
		{
			if(health == startingHealth*3/4)
			{
				phase = 2;
			}

			if(ready)
			{
				rotateCount += 11;
				//Flag that a bullet was shot
				ready = false;

				GameObject bullet1 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
				GameObject bullet2 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
				GameObject bullet3 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
				GameObject bullet4 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

				bullet1.transform.Rotate(0,0,rotateCount);
				bullet2.transform.Rotate(0,0,90+rotateCount);
				bullet3.transform.Rotate(0,0,180+rotateCount);
				bullet4.transform.Rotate(0,0,270+rotateCount);

				//Cast to a bullet type
				SimpleEnemyBullet simpleEnemyBullet1 = (SimpleEnemyBullet)bullet1.GetComponent(typeof(SimpleEnemyBullet));
				
				//Set the damage of the bullet
				simpleEnemyBullet1.setDamage(bulletDamage);
				
				//Cast to a bullet type
				SimpleEnemyBullet simpleEnemyBullet2 = (SimpleEnemyBullet)bullet2.GetComponent(typeof(SimpleEnemyBullet));
				
				//Set the damage of the bullet
				simpleEnemyBullet2.setDamage(bulletDamage);

				//Cast to a bullet type
				SimpleEnemyBullet simpleEnemyBullet3 = (SimpleEnemyBullet)bullet3.GetComponent(typeof(SimpleEnemyBullet));
				
				//Set the damage of the bullet
				simpleEnemyBullet3.setDamage(bulletDamage);
				
				//Cast to a bullet type
				SimpleEnemyBullet simpleEnemyBullet4 = (SimpleEnemyBullet)bullet4.GetComponent(typeof(SimpleEnemyBullet));
				
				//Set the damage of the bullet
				simpleEnemyBullet4.setDamage(bulletDamage);
			}

		}


		if(phase == 2)
		{
			print("PHASE TWO!");

			//The new position of the enemy after moving
			Vector3 newPos = new Vector3 (transform.position.x + moveSpeed, transform.position.y, transform.position.z);
			
			//If hitting either top or bottom boundaries, reverse the direction
			if(newPos.x >= boundaries.getRight() || newPos.x <= boundaries.getLeft())
				moveSpeed = - moveSpeed;

			transform.position = newPos;

			if(ready)
			{
				GameObject bullet1 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
				GameObject bullet2 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

				bullet2.transform.Rotate(0,0,180);

				//Cast to a bullet type
				SimpleEnemyBullet simpleEnemyBullet1 = (SimpleEnemyBullet)bullet1.GetComponent(typeof(SimpleEnemyBullet));
				
				//Set the damage of the bullet
				simpleEnemyBullet1.setDamage(bulletDamage);

				//Cast to a bullet type
				SimpleEnemyBullet simpleEnemyBullet2 = (SimpleEnemyBullet)bullet2.GetComponent(typeof(SimpleEnemyBullet));
				
				//Set the damage of the bullet
				simpleEnemyBullet2.setDamage(bulletDamage);

				simpleEnemyBullet1.speed = simpleEnemyBullet1.speed*2;
				simpleEnemyBullet2.speed = simpleEnemyBullet2.speed*2;

				secondShoot++;

				if(secondShoot % 2 == 0)
				{
					GameObject bullet3 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

					//Store the direction of the player in respect to the bullet
					Vector3 direction = player.transform.position-bullet3.transform.position;
					
					//Rotate the bullet towards the player
					bullet3.transform.rotation = Quaternion.LookRotation(direction);
					
					//Rotate the bullet along the y so that it faces the camera
					bullet3.transform.Rotate(0,90,0);

					//Cast to a bullet type
					SimpleEnemyBullet simpleEnemyBullet3 = (SimpleEnemyBullet)bullet3.GetComponent(typeof(SimpleEnemyBullet));
					
					//Set the damage of the bullet
					simpleEnemyBullet3.setDamage(bulletDamage);
				}

				ready = false;

			}
	
			if(health == startingHealth/2)
			{
				phase = 3;
				lastPos = transform.position;
			}
		}

		if(phase == 3)
		{
			print ("PHASE 3");

			Vector3 topLeft = new Vector3(boundaries.getRight()/4-boundaries.getRight (),boundaries.getTop()*3/4, 0);
			Vector3 topRight = new Vector3(boundaries.getRight()*3/4,boundaries.getTop()*3/4,0);
			Vector3 botLeft = new Vector3(boundaries.getRight()/4-boundaries.getRight(),boundaries.getTop()/4-boundaries.getTop(),0);
			Vector3 botRight = new Vector3(boundaries.getRight()*3/4,boundaries.getTop()/4-boundaries.getTop(),0);

			print (topLeft);

			if(transform.position == topLeft)
			{
				movePhase = 1;
			}
			else if(transform.position == topRight)
			{
				movePhase = 2;
			}
			else if(transform.position == botRight)
			{
				movePhase = 3;
			}
			else if(transform.position == botLeft || transform.position == lastPos)
			{
				movePhase = 0;
				print ("HERE");
			}
			
			if(movePhase == 0)
			{
				print ("HERE TOO");
				transform.position = Vector3.MoveTowards(transform.position, topLeft, speed);
			}
			else if(movePhase == 1)
			{
				transform.position = Vector3.MoveTowards(transform.position, topRight, speed);
			}
			else if(movePhase == 2)
			{
				transform.position = Vector3.MoveTowards(transform.position, botRight, speed);
			}
			else if(movePhase == 3)
			{
				transform.position = Vector3.MoveTowards(transform.position, botLeft, speed);
			}

			if(ready)
			{
				secondShoot++;

				if(secondShoot % 3 == 0)
				{
					//Spawn the missle and store it
					GameObject missile = (GameObject)Instantiate(missilePrefab,transform.position,Quaternion.identity);
				
					//Cast to an missile type
					SeekerMissile seekerMissile = (SeekerMissile)missile.GetComponent(typeof(SeekerMissile));
					
					//Set the damage of the missile
					seekerMissile.setDamage(missileDamage);
				}

				ready = false;
			}

			if(health == startingHealth/4)
			{
				phase = 4;
			}
		}

		if(phase == 4)
		{
			print ("PHASE 4");

			//The new position of the enemy after moving
			Vector3 newPos = new Vector3 (boundaries.getRight(), transform.position.y + moveSpeed, transform.position.z);

			if(newPos.y >= boundaries.getTop() || newPos.y <= boundaries.getBottom())
				moveSpeed = -moveSpeed;

			transform.position = Vector3.MoveTowards(transform.position, newPos, speed);

			if(ready)
			{
				secondShoot++;

				if(secondShoot % 5 == 0)
				{
					Instantiate(ambusherPrefab,transform.position,Quaternion.identity);
				}

				ready = false;
			}
		}


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
		if(col.gameObject.tag == "PlayerBullet" && phase != 0)
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
			bossInstance.BossDied();

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
