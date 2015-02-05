/* Module      : Power.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls for power system in the game
 *
 * Date        : 2015/2/5
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Power : MonoBehaviour {

	//The level of power from the last level
	int powerLevel;

	//The max power at this level
	int maxPower;

	//The upgrade level of power
	int powerUpgrade;

	//Shields
	public Slider powerBar;

	
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
		
		//Load the current players power level
		powerUpgrade = PlayerPrefs.GetInt ("PowerUpgrade", 1);

		switch(powerUpgrade)
		{
			case 1:
				maxPower = 10;
				break;
			case 2:
				maxPower = 15;
				break;
			case 3:
				maxPower = 20;
				break;
		}

		//The level of power from the last level
		powerLevel = PlayerPrefs.GetInt ("PowerLevel", 10);

		powerLevel = 4;

		//Set the power slider to its last value
		powerBar.value = powerLevel;
		
	}
	
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Recharges the shields and reads the slide bar
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate()
	{		
		

	}

	public void redirectPower(int powerRequired)
	{
		powerLevel += powerRequired;
		powerBar.value = powerLevel;

		PlayerPrefs.SetInt ("PowerLevel", powerLevel);
	}

	public int checkLevels(int newPower)
	{
		if((powerLevel + newPower) >= 0 && (powerLevel + newPower) <= maxPower)
		{
			return newPower;
		}
		else if((powerLevel + newPower) < 0)
		{
			return -powerLevel;
		}
		else if((powerLevel + newPower) > maxPower)
		{
			return maxPower - powerLevel;
		}

		return 0;

	}
}