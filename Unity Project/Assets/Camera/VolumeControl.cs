/* Module      : VolumeControl.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This control the volume of the game
 *
 * Date        : 2015/1/26
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeControl : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Variables to store the boundary distances from the origin
	float volume;
	
	void Start () {
		volume = 0f;
		AudioListener.volume = volume;
	}

	public void SetVolume (float newVolume) {
		volume = newVolume;
		AudioListener.volume = volume;
	}

}
