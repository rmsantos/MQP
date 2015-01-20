/* Module      : AsteroidLarge.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Large Asteroid
 *
 * Date        : 2015/1/20
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

using UnityEngine;
using System.Collections;

public class AsteroidLarge : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */
	
	//The translation variables
	float speed;
	float rotation;
	Vector2 direction;
	
	//Stores the boundaries of the game
	Boundaries boundaries;
	
	//Value of destroying this asteroid
	public int value;
	
	//ScoreHandler object to track players score
	ScoreHandler score;
	
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

		gameObject.transform.Rotate(90, 180, 0);
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//Search for the ScoreHandler object for tracking score
		score = GameObject.FindGameObjectWithTag ("ScoreHandler").GetComponent<ScoreHandler>(); 
		
		speed = Random.Range(1f, 2f);
		rotation = Random.Range(10f, 100f);
		float x = -1f;
		float y = Random.Range(-.3f, .3f);
		direction = new Vector3(x, y);
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Moves the astroid to the left and rotating it
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */
		
		//Move in the random direction
		transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
		
		//If the enemy leaves the game space
		//Leave some room for the enemy to fully exit the visible screen (by multiplying 1.2)
		if (transform.position.x < (boundaries.getLeft() * 1.2))
		{
			//Destroy the enemy
			Destroy (this.gameObject);
		}
		
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : OnCollisionEnter(Collision col)
	 *
	 * Description : Deals with collisions between the player bullets and this enemy.
	 *
	 * Parameters  : Collision col : The other object collided with
	 *
	 * Returns     : Void
	 */
	void OnCollisionEnter (Collision col)
	{
		//If this is hit by a player bullet
		if(col.gameObject.tag == "PlayerBullet")
		{
			//Destroy the player bullet and this object
			Destroy(col.gameObject);
			Destroy (this.gameObject);
			
			//Update the players score
			score.UpdateScore(value);
			
		}
	}
}
