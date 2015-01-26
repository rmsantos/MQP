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
using System.Collections;

public class PlayerCollisions : MonoBehaviour {

	public int health;
	public int moneyValue;
	public int scoreFromMoney;

	//ScoreHandler object to track players score
	public GameObject scoreObject;
	static ScoreHandler score;

	void Start () {

		health = 100;

		//Search for the ScoreHandler object for tracking score
		score = (ScoreHandler)scoreObject.GetComponent("ScoreHandler");
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : OnCollisionEnter2D (Collision2D col)
	 *
	 * Description : Deals with triggers between the player and other objects
	 *
	 * Parameters  : Collision2D other : The other object triggered with
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

			print (health);
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if(col.gameObject.tag == "EnemyMissile")
		{
			//Find the class of this missile
			SeekerMissile missile = (SeekerMissile)col.gameObject.GetComponent(typeof(SeekerMissile));

			//Subtract the health based on that bullet
			health -= missile.getBulletDamage();

			//Explode the missile
			missile.explode();

			print (health);

		}

		if(col.gameObject.tag == "EnemyBullets")
		{
			//Destroy the enemy bullet
			Destroy(col.gameObject);

			//Find the class of this collision
			SimpleEnemyBullet bullet = (SimpleEnemyBullet)col.gameObject.GetComponent(typeof(SimpleEnemyBullet));

			//Subtract the health based on that bullet
			health -= bullet.getBulletDamage();

			print (health);
			
		}

		if(col.gameObject.tag == "Enemies")
		{
			//Destroy the enemy
			Destroy(col.gameObject);

			//Find the abstract class of this collision
			BasicEnemy enemy = (BasicEnemy)col.gameObject.GetComponent(typeof(BasicEnemy));

			//Subtract the health based on that enemy
			health -= enemy.getCollisionDamage();
			
			print (health);
			
		}

		if(col.gameObject.tag == "Asteroids")
		{
			//Find the abstract class of this collision
			BasicAsteroid asteroid = (BasicAsteroid)col.gameObject.GetComponent(typeof(BasicAsteroid));

			//Subtract the health based on that asteroid
			health -= asteroid.getCollisionDamage();

			//Shatter the asteroid into smaller asteroids or money
			asteroid.shatter();
			
			print (health);
		}

		if(col.gameObject.tag == "Money")
		{
			//Destroy the money
			Destroy(col.gameObject);

			//Update the players score
			score.UpdateScore(scoreFromMoney);
			score.UpdateMoney(moneyValue);
			
		}

		if (health <= 0) {
			Destroy(this.gameObject);
		}
	}


	public void takeDamage(int damage)
	{
		health -= damage;
	}
}
