/* Module      : AbstractEnemy.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the BasicEnemy
 *
 * Date        : 2015/1/21
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

public abstract class AbstractEnemy : MonoBehaviour {

	//Stores the boundaries of the game
	protected Boundaries boundaries;

	//Player script
	protected GameObject player;

	//The audiohandler
	protected AudioHandler audioHandler;

	//Randomizer script
	protected Randomizer random;

	//ScoreHandler object to track players score
	protected static ScoreHandler score;

	//Portrait controller
	protected PortraitController portraitController;

	//The speed at which the enemy will move
	public float speed;

	//Value of destroying this enemy
	public int value;

	//The health of this enemy
	public int health;

	//The health per level
	public float healthPerLevel;
	
	//Stores the damage colliding with the player does
	public int collisionDamage;

	//The damage per level
	public float damagePerLevel;
	
	//Quitting boolean
	protected bool isQuitting;

	//Money drop rate 
	public int moneyDropRate;

	//Used as a reference for rotating back to normal
	protected Quaternion originalRotationValue;
	
	//Rotates back to normal at this speed
	public float rotationResetSpeed;
	

	public void enemySetup()
	{
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();

		//Search for player
		player = GameObject.FindGameObjectWithTag ("Player");

		//Search for the audioHandler
		audioHandler = GameObject.FindGameObjectWithTag ("AudioHandler").GetComponent<AudioHandler> ();

		//Get the randomizer script
		random = GameObject.FindGameObjectWithTag ("Randomizer").GetComponent<Randomizer>();

		//Search for the ScoreHandler object for tracking score
		score = GameObject.FindGameObjectWithTag("ScoreHandler").GetComponent<ScoreHandler>();

		//Get the portrait controller to play audio clips
		portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();
	}


}
