/* Module      : ShipDisplay.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the visual upgrades of the players ship on the upgrade screen
 *
 * Date        : 2015/3/2
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipDisplay : MonoBehaviour {

	//The current level of each relevant upgrade
	int MissileLevel;
	int ShieldLevel;
	int EngineLevel;
	int BlasterLevel;

	//Store the images for the missile upgrades
	public Sprite normalMissile;
	public Sprite upgradedMissile;
	public Sprite maxMissile;

	//The UpgradeMenu script for determining what upgrade is selected
	public UpgradeMenu menu;

	void Update()
	{
		//Pull the relevant upgrade levels
		MissileLevel = PlayerPrefs.GetInt ("MissileUpgradePayload", 0);
		BlasterLevel = PlayerPrefs.GetInt ("BlasterUpgradeFireRate", 0);
		EngineLevel = PlayerPrefs.GetInt ("EngineUpgrade", 0);
		ShieldLevel = PlayerPrefs.GetInt ("ShieldUpgradeNumber", 0);

		//Set everything invisible by default
		transform.Find("ShipShieldLines").GetComponent<Image>().enabled = false;
		transform.Find("ShipShield").GetComponent<Image>().enabled = false;
		transform.Find("ShipEngines").GetComponent<Image>().enabled = false;
		transform.Find("ShipBlasterBase").GetComponent<Image>().enabled = false;
		transform.Find("ShipBlaster").GetComponent<Image> ().enabled = false;

		//Set the missiles to normal
		transform.Find("ShipMissiles").GetComponent<Image>().sprite= normalMissile;

		//If the player is upgrading to ship engines level 3 or has ship engines level 3
		if(((menu.getSelected() == 0) && EngineLevel == 2) || EngineLevel == 3)
		{
			//Show the upgraded engines
			transform.Find("ShipEngines").GetComponent<Image>().enabled = true;
		}

		//If the player is upgrading to blasters level 1 or has ship blasters level 1 or above
		if(((menu.getSelected() == 1) && BlasterLevel == 0) || BlasterLevel > 0)
		{
			//Show the blaster and blaster base
			transform.Find("ShipBlasterBase").GetComponent<Image>().enabled = true;
			transform.Find("ShipBlaster").GetComponent<Image> ().enabled = true;
		}

		//If the player is upgrading to shields level 1 or has ship shields level 1
		if(((menu.getSelected() == 3) && ShieldLevel == 0) || ShieldLevel == 1)
		{
			//Show the shield and shield lines
			transform.Find("ShipShield").GetComponent<Image>().enabled = true;
			transform.Find("ShipShieldLines").GetComponent<Image>().enabled = true;

			//Change the alpha of the shield to make it more or less visible depending on the level
			Color originalColour = transform.Find("ShipShield").GetComponent<Image>().color;
			transform.Find("ShipShield").GetComponent<Image>().color = new Color(originalColour.r, originalColour.g, originalColour.b, 0.4f);

		}

		//If the player is upgrading to shields level 2 or has ship shields level 2
		if(((menu.getSelected() == 3) && ShieldLevel == 1) || ShieldLevel == 2)
		{
			//Show the shield and shield lines
			transform.Find("ShipShield").GetComponent<Image>().enabled = true;
			transform.Find("ShipShieldLines").GetComponent<Image>().enabled = true;
			
			//Change the alpha of the shield to make it more or less visible depending on the level
			Color originalColour = transform.Find("ShipShield").GetComponent<Image>().color;
			transform.Find("ShipShield").GetComponent<Image>().color = new Color(originalColour.r, originalColour.g, originalColour.b, 0.7f);
			
		}

		//If the player is upgrading to shields level 3 or has ship shields level 3
		if(((menu.getSelected() == 3) && ShieldLevel == 2) || ShieldLevel == 3)
		{
			//Show the shield and shield lines
			transform.Find("ShipShield").GetComponent<Image>().enabled = true;
			transform.Find("ShipShieldLines").GetComponent<Image>().enabled = true;
			
			//Change the alpha of the shield to make it more or less visible depending on the level
			Color originalColour = transform.Find("ShipShield").GetComponent<Image>().color;
			transform.Find("ShipShield").GetComponent<Image>().color = new Color(originalColour.r, originalColour.g, originalColour.b, 1);
		}

		//If the player is upgrading to missiles level 3 or has missiles level 3 or 4
		if(((menu.getSelected() == 8) && MissileLevel == 2) || MissileLevel == 3 || MissileLevel == 4)
		{
			//Display the middle level missiles
			transform.Find("ShipMissiles").GetComponent<Image>().sprite= upgradedMissile;
		}

		//If the player is upgrading to missiles level 4 or has missiles level 5
		if(((menu.getSelected() == 8) && MissileLevel == 4) || MissileLevel == 5)
		{
			//Display the max level missiles
			transform.Find("ShipMissiles").GetComponent<Image>().sprite= maxMissile;
		}
	}
	
}
