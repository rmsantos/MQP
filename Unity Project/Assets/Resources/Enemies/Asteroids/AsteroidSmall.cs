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
	public float speed;
	public float rotation;
	public float directionRange;
	Vector2 direction;

	//Stores the boundaries of the game
	Boundaries boundaries;
	
	//Value of destroying this asteroid
	public int value;

	//The damage from colliding with this asteroid
	public int collisionDamage;

	public GameObject scoreObject;
	static ScoreHandler score;

	//Randomizer script
	GameObject randomizer;
	Randomizer random;

	bool isQuitting;

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

		//Get the randomizer script
		randomizer = GameObject.FindGameObjectWithTag ("Randomizer");
		random = (Randomizer)randomizer.GetComponent("Randomizer");
		
		//Pick an asteroid sprite (9 to 11 are small asteroids)
		int asteroid = random.GetRandomInRange (9, 12);

		//Load the asteroid sprite
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<UnityEngine.Sprite> ("Asteroid Sprites/ast" + asteroid);
		
		//Reset the collider so that it autofits
		Destroy (GetComponent<PolygonCollider2D> ());
		gameObject.AddComponent ("PolygonCollider2D");

		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//Search for the ScoreHandler object for tracking score
		score = (ScoreHandler)scoreObject.GetComponent("ScoreHandler");

		speed = Random.Range(speed, speed * 2f);
		rotation = Random.Range(rotation * .1f, rotation);
		float x = -1f;
		float y = Random.Range(directionRange * -1f, directionRange);
		direction = new Vector3(x, y);

		isQuitting = false;
		
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
	/* Function    : OnCollisionEnter2D(Collision2D col)
	 *
	 * Description : Deals with collisions between the player bullets and this enemy.
	 *
	 * Parameters  : Collision2D col : The other object collided with
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
		//Destroy the asteroid
		Destroy(this.gameObject);
		
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
		return collisionDamage - PlayerPrefs.GetInt("HullUpgradeAsteroidResistance",0);
	}

	void OnApplicationQuit() {

		isQuitting = true;

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
			if(random.GetRandom(100) < crystalDropRate)
			{
				//Load the crystal prefab 
				GameObject crystal = Resources.Load<GameObject>("Crystals/Crystal");

				//Create money at this location
				Instantiate(crystal, new Vector3(position.x, position.y, position.z), Quaternion.identity);
			}

		}

	}

}
