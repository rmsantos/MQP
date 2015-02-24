/* Module      : AsteroidMedium.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Medium Asteroid
 *
 * Date        : 2015/1/20
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

using UnityEngine;
using System.Collections;

public class AsteroidMedium : AbstractAsteroid {

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes references
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Setup other asteroid variables
		setup ();
		
		//Pick an asteroid sprite (4 to 12 are medium asteroids)
		asteroidNumber = random.GetRandomInRange (4, 13);
		
		//Load the asteroid sprite
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<UnityEngine.Sprite> ("Asteroid Sprites/ast" + asteroidNumber);

		//Reset the collider so that it autofits
		Destroy (GetComponent<PolygonCollider2D> ());
		gameObject.AddComponent ("PolygonCollider2D");
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the astroid to the left and rotating it
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		//Move the asteroid like an asteroid
		move ();
		
		//Check to make sure the asteroid is in the game world
		checkBoundaries ();
		
	}

}
