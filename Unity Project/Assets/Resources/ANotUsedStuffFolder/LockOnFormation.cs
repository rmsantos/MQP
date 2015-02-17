/* Module      : BoxFormation.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This instancs spawns a box formation
 *
 * Date        : 2015/1/28
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class LockOnFormation : MonoBehaviour {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//Enemies possible to spawn
	string[] enemies = new string[3] {"Interceptor/Interceptor","DogFighterB/DogFighterB","Saboteur/Saboteur"};
	
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
		finalTime = 500;
		
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
		
		if (timer % 50 == 0 && timer >= 0) {
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
		
		//Get a random vertical location
		if(enemyNumber == 0 || enemyNumber == 7)
		{
			GameObject saboteur = Resources.Load<GameObject> ("Enemies/" + enemies[2]);

			Instantiate(saboteur, new Vector3(right * 1.2f, top/2, 0), Quaternion.identity);
			Instantiate(saboteur, new Vector3(right * 1.2f, bottom/2, 0), Quaternion.identity);

			GameObject interceptor = Resources.Load<GameObject> ("Enemies/" + enemies[0]);

			Instantiate(interceptor, new Vector3(right * 1.2f, top/4, 0), Quaternion.identity);
			Instantiate(interceptor, new Vector3(right * 1.2f, bottom/4, 0), Quaternion.identity);

			GameObject dogfighter = Resources.Load<GameObject> ("Enemies/" + enemies[1]);

			Instantiate(dogfighter, new Vector3(right * 1.2f, 0, 0), Quaternion.identity);

		}
		else if(enemyNumber == 1 || enemyNumber == 2 || enemyNumber == 3 || enemyNumber == 5 || enemyNumber == 8 || enemyNumber == 9 || enemyNumber == 10)
		{
			GameObject dogfighter = Resources.Load<GameObject> ("Enemies/" + enemies[1]);
			
			Instantiate(dogfighter, new Vector3(right * 1.2f, top/2, 0), Quaternion.identity);
			Instantiate(dogfighter, new Vector3(right * 1.2f, bottom/2, 0), Quaternion.identity);
		}


	}
	
}


