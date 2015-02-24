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

public class FlagshipB :  AbstractEnemy {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The movement speed of the boss. This variable is meant to be modified
	float moveSpeed;
	
	//The Boss Instance Object
	public GameObject boss2;
	Boss2 bossInstance;
	
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
	
	//the shield
	public GameObject shield;

	//Direction to rotate
	int sign;
	
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
		bossInstance = (Boss2) boss2.GetComponent("Boss2");
		
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

		//Rotate up to start
		sign = 1;
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
		
		//Phase 0 the boss moves to the center right of the screen
		if(phase == 0)
		{
			//Move to the center right
			Vector3 newPos = new Vector3(boundaries.getRight() * 0.8f, 0, 0);

			//Move towards the right center of the screen
			transform.position = Vector3.MoveTowards(transform.position, newPos, speed);
			
			//When hitting the right center, move to phase 1
			if(transform.position == newPos)
			{
				//Play the sound effect upon boss switching phases
				portraitController.playBossPhase();
				
				//Move to phase 1
				phase = 1;

				//Disable shields
				shield.SetActive(false);
			}
			
			
		}
		
		//Shoot diagonal bullets and one down the center
		if(phase == 1)
		{
			if(ready)
			{
				//chose a random range at the direction
				rotateCount += random.GetRandomInRange(4,7) * sign;

				//Spawn 3 bullets
				GameObject bullet1 = shoot (turret);
				GameObject bullet2 = shoot (turret);
				GameObject bullet3 = shoot (turret);
				
				//Rotate the bullets so that they form a "x" formation
				bullet1.transform.Rotate(0,0,45-rotateCount);
				bullet2.transform.Rotate(0,0,-45+rotateCount);

				//If hitting an end, then reverse
				if(rotateCount >= 40 || rotateCount <= 0)
					sign = -sign;
			}

			//If the boss takes enough damage, then move to the next phase
			if(health <= startingHealth*3/4)
			{
				//Play the sound effect upon boss switching phases
				portraitController.playBossPhase();
	
				//Switch to phase 2
				phase = 2;
			}
			
		}

		//Phase two the boss will bob back and forth shoot bullets and missiles
		if(phase == 2)
		{
			//The new position of the boss after moving
			Vector3 newPos = new Vector3 (boundaries.getRight() * .8f, transform.position.y + moveSpeed, transform.position.z);
			
			//Bounce between the top and bottom of the screen
			if(newPos.y >= boundaries.getTop() * .8f || newPos.y <= boundaries.getBottom() * .8f)
				moveSpeed = -moveSpeed;
			
			//Make the move
			transform.position = Vector3.MoveTowards(transform.position, newPos, speed);

			//If ready to shoot
			if(ready)
			{
				//Spawn 2 bullets
				GameObject bullet1 = shoot (turret);
				GameObject bullet2 = shoot (turret);
				
				//Rotate the bullets so that they form a "x" formation
				bullet1.transform.Rotate(0,0,30);
				bullet2.transform.Rotate(0,0,-30);

				//Increment the secondary shooter time
				//This will increment every 100 updates
				secondShoot++;
				
				//For every five main bullets, shoot a missile
				if(secondShoot % 5 == 0)
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

			}
			
			//If the health of the boss reaches a certain point
			if(health <= startingHealth/2)
			{
				//Play the sound effect upon boss switching phases
				portraitController.playBossPhase();
				
				//Change to the next phase
				phase = 3;
				
				//Reset the shootcount
				secondShoot = 0;
			}
		}
		
		//Phase 3 the boss will charge the player
		if(phase == 3)
		{
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

			//Move towards the player
			transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed);

			//If ready to shoot
			if(ready)
			{
				//Shoot a player towards the player
				GameObject bullet1 = shoot (turret);

				//Store the direction of the player in respect to the bullet
				Vector3 direction = playerPosition-bullet1.transform.position;
					
				//Rotate the bullet towards the player
				bullet1.transform.rotation = Quaternion.LookRotation(direction);

				//Rotate the bullet so it looks correct
				bullet1.transform.Rotate (0,90,0);

			}

			//If the health of the boss reaches a certain point
			if(health <= startingHealth/4)
			{
				//Play the sound effect upon boss switching phases
				portraitController.playBossPhase();
				
				//Change to the next phase
				phase = 4;

				//Set the shield active
				shield.SetActive(true);

				//Reset the shootcount
				secondShoot = 0;
			}
		}

		//Phase 4 the boss moves to the center of the screen
		if(phase == 4)
		{
			//Move towards the center of the screen
			transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed);

			//If reaching the center
			if(transform.position == Vector3.zero)
			{
				//Switch phases
				phase = 5;

				//Deactivate the shield again
				shield.SetActive(false);
			}
		}

		//In the final phase, the boss thoots a bullet in each direction every so often
		if(phase == 5)
		{
			//If the boss is ready to shoot
			if(ready)
			{
				//Increment the secondary shooter time
				//This will increment every 100 updates
				secondShoot++;

				ready = false;

				//For every five main bullets, shoot a missile
				if(secondShoot % 3 == 0)
				{
					//Rotate the angle at which the boss will shoot (so that it spins uniformly
					rotateCount += 11;
					
					//Spawn 12 bullets
					GameObject bullet1 = shoot (turret);
					GameObject bullet2 = shoot (turret);
					GameObject bullet3 = shoot (turret);
					GameObject bullet4 = shoot (turret);

					GameObject bullet5 = shoot (turret);
					GameObject bullet6 = shoot (turret);
					GameObject bullet7 = shoot (turret);
					GameObject bullet8 = shoot (turret);

					GameObject bullet9 = shoot (turret);
					GameObject bullet10 = shoot (turret);
					GameObject bullet11 = shoot (turret);
					GameObject bullet12 = shoot (turret);
					
					//Rotate the bullets so that they form a circle formation
					bullet1.transform.Rotate(0,0,rotateCount);
					bullet2.transform.Rotate(0,0,rotateCount+30);
					bullet3.transform.Rotate(0,0,rotateCount-30);
					bullet4.transform.Rotate(0,0,rotateCount+90);
					bullet5.transform.Rotate(0,0,rotateCount+120);
					bullet6.transform.Rotate(0,0,rotateCount+60);
					bullet7.transform.Rotate(0,0,rotateCount-90);
					bullet8.transform.Rotate(0,0,rotateCount-120);
					bullet9.transform.Rotate(0,0,rotateCount-60);
					bullet10.transform.Rotate(0,0,rotateCount+180);
					bullet11.transform.Rotate(0,0,rotateCount+210);
					bullet12.transform.Rotate(0,0,rotateCount-150);
				}
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
		if(col.gameObject.tag == "PlayerBullet" && !shield.activeSelf)
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
			
			//Create the explosion at this location
			Instantiate(Resources.Load<GameObject>("Explosions/BigExplosion"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);	
			
			//Update the players score
			score.UpdateScore(value);
		}
		
		
	}
	
}
