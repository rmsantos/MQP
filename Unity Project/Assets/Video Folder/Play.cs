/* Module      : Play.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the movie texture
 *
 * Date        : 2015/3/4
 *  
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

public class Play : MonoBehaviour {

	//The movie to play
	MovieTexture movie;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Scales the movie plane and plays movie
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Find the width and height of the screen
		float height = Camera.main.orthographicSize * 2.0f;
		float width = height * Screen.width / Screen.height;

		//Scale the video to be full screen
		transform.localScale = new Vector3(width/10, 0, (height/10) + 0.01f);

		//Cast the video to a movie texture
		movie = (MovieTexture)GetComponent<Renderer>().material.mainTexture;

		//Play the movie!
		movie.Play ();

	
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Moves to the main menu when the movie is done playing
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update()
	{
		//if the movie is done
		if(!movie.isPlaying)
		{
			//Store the location in the menu music
			PlayerPrefs.SetFloat("MainMenuLocation",Camera.main.audio.time);

			//Load the menu
			Application.LoadLevel(1);
		}
	}
}
