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
	static bool gameOver;
	static int gameOverTimer;
	
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
	public Text levelComplete3;

	public Image healthImage;
	static int bossHealth;

	static int[] pickedInstances;
	static int[] waveOrder;

	//Get the portrait controller to play audio clips
	PortraitController portraitController;

	//Time to spawn each wave
	public float spawningTime;

	//Boss max helath
	int maxHealth;

	//The audiohandler
	AudioHandler audioHandler;

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
	
		//Search for the audioHandler
		audioHandler = GameObject.FindGameObjectWithTag ("AudioHandler").GetComponent<AudioHandler> ();

		//Get the randomizer script
		random = (Randomizer)randomizer.GetComponent("Randomizer");

		background = (Background)backgroundObject.GetComponent("Background");

		//Initialize spawning variables
		timeBetweenSpawning = spawningTime;
		spawnTimer = timeBetweenSpawning;
		canSpawn = true;

		//Pull the values from player prefs
		level = PlayerPrefs.GetInt ("Level", 100);

		wave = 0;

		levelCompleted = false;
		levelCompletedTimer = 0;
		gameOver = false;
		gameOverTimer = 0;
		
		updateLevel = (UpdateLevel)levelText.GetComponent("UpdateLevel");

		updateLevel.UpdateText (level);

		healthImage.enabled = false;
		bossHealth = 100;
		maxHealth = bossHealth;

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

		//Store the max health of the boss to display
		if(bossHealth > maxHealth)
			maxHealth = bossHealth;

		//Display the boss' health bar
		displayHealth ();

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
						int randomInstance = GetRandomAsteroidWave();
						Instantiate(instancesAsteroid[randomInstance]);
						portraitController.playApproachingAsteroids();
					}
					//hard instance
					else {
						int randomInstance = GetRandomHardWave();
						Instantiate(instancesHard[randomInstance]);
					}

				}
				else {
					Instantiate(bosses[0]);
					background.StopBackground();
					healthImage.enabled = true;

					//Play the boss spawn audio clip
					portraitController.playBossSpawn();

					//Play the alarm sound clip
					audioHandler.playAlarm();
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
			else if (levelCompletedTimer == 110) {
				levelComplete3.active = true;
				levelComplete3.text = "Bonus Credits:  + " + (5 + (5 * level) + (10 * PlayerPrefs.GetInt("CargoUpgradeCredits", 0))).ToString();
			}
			else if (levelCompletedTimer >= 170) {
				NextLevel ();
			}
		}
		if (gameOver) {
			gameOverTimer++;
			if (gameOverTimer > 100) {
				GameOver();
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

	public void PlayerDied() {
		gameOver = true;
	}

	public void GameOver() {
		PlayerPrefs.SetInt ("Score", (int)ScoreHandler.score);
		Application.LoadLevel (5);
	}

	public void NextLevel() {

		level++;
		PlayerPrefs.SetInt ("Level", level);
		PlayerPrefs.SetInt ("Score", (int)ScoreHandler.score);
		PlayerPrefs.SetInt ("Money", (int)ScoreHandler.money + (5 * level) + (10 * PlayerPrefs.GetInt("CargoUpgradeCredits", 0)));
		PlayerPrefs.SetInt ("Crystals", (int)ScoreHandler.crystals);

		//Load the UpgradeScene
		Application.LoadLevel (2);
	}

	public int GetRandomBasedOnLevel() {

		int highest = Mathf.Min (level + 2, instances.GetLength(0) - 1);
		int lowest = Mathf.Min (instances.GetLength(0) - 1, Mathf.Max ((level / 2) - 1, 0));
		bool gotInt = false;
		int value = -1;
		while (!gotInt) {
			value = random.GetRandomInRange (lowest, highest);
			if (!pickedInstances.Contains(value) || random.GetRandom(100) < 33) {
				gotInt = true;
			}
		}
		pickedInstances[wave] = value;
		return value;
	}

	public int GetRandomAsteroidWave() {
		return Mathf.Min (random.GetRandomInRange (0, level - 1), instancesAsteroid.Length - 1);
	}

	public int GetRandomHardWave() {
		return Mathf.Min (random.GetRandomInRange (0, level - 1), instancesHard.Length - 1);
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

	/* ----------------------------------------------------------------------- */
	/* Function    : displayHealth()
	 *
	 * Description : Displays the boss' health on screen
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void displayHealth() 
	{
		//Use an algorithm to map the images to the players health
		int imageValue = bossHealth;
		int healthMax = maxHealth;
		int healthMin = 1;
		int imageMax = 22;
		int imageMin = 1;
		
		//Figure out which images go with each health value
		int imageNumber = healthMin + (imageValue-healthMin)*(imageMax-imageMin)/(healthMax-healthMin);
		
		//Only show 0 if the player has no health left
		if(bossHealth <= 0)
			imageNumber = 0;
		
		//Load the appropriate sprite
		healthImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/Enemy health/" + imageNumber);
	}

}


