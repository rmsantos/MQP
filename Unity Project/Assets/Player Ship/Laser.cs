/* Module      : Laser.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the laser.
 *
 * Date        : 2015/1/16
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Laser : MonoBehaviour {
	
	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//The damage this bullet deals
	int damage;

	//Time to Live
	public int TTL;

	//COunter to keep track of the lifespan
	int counter; 

	//The position of the mouse
	Vector3 mouseWorldPos;

	//The radius at which the raycast will cast
	public float castRadius;

	//List of enemies hit
	//This prevents enemies from taking double damage
	ArrayList hitList = new ArrayList();

	
	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Draws a line from the players current position to the mouse location
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//The LineRenderer component for visuals
		LineRenderer line = GetComponent<LineRenderer> ();

		//Store the start point of the line to be the origin of the laser
		line.SetPosition (0, this.transform.position);

		//Read the mouse location in pixels
		Vector3 mousePos = Input.mousePosition;
		
		//Set the z offset since the camera is at -10z
		//We use 9.99999 because the line renderor can't be exactly horizontal
		mousePos.z = 9.9999f;

		//Store the mouse's position in world coordinates
		mouseWorldPos = Camera.main.ScreenToWorldPoint (mousePos);

		//Store the end point to be the mouse position
		line.SetPosition (1, mouseWorldPos);

	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Counts until the laser destroys itself. Draws a line every update and damages anything 
	 * 				that hits it.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {
		
		/* -- LOCAL VARIABLES ---------------------------------------------------- */

		//Increment the TTL counter
		counter++;

		//If the counter expires
		if(counter == TTL)
		{
			//Destroy this laser
			Destroy(this.gameObject);
		}

		//Maximum distance the raycast will go
		float distance = Vector3.Distance (transform.position, mouseWorldPos);

		//Draw a raycast from here to the mouse position
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position,castRadius, mouseWorldPos-transform.position, distance);

		//For every collision
		foreach(RaycastHit2D hit in hits)
		{
			//If this object hasnt been hit yet
			if(!hitList.Contains(hit.transform.gameObject))
			{
				//Adds this object to the hit list
				hitList.Add(hit.transform.gameObject);

				//If the collision was with an enemy or boss
				if(hit.transform.tag == "Enemies")
				{
					//Find the component that extends BasicEnemy (the enemy script)
					BasicEnemy enemy = (BasicEnemy)hit.transform.GetComponent(typeof(BasicEnemy));
						
					//Deal damage to that enemy
					enemy.takeDamage(damage);
				}
							
				//If the object is an asteroid
				if(hit.transform.tag == "Asteroids")
				{
					//Cast to an asteroid type
					BasicAsteroid asteroid = (BasicAsteroid)hit.transform.GetComponent(typeof(BasicAsteroid));
					
					//And shatter the asteroid
					asteroid.shatter();
					
				}
				
				//If the object is an enemy missile
				if(hit.transform.tag == "EnemyMissile")
				{
					//Cast to an asteroid type
					SeekerMissile seekerMissile = (SeekerMissile)hit.transform.GetComponent(typeof(SeekerMissile));
					
					//And explode the missile
					seekerMissile.explode();
					
				}

				if(hit.transform.tag == "Boss")
				{
					if(!hit.transform.GetComponent<Flagship>().startingPhase())
					{
						//Find the component that extends BasicEnemy (the enemy script)
						BasicEnemy enemy = (BasicEnemy)hit.transform.GetComponent(typeof(BasicEnemy));
						
						//Deal damage to that enemy
						enemy.takeDamage(damage);
					}
				}
			}
		}
		
	}

	
	/* ----------------------------------------------------------------------- */
	/* Function    : setDamage()
	 *
	 * Description : Used to store the damage the gunner will deal.
	 * 				Called from Gunner.cs.
	 *
	 * Parameters  : int newDamage : The new damage amount
	 *
	 * Returns     : Void
	 */
	public void setDamage(int newDamage)
	{
		damage = newDamage;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : getDamage()
	 *
	 * Description : Used to retrieve the damage the gunner will deal.
	 * 				Called from Gunner.cs.
	 *
	 * Parameters  : None
	 *
	 * Returns     : int : The damage the bullet will deal
	 */
	public int getDamage()
	{
		return damage;
	}
}
