/* Module      : Flagship.cs
 * Author      : Ryan Santos
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
using UnityEngine.UI;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Flagship :  MonoBehaviour, BasicEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The speed at which the enemy will move
	public float speed;

	//The movement speed of the boss. This variable is meant to be modified
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
	public GameObject scoreObject;
	static ScoreHandler score;

	//The health of this enemy
	public int health;

	//Stores the damage colliding with the player does
	public int collisionDamage;
	
	//Stores the damage the bullet does
	public int bulletDamage;

	//The Boss Instance Object
	public GameObject boss1;
	Boss1 bossInstance;

	//The Level Handler
	public GameObject levelHandlerObject;
	LevelHandler levelHandler;

	//The angle at which the bullets will rotate in the spinning phase
	int rotateCount;

	//The state that the boss is in
	int phase;

	//The timer for boss bullets that arent main bullets
	int secondShoot;

	//The prefab for missiles
	public GameObject missilePrefab;

	//The prefab for ambushers
	public GameObject ambusherPrefab;

	//Variable to keep track of where to move the boss next in phase 3
	int movePhase;

	//The last position (in phase 2) before the boss starts moving
	Vector3 lastPos;

	//The damage the missiles will do
	public int missileDamage;

	//The starting health of the boss
	int startingHealth;

	//Player script
	GameObject player;

	//Player position
	Vector3 playerPosition;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the boss variables.
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
		score = (ScoreHandler)scoreObject.GetComponent("ScoreHandler");; 

		//Get the script that created this boss
		bossInstance = (Boss1) boss1.GetComponent("Boss1");

		//Get the script that controls the level
		levelHandler = (LevelHandler) levelHandlerObject.GetComponent("LevelHandler");

		//Initialize phase to 0 to move into place
		phase = 0;

		//Move phase is initially 0
		movePhase = 0;

		//Move speed is set to speed
		moveSpeed = speed;

		//Second shoot is also initialized to 0
		secondShoot = 0;

		//Store the starting health
		startingHealth = health;

		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Contains the boss state machine
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {

		//Update the health
		levelHandler.UpdateBossHealth (health);
		print (health);

		//Phase 0 the boss moves to the center of the screen
		if(phase == 0)
		{
			//Move towards the center of the screen
			transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed);

			//When hitting the center, move to phase 1
			if(transform.position == Vector3.zero)
			{
				phase = 1;
			}


		}

		//Spinny bullet hell state
		if(phase == 1)
		{
			//If the boss takes enough damage, then move to the next phase
			if(health <= startingHealth*3/4)
			{
				phase = 2;
			}

			//If the boss is ready to shoot
			if(ready)
			{
				//Rotate the angle at which the boss will shoot (so that it spins uniformly
				rotateCount += 11;

				//Flag that a bullet was shot
				ready = false;

				//Spawn 4 bullets
				GameObject bullet1 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
				GameObject bullet2 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
				GameObject bullet3 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
				GameObject bullet4 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

				//Rotate the bullets so that they form a "t" formation, then rotate them by rotateCount
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

		//Phase two traps the player in one horizontal half of the screen and bobs back and forth while shooting at the player
		if(phase == 2)
		{
			//The new position of the enemy after moving
			Vector3 newPos = new Vector3 (transform.position.x + moveSpeed, transform.position.y, transform.position.z);
			
			//If hitting either left or right boundaries, reverse the direction
			if(newPos.x >= boundaries.getRight() || newPos.x <= boundaries.getLeft())
				moveSpeed = - moveSpeed;

			//Make the move
			transform.position = newPos;

			//If ready to shoot
			if(ready)
			{
				//Spawn two bullets
				GameObject bullet1 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
				GameObject bullet2 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

				//One bullet moves in the opposite direction
				//This will cut the screen horizontally in half with bullets
				bullet2.transform.Rotate(0,0,180);

				//Cast to a bullet type
				SimpleEnemyBullet simpleEnemyBullet1 = (SimpleEnemyBullet)bullet1.GetComponent(typeof(SimpleEnemyBullet));
				
				//Set the damage of the bullet
				simpleEnemyBullet1.setDamage(bulletDamage);

				//Cast to a bullet type
				SimpleEnemyBullet simpleEnemyBullet2 = (SimpleEnemyBullet)bullet2.GetComponent(typeof(SimpleEnemyBullet));
				
				//Set the damage of the bullet
				simpleEnemyBullet2.setDamage(bulletDamage);

				//Speed these bullets up a bit so they look more uniform
				simpleEnemyBullet1.speed = simpleEnemyBullet1.speed*2;
				simpleEnemyBullet2.speed = simpleEnemyBullet2.speed*2;

				//Increment the secondary shooter time
				//This will increment every 100 updates
				secondShoot++;

				//For every two main bullets, a third will shoot at the player
				if(secondShoot % 2 == 0)
				{
					//Spawn the bullet
					GameObject bullet3 = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

					//Store the direction of the player in respect to the bullet
					Vector3 direction = player.transform.position-bullet3.transform.position;
					
					//Rotate the bullet towards the player
					bullet3.transform.rotation = Quaternion.LookRotation(direction);
					
					//Rotate the bullet along the y so that it faces the player
					bullet3.transform.Rotate(0,90,0);

					//Cast to a bullet type
					SimpleEnemyBullet simpleEnemyBullet3 = (SimpleEnemyBullet)bullet3.GetComponent(typeof(SimpleEnemyBullet));
					
					//Set the damage of the bullet
					simpleEnemyBullet3.setDamage(bulletDamage);
				}

				//Flag the enemy has fired
				ready = false;

			}
	
			//If the health of the boss reaches a certain point
			if(health <= startingHealth/2)
			{
				//Change to the next phase
				phase = 3;

				//Store this position
				lastPos = transform.position;

				//Reset the shootcount
				secondShoot = 0;
			}
		}

		//Phase three is missile firing mode
		if(phase == 3)
		{

			//Store the location of the top/bottom left/right corners of the screen
			Vector3 topLeft = new Vector3(boundaries.getRight()/4-boundaries.getRight (),boundaries.getTop()*3/4, 0);
			Vector3 topRight = new Vector3(boundaries.getRight()*3/4,boundaries.getTop()*3/4,0);
			Vector3 botLeft = new Vector3(boundaries.getRight()/4-boundaries.getRight(),boundaries.getTop()/4-boundaries.getTop(),0);
			Vector3 botRight = new Vector3(boundaries.getRight()*3/4,boundaries.getTop()/4-boundaries.getTop(),0);

			//Determine what move phase the enemy is in
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
			}

			//Move the boss to a certain position depending on its move phase
			if(movePhase == 0)
			{
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

			//If ready to shoot
			if(ready)
			{
				//Increment the secondary shooter time
				//This will increment every 100 updates
				secondShoot++;

				//For every three main bullets, a missile will shoot at the player
				if(secondShoot % 3 == 0)
				{
					//Spawn the missle and store it
					GameObject missile = (GameObject)Instantiate(missilePrefab,transform.position,Quaternion.identity);
				
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
				}

				//Flag that the enemy has shot
				ready = false;
			}

			//If the health of the boss reaches a certain point
			if(health <= startingHealth/4)
			{
				//Change to the next phase
				phase = 4;

				//Reset the shootcount
				secondShoot = 0;
			}
		}

		//Phase 4 spawns ambushers
		if(phase == 4)
		{
			//The new position of the enemy after moving
			Vector3 newPos = new Vector3 (boundaries.getRight(), transform.position.y + moveSpeed, transform.position.z);

			//Bounce between the top and bottom of the screen
			if(newPos.y >= boundaries.getTop() || newPos.y <= boundaries.getBottom())
				moveSpeed = -moveSpeed;

			//Make the move
			transform.position = Vector3.MoveTowards(transform.position, newPos, speed);

			//If ready to shoot
			if(ready)
			{
				//Increment the secondary shooter time
				//This will increment every 100 updates
				secondShoot++;

				//For every five main bullets, an ambusher will spawn
				if(secondShoot % 5 == 0)
				{
					Instantiate(ambusherPrefab,transform.position,Quaternion.identity);
				}

				//Flag that the boss has fired
				ready = false;
			}
		}


		//If the enemy is "reloading", don't decrement the timer
		if (!ready) {
			
			//Decrements the shoot timer
			shootTimer--;
			
			//If the shoot timer has reached 0, reset it and flag that the enemy can shoot
			if (shootTimer <= 0) 
			{	
				//Set ready to fire
				ready = true;

				//Reset the time
				shootTimer = reloadTime;
			}
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
		//Ignore collisions while in phase 0
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
			//The boss has died
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
