/* Module      : BackgroundImage.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of background images
 *
 * Date        : 2015/3/4
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

public class BackgroundImage : MonoBehaviour {

	//Stores the boundaries of the game
	Boundaries boundaries;

	//Speed of this image
	float speed;

	//Check so that the object setups on the first update (so that other objects load first)
	bool setup;

	//Randomizer script
	Randomizer random;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Picks a random speed
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Start to false
		setup = false;

		//Find the speed of the objects
		speed = Random.Range(0.01f, 0.02f);
	
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Setups the sprites and locations and moves the image to the left
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {

		//If setup has not been done yet
		if(!setup)
		{
			//Setup!
			setup = true;

			//Get the randomizer script
			random = GameObject.FindGameObjectWithTag ("Randomizer").GetComponent<Randomizer>();

			//Find the number of the image to load
			int imageNumber = random.GetRandomInRange(1,9);

			//Load the correct image sprite
			GetComponent<SpriteRenderer> ().sprite = Resources.Load<UnityEngine.Sprite> ("planets/" + imageNumber);

			//Pull the boundaries script from the main camera object and store it
			boundaries = Camera.main.GetComponent<Boundaries>();

			//Spawn the object off screen to the right
			float xPos = boundaries.getRight () * 2f;

			if(imageNumber == 7)
				xPos *= 2;

			//And at a random y position
			float yPos = Random.Range (boundaries.getBottom (), boundaries.getTop ());

			//Unless the object is full screen, then spawn at 0
			if(imageNumber == 7 || imageNumber == 1)
				yPos = 0;

			//Set the start position
			transform.position = new Vector3 (xPos, yPos, 0);
		}
	
		//Move the object slowly to the left
		transform.position += Vector3.left * speed;

		//If the object leaves the game space
		//Leave some room for the enemy to fully exit the visible screen (by multiplying 1.5)
		if (transform.position.x < (boundaries.getLeft() * 1.5))
		{
			//Destroy the object
			Destroy (this.gameObject);
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setSpeed(float newSpeed)
	 *
	 * Description : Sets the new speed of the image
	 *
	 * Parameters  : float newSpeed : The new speed of the image
	 *
	 * Returns     : Void
	 */
	public void setSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
}
