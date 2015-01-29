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
		//portrait1 = new Portrait (PlayerPrefs.GetInt ("Portrait1Job"), portraits [PlayerPrefs.GetInt ("Portrait1")], dialogue[PlayerPrefs("Portrait1")]);
		//portrait2 = new Portrait (PlayerPrefs.GetInt ("Portrait2Job"), portraits [PlayerPrefs.GetInt ("Portrait2")], dialogue[PlayerPrefs("Portrait2")]);
		//portrait3 = new Portrait (PlayerPrefs.GetInt ("Portrait3Job"), portraits [PlayerPrefs.GetInt ("Portrait3")], dialogue[PlayerPrefs("Portrait3")]);
		//portrait4 = new Portrait (PlayerPrefs.GetInt ("Portrait4Job"), portraits [PlayerPrefs.GetInt ("Portrait4")], dialogue[PlayerPrefs("Portrait4")]);

		//Load the portrait images
		//portrait1Object.GetComponent<SpriteRenderer> ().sprite = portrait1.getSprite ();
		//portrait2Object.GetComponent<SpriteRenderer> ().sprite = portrait2.getSprite ();
		//portrait3Object.GetComponent<SpriteRenderer> ().sprite = portrait3.getSprite ();
		//portrait4Object.GetComponent<SpriteRenderer> ().sprite = portrait4.getSprite ();

		playVictory ();

	}

	public void playVictory()
	{
		//print (random.GetAnyRandom());
		////print (random.GetRandom (4));
		//switch(random.GetRandom(4)
		//{

		//}
	}

}
