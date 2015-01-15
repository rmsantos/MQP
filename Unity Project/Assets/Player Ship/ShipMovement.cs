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
	float xPos;
	float yPos;

	//Variable used to implement acceleration
	//One for x and y acceleration
	float xAcceleration;
	float yAcceleration;

	//Variables to track key holding
	bool upHeld;
	bool downHeld;
	bool leftHeld;
	bool rightHeld;
	
	//Variable to store the acceleration rate
	public float accelerationRate;

	//Variable to store the max acceleration
	public float maxAcceleration;

	//Variables to store boundaries
	float top;
	float bottom;
	float left;
	float right;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Stores the X and Y positions of the ship in global variables.
	 * 				Initializes acceleration to 0, the held flags to false, and the
	 * 				boundaries to their vector locations.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Store X and Y positions of the ship
		xPos = transform.position.x;
		yPos = transform.position.y;

		//Initialize acceleration to 0
		xAcceleration = 0;
		yAcceleration = 0;

		//Initialize the button held keys. False means the button is not held
		upHeld = false;
		downHeld = false;
		leftHeld = false;
		rightHeld = false;

		//Initialize the boundary variables to the appropriate parametes
		top = 6;
		bottom = -5;
		left = -10;
		right = 10;

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

		//Read input from the user. Set flags to determine which buttons are being held.

		//Up or 'w'
		if(Input.GetKeyDown("w") || Input.GetKeyDown ("up"))
		{
			//Flag that up is held
			upHeld = true;
		}
		else if(Input.GetKeyUp ("w") || Input.GetKeyUp("up"))
		{
			//Flag that up has been released
			upHeld = false;
		}

		//Down or 's'
		if(Input.GetKeyDown("s") || Input.GetKeyDown ("down"))
		{
			//Flag that down is held
			downHeld = true;
		}
		else if(Input.GetKeyUp ("s") || Input.GetKeyUp("down"))
		{
			//Flag that down has been released
			downHeld = false;
		}

		//Left or 'a'
		if(Input.GetKeyDown("a") || Input.GetKeyDown ("left"))
		{
			//Flag the left is held
			leftHeld = true;
		}
		else if(Input.GetKeyUp ("a") || Input.GetKeyUp("left"))
		{
			//Flag that left has been released
			leftHeld = false;
		}

		//Right or 'd'
		if(Input.GetKeyDown("d") || Input.GetKeyDown ("right"))
		{
			//Flag that right is held
			rightHeld = true;
		}
		else if(Input.GetKeyUp ("d") || Input.GetKeyUp("right"))
		{
			//Flag that right has been released
			rightHeld = false;
		}



		//Is the up key being held?
		if(upHeld)
		{
			//If so then check if the ship is currently at maximum vertical acceleration
			if(yAcceleration < maxAcceleration)
			{
				//If not then speed up
				yAcceleration+= accelerationRate;
			}

			//Adjust the ships new position
			yPos = yPos + yAcceleration;

		}
		//Else is down being held? (Both can't be held at the same time)
		else if(downHeld)
		{
			//If so then check if the ship is currently at maximum deceleration
			if(yAcceleration > -maxAcceleration)
			{
				//If not then slow down
				yAcceleration-= accelerationRate;
			}

			//Adjust the ships new position
			yPos = yPos + yAcceleration;
		}


		//Is the left key being held
		if(leftHeld)
		{
			//If so then check if the ship is currently at maximum horizontal acceleration
			if(xAcceleration > -maxAcceleration)
			{
				//If not then speed up
				xAcceleration-= accelerationRate;
			}

			//Adjust the ships new position
			xPos = xPos + xAcceleration;
		}
		//Else is right being held? (Both can't be held at the same time)
		else if(rightHeld)
		{
			//If so then check if the ships is currently at maximum horizontal deceleration
			if(xAcceleration < maxAcceleration)
			{
				//If not the slow down
				xAcceleration+= accelerationRate;
			}

			//Adjust the ships new position
			xPos = xPos + xAcceleration;
		}

		//If no horizontal keys are being held
		if(!leftHeld && !rightHeld)
		{
			//If the horizontal acceleration is not 0, then decrease until it is 0
			if(xAcceleration > 0f)
			{
				xAcceleration-=accelerationRate;
			}
			else if(xAcceleration < 0f)
			{
				xAcceleration+=accelerationRate;
			}

			//The acceleration will most likely not hit exactly after moving some. In this case
			//we have a tolerance of accelerationRate before we reset it to 0
			if(xAcceleration < accelerationRate && xAcceleration > -accelerationRate)
				xAcceleration = 0;

			//Update the ships new position
			xPos = xPos + xAcceleration;

		}

		//If no vertical keys are being held
		if(!upHeld && !downHeld)
		{
			//If the vertical acceleration is not 0, then decrease until it is 0
			if(yAcceleration > 0f)
			{
				yAcceleration-=accelerationRate;
			}
			else if(yAcceleration < 0f)
			{
				yAcceleration+=accelerationRate;
			}

			//The acceleration will most likely not hit exactly after moving some. In this case
			//we have a tolerance of accelerationRate before we reset it to 0
			if(yAcceleration < accelerationRate && yAcceleration > -accelerationRate)
				yAcceleration = 0;

			//Update the ships new position
			yPos = yPos + yAcceleration;
		}


		//Check if the ship has moved outside of the boundaries
		//If so then set the ships position to that boundary and reset acceleration
		if(yPos >= top)
		{
			yPos = top;
			yAcceleration = 0;
		}
		else if(yPos <= bottom)
		{
			yPos = bottom;
			yAcceleration = 0;
		}

		if(xPos >= right)
		{
			xPos = right;
			xAcceleration = 0;
		}
		else if(xPos <= left)
		{
			xPos = left;
			xAcceleration = 0;
		}

		//Update new ship position
		transform.position = new Vector3 (xPos, yPos, transform.position.z);

	}
}
