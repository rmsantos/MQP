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

public class AsteroidSmall : MonoBehaviour, BasicAsteroid {

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

	//The damage from colliding with this asteroid
	public int collisionDamage;

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
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//Search for the ScoreHandler object for tracking score
		score = GameObject.FindGameObjectWithTag ("ScoreHandler").GetComponent<ScoreHandler>(); 

		speed = Random.Range(2f, 4f);
		rotation = Random.Range(20f, 200f);
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
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */
		
		//Move in the random direction and rotation
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
	
	/* ----------------------------------------------------------------------- */
	/* Function    : OnCollisionEnter(Collision col)
	 *
	 * Description : Deals with collisions between the player bullets and this enemy.
	 *
	 * Parameters  : Collision col : The other object collided with
	 *
	 * Returns     : Void
	 */
	void OnCollisionEnter2D (Collision2D col)
	{
		//If this is hit by a player bullet
		if(col.gameObject.tag == "PlayerBullet")
		{

			//Destroy the player bullet and this object
			Destroy(col.gameObject);
			
			//Shatter this asteroid
			shatter();			
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : shatter()
	 *
	 * Description : Destroys the small asteroid and creates money
	 *
	 * Parameters  : Collision col : The other object collided with
	 *
	 * Returns     : Void
	 */
	public void shatter ()
	{
		//Load the money prefab
		GameObject money = Resources.Load<GameObject>("Money/Money");

		//Store the position of the asteroid
		var position = gameObject.transform.position;

		//Destroy the asteroid
		Destroy(this.gameObject);

		//Create money at this location
		Instantiate(money, new Vector3(position.x, position.y, position.z), Quaternion.identity);
		
		//Update the players score
		score.UpdateScore(value);
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : getCollisionDamage()
	 *
	 * Description : Returns the collision damage for this asteroid
	 *
	 * Parameters  : None
	 *
	 * Returns     : int : The damage when colliding with this asteroid
	 */
	public int getCollisionDamage()
	{
		return collisionDamage;
	}
}
