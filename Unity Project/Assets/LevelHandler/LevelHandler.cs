﻿/* Module      : Spawner.cs
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

	string[] instances = new string[9] {"FlyingV", "ExampleInstance", "DogfighterAttack", "LargeAsteroids", "MediumAsteroids", "SmallAsteroids", "MixedAsteroids", "JuggernautWave", "HeavyWave"};
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

	//Get the portrait controller to play audio clips
	PortraitController portraitController;

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

		//Pull the values from player prefs
		level = PlayerPrefs.GetInt ("Level", 100);

		wave = 0;
		//TODO this is an unused variable. It could be used for pausing at the end of levels, shooting fireworks, displaying UI, or whatever
		levelCompleted = false;
		
		updateLevel = (UpdateLevel)levelText.GetComponent("UpdateLevel");

		updateLevel.UpdateText (level);

		bossHealthSlider.active = false;
		bossHealth = 100;

		//Find the portrait controller script
		portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
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

				if (wave <= 3) {
					//TODO have a more advanced instance picker
					string randomInstance = "Instances/" + instances[random.GetRandom(instances.GetLength(0))];
					Instantiate(Resources.Load<GameObject>(randomInstance));

					//If the instance is an asteroid instance
					if(randomInstance == "Instances/SmallAsteroids" || randomInstance == "Instances/MediumAsteroids" ||
					   randomInstance == "Instances/LargeAsteroids" || randomInstance == "Instances/MixedAsteroids")
					{
						//Play the asteroid field audio clip
						portraitController.playApproachingAsteroids();
					}
					//Else the wave is of mostly enemies
					else
					{
						//Play the audio clip for enemies incoming
						portraitController.playEnemiesIncoming();
					}

				}
				else {
					Instantiate(Resources.Load<GameObject>("BossInstances/" + bosses[0]));
					background.StopBackground();
					bossHealthSlider.active = true;

					//Play the boss spawn audio clip
					portraitController.playBossSpawn();
				}
				
			}
		}
	}

	public void LevelComplete() {
		//TODO Perhaps change scene or do something with buying
		//More level completion stuff can be put here
		var enemies = GameObject.FindGameObjectsWithTag("Enemies");
		foreach (var obj in enemies) {
			Destroy(obj);
		}

		NextLevel ();
	}

	public void NextLevel() {

		level++;
		PlayerPrefs.SetInt ("Level", level);
		PlayerPrefs.SetInt ("Score", (int)ScoreHandler.score);
		PlayerPrefs.SetInt ("Money", (int)ScoreHandler.money);
		//Load the UpgradeScene
		Application.LoadLevel (2);
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


