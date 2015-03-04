/* Module      : AudioHandler.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This control the sound effect volume of the game
 *
 * Date        : 2015/2/13
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Randomizer script
	GameObject randomizer;
	Randomizer random;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes references
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start()
	{
		//Get the randomizer script
		randomizer = GameObject.FindGameObjectWithTag ("Randomizer");
		random = (Randomizer)randomizer.GetComponent("Randomizer");

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setSoundEffectsVolume()
	 *
	 * Description : Set the volume of all sound effects
	 *
	 * Parameters  : float newVolume : The new volume of all sound effects
	 *
	 * Returns     : Void
	 */
	public void setSoundEffectsVolume(float newVolume)
	{
		//Get all audio sounces
		AudioSource[] allSoundEffects = GetComponentsInChildren<AudioSource> ();

		//Adjust the volume of each source
		foreach(AudioSource source in allSoundEffects)
		{
			source.volume = newVolume;
		}
		GameObject.Find ("laser1").GetComponent<AudioSource> ().volume = .25f * newVolume;
		GameObject.Find ("laser2").GetComponent<AudioSource> ().volume = .25f * newVolume;
		GameObject.Find ("laser3").GetComponent<AudioSource> ().volume = .25f * newVolume;
		GameObject.Find ("laser4").GetComponent<AudioSource> ().volume = .25f * newVolume;
		GameObject.Find ("laser5").GetComponent<AudioSource> ().volume = .25f * newVolume;
		GameObject.Find ("laser6").GetComponent<AudioSource> ().volume = .25f * newVolume;
		GameObject.Find ("burstLaser").GetComponent<AudioSource> ().volume = .15f * newVolume;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playPlayerExplosion()
	 *
	 * Description : Play the sound effect for player death
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playPlayerExplosion()
	{
		GameObject.Find ("playerExplosion").GetComponent<AudioSource> ().Play ();
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playShieldRecharge()
	 *
	 * Description : Play the sound effect for shield recharging
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playShieldRecharge()
	{
		GameObject.Find ("shieldRecharge").GetComponent<AudioSource> ().Play ();
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playMediumEnemyExplosion()
	 *
	 * Description : Play the sound effect for medium enemies exploding
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playMediumEnemyExplosion()
	{
		GameObject.Find ("mediumEnemyExplosion").GetComponent<AudioSource> ().Play ();
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playSmallEnemyExplosion()
	 *
	 * Description : Play the sound effect for medium enemies exploding
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playSmallEnemyExplosion()
	{
		//Play a random clip
		if(random.GetRandom(2) == 0)
			GameObject.Find ("smallEnemyExplosion1").GetComponent<AudioSource> ().Play ();
		else
			GameObject.Find ("smallEnemyExplosion2").GetComponent<AudioSource> ().Play ();

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playBossExplosion()
	 *
	 * Description : Play the sound effect for bosses exploding
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playBossExplosion()
	{
		GameObject.Find ("bossExplosion").GetComponent<AudioSource> ().Play ();
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playAsteroidExplosion()
	 *
	 * Description : Play the sound effect for asteroids exploding
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playAsteroidExplosion()
	{
		//Pick a random sound effect
		int number = random.GetRandom (4);

		//Play that random sound effect
		if(number == 0)
			GameObject.Find ("asteroidExplosion1").GetComponent<AudioSource> ().Play ();
		else if(number == 1)
			GameObject.Find ("asteroidExplosion2").GetComponent<AudioSource> ().Play ();
		else if(number == 2)
			GameObject.Find ("asteroidExplosion3").GetComponent<AudioSource> ().Play ();
		else
			GameObject.Find ("asteroidExplosion4").GetComponent<AudioSource> ().Play ();

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playLaser()
	 *
	 * Description : Play the sound effect for shooting lasers
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playLaser()
	{
		//Pick a random sound effect
		int number = random.GetRandom (6);

		//Play that random sound effect
		if(number == 0)
			GameObject.Find ("laser1").GetComponent<AudioSource> ().Play ();
		else if(number == 1)
			GameObject.Find ("laser2").GetComponent<AudioSource> ().Play ();
		else if(number == 2)
			GameObject.Find ("laser3").GetComponent<AudioSource> ().Play ();
		else if(number == 3)
			GameObject.Find ("laser4").GetComponent<AudioSource> ().Play ();
		else if(number == 4)
			GameObject.Find ("laser5").GetComponent<AudioSource> ().Play ();
		else 
			GameObject.Find ("laser6").GetComponent<AudioSource> ().Play ();
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playBurstLaser()
	 *
	 * Description : Play the sound effect for shooting burst lasers
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playBurstLaser()
	{
		GameObject.Find ("burstLaser").GetComponent<AudioSource> ().Play ();
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playBlaster()
	 *
	 * Description : Play the sound effect for shooting the blaster
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playBlaster()
	{
		GameObject.Find ("blaster").GetComponent<AudioSource> ().Play ();
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : playAlarm()
	 *
	 * Description : Play the sound effect for shooting the blaster
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void playAlarm()
	{
		GameObject.Find ("alarm").GetComponent<AudioSource> ().Play ();
	}
	
}
