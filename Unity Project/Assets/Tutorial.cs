/* Module      : Tutorial.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the tutorial
 *
 * Date        : 2015/2/23
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour {

	//The tutorial audio files
	public AudioClip[] tutorialClips;

	//Current clip index
	int index;

	//Counter Timer
	int timer;

	//Audio Trigger
	int triggerTime;

	//Audio Source
	AudioSource source;

	//Tutorial Arrows
	public Image arrow1;
	public Image arrow2;
	public Image arrow3;
	public Image arrow4;
	public Image arrow5;
	public Image arrow6;
	
	// Use this for initialization
	void Start () {
	
		//Find the audiosource
		source = GetComponent<AudioSource> ();

		//Initialize timer
		timer = 0;

		//Starting trigger is after 100 updates
		triggerTime = 100;

		//Index starts at 0
		index = 0;
	
		//Initialize arrows to false
		arrow1.enabled = false;
		arrow2.enabled = false;
		arrow3.enabled = false;
		arrow4.enabled = false;
		arrow5.enabled = false;
		arrow6.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		//Count to play the audio clip if there isnt a clip playing
		if(!source.isPlaying)
			timer++;

		//If the timer elapses
		if(timer == triggerTime)
		{
			//Reset the timer
			timer = 0;

			//if the tutorial is over then go to the main menu
			if(index == 16)
			{
				Application.LoadLevel(0);
			}

			//Play the next audio clip
			source.clip = tutorialClips[index];
			source.Play();

			//Increment the index
			index++;

			//Disable all arrows
			arrow1.enabled = false;
			arrow2.enabled = false;
			arrow3.enabled = false;
			arrow4.enabled = false;
			arrow5.enabled = false;
			arrow6.enabled = false;

			//Enabled arrows at certain audio cues
			if(index == 6)
			{
				//Direct the tutorial arrow
				arrow1.enabled = true;
			}
			else if(index == 8)
			{
				arrow2.enabled = true;
			}
			else if(index == 10)
			{
				arrow3.enabled = true;
			}
			else if(index == 11)
			{
				arrow4.enabled = true;
			}
			else if(index == 12)
			{
				arrow5.enabled = true;
			}
			else if(index == 14)
			{
				arrow6.enabled = true;
			}
		}
	
	}
}
