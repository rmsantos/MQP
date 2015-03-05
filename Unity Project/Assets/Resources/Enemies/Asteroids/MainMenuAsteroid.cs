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

public class MainMenuAsteroid : MonoBehaviour {

	//The translation variables
	public float speed;
	public float rotation;
	public float directionRange;
	Vector2 direction;
	
	//Stores the boundaries of the game
	Boundaries boundaries;
	
	//Randomizer script
	Randomizer random;
	
	//The image number for this asteroid
	int asteroidNumber;

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
		 
		//Get the randomizer script
		random = GameObject.FindGameObjectWithTag ("Randomizer").GetComponent<Randomizer>();
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		speed = Random.Range(speed, speed * 2f);
		rotation = Random.Range(rotation * .1f, rotation);
		float x = -1f;
		float y = Random.Range(directionRange * -1f, directionRange);
		direction = new Vector3(x, y);
		
		//Pick an asteroid sprite (1 to 16 are asteroids)
		asteroidNumber = random.GetRandomInRange (1, 17);

		//Load the asteroid sprite
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<UnityEngine.Sprite> ("Asteroid Sprites/ast" + asteroidNumber);

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

		//Move in the random direction
		transform.Translate(direction.normalized * speed, Space.World);
		transform.Rotate(Vector3.forward * rotation, Space.World);
		
		//If the enemy leaves the game space
		//Leave some room for the enemy to fully exit the visible screen (by multiplying 1.2)
		if (transform.position.x < (boundaries.getLeft() * 1.2))
		{
			//Destroy the enemy
			Destroy (this.gameObject);
		}
		
	}

}
