/* Module      : Boss1.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This is the first created instance for the game
 *
 * Date        : 2015/1/23
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Boss1 : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Enemies possible to spawn
	string boss = "Flagship/Flagship";
	
	//The spawner object
	public GameObject enemySpawner;
	LevelHandler levelHandler;
	
	//The current time of the instance
	int timer;
	
	//The final time of the instance when it self destructs
	int finalTime;

	//Stores the boundaries of the game
	Boundaries boundaries;

	//The right boundaries of the map
	float right;

	//Boss checker variables
	bool spawned;
	static bool killed;
	int bossTimer;

	//The Portrait Controller script
	PortraitController portraitController;

	//The Radar script
	Radar radar;


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
		spawned = false;
		killed = false;
		bossTimer = 0;

		//Get the script that created this instance
		levelHandler = (LevelHandler) enemySpawner.GetComponent("LevelHandler");

		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();

		//Get the right boundaries
		right = boundaries.getRight();

		//Find the radar
		radar = GameObject.FindGameObjectWithTag ("Player").GetComponent<Radar> ();
		
		//And set the current wave length
		radar.setWaveLength (0);
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

		//TODO Notify the player via the character audio handler and probably some UI elements that a boss is coming

		if (!spawned) {
			//Increment the timer at each pass
			timer++;

			if (timer % 200 == 0 && timer >= 0) {
				SpawnEnemy();
			}
		}
		else {
			if (!killed) {
				bossTimer++;
				//TODO probably have the timer here that will kill the player or something
			}
			else {
				//Search for the controller again (can't store it for some reason or we get null)
				//Is it because of fixed update?
				portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();

				//If the audio clip is done playing, finish the level
				if(!portraitController.GetComponentInParent<AudioSource> ().isPlaying)
				{
					levelHandler.LevelComplete();
					gameObject.SetActive(false);
				}
			}
		}
	}
	
	void SpawnEnemy () {

		string bossPath = "Enemies/" + boss;
		GameObject enemy = Resources.Load<GameObject> (bossPath);
		Instantiate(enemy, new Vector3(right * 1.2f, 0f, 0f), Quaternion.identity);

		spawned = true;

	}

	public void BossDied() {
		//Find the portrait controller script
		portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();

		//Play the victory dialogue
		portraitController.playVictory ();

		//Flag that the boss is killed
		killed = true;

		//Find the radar
		radar = GameObject.FindGameObjectWithTag ("Player").GetComponent<Radar> ();

		//Flag the boss death
		radar.setWaveLength (-1);
	}

}


