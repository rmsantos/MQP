/* Module      : ShipMovement.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the movement of the player ship.
 *
 * Date        : 2015/1/15
 *
 * History:
 * Revision      Date          Changed By
 * --------      ----------    ----------
 * 01.00         2015/1/15    rmsantos
 * 
 * First release.
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None


public class ShipMovement : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Will hold the X and Y position in the ship for comparison later
	public float xPos;
	public float yPos;


	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Stores the X and Y positions of the ship in global variables.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Store X and Y positions of the ship
		xPos = transform.position.x;
		yPos = transform.position.y;

	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Read in key inputs from the player and moves the ship accordingly.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */
		//None

	
		//LOLOL PRIMITIVE MOVEMENT
		if(Input.GetKeyDown("w") || Input.GetKeyDown ("up"))
		{
			if(yPos < 6)
				yPos++;

		}
		
		if(Input.GetKeyDown("s") || Input.GetKeyDown ("down"))
		{
			if(yPos > -5)
				yPos--;
		}

		if(Input.GetKeyDown("a") || Input.GetKeyDown ("left"))
		{
			if(xPos > -10)
				xPos--;
		}

		if(Input.GetKeyDown("d") || Input.GetKeyDown ("right"))
		{
			if(xPos < 10)
				xPos++;

		}

		//Update new ship position
		transform.position = new Vector3 (xPos, yPos, transform.position.z);

	}
}
