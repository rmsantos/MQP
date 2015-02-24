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
	bool menu;

	public GameObject pauseMenu;
	
	void Start () {
		paused = false;
		pauseMenu.active = false;
		Time.timeScale = 1;
		Screen.showCursor = false;
	}

	void Update () {
	
		//pause key
		if (Input.GetKeyDown ("p") || Input.GetKeyDown(KeyCode.Escape)) {
			paused = !paused;
			if (paused) {
				Screen.showCursor = true;
				Time.timeScale = 0;
				pauseMenu.active = true;
			}
			else {
				Screen.showCursor = false;
				Time.timeScale = 1;
				pauseMenu.active = false;
			}
		}

		if(menu && !audio.isPlaying)
		{
			Application.LoadLevel(0);
		}

	}

	public bool IsPaused() {
		return paused;
	}

	public void SetPause(bool pauseState) {
		if (pauseState != paused) {
			paused = pauseState;
			if (paused) {
				Screen.showCursor = true;
				Time.timeScale = 0;
				pauseMenu.active = true;
			}
			else {
				Screen.showCursor = false;
				Time.timeScale = 1;
				pauseMenu.active = false;
			}
		}
	}

	public void setReturn(bool returnMenu)
	{
		menu = returnMenu;
	}

	void OnApplicationFocus(bool focusStatus) {
		if (!focusStatus) {
			SetPause (!focusStatus);
		}

	}

	//Used to return the cursor back to the screen
	void OnDestroy() {
		Screen.showCursor = true;
	}

}
