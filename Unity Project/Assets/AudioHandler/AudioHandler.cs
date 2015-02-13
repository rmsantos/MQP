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
}
