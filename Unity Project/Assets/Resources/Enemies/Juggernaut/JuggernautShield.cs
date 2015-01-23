using UnityEngine;
using System.Collections;

public class JuggernautShield : MonoBehaviour {

	//Damage this object will do when collided with
	int collisionDamage;

	/* ----------------------------------------------------------------------- */
	/* Function    : OnCollisionEnter2D (Collision2D col)
	 *
	 * Description : Deals with triggers between the player bullets and this shield.
	 *
	 * Parameters  : Collision2D other : The other object triggered with
	 *
	 * Returns     : Void
	 */
	void OnTriggerEnter2D(Collider2D other) 
	{
		//If this is hit by a player bullet
		if(other.gameObject.tag == "PlayerBullet")
		{
			//Destroy the player bullet
			Destroy (other.gameObject);
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setCollisionDamage(int damage)
	 *
	 * Description : Sets the amount of collision damage this object will do
	 *
	 * Parameters  : int damage : The amount of damage
	 *
	 * Returns     : Void
	 */
	public void setCollisionDamage(int damage)
	{
		collisionDamage = damage;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : getCollisionDamage()
	 *
	 * Description : Gets the amount of collision damage this object will do
	 *
	 * Parameters  : None
	 *
	 * Returns     : int : The collision damage that will be done
	 */
	public int getCollisionDamage()
	{
		return collisionDamage;
	}
}
