/* Module      : Boss1.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This is the first created instance for the game
 *
 * Date        : 2015/1/23
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Boss1 : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Enemies possible to spawn
	string boss = "Flagship/Flagship";
	
	//The spawner object
	public GameObject enemySpawner;
	LevelHandler levelHandler;
	
	//The current time of the instance
	int timer;
	
	//The final time of the instance when it self destructs
	int finalTime;

	//Stores the boundaries of the game
	Boundaries boundaries;

	//The right, top, and bottom boundaries of the map
	float top;
	float bottom;
	float right;

	//Boss checker variables
	bool spawned;
	static bool killed;
	int bossTimer;

	//Randomizer script
	public GameObject randomizer;
	Randomizer random;

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
		finalTime = 1000;
		spawned = false;
		killed = false;
		bossTimer = 0;

		//Get the script that created this instance
		levelHandler = (LevelHandler) enemySpawner.GetComponent("LevelHandler");

		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();

		//Get the top, bottom, and right boundaries
		top = boundaries.getTop();
		bottom = boundaries.getBottom();
		right = boundaries.getRight();

		//Get the randomizer script
		random = (Randomizer)randomizer.GetComponent("Randomizer");
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

	void FixedUpdate () {

		//TODO Notify the player via the character audio handler and probably some UI elements that a boss is coming

		if (!spawned) {
			//Increment the timer at each pass
			timer++;

			if (timer % 100 == 0 && timer >= 0) {
				SpawnEnemy();
			}
		}
		else {
			if (!killed) {
				bossTimer++;
				//TODO probably have the timer here that will kill the player or something
			}
			else {
				levelHandler.LevelComplete();
				gameObject.SetActive(false);
			}
		}
	}
	
	void SpawnEnemy () {

		string bossPath = "Enemies/" + boss;
		GameObject enemy = Resources.Load<GameObject> (bossPath);
		Instantiate(enemy, new Vector3(right * 1.2f, 0f, 0f), Quaternion.identity);

		spawned = true;

	}

	public void BossDied() {
		killed = true;
	}

}


