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
	
	//The level of the players shield
	int shieldLevel;
	
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
		
		//Shield level starts at 1
		shieldLevel = PlayerPrefs.GetInt ("ShieldLevel", 5);
		
		//Set the shield slider
		shieldBar.value = shieldLevel;
		
		//Intitialize timer
		timer = 0;

		//Start with the appropriate amount of shields for the current level
		if(shieldLevel == 1 || shieldLevel == 2)
			shields = 1;
		else if(shieldLevel == 3)
			shields = 2;
		else
			shields = 3;

		//Set the tranparency of the shield
		setTransparency ();
		
		//Initialize the shield timers and levels
		setShields (shieldLevel);
		
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
		//Store the level before reading the slide bar
		int previousLevel = shieldLevel;
		
		//Read the shields from the slide bar
		shieldLevel = (int)shieldBar.value;
		
		//If the level changed
		if(previousLevel != shieldLevel)
		{

			//If the previous level is higher than the current
			if(previousLevel > shieldLevel)
			{
				int power = GetComponent<Power>().checkLevels(previousLevel-shieldLevel);
			
				print (power);

				//Add more pwoer to the power reserve
				GetComponent<Power>().redirectPower(power);

			}
			else
			{
				int power = GetComponent<Power>().checkLevels(-(shieldLevel-previousLevel));

				//shieldLevel = -power;
				//shieldBar.value = -power;

				print (power);

				//Else subtract it
				GetComponent<Power>().redirectPower(power);
			}

			//Reset shield stats
			setShields(shieldLevel);

			//Set the new transparency
			setTransparency();
		}
		
		//Store the player pref
		PlayerPrefs.SetInt ("ShieldLevel", shieldLevel);
		
		//Incremement the shield recharge timer only if shields are not full
		if(shields != maxShields)
			timer++;
		
		//if the timer elapses
		if(timer == shieldTimer)
		{
			//Reset it
			timer = 0;

			//If the shields arent maxed out, recharge them
			if(shields < maxShields)
			{
				//Increment the shield
				shields++;

				//And change the transparency level
				setTransparency();
			}		
			
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
		//Initialize the shield timers and levels
		switch (level)
		{
		case 1:
			maxShields = 1;
			shieldTimer = 300;
			break;
		case 2:
			maxShields = 1;
			shieldTimer = 240;
			break;
		case 3:
			maxShields = 2;
			shieldTimer = 240;
			break;
		case 4:
			maxShields = 3;
			shieldTimer = 240;
			break;
		case 5:
			maxShields = 3;
			shieldTimer = 150;
			break;
		}

		//Make sure that the player doesn't have more shields than allowed
		if(maxShields < shields)
			shields = maxShields;

		//Also reset the timer
		timer = 0;
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
