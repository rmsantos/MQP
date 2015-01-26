/* Module      : PauseController.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the pause menu of the game
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

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class PauseController : MonoBehaviour {
	
	static bool paused;

	public Button resumeButton;
	
	void Start () {
		paused = false;
		resumeButton.active = false;
	}

	void Update () {
	
		//pause key
		if (Input.GetKeyDown ("p")) {
			paused = !paused;
			if (paused) {
				Time.timeScale = 0;
				resumeButton.active = true;
			}
			else {
				Time.timeScale = 1;
				resumeButton.active = false;
			}
		}

	}

	public bool IsPaused() {
		return paused;
	}

	public void SetPause(bool pauseState) {
		if (pauseState != paused) {
			paused = pauseState;
			if (paused) {
				Time.timeScale = 0;
				resumeButton.active = true;
			}
			else {
				Time.timeScale = 1;
				resumeButton.active = false;
			}
		}
	}

}
