/* Module      : Spawner.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file directs the spawning of all enemy units.
 *
 * Date        : 2015/1/15
 *
 * History:
 * Revision      Date          Changed By
 * --------      ----------    ----------
 * 01.00         2015/1/15    jbmorse
 * 
 * First release.
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Spawner : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Variable to store the time until next spawn
	float spawnTimer;

	//Flag to say if spawning is occurring
	bool spawning;

	//Variable, changed in Unity prefab, that is used to reset the spawn timer to a set amount
	public float timeBetweenSpawning;

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

				//TODO Include some logic that creates another prefab that has spawning logic on it.
				
			}
		}
	}

	void SpawningHasStopped () {

		spawning = false;

	}

	void SetSpawnTimer (int time) {

		spawnTimer = time;

	}
}


