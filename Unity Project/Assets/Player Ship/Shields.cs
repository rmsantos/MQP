/* Module      : Shields.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls for shields
 *
 * Date        : 2015/2/4
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shields : MonoBehaviour {
	
	//Shields the player has
	int shields;
	
	//The power level of the players shield
	int shieldPower;
	
	//The timer for shield regeneration
	int timer;
	
	//Time before shield regenerates
	int shieldTimer;
	
	//The max shields at this level
	int maxShields;
	
	//Shields
	public Slider shieldBar;

	//The render of the shield image
	public GameObject shieldRender;

	//Audio source to play recharge sound
	public AudioSource source;

	//Recharge sound
	public AudioClip recharge;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the players shield
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {
		
		//Shield power starts at 0
		shieldPower = PlayerPrefs.GetInt ("ShieldPower", 0);
		
		//Set the shield slider
		shieldBar.value = shieldPower;
		
		//Intitialize timer
		timer = 0;
		
		//Initialize the shield timers and levels
		setShields (shieldPower);

		//Set the tranparency of the shield
		setTransparency ();
		
	}


	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate(0
	 *
	 * Description : Recharges the shields and reads the slide bar
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate()
	{		
		//Incremement the shield recharge timer only if shields are not full
		if(shields != maxShields)
			timer++;
		
		//if the timer elapses
		if(timer == shieldTimer)
		{
			//Reset it
			timer = 0;

			//Increment the shield
			shields++;

			//Play the recharge sound if the shield is turning on
			if(shields == 1)
				source.PlayOneShot(recharge);

			//And change the transparency level
			setTransparency();
		}
	}

	
	/* ----------------------------------------------------------------------- */
	/* Function    : setShields(int level)
	 *
	 * Description : Adjusts the shield variables to work with level
	 *
	 * Parameters  : int level : The shield level moving to
	 *
	 * Returns     : Void
	 */
	void setShields(int level)
	{
		//Initialize the shield timer, shield, and shield power
		switch (level)
		{
			case 0:
				maxShields = 0;
				shieldTimer = 300;
				break;
			case 1:
				maxShields = 1;
				shieldTimer = 270;
				break;
			case 2:
				maxShields = 1;
				shieldTimer = 240;
				break;
			case 3:
				maxShields = 2;
				shieldTimer = 210;
				break;
			case 4:
				maxShields = 3;
				shieldTimer = 180;
				break;
			case 5:
				maxShields = 3;
				shieldTimer = 150;
				break;
		}

		//If the player has the upgrade for shield recharge
		//Decrease recharge time by 1/3 second
		shieldTimer -= PlayerPrefs.GetInt ("ShieldUpgradeRecharge", 0) * 20;

		//Set the shields to start
		shields = maxShields;
	}

	
	/* ----------------------------------------------------------------------- */
	/* Function    : setTranparency()
	 *
	 * Description : Sets the tranparency of the shield
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void setTransparency()
	{
		//Defaultly 0 (if no shields)
		float transparency = 0f;

		//Interpolate between the three levels
		if(shields == 1)
		{
			transparency = 0.4f;
		}
		else if(shields == 2)
		{
			transparency = 0.7f;
		}
		else if(shields == 3)
		{
			transparency = 1f;
		}

		//Change the alpha of the shield to make it more or less visible
		Color originalColour = shieldRender.renderer.material.color;
		shieldRender.renderer.material.color = new Color(originalColour.r, originalColour.g, originalColour.b, transparency);
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : getShields()
	 *
	 * Description : Gets the players current shield level
	 *
	 * Parameters  : None
	 *
	 * Returns     : int : Current shield levek
	 */
	public int getShields()
	{
		return shields;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : weakenShields()
	 *
	 * Description : Decreases the players shields
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	public void weakenShields()
	{ 
		//Decrement the shields
		shields--;

		//And set the new transparency levels
		setTransparency ();
	}
}
