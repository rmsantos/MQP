/* Module      : AsteroidSmall.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Small Asteroid
 *
 * Date        : 2015/1/20
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

using UnityEngine;
using System.Collections;

public class AsteroidSmall : AbstractAsteroid {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Chance to drop crystals
	public int crystalDropRate;

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
		
		//Pick an asteroid sprite (13 to 16 are small asteroids)
		asteroidNumber = random.GetRandomInRange (13, 17);

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

	void OnDestroy() {

		if (!isQuitting) {

			//Load the explosion
			GameObject explosion = Resources.Load<GameObject>("Explosions/AsteroidExplosion");

			//Position of the asteroid
			var position = gameObject.transform.position;

			//Create the explosion at this location
			Instantiate(explosion, new Vector3(position.x, position.y, position.z), Quaternion.identity);

			//Spawn a crystal with a certain chance
			//if(random.GetRandom(100) < crystalDropRate)
			if(asteroidNumber == 16)
			{
				//Load the crystal prefab
				GameObject crystal = Resources.Load<GameObject>("Crystals/Crystal");

				//Create money at this location
				Instantiate(crystal, new Vector3(position.x, position.y, position.z), Quaternion.identity);
			}
		}

	}

}
