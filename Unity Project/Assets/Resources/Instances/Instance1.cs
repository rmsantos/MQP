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

	//Enemies possible to spawn
	string[] enemies = new string[4] {"DogFighterB/DogFighterB", "DogFighterA/DogFighterA", "Asteroids/AsteroidSmall", "Seeker/Seeker"};
	
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

		//Increment the timer at each pass
		timer++;

		if (timer % 100 == 0 && timer >= 0) {
			SpawnEnemy();
		}

		if (timer >= finalTime) {
			timer = -999999;
			//Destroys itself and notifies the master spawner
			levelHandler.SpawningHasStopped();
			gameObject.SetActive(false);
		}
	}

	float GetRandomLocation() {

		return (top - bottom) / (100 / (float) (random.GetRandom(100)+1)) - ((top - bottom) / 2);
	}

	//Uses some logic to spawn enemies
	void SpawnEnemy () {

		//Get a random vertical location

		float randomLocation = GetRandomLocation();

		string randomEnemy = "Enemies/" + enemies[random.GetRandom(enemies.GetLength(0))];
		GameObject enemy = Resources.Load<GameObject> (randomEnemy);
		Instantiate(enemy, new Vector3(right * 1.2f, randomLocation, -.1f), Quaternion.identity);

	}
	
}


