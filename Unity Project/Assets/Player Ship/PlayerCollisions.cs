/* Module      : PlayerCollisions.cs
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
	int moneyValue;
	public int scoreFromMoney;

	//ScoreHandler object to track players score
	public GameObject scoreObject;
	static ScoreHandler score;

	public Slider healthBar;

	//Get the portrait controller to play audio clips
	PortraitController portraitController;

	//Threshold for the money audioclip
	public int moneyThreshold;

	//Quitting boolean
	bool isQuitting;

	void Start () {
		
		//Pull the values from player prefs
		health = PlayerPrefs.GetInt ("Health", 1);
		moneyValue = PlayerPrefs.GetInt ("MoneyValue", 99999);

		//Search for the ScoreHandler object for tracking score
		score = (ScoreHandler)scoreObject.GetComponent("ScoreHandler");

		//Find the portrait controller script
		portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();

		//Not quitting the application
		isQuitting = false;

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
			health -= shield.getCollisionDamage();

			//Disable and make the shield invisible
			//It will be deleted along with the juggernaut later
			other.enabled = false;
			other.renderer.enabled = false;
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

			//Subtract the health based on that bullet
			takeDamage(bullet.getBulletDamage());
			
		}

		if(col.gameObject.tag == "Enemies")
		{
			//Destroy the enemy
			Destroy(col.gameObject);

			//Find the abstract class of this collision
			BasicEnemy enemy = (BasicEnemy)col.gameObject.GetComponent(typeof(BasicEnemy));

			//Subtract the health based on that enemy
			takeDamage(enemy.getCollisionDamage());
			
		}

		if(col.gameObject.tag == "Asteroids")
		{
			//Play the sound effect upon hitting an asteroid
			portraitController.playAsteroidHit();

			//Find the abstract class of this collision
			BasicAsteroid asteroid = (BasicAsteroid)col.gameObject.GetComponent(typeof(BasicAsteroid));

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

			//Play the audioclip if the money goes above the threshold
			if(score.GetMoney() > moneyThreshold)
				portraitController.playMoneyHigh();

		}

		if(col.gameObject.tag == "Boss")
		{
			//Find the class of this collision
			Flagship boss = (Flagship)col.gameObject.GetComponent(typeof(Flagship));

			//Subtract the health based on that boss
			takeDamage(boss.getCollisionDamage());
		}

		if (health <= 0) {
			Destroy(this.gameObject);
		}

		print (health);
	}


	public void takeDamage(int damage)
	{
		Shields shield = GetComponent<Shields> ();

		//Deduct damage from health
		if(shield.getShields() != 0)
		{
			shield.weakenShields();
		}
		else
			health -= damage;

		//And display on the health bar
		healthBar.value = health;
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
}
