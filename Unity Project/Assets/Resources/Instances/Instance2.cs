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

public class Instance2 : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Enemies possible to spawn
	string[] enemies = new string[1] {"DogfighterA/DogfighterA"};
	
	//The spawner object
	public GameObject enemySpawner;
	Spawner spawner;
	
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

	//Keeps track of which enemy has spawned
	int enemyNumber;

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

		enemyNumber = 0;
		finalTime = 250;

		//Initializes the timer to 0
		timer = 0;

		//Get the script that created this instance
		spawner = (Spawner) enemySpawner.GetComponent("Spawner");

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
	/* Function    : Update()
	 *
	 * Description : Updates the timer and initializes a spawn if necessary.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */

	void Update () {

		//Increment the timer at each pass
		timer++;

		if (timer % 50 == 0 && timer >= 0) {
			SpawnEnemy(enemyNumber);
			enemyNumber++;
		}

		if (timer >= finalTime) {
			timer = -999999;
			//Destroys itself and notifies the master spawner
			spawner.SpawningHasStopped();
			gameObject.SetActive(false);
		}
	}

	float GetEnemyLocation(int enemyNumber) {

		return (top - bottom) * ((float) enemyNumber / 10f);

	}

	//Uses some logic to spawn enemies
	void SpawnEnemy (int enemyNumber) {

		//Get a random vertical location
		float enemyLocation = GetEnemyLocation(enemyNumber);

		string randomEnemy = "Enemies/" + enemies[random.GetRandom(enemies.GetLength(0))];
		GameObject enemy = Resources.Load<GameObject> (randomEnemy);
		Instantiate(enemy, new Vector3(right * 1.2f, enemyLocation + .5f, -.1f), Quaternion.identity);
		Instantiate(enemy, new Vector3(right * 1.2f, 0 - enemyLocation - .5f, -.1f), Quaternion.identity);

	}
	
}


