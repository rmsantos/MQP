/* Module      : Blaster.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the blaster.
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

public class Blaster : MonoBehaviour {
	
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

		//Read the mouse location in pixels
		Vector3 mousePos = Input.mousePosition;
		
		//Set the z offset since the camera is at -10z
		mousePos.z = 10;
		
		//Store the mouse's position in world coordinates
		mouseWorldPos = Camera.main.ScreenToWorldPoint (mousePos);

		//Store the direction of the mouse in respect to the blaster
		Vector3 direction = mouseWorldPos-transform.position;
		
		//Rotate the blaster towards the mouse
		transform.rotation = Quaternion.LookRotation(direction);

		//Rotate -90 on the Y so it appears correct
		transform.Rotate (0, -90, 0);

	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Counts until the blaster destroys itself. Draws a line every update and damages anything 
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
			//Destroy this blaster
			Destroy(this.gameObject);
		}

		//Draw a raycast from here to the mouse position
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position,castRadius, mouseWorldPos-transform.position);

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
					AbstractEnemy enemy = (AbstractEnemy)hit.transform.GetComponent(typeof(AbstractEnemy));
						
					//Deal damage to that enemy
					enemy.takeDamage(damage);
				}
							
				//If the object is an asteroid
				if(hit.transform.tag == "Asteroids")
				{
					//Cast to an asteroid type
					AbstractAsteroid asteroid = (AbstractAsteroid)hit.transform.GetComponent(typeof(AbstractAsteroid));
					
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
						Flagship enemy = (Flagship)hit.transform.GetComponent(typeof(Flagship));
						
						//Deal damage to that enemy
						enemy.TakeDamage(damage);
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
