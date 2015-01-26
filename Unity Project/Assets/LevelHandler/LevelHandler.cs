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
using UnityEngine.UI;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//NONE


public class LevelHandler : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Instance list. These will be stored here as references to other prefabs. This should be updated to reflect new instances. 
	//The instances should follow a particular naming pattern.

	string[] instances = new string[1] {"Instance2"};
	string[] bosses = new string[1] {"Boss1"};

	//Level tracker variables
	static int level;
	static int wave;
	static bool levelCompleted;
	
	//Randomizer script
	public GameObject randomizer;
	Randomizer random;

	//Randomizer script
	public GameObject backgroundObject;
	static Background background;

	//Variable to store the time until next spawn
	static float spawnTimer;

	//Flag to say if able to spawn next wave
	static bool canSpawn;

	//Variable, changed in Unity prefab, that is used to reset the spawn timer to a set amount
	public static float timeBetweenSpawning;
	
	public GameObject levelText;
	static UpdateLevel updateLevel;

	public Slider bossHealthSlider;
	static int bossHealth;

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

		background = (Background)backgroundObject.GetComponent("Background");

		//Initialize spawning variables
		timeBetweenSpawning = 100;
		spawnTimer = timeBetweenSpawning;
		canSpawn = true;

		level = 1;
		wave = 0;
		//TODO this is an unused variable. It could be used for pausing at the end of levels, shooting fireworks, displaying UI, or whatever
		levelCompleted = false;
		
		updateLevel = (UpdateLevel)levelText.GetComponent("UpdateLevel");

		bossHealthSlider.active = false;
		bossHealth = 100;
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

	void FixedUpdate () {

		bossHealthSlider.value = bossHealth;

		//If spawning is occurring, don't decrement the timer
		if (canSpawn) {

			//Decrements the spawn timer
			spawnTimer--;
			
			//If the spawn timer has reached 0, initialize an instance of spawning
			if (spawnTimer <= 0) {
				
				canSpawn = false;
				spawnTimer = timeBetweenSpawning;
				wave++;

				if (wave <= 1) {
					//TODO have a more advanced instance picker
					string randomInstance = "Instances/" + instances[random.GetRandom(instances.GetLength(0))];
					Instantiate(Resources.Load<GameObject>(randomInstance));
				}
				else {
					Instantiate(Resources.Load<GameObject>("BossInstances/" + bosses[0]));
					background.StopBackground();
					bossHealthSlider.active = true;
				}
				
			}
		}
	}

	public void LevelComplete() {
		//TODO Perhaps change scene or do something with buying
		//More level completion stuff can be put here
		NextLevel ();
	}

	public void NextLevel() {
		level++;
		wave = 0;
		canSpawn = true;
		background.ChangeBackground();
		updateLevel.UpdateText (level);
	}

	public void SpawningHasStopped () {
		canSpawn = true;
	}

	public void SetSpawnTimer (int time) {
		spawnTimer = time;
	}

	public void UpdateBossHealth(int health) {
		bossHealth = health;
	}

}


