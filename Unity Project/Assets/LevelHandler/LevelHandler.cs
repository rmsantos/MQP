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
using System.Linq;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//NONE


public class LevelHandler : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Instance list. These will be stored here as references to other prefabs. This should be updated to reflect new instances. 
	//The instances should follow a particular naming pattern.
	public GameObject[] instances;
	public GameObject[] instancesHard;
	public GameObject[] instancesAsteroid;
	public GameObject[] bosses;

	//Level tracker variables
	static int level;
	static int wave;
	static bool levelCompleted;
	static int levelCompletedTimer;
	
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

	public GameObject levelComplete1;
	public GameObject levelComplete2;

	public Slider bossHealthSlider;
	static int bossHealth;

	static int[] pickedInstances;
	static int[] waveOrder;

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

		levelCompleted = false;
		levelCompletedTimer = 0;
		
		updateLevel = (UpdateLevel)levelText.GetComponent("UpdateLevel");

		updateLevel.UpdateText (level);

		bossHealthSlider.active = false;
		bossHealth = 100;

		//Find the portrait controller script
		portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();

		pickedInstances = new int[] {-1, -1, -1, -1, -1};

		waveOrder = new int[] {0, 0, 0, 1, 2};
		waveOrder = random.Shuffle(waveOrder);
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
		if (canSpawn && !levelCompleted) {

			//Decrements the spawn timer
			spawnTimer--;
			
			//If the spawn timer has reached 0, initialize an instance of spawning
			if (spawnTimer <= 0) {
				
				canSpawn = false;
				spawnTimer = timeBetweenSpawning;

				if (wave <= 4) {
					//normal instance
					if (waveOrder[wave] == 0) {
						int randomInstance = GetRandomBasedOnLevel();
						Instantiate(instances[randomInstance]);
						portraitController.playEnemiesIncoming();
					}
					//asteroid instance
					else if (waveOrder[wave] == 1) {
						Instantiate(instancesAsteroid[0]);
						portraitController.playApproachingAsteroids();
					}
					//hard instance
					else {
						Instantiate(instancesHard[0]);
					}

				}
				else {
					Instantiate(bosses[0]);
					background.StopBackground();
					bossHealthSlider.active = true;

					//Play the boss spawn audio clip
					portraitController.playBossSpawn();
				}

				wave++;
				
			}
		}

		if (levelCompleted) {

			levelCompletedTimer++;
			if (levelCompletedTimer == 40) {
				Instantiate(levelComplete1);
			}
			else if (levelCompletedTimer == 70) {
				Instantiate(levelComplete2);
			}
			else if (levelCompletedTimer >= 140) {
				NextLevel ();
			}
		}
	}

	public void LevelComplete() {
		var enemies = GameObject.FindGameObjectsWithTag("Enemies");
		foreach (var obj in enemies) {
			Destroy(obj);
		}
		levelCompleted = true;
	}

	public void NextLevel() {

		level++;
		PlayerPrefs.SetInt ("Level", level);
		PlayerPrefs.SetInt ("Score", (int)ScoreHandler.score);
		PlayerPrefs.SetInt ("Money", (int)ScoreHandler.money);
		//Load the UpgradeScene
		Application.LoadLevel (2);
	}

	public int GetRandomBasedOnLevel() {

		int highest = Mathf.Min (level + 2, instances.GetLength(0));
		int lowest = Mathf.Max ((level / 2) - 1, 0);
		bool gotInt = false;
		int value = -1;
		while (!gotInt) {
			value = random.GetRandomInRange (lowest, highest);
			if (!pickedInstances.Contains(value) || random.GetRandom(100) < 50) {
				gotInt = true;
			}
		}
		pickedInstances[wave] = value;
		return value;
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


