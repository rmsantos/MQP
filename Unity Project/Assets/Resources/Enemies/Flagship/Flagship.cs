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

public class Flagship :  AbstractEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The movement speed of the boss. This variable is meant to be modified
	float moveSpeed;

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

	//Player position
	Vector3 playerPosition;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes enemy variables
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Initialize base objects
		setup ();

		//Get the script that created this boss
		bossInstance = (Boss1) boss1.GetComponent("Boss1");

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

		//Get the script that controls the level
		levelHandler = (LevelHandler) levelHandlerObject.GetComponent("LevelHandler");

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

		//Phase 0 the boss moves to the center of the screen
		if(phase == 0)
		{
			//Move towards the center of the screen
			transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed);

			//When hitting the center, move to phase 1
			if(transform.position == Vector3.zero)
			{
				//Play the sound effect upon boss switching phases
				portraitController.playBossPhase();

				//Move to phase 1
				phase = 1;
			}


		}

		//Spinny bullet hell state
		if(phase == 1)
		{
			//If the boss takes enough damage, then move to the next phase
			if(health <= startingHealth*3/4)
			{
				//Play the sound effect upon boss switching phases
				portraitController.playBossPhase();

				phase = 2;
			}

			//If the boss is ready to shoot
			if(ready)
			{
				//Rotate the angle at which the boss will shoot (so that it spins uniformly
				rotateCount += 11;

				//Spawn 4 bullets
				GameObject bullet1 = shoot (turret);
				GameObject bullet2 = shoot (turret);
				GameObject bullet3 = shoot (turret);
				GameObject bullet4 = shoot (turret);

				//Rotate the bullets so that they form a "t" formation, then rotate them by rotateCount
				bullet1.transform.Rotate(0,0,rotateCount);
				bullet2.transform.Rotate(0,0,90+rotateCount);
				bullet3.transform.Rotate(0,0,180+rotateCount);
				bullet4.transform.Rotate(0,0,270+rotateCount);
			}

		}

		//Phase two traps the player in one horizontal half of the screen and bobs back and forth while shooting at the player
		if(phase == 2)
		{
			//The new position of the enemy after moving
			Vector3 newPos = new Vector3 (transform.position.x + moveSpeed, transform.position.y, transform.position.z);
			
			//If hitting either left or right boundaries, reverse the direction
			if(newPos.x >= boundaries.getRight() * .8f || newPos.x <= boundaries.getLeft() * .8f)
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
					GameObject bullet3 = shoot (turret);

					//Store the direction of the player in respect to the bullet
					Vector3 direction = player.transform.position-bullet3.transform.position;
					
					//Rotate the bullet towards the player
					bullet3.transform.rotation = Quaternion.LookRotation(direction);
					
					//Rotate the bullet along the y so that it faces the player
					bullet3.transform.Rotate(0,90,0);
				}

				//Flag the enemy has fired
				ready = false;

			}
	
			//If the health of the boss reaches a certain point
			if(health <= startingHealth/2)
			{
				//Play the sound effect upon boss switching phases
				portraitController.playBossPhase();

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
			Vector3 topLeft = new Vector3(boundaries.getRight()/3-boundaries.getRight (),boundaries.getTop()*2/3, 0);
			Vector3 topRight = new Vector3(boundaries.getRight()*2/3,boundaries.getTop()*2/3,0);
			Vector3 botLeft = new Vector3(boundaries.getRight()/3-boundaries.getRight(),boundaries.getTop()/3-boundaries.getTop(),0);
			Vector3 botRight = new Vector3(boundaries.getRight()*2/3,boundaries.getTop()/3-boundaries.getTop(),0);

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
				//Play the sound effect upon boss switching phases
				portraitController.playBossPhase();

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
			Vector3 newPos = new Vector3 (boundaries.getRight() * .8f, transform.position.y + moveSpeed, transform.position.z);

			//Bounce between the top and bottom of the screen
			if(newPos.y >= boundaries.getTop() * .8f || newPos.y <= boundaries.getBottom() * .8f)
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

		//Reload the weapon
		reload ();

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
		//Ignore collisions while in phase 0
		if(col.gameObject.tag == "PlayerBullet" && !startingPhase())
		{
			//Destroy the player bullet and this object
			Destroy(col.gameObject);

			//Get the damage the player bullet will deal
			int damage = col.gameObject.GetComponent<Laser>().getDamage();

			//Deal the damage to this enemy
			TakeDamage(damage);

			//Play the audioclip of hitting the boss with a laser
			portraitController.playLaserBoss();
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : startingPhase()
	 *
	 * Description : Returns true if the boss is in its starting phase
	 *
	 * Parameters  : None
	 *
	 * Returns     : bool : True if the boss is in starting phase. False otherwise.
	 */
	public bool startingPhase()
	{
		return phase == 0;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : TakeDamage(float damage)
	 *
	 * Description : Deals damage to the enemies health
	 *
	 * Parameters  : int damage : The damage to be dealt
	 *
	 * Returns     : Void
	 */
	public void TakeDamage(int damage)
	{
		//Subtract health from the enemy
		health -= damage;

		//Show the hit fader effect
		GetComponent<HitFader>().BeenHit();

		//If health hits 0, then the enemy dies
		if(health <= 0)
		{
			health = 0;

			//Update the health
			levelHandler.UpdateBossHealth (health);

			//Play the sound effect
			audioHandler.playBossExplosion();

			//The boss has died
			bossInstance.BossDied();

			//Destroy the enemy
			Destroy(this.gameObject);

			//Update the players score
			score.UpdateScore(value);
		}
		


	}

}
