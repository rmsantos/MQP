/* Module      : Instance1.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This is the first created instance for the game
 *
 * Date        : 2015/1/16
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Instance1 : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The spawner object
	public GameObject enemySpawner;
	Spawner spawner;
	
	//The current time of the instance
	int timer;
	
	//The final time of the instance when it self destructs
	public int finalTime;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the Instance
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */

	void Start () {
		timer = 0;
		spawner = (Spawner) enemySpawner.GetComponent("Spawner");
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Updates the timer and initializes a spawn if necessary.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */

	void Update () {

		timer++;

		if (timer >= finalTime) {
			timer = -999999;
			//Destroys itself and notifies the master spawner
			spawner.SpawningHasStopped();
			//Destroy(this.gameObject);
		}
	}


}


