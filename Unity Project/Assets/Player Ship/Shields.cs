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
		shieldLevel = PlayerPrefs.GetInt ("ShieldLevel", 1);
		
		//Set the shield slider
		shieldBar.value = shieldLevel;
		
		//Intitialize timer
		timer = 0;
		
		//Shields are defaultly set to 1
		shields = 1;
		
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
		print ("SHIELDS: " + shields);
		
		//Store the level before reading the slide bar
		int previousLevel = shieldLevel;
		
		//Read the shields from the slide bar
		shieldLevel = (int)shieldBar.value;
		
		//If the level changed
		if(previousLevel != shieldLevel)
		{
			//Reset shield stats
			setShields(shieldLevel);
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
				shields++;
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
		
		//Do visual stuff here as well
		
		//Also reset the timer
		timer = 0;
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
		shields--;
	}
}
