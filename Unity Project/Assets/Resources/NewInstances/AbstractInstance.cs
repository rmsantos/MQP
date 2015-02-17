/* Module      : ExampleInstance.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file holds all the variables and methods that all instances share.
 *
 * Date        : 2015/2/17
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Linq;
using System.Collections;

public abstract class AbstractInstance : MonoBehaviour {

	//This is the number of enemies that should be spawned for wave [i] from the enemiesToSpawn array.
	//The total of all elements in this array must be equal to the number of elements in enemiesToSpawn
	public int[] spawnGroups;

	//This is the wait time that the wave should take until the next spawnGroup.
	//First spawnInterval is before the first spawnGroup. This array should be 1 longer than the spawnGroups array.
	public int[] spawnIntervals;

	//This is an array (assigned in Unity) that holds an ordered list of all the enemies that are to be spawned.
	//They can be grouped with spawnGroups and they can have delays between spawning with spawnIntervals.
	public GameObject[] enemiesToSpawn;

	//This is an array of locations (direct 1:1 relationship with enemiesToSpawn) that dictates where the enemies 
	//will be spawning on the map. This should be a value between 0 to 1. If this value is any value besides those between 
	//0 to 1, it will spawn the enemy in a completely random location
	//Try not to place the enemies between 0 and .1, and .9 and 1.
	public float[] spawnLocation;

	//The global levelHandler gameObject
	public GameObject levelHandlerObject;
	protected LevelHandler levelHandler;
	
	//The current time of the spawnInterval
	protected int timer;

	//The next spawn group
	protected int nextSpawnGroup;

	//The next spawned enemy
	protected int nextEnemy;
	
	//Stores the boundaries of the game
	protected Boundaries boundaries;
	
	//The right, top, and bottom boundaries of the map
	protected float top;
	protected float bottom;
	protected float right;
	
	//Keeps track of which enemy has spawned
	protected int enemyNumber;
	
	//Randomizer script
	public GameObject randomizer;
	protected Randomizer random;

	/*
	 * Function that initializes the instance
	 */
	protected void Initialize() {

		//Get the scripts from useful components
		boundaries = Camera.main.GetComponent<Boundaries>();
		levelHandler = (LevelHandler)levelHandlerObject.GetComponent ("LevelHandler");
		random = (Randomizer)randomizer.GetComponent("Randomizer");

		//Set a bunch of variables to 0 or their default values
		timer = 0;
		nextSpawnGroup = 0;
		nextEnemy = 0;
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
	
	protected void DefaultUpdate() {
		
		//Increment the timer at each pass
		timer++;

		//If the timer is greater than the allotted waittime
		if (timer >= spawnIntervals[nextSpawnGroup]) {
			//If the last wave has already spawned, end this cycle
			if (nextSpawnGroup >= spawnGroups.Length) {
				levelHandler.SpawningHasStopped();
				gameObject.SetActive(false);
			}
			else {
				//Spawn as many enemies that are in this group
				for (int i = 0; i < spawnGroups[nextSpawnGroup]; i++) {
					SpawnEnemy(nextEnemy);
					nextEnemy++;
				}
				nextSpawnGroup++;
				timer = 0;
			}
		}
	}

	protected float GetEnemyLocation(int enemyNumber) {

		//If the given value signifies a random location, get one
		if (spawnLocation[enemyNumber] > 1f || spawnLocation[enemyNumber] < 0f) {
			return (top - bottom) * Random.Range(-.35f, .35f);
		}
		//A non-random location, based on the given value
		else {
			return (top - bottom) * (spawnLocation[enemyNumber] - .5f);
		}

	}

	//Uses some logic to spawn enemies
	protected void SpawnEnemy (int enemyNumber) {

		float location = GetEnemyLocation(enemyNumber);
		Instantiate(enemiesToSpawn[enemyNumber], new Vector3(right * 1.2f, location, 0f), Quaternion.identity);
		
	}

	//Used to retreive the length of the wave
	//Should be useful for radar type things
	public int GetWaveLength() {

		return spawnIntervals.Sum();

	}
	
}
