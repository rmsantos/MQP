/*Module      : HeavyWave.cs
* Author      : Joshua Morse
* Email       : jbmorse@wpi.edu
* Course      : IMGD MQP
*
* Description : This instancs spawns the Juggernaut Wave
*
* Date        : 2015/1/28
*
*
* (c) Copyright 2015, Worcester Polytechnic Institute.
*/

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using System.Collections;

public class HeavyWave : MonoBehaviour {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//Enemies possible to spawn
	string[] enemies = new string[4] {"Asteroids/AsteroidLarge", "Interceptor/Interceptor", "Saboteur/Saboteur", "Ambusher/Ambusher"};
	
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
		finalTime = 200;
		
		//Initializes the timer to 0
		timer = 0;
		
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
		
		if (timer % 30 == 0 && timer >= 0 && timer <= 121) {
			SpawnEnemy(enemyNumber);
			enemyNumber++;
		}
		
		if (timer >= finalTime) {
			timer = -999999;
			//Destroys itself and notifies the master spawner
			levelHandler.SpawningHasStopped();
			gameObject.SetActive(false);
		}
	}

	//Uses some logic to spawn enemies
	void SpawnEnemy (int enemyNumber) {

		GameObject enemy;

		switch (enemyNumber) {

			case 0:
				string asteroid = "Enemies/" + enemies[0];
				enemy = Resources.Load<GameObject> (asteroid);
				Instantiate(enemy, new Vector3(right * 1.2f, 0f, 0), Quaternion.identity);
				break;
			case 1:
				string saboteurs = "Enemies/" + enemies[2];
				enemy = Resources.Load<GameObject> (saboteurs);
				Instantiate(enemy, new Vector3(right * 1.2f, top/2, 0), Quaternion.identity);
				Instantiate(enemy, new Vector3(right * 1.2f, -1 * top/2, 0), Quaternion.identity);
				break;
			case 2:
				string ambushers = "Enemies/" + enemies[3];
				enemy = Resources.Load<GameObject> (ambushers);
				Instantiate(enemy, new Vector3(right * 1.2f, top/2, 0), Quaternion.identity);
				Instantiate(enemy, new Vector3(right * 1.2f, -1 * top/2, 0), Quaternion.identity);
				break;
			case 3:
				string interceptors = "Enemies/" + enemies[1];
				enemy = Resources.Load<GameObject> (interceptors);
				Instantiate(enemy, new Vector3(right * 1.2f, top/2, 0), Quaternion.identity);
				Instantiate(enemy, new Vector3(right * 1.2f, -1 * top/2, 0), Quaternion.identity);
				break;

		}

	}
	
}

