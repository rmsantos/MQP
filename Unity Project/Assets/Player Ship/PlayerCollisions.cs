﻿/* Module      : PlayerCollisions.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the health and the collisions for the player
 *
 * Date        : 2015/1/21
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCollisions : MonoBehaviour {

	int health;
	public int moneyValue;
	int crystalValue;

	public int scoreFromMoney;
	public int scoreFromCrystals;

	//The spawner object
	public GameObject spawner;
	LevelHandler levelHandler;

	//ScoreHandler object to track players score
	public GameObject scoreObject;
	static ScoreHandler score;

	//Image object for the health bar
	public Image healthImage;

	//Get the portrait controller to play audio clips
	PortraitController portraitController;

	//Threshold for the crystals audioclip
	public int crystalThreshold;

	//Quitting boolean
	bool isQuitting;

	//The audiohandler
	GameObject audioHandlerObject;
	AudioHandler audioHandler;

	//The image for repair power levels
	public Image repairImage;

	//The current repair power level
	int repairPower;

	//The text for the repair button
	public Text repairText;
	
	void Start () {
		
		//Pull the values from player prefs
		health = PlayerPrefs.GetInt ("Health", 1);
		crystalValue = PlayerPrefs.GetInt ("CrystalValue", 99999);
		repairPower = PlayerPrefs.GetInt ("RepairPower", 0);

		//Search for the audioHandler
		audioHandlerObject = (GameObject)GameObject.FindGameObjectWithTag ("AudioHandler");
		audioHandler = audioHandlerObject.GetComponent<AudioHandler> ();

		//Search for the ScoreHandler object for tracking score
		score = (ScoreHandler)scoreObject.GetComponent("ScoreHandler");

		//Find the portrait controller script
		portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();

		//Not quitting the application
		isQuitting = false;

		//Get the levelHandler
		levelHandler = (LevelHandler) spawner.GetComponent("LevelHandler");

		//Display the players health
		displayHealth ();

		//Load the correct image sprite
		repairImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/Repairing/" + repairPower);

	}

	void FixedUpdate()
	{
		//If player ded
		if (health <= 0) {
			//Play the explosion sound effect
			audioHandler.playPlayerExplosion();
			levelHandler.PlayerDied();
			Destroy(this.gameObject);
		}
	}


	/* ----------------------------------------------------------------------- */
	/* Function    : OnTriggerEnter2D (Collider2D other)
	 *
	 * Description : Deals with triggers between the player and other objects
	 *
	 * Parameters  : Collider2D other : The other object triggered with
	 *
	 * Returns     : Void
	 */
	void OnTriggerEnter2D(Collider2D other) 
	{
		//If the player triggers the shield
		if(other.gameObject.tag == "EnemyShield")
		{
			//The only enemy with a shield is a juggernaut
			//Cast to that class
			JuggernautShield shield = (JuggernautShield)other.gameObject.GetComponent(typeof(JuggernautShield));

			//Subtract the appropriate damage
			takeDamage(shield.getCollisionDamage());

			//Disable and make the shield invisible
			//It will be deleted along with the juggernaut later
			other.enabled = false;
			other.renderer.enabled = false;
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : displayHealth()
	 *
	 * Description : Displays the players health on screen
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayHealth() 
	{
		//Use an algorithm to map the images to the players health
		int imageValue = health;
		int healthMax = 100;
		int healthMin = 1;
		int imageMax = 23;
		int imageMin = 1;

		//Figure out which images go with each health value
		int imageNumber = healthMin + (imageValue-healthMin)*(imageMax-imageMin)/(healthMax-healthMin);

		//Only show 0 if the player has no health left
		if(health <= 0)
			imageNumber = 0;

		//Load the appropriate sprite
		healthImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/Health/" + imageNumber);
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
		if(col.gameObject.tag == "EnemyMissile")
		{
			//Play the sound effect upon hitting an enemy missile
			portraitController.playBulletHit();

			//Find the class of this missile
			SeekerMissile missile = (SeekerMissile)col.gameObject.GetComponent(typeof(SeekerMissile));

			//Explode the missile
			missile.explode();

		}

		if(col.gameObject.tag == "EnemyBullets")
		{
			//Play the sound effect upon hitting an enemy bullet
			portraitController.playBulletHit();

			//Destroy the enemy bullet
			Destroy(col.gameObject);

			//Find the class of this collision
			SimpleEnemyBullet bullet = (SimpleEnemyBullet)col.gameObject.GetComponent(typeof(SimpleEnemyBullet));

			//If the player has hardened shields, ignore dogfighter and cruiser bullets
			if(PlayerPrefs.GetInt("ShieldUpgradeHardened",0) == 1)
			{
				//Find the shield
				Shields shield = GetComponent<Shields> ();

				//Only ignore damage if the player has shields
				if(shield.getShields() > 0)
				{
					if(bullet.name == "DogFighterBullet(Clone)" || bullet.name == "CruiserBullet(Clone)")
						return;
				}
			}

			//Subtract the health based on that bullet
			takeDamage(bullet.getBulletDamage());
			
		}

		if(col.gameObject.tag == "Enemies")
		{
			//Find the abstract class of this collision
			AbstractEnemy enemy = (AbstractEnemy)col.gameObject.GetComponent(typeof(AbstractEnemy));

			//Subtract the health based on that enemy
			takeDamage(enemy.getCollisionDamage());

			//Destroy this enemy
			enemy.takeDamage(99999);
			
		}

		if(col.gameObject.tag == "Asteroids")
		{
			//Play the sound effect upon hitting an asteroid
			portraitController.playAsteroidHit();

			//Find the abstract class of this collision
			AbstractAsteroid asteroid = (AbstractAsteroid)col.gameObject.GetComponent(typeof(AbstractAsteroid));

			//Subtract the health based on that asteroid
			takeDamage(asteroid.getCollisionDamage());

			//Shatter the asteroid into smaller asteroids or money
			asteroid.shatter();
		}

		if(col.gameObject.tag == "Money")
		{
			//Destroy the money
			Destroy(col.gameObject);

			//Update the players score
			score.UpdateScore(scoreFromMoney);
			score.UpdateMoney(moneyValue);

		}

		if(col.gameObject.tag == "Crystal")
		{
			//Destroy the money
			Destroy(col.gameObject);
			
			//Update the players score
			score.UpdateScore(scoreFromCrystals);
			score.UpdateCrystals(crystalValue);
			
			//Play the audioclip if the money goes above the threshold
			if(score.GetCrystals() > crystalThreshold)
				portraitController.playCrystalsHigh();
			
		}

		if(col.gameObject.tag == "Boss")
		{
			//Find the class of this collision
			Flagship boss = (Flagship)col.gameObject.GetComponent(typeof(Flagship));

			//Subtract the health based on that boss
			takeDamage(boss.getCollisionDamage());
		}

		//If the object is another mine
		if(col.gameObject.tag == "Mine")
		{
			//Cast to a mine type and explode
			Mine mine = (Mine)col.gameObject.GetComponent(typeof(Mine));
			
			//The player takes damage
			mine.explode();
		}
	}


	public void takeDamage(int damage)
	{
		Shields shield = GetComponent<Shields> ();

		//Deduct damage from health
		if(shield.getShields() != 0)
		{
			shield.weakenShields(damage);
		}
		else
		{
			//If hitting a small asteroid, return
			if(damage == 0)
				return;

			//If the player has the hull upgrade, then decrease damage taken
			damage -= PlayerPrefs.GetInt("HullUpgradeReinforced",0);

			//Don't reduce all damage
			if(damage < 1)
				damage = 1;

			//Take the damage
			health -= damage;
		}

		//Store the new health
		PlayerPrefs.SetInt ("Health", health);

		//Display the players health
		displayHealth ();
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
			
		}
		
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Poll for user input to repair the ship
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update() {

		//Only consider if the user has power to the rapair station
		if(repairPower > 0)
		{
			//If the player has max health or no crystals
			if(health == 100 || score.GetCrystals() == 0)
			{
				//Color the repair text red and exit
				repairText.color = Color.red;
				return;
			}
			else 
			{
				//Else color it white (that its available)
				repairText.color = Color.white;

				//if the user repairs
				if(Input.GetKeyDown("r"))
				{
					//Update crystals
					score.UpdateCrystals(-1);
					
					//Heal the player ship
					health += 20;

					//Dont go over 100 health
					if(health > 100)
						health = 100;

					//display the health bar
					displayHealth();
				}
			}
		}
		//Else make the text red to signify that it cant be used
		else
			repairText.color = Color.red;

	}
}
