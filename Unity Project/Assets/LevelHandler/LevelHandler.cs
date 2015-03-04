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

	//Alarm image
	public Image alarmImage;

	//Last edge alpha the alarm was at
	float edge;

	//Alert flag
	bool alert;

	//Alert timer
	int alertTimerMax;
	int alertTimer;

	//The music for the boss fight
	public AudioClip bossMusic;

	//The music for the victory music
	public AudioClip victoryMusic;

	//Timer to spawn background objects
	int backgroundObjectsTimer;
	int backgroundObjectsTimerMax;

	//prefab to spawn background objects
	public GameObject backgroundObjects;

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
	
		//Set the timer to be 1000. Spawn every 1000 updates
		backgroundObjectsTimerMax = 1000;

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

		//Set the egde to the default value of the image
		edge = alarmImage.color.a;

		//Flag not to start the alert
		alert = false;

		//Set the amount of time the alert will flash
		alertTimerMax = 420;
		alertTimer = 0;
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

		//Increment the timer
		backgroundObjectsTimer++;

		//If the timer interrupts
		if(backgroundObjectsTimer == backgroundObjectsTimerMax)
		{
			//Reset the timer
			backgroundObjectsTimer = 0;

			//Spawn a background object
			Instantiate(backgroundObjects, new Vector3(100,100,0), Quaternion.identity);
		}


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
					Instantiate(bosses[random.GetRandom(2)]);
					background.StopBackground();
					healthImage.enabled = true;

					//Play the boss spawn audio clip
					portraitController.playBossSpawn();

					//Play the alarm sound clip
					audioHandler.playAlarm();

					//Flag to start the alert animation
					alert = true;

					//Play the boss music
					Camera.main.audio.clip = bossMusic;
					Camera.main.audio.time = 0;
					Camera.main.audio.Play();

					//Adjust the music scale to be lower volume (since the boss music is louder)
					Camera.main.GetComponent<VolumeControl>().setMusicScale(0.03f);
					Camera.main.GetComponent<VolumeControl>().SetMusic(1f);
				}

				wave++;
				
			}
		}

		//If the boss is incoming, then flash the alert image
		if(alert)
		{
			//Count how long to be here
			alertTimer++;

			//If the alpha value is at the 1 edge
			if(edge == 1f)
			{
				//Slowly decrease the alpha
				var originalColour = alarmImage.color;
				alarmImage.color = new Color(originalColour.r, originalColour.g, originalColour.b, originalColour.a-0.1f);

				//When hitting 0 make the new edge 0
				if(originalColour.a <= 0)
				{
					edge = 0;
				}
			}
			//Else if the edge is 0 do the opposite
			else if(edge == 0f)
			{
				//Increase the alpha
				var originalColour = alarmImage.color;
				alarmImage.color = new Color(originalColour.r, originalColour.g, originalColour.b, originalColour.a+0.1f);

				//Flip the egde
				if(originalColour.a >= 1f)
				{
					edge = 1f;
				}
			}

			//Count how long the flashing should occur for
			if(alertTimer == alertTimerMax)
			{
				//And reset everything afterwards
				alert = false;
				alertTimer = 0;
				edge = 1f;

				var originalColour = alarmImage.color;
				alarmImage.color = new Color(originalColour.r, originalColour.g, originalColour.b, 1f);

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

		//Play the victory music
		Camera.main.audio.clip = victoryMusic;
		Camera.main.audio.time = 0;
		Camera.main.audio.Play();

		//Adjust the music scale to be lower volume (since the victory music is quieter)
		Camera.main.GetComponent<VolumeControl>().setMusicScale(0.3f);
		Camera.main.GetComponent<VolumeControl>().SetMusic(1f);

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
		Application.LoadLevel (6);
	}

	public void NextLevel() {

		level++;
		PlayerPrefs.SetInt ("Level", level);
		PlayerPrefs.SetInt ("Score", (int)ScoreHandler.score);
		PlayerPrefs.SetInt ("Money", (int)ScoreHandler.money + (5 * level) + (10 * PlayerPrefs.GetInt("CargoUpgradeCredits", 0)));
		PlayerPrefs.SetInt ("Crystals", (int)ScoreHandler.crystals);

		//Store the location of the audio at this time
		PlayerPrefs.SetFloat ("VictoryLocation", Camera.main.audio.time);

		//Load the UpgradeScene
		Application.LoadLevel (3);
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

	/* ----------------------------------------------------------------------- */
	/* Function    : getWaveOrder()
	 *
	 * Description : Returns the wave order
	 *
	 * Parameters  : None
	 *
	 * Returns     : int[] waveOrder : The wave order
	 */
	public int[] getWaveOrder()
	{
		return waveOrder;
	}

}


