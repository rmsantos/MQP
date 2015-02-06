/* Module      : ShipMovement.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the movement of the player ship.
 *
 * Date        : 2015/1/15
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


public class ShipMovement : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Will hold the X and Y position in the ship for comparison later
	float xPos;
	float yPos;

	//Will hold the Z rotation for the player ship
	float zRotation;

	//Variables to store the max and min rotation angles
	float maxRotate;
	float minRotate;

	//Variable to store rotation rate
	float rotationRate;

	//Variables used to implement acceleration
	//One for x and y acceleration
	float xAcceleration;
	float yAcceleration;

	//Variables to track key holding
	bool upHeld;
	bool downHeld;
	bool leftHeld;
	bool rightHeld;
	
	//Variable to store the acceleration rate
	float accelerationRate;

	//Variable to store the max acceleration
	float maxAcceleration;

	//Stores the boundaries script to access later
	Boundaries boundaries;

	//The engine power
	int enginePower;

	//Engine slider
	public Slider engineBar;

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

		//Store the Z rotation of the ship
		zRotation = transform.rotation.z;

		//Store the max and min rotation angles
		maxRotate = 15;
		minRotate = -15;

		//Initialize rotation rate
		rotationRate = 1;

		//Initialize acceleration to 0
		xAcceleration = 0;
		yAcceleration = 0;

		//Initialize the button held keys. False means the button is not held
		upHeld = false;
		downHeld = false;
		leftHeld = false;
		rightHeld = false;

		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>(); 

		//Pull the values from player prefs
		enginePower = PlayerPrefs.GetInt ("EnginePower", 0);

		//Set the engine values based on the power
		setEngine (enginePower);

		//Display the engine power
		engineBar.value = enginePower;

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setEngine(int level)
	 *
	 * Description : Used to set the appropriate values based on engine power level
	 *
	 * Parameters  : int level : The engine power level
	 *
	 * Returns     : Void
	 */
	void setEngine(int level)
	{
		//Set the acceleration stats based on power level
		switch (level)
		{
			case 0:
				accelerationRate = 0.002f;
				maxAcceleration = 0.06f;
				break;
			case 1:
				accelerationRate = 0.003f;
				maxAcceleration = 0.06f;
				break;
			case 2:
				accelerationRate = 0.003f;
				maxAcceleration = 0.08f;
				break;
			case 3:
				accelerationRate = 0.004f;
				maxAcceleration = 0.08f;
				break;
			case 4:
				accelerationRate = 0.004f;
				maxAcceleration = 0.1f;
				break;
			case 5:
				accelerationRate = 0.005f;
				maxAcceleration = 0.1f;
				break;
		}
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

		//Read input from the user. Set flags to determine which buttons are being held.

		//Up or 'w'
		if (Input.GetKeyDown ("w") || Input.GetKeyDown ("up")) {
				//Flag that up is held
				upHeld = true;
		} else if (Input.GetKeyUp ("w") || Input.GetKeyUp ("up")) {
				//Flag that up has been released
				upHeld = false;
		}

		//Down or 's'
		if (Input.GetKeyDown ("s") || Input.GetKeyDown ("down")) {
				//Flag that down is held
				downHeld = true;
		} else if (Input.GetKeyUp ("s") || Input.GetKeyUp ("down")) {
				//Flag that down has been released
				downHeld = false;
		}

		//Left or 'a'
		if (Input.GetKeyDown ("a") || Input.GetKeyDown ("left")) {
				//Flag the left is held
				leftHeld = true;
		} else if (Input.GetKeyUp ("a") || Input.GetKeyUp ("left")) {
				//Flag that left has been released
				leftHeld = false;
		}

		//Right or 'd'
		if (Input.GetKeyDown ("d") || Input.GetKeyDown ("right")) {
				//Flag that right is held
				rightHeld = true;
		} else if (Input.GetKeyUp ("d") || Input.GetKeyUp ("right")) {
				//Flag that right has been released
				rightHeld = false;
		}

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Read in key inputs from the player and moves the ship accordingly.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate() {

		//Is the up key being held?
		if (upHeld)
		{
			//If so then check if the ship is currently at maximum vertical acceleration
			if(yAcceleration < maxAcceleration)
			{
				//If not then speed up
				yAcceleration+= accelerationRate;
			}

			//Increment the rotation of the ship by 1 to tilt up upwards when going up
			zRotation = zRotation + rotationRate;

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

			//Decrement the rotation of the ship by 1 to tilt up downwards when going down
			zRotation = zRotation - rotationRate;

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
			//If the vertical acceleration is not 0, then increase or decrease until it is 0
			if(yAcceleration > 0f)
			{
				yAcceleration-=accelerationRate;
			}
			else if(yAcceleration < 0f)
			{
				yAcceleration+=accelerationRate;
			}

			//If the zRotation is not 0, then increase or decrease until it is 0
			if(zRotation > 0)
			{
				zRotation -= rotationRate;
			}
			else if(zRotation < 0)
			{
				zRotation += rotationRate;
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
		if(yPos >= boundaries.getTop() * .8f)
		{
			yPos = boundaries.getTop() * .8f;
			yAcceleration = 0;
		}
		else if(yPos <= boundaries.getBottom () * .8f)
		{
			yPos = boundaries.getBottom () * .8f;
			yAcceleration = 0;
		}

		if(xPos >= boundaries.getRight () * .875f)
		{
			xPos = boundaries.getRight () * .875f;
			xAcceleration = 0;
		}
		else if(xPos <= boundaries.getLeft () * .875f)
		{
			xPos = boundaries.getLeft() * .875f;
			xAcceleration = 0;
		}


		//Check if the ship has rotation outside of the allowed angle
		//If so then reset it to that magnitude
		if(zRotation > maxRotate)
			zRotation = maxRotate;
		else if(zRotation < minRotate)
			zRotation = minRotate;

		//Update new ship position
		transform.position = new Vector3 (xPos, yPos, transform.position.z);

		//Update the ships new rotation
		transform.rotation = Quaternion.AngleAxis (zRotation, Vector3.forward);

	}


	/* ----------------------------------------------------------------------- */
	/* Function    : getPosition()
	 *
	 * Description : Returns the current position of the ship.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Vector3 : Position of the players ship
	 */
	public Vector3 getPosition()
	{
		return transform.position;
	}

	void OnApplicationFocus(bool focusStatus) {
		if (focusStatus) {
			upHeld = false;
			downHeld = false;
			leftHeld = false;
			rightHeld = false;
		}
	}

}
