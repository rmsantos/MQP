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
using UnityEngine.UI;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class PortraitController : MonoBehaviour {

	//Randomizer script
	public GameObject randomizer;
	Randomizer random;

	//Each character portrait
	public Image portrait1Object;
	public Image portrait2Object;
	public Image portrait3Object;
	public Image portrait4Object;

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

	//Flag to check whether the crystal dialogue was already said on this level
	bool crystalCheck;

	//Flag for the pilot thanks dialogue
	bool pilotThanks;

	//Flag for the gunner thanks dialogue
	bool gunnerThanks;

	//Flag for the mechanic response
	bool mechanicResponse;

	//Flag for the radar power dialogue
	bool radarCheck;

	//Timer to count tip
	int timer;

	//Timer to count radar power
	int timer2;

	//Time before a tip is said
	public int tipTimer;

	//Time before the no power radar dialogue is said
	public int radarTimer;

	//The pause controller
	PauseController pause;

	//The audio speech images
	public Image speech1;
	public Image speech2;
	public Image speech3;
	public Image speech4;

	//Flags for the speech images
	bool player1Speech;
	bool player2Speech;
	bool player3Speech;
	bool player4Speech;

	//Flag for boss phase
	bool bossPhase;

	//Radar power level
	int radarPower;

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

		//Set the power levels
		radarPower = PlayerPrefs.GetInt ("RadarPower", 0);

		//Set to false to start
		crystalCheck = false;
		pilotThanks = false;
		gunnerThanks = false;
		mechanicResponse = false;
		bossPhase = false;
		radarCheck = false;
		player1Speech = false;
		player2Speech = false;
		player3Speech = false;
		player4Speech = false;

		//Get the pause controller
		pause = GameObject.FindGameObjectWithTag ("PauseController").GetComponent<PauseController> ();

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
		portrait1Object.overrideSprite = portraits[PlayerPrefs.GetInt("Portrait1")];
		portrait2Object.overrideSprite = portraits[PlayerPrefs.GetInt("Portrait2")];
		portrait3Object.overrideSprite = portraits[PlayerPrefs.GetInt("Portrait3")];
		portrait4Object.overrideSprite = portraits[PlayerPrefs.GetInt("Portrait4")];

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

		//If no source is playing then remove all speech bubbles
		if(!source.isPlaying)
		{
			player1Speech = false;
			player2Speech = false;
			player3Speech = false;
			player4Speech = false;
		}

		//Display the speech bubble if the player is talking
		//Else make it not visible
		if(player1Speech)
		{
			speech1.color = new Color(speech1.color.r, speech1.color.g, speech1.color.b, 1);
		}
		else
		{
			speech1.color = new Color(speech1.color.r, speech1.color.g, speech1.color.b, 0);
		}
		
		if(player2Speech)
		{
			speech2.color = new Color(speech2.color.r, speech2.color.g, speech2.color.b, 1);
		}
		else
		{
			speech2.color = new Color(speech2.color.r, speech2.color.g, speech2.color.b, 0);
		}
		
		if(player3Speech)
		{
			speech3.color = new Color(speech3.color.r, speech3.color.g, speech3.color.b, 1);
		}
		else
		{
			speech3.color = new Color(speech3.color.r, speech3.color.g, speech3.color.b, 0);
		}
		
		if(player4Speech)
		{
			speech4.color = new Color(speech4.color.r, speech4.color.g, speech4.color.b, 1);
		}
		else
		{
			speech4.color = new Color(speech4.color.r, speech4.color.g, speech4.color.b, 0);
		}

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

		//Increment the timer if the game isnt paused
		if(!pause.IsPaused())
		{
			timer++;

			//If the player has not supplied any power to the radar
			if(radarPower == 0)
				timer2++;

		}

		//If the tip timer elapses
		if(timer == tipTimer)
		{
			//Reset the timer
			timer = 0;

			//Play a random info clip
			playMiscInfo();
		}

		//If the radar timer elapses
		if(timer2 == radarTimer)
		{
			//Reset the timer
			timer2 = 0;

			//Play the radar no power dialogue
			playNoRadarPower();
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
			//If audio isnt current playing
			if(!source.isPlaying)
			{
				print("ASTEROID HIT!");

				//Load the audio clip and play it
				source.clip = portrait1[1];
				source.Play();

				//Flag the mechanic for a response
				mechanicResponse = true;

				//Flag that player 1 is speaking
				player1Speech = true;
			}
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
			//If audio isnt current playing
			if(!source.isPlaying)
			{
				print("ENEMIES INCOMING!");

				//Load the audio clip and play it
				source.clip = portrait1[2];
				source.Play();

				//Flag that player 1 is speaking
				player1Speech = true;
			}
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
			//If audio isnt current playing
			if(!source.isPlaying)
			{
				print("LARGE ENEMY DESTROYED!");

				//Load the audio clip and play it
				source.clip = portrait1[3];
				source.Play();

				//Flag the gunner for a response
				gunnerThanks = true;

				//Flag that player 1 is speaking
				player1Speech = true;
			}
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
		//If audio isnt current playing
		if(!source.isPlaying)
		{
			print("PILOT THANKS");

			//Flag that the clip is playing
			pilotThanks = false;

			//Load the audio clip and play it
			source.clip = portrait1[4];
			source.Play();

			//Flag that player 1 is speaking
			player1Speech = true;
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playGunnerNoPower()
	 *
	 * Description : Plays when the player tries to fire a weapon with no power supplied to it
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playGunnerNoPower()
	{
		//If audio isnt current playing
		if(!source.isPlaying)
		{
			print("GUNNER NO POWER");
				
			//Load the audio clip and play it
			source.clip = portrait2[5];
			source.Play();
				
			//Flag that player 2 is speaking
			player2Speech = true;
		}
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
			//If audio isnt current playing
			if(!source.isPlaying)
			{
				print("BULLET/MISSILE HIT");

				//Load the audio clip and play it
				source.clip = portrait2[6];
				source.Play();

				//Flag that player 2 is speaking
				player2Speech = true;
			}
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playBossPhase()
	 *
	 * Description : Plays on a 25% chance when a boss switches phases
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playBossPhase()
	{
		//If this clip hasn't been played this level
		if(!bossPhase)
		{
			//25% to play this clip
			if(random.GetRandom(100) < 25)
			{
				//If audio isnt current playing
				if(!source.isPlaying)
				{
					print("BOSS PHASE");

					//Load the audio clip and play it
					source.clip = portrait2[7];
					source.Play();

					//Flag that player 2 is speaking
					player2Speech = true;

					//Flag that this clip was played
					bossPhase = true;
				}
			}
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
		//If audio isnt current playing
		if(!source.isPlaying)
		{
			print("GUNNER THANKS");
				
			//Load the audio clip and play it
			source.clip = portrait2[8];
			source.Play();

			//Flag that the clip is playing
			gunnerThanks = false;

			//Flag that player 2 is speaking
			player3Speech = true;
		}
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
		//If audio isnt current playing
		if(!source.isPlaying)
		{
			//Called when missiles go under 5
			print("MISSILES LOW");

			//Load the audio clip and play it
			source.clip = portrait2[9];
			source.Play();

			//Flag that player 2 is speaking
			player2Speech = true;
		}
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
		//If audio isnt current playing
		if(!source.isPlaying)
		{

			print("MECHANIC RESPONSE");
				
			//Load the audio clip and play it
			source.clip = portrait3[11];
			source.Play();

			//Flag that the clip is playing
			mechanicResponse = false;

			//Flag that player 3 is speaking
			player3Speech = true;
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playCrystalsHigh()
	 *
	 * Description : Plays when the players money goes above a threshhold
	 * 				Only played once per level at most
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playCrystalsHigh()
	{
		//Called when the player collects a certain amount of crysals
		//Only called once per level
		if(!crystalCheck)
		{
			//If audio isnt current playing
			if(!source.isPlaying)
			{
				print("MONEY HIGH!");

				//Load the audio clip and play it
				source.clip = portrait3[12];
				source.Play();

				//Flag that this clip has already been played this level
				crystalCheck = true;

				//Flag that player 3 is speaking
				player3Speech = true;
			}
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
		//1% to play this clip
		if(random.GetRandom(100) < 1)
		{
			//If audio isnt current playing
			if(!source.isPlaying)
			{
				print("LASER BOSS");

				//Load the audio clip and play it
				source.clip = portrait3[13];
				source.Play();

				//Flag that player 3 is speaking
				player3Speech = true;
			}
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
		//If audio isnt current playing
		if(!source.isPlaying)
		{
			//Play this clip whenever?
			print("MISC INFO");

			//Get a random number between 0 and 3 to represent each crew member
			switch(random.GetRandom(4))
			{
				//Pilot
				case 0:
					print ("Character 1");
					
					//Load the audio clip and play it
					source.clip = portrait1[14];
					source.Play();

					//Flag that player 1 is speaking
					player1Speech = true;

					break;
					//Gunner
				case 1:
					print ("Character 2");
					
					//Load the audio clip and play it
					source.clip = portrait2[14];
					source.Play();

					//Flag that player 2 is speaking
					player2Speech = true;

					break;
					//Mechanic
				case 2:
					print ("Character 3");
					
					//Load the audio clip and play it
					source.clip = portrait3[14];
					source.Play();

					//Flag that player 3 is speaking
					player3Speech = true;

					break;
					//Radar Operator
				case 3:
					print ("Character 4");
					
					//Load the audio clip and play it
					source.clip = portrait4[14];
					source.Play();

					//Flag that player 4 is speaking
					player4Speech = true;

					break;
			}

		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playNoRadarPower()
	 *
	 * Description : Plays this clip when the player has no power to the radars
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playNoRadarPower()
	{
		if(!radarCheck)
		{
			//If audio isnt current playing
			if(!source.isPlaying)
			{
				//Play this clip always
				print("NO POWER RADAR");
				
				//Load the audio clip and play it
				source.clip = portrait4[15];
				source.Play();
				
				//Flag that this audio was said
				radarCheck = true;
				
				//Flag that player 4 is speaking
				player4Speech = true;

			}
		}
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
		//If audio isnt current playing
		if(!source.isPlaying)
		{
			//Play this clip always
			print("APPROACHING ASTEROIDS");

			//Load the audio clip and play it
			source.clip = portrait4[16];
			source.Play();

			//Flag the pilot for a response
			pilotThanks = true;

			//Flag that player 4 is speaking
			player4Speech = true;
		}
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
		//If audio isnt current playing
		if(!source.isPlaying)
		{
			//Play this clip always
			print("BOSS SPAWN");

			//Load the audio clip and play it
			source.clip = portrait4[17];
			source.Play();

			//Flag that player 4 is speaking
			player4Speech = true;
		}

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
			//If audio isnt current playing
			if(!source.isPlaying)
			{
				print("AMBUSHER SPAWN");

				//Load the audio clip and play it
				source.clip = portrait4[18];
				source.Play();

				//Flag that player 4 is speaking
				player4Speech = true;
			}
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playMinefield()
	 *
	 * Description : Plays this clip on a 75% chance when a minefield approaches
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playMinefield()
	{
		//75% to play this clip
		if(random.GetRandom(100) < 75)
		{
			//If audio isnt current playing
			if(!source.isPlaying)
			{
				print("MINEFIELD APPROACHING");
				
				//Load the audio clip and play it
				source.clip = portrait4[19];
				source.Play();
				
				//Flag that player 4 is speaking
				player4Speech = true;
			}
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
		//Reset player speech
		player1Speech = false;
		player2Speech = false;
		player3Speech = false;
		player4Speech = false;

		//Get a random number between 0 and 3 to represent each crew member
		switch(random.GetRandom(4))
		{
			//Pilot
			case 0:
				print ("Character 1");

				//Load the audio clip and play it
				source.clip = portrait1[20];
				source.Play();

				//Flag that player 1 is speaking
				player1Speech = true;

				break;
			//Gunner
			case 1:
				print ("Character 2");

				//Load the audio clip and play it
				source.clip = portrait2[20];
				source.Play();

				//Flag that player 2 is speaking
				player2Speech = true;

				break;
			//Mechanic
			case 2:
				print ("Character 3");

				//Load the audio clip and play it
				source.clip = portrait3[20];
				source.Play();

				//Flag that player 3 is speaking
				player3Speech = true;

				break;
			//Radar Operator
			case 3:
				print ("Character 4");

				//Load the audio clip and play it
				source.clip = portrait4[20];
				source.Play();

				//Flag that player 4 is speaking
				player4Speech = true;

				break;
		}
	}
}
