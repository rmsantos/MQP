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
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Randomizer : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Random variable
	Random random;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : initializes the random generator
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */

	void Start () {
	
		random = new Random((int) DateTime.Now.Ticks & 0x0000FFFF);

	}

	int GetRandom (int max) {

		return random.Next(max);

	}
	
	int GetAnyRandom () {
	
		return random.Next();
	
	}
	
	int GetRandomInRange (int min, int max) {
	
		return random.Next(min, max);
	
	}

}


