using UnityEngine;
using System.Collections;

public abstract class AbstractAsteroid : MonoBehaviour {
	
	//The translation variables
	public float speed;
	public float rotation;
	public float directionRange;
	Vector2 direction;
	
	//Stores the boundaries of the game
	Boundaries boundaries;
	
	//Value of destroying this asteroid
	public int value;
	
	//ScoreHandler object to track players score
	static ScoreHandler score;
	
	//The damage from colliding with this asteroid
	public int collisionDamage;
	
	//Randomizer script
	protected Randomizer random;
	
	protected bool isQuitting;

	
	/* ----------------------------------------------------------------------- */
	/* Function    : setup ()
	 *
	 * Description : Initializes references
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	protected void setup () {
		
		//Get the randomizer script
		random = GameObject.FindGameObjectWithTag ("Randomizer").GetComponent<Randomizer>();
		
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
		
		//Search for the ScoreHandler object for tracking score
		score = GameObject.FindGameObjectWithTag("ScoreHandler").GetComponent<ScoreHandler>();
		
		speed = Random.Range(speed, speed * 2f);
		rotation = Random.Range(rotation * .1f, rotation);
		float x = -1f;
		float y = Random.Range(directionRange * -1f, directionRange);
		direction = new Vector3(x, y);
		
		isQuitting = false;
		
	}

	protected void move ()
	{
		//Move in the random direction
		transform.Translate(direction.normalized * speed, Space.World);
		transform.Rotate(Vector3.forward * rotation, Space.World);
	}


	/* ----------------------------------------------------------------------- */
	/* Function    : checkBoundaries()
	 *
	 * Description : Destroys the enemy if it goes off screen to the left
	 *
	 * Parameters  : None.
	 *
	 * Returns     : None.
	 */
	public void checkBoundaries()
	{
		//If the enemy leaves the game space
		//Leave some room for the enemy to fully exit the visible screen (by multiplying 1.2)
		if (transform.position.x < (boundaries.getLeft() * 1.2))
		{
			//Destroy the enemy
			Destroy (this.gameObject);
		}

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
			
			//Destroy the asteroid
			Destroy(this.gameObject);
			
			//Create the explosion at this location
			Instantiate(explosion, new Vector3(position.x, position.y, position.z), Quaternion.identity);	
			
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
			shatter ();
			
		}
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : shatter()
	 *
	 * Description : Shatters the asteroid into smaller asteroids
	 *
	 * Parameters  : Collision col : The other object collided with
	 *
	 * Returns     : Void
	 */
	public void shatter ()
	{
		if(name == "AsteroidLarge" || name == "AsteroidLarge(Clone)")
		{
			//Load the medium asteroid prefab
			GameObject mediumAsteroid = Resources.Load<GameObject>("Enemies/Asteroids/AsteroidMedium");
			
			//Position of the asteroid
			var position = gameObject.transform.position;

			//Create two new medium asteroids
			Instantiate(mediumAsteroid, new Vector3(position.x, position.y - .5f, position.z), Quaternion.identity);
			Instantiate(mediumAsteroid, new Vector3(position.x, position.y + .5f, position.z), Quaternion.identity);

		} else if (name == "AsteroidMedium" || name == "AsteroidMedium(Clone)")
		{
			//Load the medium asteroid prefab
			GameObject smallAsteroid = Resources.Load<GameObject>("Enemies/Asteroids/AsteroidSmall");
			
			//Position of the asteroid
			var position = gameObject.transform.position;
			
			//Create two new small asteroids
			Instantiate(smallAsteroid, new Vector3(position.x, position.y + .3f, position.z), Quaternion.identity);
			Instantiate(smallAsteroid, new Vector3(position.x, position.y - .3f, position.z), Quaternion.identity);

		}

		//Destroy the asteroid
		Destroy(this.gameObject);
	
		//Update the players score
		score.UpdateScore(value);
	}
}
