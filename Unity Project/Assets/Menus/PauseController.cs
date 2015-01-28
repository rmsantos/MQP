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

	public GameObject pauseMenu;
	
	void Start () {
		paused = false;
		pauseMenu.active = false;
		Time.timeScale = 1;
	}

	void Update () {
	
		//pause key
		if (Input.GetKeyDown ("p")) {
			paused = !paused;
			if (paused) {
				Time.timeScale = 0;
				pauseMenu.active = true;
			}
			else {
				Time.timeScale = 1;
				pauseMenu.active = false;
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
				pauseMenu.active = true;
			}
			else {
				Time.timeScale = 1;
				pauseMenu.active = false;
			}
		}
	}

	void OnApplicationFocus(bool focusStatus) {
		if (!focusStatus) {
			SetPause (!focusStatus);
		}

	}

}
