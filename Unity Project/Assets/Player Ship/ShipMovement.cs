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
	
	//Variable to sweat the acceleration rate
	public float accelerationRate;

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

		//Initialize acceleration to 0
		xAcceleration = 0;
		yAcceleration = 0;

		//Initialize the button held keys. False means the button is not held
		upHeld = false;
		downHeld = false;
		leftHeld = false;
		rightHeld = false;

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

		if(Input.GetKeyDown("w") || Input.GetKeyDown ("up"))
		{
			upHeld = true;
		}
		else if(Input.GetKeyUp ("w") || Input.GetKeyUp("up"))
			upHeld = false;

		if(Input.GetKeyDown("s") || Input.GetKeyDown ("down"))
		{
			downHeld = true;
		}
		else if(Input.GetKeyUp ("s") || Input.GetKeyUp("down"))
			downHeld = false;

		if(Input.GetKeyDown("a") || Input.GetKeyDown ("left"))
		{
			leftHeld = true;
		}
		else if(Input.GetKeyUp ("a") || Input.GetKeyUp("left"))
			leftHeld = false;

		if(Input.GetKeyDown("d") || Input.GetKeyDown ("right"))
		{
			rightHeld = true;
		}
		else if(Input.GetKeyUp ("d") || Input.GetKeyUp("right"))
			rightHeld = false;


		if(upHeld)
		{
			if(yAcceleration < 0.5f)
				yAcceleration+= accelerationRate;

			if((yPos + yAcceleration) < 6)
				yPos = yPos + yAcceleration;
			else
				yAcceleration = 0;

		}

		if(downHeld)
		{
			if(yAcceleration > -0.5f)
				yAcceleration-= accelerationRate;
			
			if((yPos + yAcceleration) > -5)
				yPos = yPos + yAcceleration;
			else
				yAcceleration = 0;
		}

		if(leftHeld)
		{
			if(xAcceleration > -0.5f)
				xAcceleration-= accelerationRate;

			if((xPos + xAcceleration) > -10)
				xPos = xPos + xAcceleration;
			else
				xAcceleration = 0;
		}

		if(rightHeld)
		{
			if(xAcceleration < 0.5f)
				xAcceleration+= accelerationRate;
			
			if((xPos + xAcceleration) < 10)
				xPos = xPos + xAcceleration;
			else
				xAcceleration = 0;
		}

		if(!leftHeld && !rightHeld)
		{

			if(xAcceleration > 0f)
			{
				xAcceleration-=accelerationRate;
			}
			else if(xAcceleration < 0f)
			{
				xAcceleration+=accelerationRate;
			}
			
			if(xAcceleration < accelerationRate && xAcceleration > -accelerationRate)
				xAcceleration = 0;

			if((xPos + xAcceleration) < 10 && (yPos + xAcceleration) > -10)
				xPos = xPos + xAcceleration;
			else
				xAcceleration = 0;
		}

		if(!upHeld && !downHeld)
		{
			if(yAcceleration > 0f)
			{
				yAcceleration-=accelerationRate;
			}
			else if(yAcceleration < 0f)
			{
				yAcceleration+=accelerationRate;
			}
			
			if(yAcceleration < accelerationRate && yAcceleration > -accelerationRate)
				yAcceleration = 0;

			if((yPos + yAcceleration) < 6 && (yPos + yAcceleration) > -5)
				yPos = yPos + yAcceleration;
			else
				yAcceleration = 0;
		}


		//Update new ship position
		transform.position = new Vector3 (xPos, yPos, transform.position.z);

	}
}
