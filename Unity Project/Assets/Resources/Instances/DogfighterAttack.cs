/* Module      : DogfighterAttack.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This instancs spawns DogfighterAs in the center area of the screen, and DogfighterB's in the top and bottom
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

public class DogfighterAttack : MonoBehaviour {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//Enemies possible to spawn
	string[] enemies = new string[2] {"DogFighterA/DogFighterA","DogFighterB/DogFighterB"};
	
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
	
	float GetEnemyLocation(int enemyNumber) {

		switch(enemyNumber)
		{
			case 0:
				return (top/2);
			case 1:
				return (top/2) + 0.5f;
			case 2:
				return (top/2) - 0.5f;
			case 3:
			case 8:
				return top - 0.5f;
			case 4:
			case 9:
				return bottom + 0.5f;
			case 5:
				return -(top/2);
			case 6:
				return -(top/2) + 0.5f;
			case 7:
				return -(top/2) - 0.5f;
			default:
				return 0;
		}
		
	}
	
	//Uses some logic to spawn enemies
	void SpawnEnemy (int enemyNumber) {
		
		//Get a random vertical location
		float enemyLocation = GetEnemyLocation(enemyNumber);
		
		string randomEnemy;

		if(enemyNumber < 3 || (enemyNumber > 4 && enemyNumber < 8))
			randomEnemy = "Enemies/" + enemies[0];
		else
			randomEnemy = "Enemies/" + enemies[1];

		GameObject enemy = Resources.Load<GameObject> (randomEnemy);

		Instantiate(enemy, new Vector3(right * 1.2f, enemyLocation, 0), Quaternion.identity);
	}
	
}


