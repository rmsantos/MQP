/* Module      : Spawner.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file directs the spawning of all enemy units.
 *
 * Date        : 2015/1/15
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//NONE


public class Spawner : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Instance list. These will be stored here as references to other prefabs. This should be updated to reflect new instances. 
	//The instances should follow a particular naming pattern.

	string[] instances = new string[1] {"Instance4"};
	
	//Randomizer script
	public GameObject randomizer;
	Randomizer random;

	//Variable to store the time until next spawn
	static float spawnTimer;

	//Flag to say if spawning is occurring
	static bool spawning;

	//Variable, changed in Unity prefab, that is used to reset the spawn timer to a set amount
	public static float timeBetweenSpawning;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : initializes the spawn timer
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */

	void Start () {
	
		//Get the randomizer script
		random = (Randomizer)randomizer.GetComponent("Randomizer");

		//Initialize spawning variables
		timeBetweenSpawning = 300;
		spawnTimer = timeBetweenSpawning;
		spawning = false;

	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Updates the timer and initializes a spawning instance if necessary.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */

	void Update () {

		//If spawning is occurring, don't decrement the timer
		if (!spawning) {

			//Decrements the spawn timer
			spawnTimer--;
			
			//If the spawn timer has reached 0, initialize an instance of spawning
			if (spawnTimer <= 0) {
				
				spawning = true;
				spawnTimer = timeBetweenSpawning;

				string randomInstance = "Instances/" + instances[random.GetRandom(instances.GetLength(0))];
				
				Instantiate(Resources.Load<GameObject>(randomInstance));
				
			}
		}
	}

	public void SpawningHasStopped () {

		spawning = false;

	}

	public void SetSpawnTimer (int time) {

		spawnTimer = time;

	}

}


