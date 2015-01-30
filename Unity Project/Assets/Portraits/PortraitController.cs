/* Module      : PortraitController.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the portraits
 *
 * Date        : 2015/1/29
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class PortraitController : MonoBehaviour {

	//Randomizer script
	public GameObject randomizer;
	Randomizer random;

	//Each character portrait
	public GameObject portrait1Object;
	public GameObject portrait2Object;
	public GameObject portrait3Object;
	public GameObject portrait4Object;

	//Audioclips paired with each portrait
	//Portraits are paired in order of jobs
	//Pilot, gunner, mechanic, radio
	AudioClip[] portrait1;
	AudioClip[] portrait2;
	AudioClip[] portrait3;
	AudioClip[] portrait4;

	//Array of all possible character portraits
	public Sprite[] portraits = new Sprite[14];

	//Dialogue for each character
	public AudioClip[] character1 = new AudioClip[22];
	public AudioClip[] character2 = new AudioClip[22];
	public AudioClip[] character3 = new AudioClip[22];
	public AudioClip[] character4 = new AudioClip[22];
	public AudioClip[] character5 = new AudioClip[22];
	public AudioClip[] character6 = new AudioClip[22];
	public AudioClip[] character7 = new AudioClip[22];
	public AudioClip[] character8 = new AudioClip[22];
	public AudioClip[] character9 = new AudioClip[22];
	public AudioClip[] character10 = new AudioClip[22];
	public AudioClip[] character11 = new AudioClip[22];
	public AudioClip[] character12 = new AudioClip[22];
	public AudioClip[] character13 = new AudioClip[22];
	public AudioClip[] character14 = new AudioClip[22];

	//Combined audio clips for each character
	AudioClip[][] dialogue = new AudioClip[14][];

	//The audio source on this object
	AudioSource source;

	//Flag to check whether the money dialogue was already said on this level
	bool moneyCheck;

	//Flag for the pilot thanks dialogue
	bool pilotThanks;

	//Flag for the gunner thanks dialogue
	bool gunnerThanks;

	//Flag for the mechanic response
	bool mechanicResponse;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the portrait images and auido clips
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Set to false to start
		moneyCheck = false;
		pilotThanks = false;
		gunnerThanks = false;
		mechanicResponse = false;

		//Get the audio source object
		source = GetComponentInParent<AudioSource> ();

		//Get the randomizer script
		random = (Randomizer)randomizer.GetComponent("Randomizer");

		//Assign each character and audioclips to an index
		dialogue [0] = character1;
		dialogue [1] = character2;
		dialogue [2] = character3;
		dialogue [3] = character4;
		dialogue [4] = character5;
		dialogue [5] = character6;
		dialogue [6] = character7;
		dialogue [7] = character8;
		dialogue [8] = character9;
		dialogue [9] = character10;
		dialogue [10] = character11;
		dialogue [11] = character12;
		dialogue [12] = character13;
		dialogue [13] = character14;

		//Create each character portrait dialogue list
		portrait1 = dialogue[PlayerPrefs.GetInt("Portrait1")];
		portrait2 = dialogue[PlayerPrefs.GetInt("Portrait2")];
		portrait3 = dialogue[PlayerPrefs.GetInt("Portrait3")];
		portrait4 = dialogue[PlayerPrefs.GetInt("Portrait4")];

		//Load the portrait images
		portrait1Object.GetComponent<SpriteRenderer> ().sprite = portraits [PlayerPrefs.GetInt ("Portrait1")];
		portrait2Object.GetComponent<SpriteRenderer> ().sprite = portraits [PlayerPrefs.GetInt ("Portrait2")];
		portrait3Object.GetComponent<SpriteRenderer> ().sprite = portraits [PlayerPrefs.GetInt ("Portrait3")];
		portrait4Object.GetComponent<SpriteRenderer> ().sprite = portraits [PlayerPrefs.GetInt ("Portrait4")];
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Used for dialogue responses.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update() {

		//Play the pilotThanks audio clip after the radar operator gives a warning
		if(pilotThanks && !source.isPlaying)
		{
			playPilotThanks();
		}

		//Play the gunnerThanks audio clip after the pilot gives praise
		if(gunnerThanks && !source.isPlaying)
		{
			playGunnerThanks();
		}

		//Play the mechanicResponse audio clip after the pilot complains
		if(mechanicResponse && !source.isPlaying)
		{
			playMechanicResponse();
		}
	}


	/* ----------------------------------------------------------------------- */
	/* Function    : playAsteroidHit()
	 *
	 * Description : Plays on a 5% chance when an asteroid hits
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playAsteroidHit()
	{
		//5% to play this clip
		if(random.GetRandom(100) < 5)
		{
			print("ASTEROID HIT!");

			//Load the audio clip and play it
			source.clip = portrait1[1];
			source.Play();

			//Flag the mechanic for a response
			mechanicResponse = true;
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playEnemiesIncoming()
	 *
	 * Description : Plays on a 25% chance when an instance spawns
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playEnemiesIncoming()
	{
		//25% to play this clip
		if(random.GetRandom(100) < 25)
		{
			print("ENEMIES INCOMING!");

			//Load the audio clip and play it
			source.clip = portrait1[2];
			source.Play();
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playLargeEnemyDestroyed()
	 *
	 * Description : Plays on a 2% chance when a large enemy is destroyed
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playLargeEnemyDestroyed()
	{
		//25% to play this clip
		if(random.GetRandom(100) < 25)
		{
			print("LARGE ENEMY DESTROYED!");

			//Load the audio clip and play it
			source.clip = portrait1[3];
			source.Play();

			//Flag the gunner for a response
			gunnerThanks = true;
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playPilotThanks()
	 *
	 * Description : Plays on a response to a radio operator dialogue
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playPilotThanks()
	{
	
		print("PILOT THANKS");

		//Flag that the clip is playing
		pilotThanks = false;

		//Load the audio clip and play it
		source.clip = portrait1[4];
		source.Play();
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playBulletHit()
	 *
	 * Description : Plays on a 5% chance when a bullet hits the player
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playBulletHit()
	{
		//5% to play this clip
		if(random.GetRandom(100) < 5)
		{
			print("BULLET/MISSILE HIT");

			//Load the audio clip and play it
			source.clip = portrait2[6];
			source.Play();
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playBossStart()
	 *
	 * Description : Plays on a 25% chance when a boss starts the first phase
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playBossStart()
	{
		//25% to play this clip
		if(random.GetRandom(100) < 25)
		{
			print("BOSS START");

			//Load the audio clip and play it
			source.clip = portrait2[7];
			source.Play();
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playGunnerThanks()
	 *
	 * Description : Plays when the pilot gives praise
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playGunnerThanks()
	{
		print("GUNNER THANKS");
			
		//Load the audio clip and play it
		source.clip = portrait2[8];
		source.Play();

		//Flag that the clip is playing
		gunnerThanks = false;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : playMissilesLow()
	 *
	 * Description : Plays when  missiles go under 5
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playMissilesLow()
	{
		//Called when missiles go under 5
		print("MISSILES LOW");

		//Load the audio clip and play it
		source.clip = portrait2[9];
		source.Play();
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playMechanicResponse()
	 *
	 * Description : Plays when the pilot complains about asteroid damage
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playMechanicResponse()
	{

		print("MECHANIC RESPONSE");
			
		//Load the audio clip and play it
		source.clip = portrait3[11];
		source.Play();

		//Flag that the clip is playing
		mechanicResponse = false;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playMoneyHigh()
	 *
	 * Description : Plays when the players money goes above a threshhold
	 * 				Only played once per level at most
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playMoneyHigh()
	{
		//Called when the player collects a certain amount of crysals
		//Only called once per level
		if(!moneyCheck)
		{
			print("MONEY HIGH!");

			//Load the audio clip and play it
			source.clip = portrait3[12];
			source.Play();

			//Flag that this clip has already been played this level
			moneyCheck = true;
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playLaserBoss()
	 *
	 * Description : Plays when shooting the boss with a laser
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playLaserBoss()
	{
		//50% to play this clip
		if(random.GetRandom(100) < 50)
		{
			print("LASER BOSS");

			//Load the audio clip and play it
			source.clip = portrait3[13];
			source.Play();
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playMiscInfo()
	 *
	 * Description : Plays this clip randomly throughout a level
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playMiscInfo()
	{
		//Play this clip whenever?
		print("MISC INFO");

		//Load the audio clip and play it
		source.clip = portrait3[14];
		source.Play();
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playApproachingAsteroids()
	 *
	 * Description : Plays this clip when an asteroid instance spawns
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playApproachingAsteroids()
	{
		//Play this clip always
		print("APPROACHING ASTEROIDS");

		//Load the audio clip and play it
		source.clip = portrait4[16];
		source.Play();

		//Flag the pilot for a response
		pilotThanks = true;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playBossSpawn()
	 *
	 * Description : Plays this clip when the boss spawns
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playBossSpawn()
	{
		//Play this clip always
		print("BOSS SPAWN");

		//Load the audio clip and play it
		source.clip = portrait4[17];
		source.Play();

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playAmbusherSpawn()
	 *
	 * Description : Plays this clip on a 75% chance when an ambusher spawns
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playAmbusherSpawn()
	{
		//75% to play this clip
		if(random.GetRandom(100) < 75)
		{
			print("AMBUSHER SPAWN");

			//Load the audio clip and play it
			source.clip = portrait4[18];
			source.Play();
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playVictory()
	 *
	 * Description : Picks a random crew member and plays their victory dialogue
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playVictory()
	{
		//Get a random number between 0 and 3 to represent each crew member
		switch(random.GetRandom(4))
		{
			//Pilot
			case 0:
				print ("Character 1");

				//Load the audio clip and play it
				source.clip = portrait1[20];
				source.Play();
				break;
			//Gunner
			case 1:
				print ("Character 2");

				//Load the audio clip and play it
				source.clip = portrait2[20];
				source.Play();
				break;
			//Mechanic
			case 2:
				print ("Character 3");

				//Load the audio clip and play it
				source.clip = portrait3[20];
				source.Play();
				break;
			//Radar Operator
			case 3:
				print ("Character 4");

				//Load the audio clip and play it
				source.clip = portrait4[20];
				source.Play();
				break;
		}
	}
}
