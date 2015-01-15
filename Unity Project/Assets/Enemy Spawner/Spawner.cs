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

	//Variable that is used to reset the spawn timer
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
	
	}

}


