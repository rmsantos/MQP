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

	//Portrait objects
	Portrait portrait1;
	Portrait portrait2;
	Portrait portrait3;
	Portrait portrait4;

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


	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the portrait images
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Set to false to start
		moneyCheck = false;
		pilotThanks = false;

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

		//Create each character portrait object
		portrait1 = new Portrait (portraits [PlayerPrefs.GetInt ("Portrait1")], dialogue[PlayerPrefs.GetInt("Portrait1")]);
		portrait2 = new Portrait (portraits [PlayerPrefs.GetInt ("Portrait2")], dialogue[PlayerPrefs.GetInt("Portrait2")]);
		portrait3 = new Portrait (portraits [PlayerPrefs.GetInt ("Portrait3")], dialogue[PlayerPrefs.GetInt("Portrait3")]);
		portrait4 = new Portrait (portraits [PlayerPrefs.GetInt ("Portrait4")], dialogue[PlayerPrefs.GetInt("Portrait4")]);

		//Load the portrait images
		portrait1Object.GetComponent<SpriteRenderer> ().sprite = portrait1.getSprite ();
		portrait2Object.GetComponent<SpriteRenderer> ().sprite = portrait2.getSprite ();
		portrait3Object.GetComponent<SpriteRenderer> ().sprite = portrait3.getSprite ();
		portrait4Object.GetComponent<SpriteRenderer> ().sprite = portrait4.getSprite ();

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Stuff
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update() {
		if(pilotThanks && !source.isPlaying)
		{
			playPilotThanks();
		}
	}


	public void playAsteroidHit()
	{
		//5% to play this clip
		if(random.GetRandom(100) < 5)
		{
			print("ASTEROID HIT!");
			source.clip = portrait1.getDialogue()[1];
			source.Play();
		}
	}

	public void playEnemiesIncoming()
	{
		//25% to play this clip
		if(random.GetRandom(100) < 25)
		{
			print("ENEMIES INCOMING!");
			source.clip = portrait1.getDialogue()[2];
			source.Play();
		}
	}

	public void playLargeEnemyDestroyed()
	{
		//2% to play this clip
		if(random.GetRandom(100) < 2)
		{
			print("LARGE ENEMY DESTROYED!");
			source.clip = portrait1.getDialogue()[3];
			source.Play();
		}
	}
	public void playPilotThanks()
	{
		if(pilotThanks)
		{
			pilotThanks = false;
			print("PILOT THANKS");
			source.clip = portrait1.getDialogue()[4];
			source.Play();
		}
	}

	
	public void playBulletHit()
	{
		//5% to play this clip
		if(random.GetRandom(100) < 5)
		{
			print("BULLET/MISSILE HIT");
			source.clip = portrait2.getDialogue()[6];
			source.Play();
		}
	}

	public void playBossStart()
	{
		//25% to play this clip
		if(random.GetRandom(100) < 25)
		{
			print("BOSS START");
			source.clip = portrait2.getDialogue()[7];
			source.Play();
		}
	}

	public void playMissilesLow()
	{
		//Called when missiles go under 5
		print("MISSILES LOW");
		source.clip = portrait2.getDialogue()[9];
		source.Play();
	}

	public void playMoneyHigh()
	{
		//Called when the player collects a certain amount of crysals
		//Only called once per level
		if(!moneyCheck)
		{
			print("MONEY HIGH!");
			source.clip = portrait3.getDialogue()[12];
			source.Play();
			moneyCheck = true;
		}
	}

	public void playLaserBoss()
	{
		//50% to play this clip
		if(random.GetRandom(100) < 50)
		{
			print("LASER BOSS");
			source.clip = portrait3.getDialogue()[13];
			source.Play();
		}
	}

	public void playMiscInfo()
	{
		//Play this clip whenever?
		print("MISC INFO");
		source.clip = portrait3.getDialogue()[14];
		source.Play();
	}

	public void playApproachingAsteroids()
	{
		//Play this clip always
		print("APPROACHING ASTEROIDS");
		source.clip = portrait4.getDialogue()[16];
		source.Play();
		pilotThanks = true;
	}

	public void playBossSpawn()
	{
		//Play this clip always
		print("BOSS SPAWN");
		source.clip = portrait4.getDialogue()[17];
		source.Play();

	}

	public void playAmbusherSpawn()
	{
		//75% to play this clip
		if(random.GetRandom(100) < 75)
		{
			print("AMBUSHER SPAWN");
			source.clip = portrait4.getDialogue()[18];
			source.Play();
		}
	}

	public void playVictory()
	{
		switch(random.GetRandom(4))
		{
			case 0:
				print ("Character 1");
				source.clip = portrait1.getDialogue()[20];
				source.Play();
				break;
			case 1:
				print ("Character 2");
				source.clip = portrait2.getDialogue()[20];
				source.Play();
				break;
			case 2:
				print ("Character 3");
				source.clip = portrait3.getDialogue()[20];
				source.Play();
				break;
			case 3:
				print ("Character 4");
				source.clip = portrait4.getDialogue()[20];
				source.Play();
				break;
		}
	}

	public void playCatchPhrase(int job)
	{
		switch(job)
		{
			case 0:
				print ("Character 1");
				source.clip = portrait1.getDialogue()[21];
				source.Play();
				break;
			case 1:
				print ("Character 2");
				source.clip = portrait2.getDialogue()[21];
				source.Play();
				break;
			case 2:
				print ("Character 3");
				source.clip = portrait3.getDialogue()[21];
				source.Play();
				break;
			case 3:
				print ("Character 4");
				source.clip = portrait4.getDialogue()[21];
				source.Play();
				break;
		}
	}
}
