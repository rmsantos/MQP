/* Module      : Random.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the randomness of the game
 *
 * Date        : 2015/1/16
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using System;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Randomizer : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Random variable
	protected static System.Random random;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : initializes the random generator
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */

	void Start() {
	
		random = new System.Random((int) DateTime.Now.Ticks & 0x0000FFFF);

	}

	public int GetRandom (int max) {

		return random.Next(max);

	}
	
	public int GetAnyRandom () {
	
		return random.Next();
	
	}
	
	public int GetRandomInRange (int min, int max) {
	
		return random.Next(min, max);
	
	}

	public int[] Shuffle(int[] ints)
	{
		for (int t = 0; t < ints.Length; t++ )
		{
			int tmp = ints[t];
			int r = random.Next(t, ints.Length);
			ints[t] = ints[r];
			ints[r] = tmp;
		}

		return ints;
	}

}


