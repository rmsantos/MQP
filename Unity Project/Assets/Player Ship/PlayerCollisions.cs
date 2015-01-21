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

	//ScoreHandler object to track players score and money
	ScoreHandler score;

	void Start () {

		health = 100;

		//Search for the ScoreHandler object for tracking score
		score = GameObject.FindGameObjectWithTag ("ScoreHandler").GetComponent<ScoreHandler>();
	}
	
	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "EnemyBullets")
		{
			//Destroy the enemy bullet
			Destroy(col.gameObject);
			health -= 5;

			print (health);
			
		}

		if(col.gameObject.tag == "Enemies" || col.gameObject.tag == "Asteroids")
		{
			//Destroy the enemy
			Destroy(col.gameObject);
			health -= 10;
			
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

}
