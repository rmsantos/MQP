/* Module      : MainMenuInstance.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file creates asteroids and things for the main menu
 *
 * Date        : 2015/3/5
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Linq;
using System.Collections;

public class MainMenuInstance : MonoBehaviour {

	//This is an array (assigned in Unity) that holds an ordered list of all the enemies that are to be spawned.
	public GameObject[] enemiesToSpawn;

	//The current time of the spawnInterval
	protected int timer;

	//Stores the boundaries of the game
	protected Boundaries boundaries;
	
	//The right, top, and bottom boundaries of the map
	protected float top;
	protected float bottom;
	protected float right;
	
	//Randomizer script
	public GameObject randomizer;
	protected Randomizer random;

	/*
	 * Function that initializes the instance
	 */
	protected void Start() {

		//Get the scripts from useful components
		boundaries = Camera.main.GetComponent<Boundaries>();
		random = (Randomizer)randomizer.GetComponent("Randomizer");

		//Set a bunch of variables to 0 or their default values
		timer = 0;
		top = boundaries.getTop();
		bottom = boundaries.getBottom();
		right = boundaries.getRight();

	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Updates the timer and initializes a spawn if necessary.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	
	protected void FixedUpdate() {
		
		//Increment the timer at each pass
		timer++;

		//If the timer is greater than the allotted waittime
		if (timer >= 300) {
			SpawnEnemy(random.GetRandom(enemiesToSpawn.Length));
			timer = 0;
		}
	}

	protected float GetEnemyLocation() {

		return (top - bottom) * Random.Range(-.35f, .35f);

	}

	//Uses some logic to spawn enemies
	protected void SpawnEnemy (int enemyNumber) {

		float location = GetEnemyLocation();
		Instantiate(enemiesToSpawn[enemyNumber], new Vector3(right * 1.2f, location, 0f), Quaternion.identity);
		
	}

}
