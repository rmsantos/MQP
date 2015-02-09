/* Module      : ShipUpgrades.cs
 * Author      : Joshua Morse
 * Email       : jbmorses@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the visual upgrades of the players ship
 *
 * Date        : 2015/2/2
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

public class ShipUpgrades : MonoBehaviour {

	int MissileLevel;
	int ShieldLevel;
	int LaserLevel;
	int TurretLevel;
	int EngineLevel;

	public Sprite upgradedMissile;
	public Sprite maxMissile;

	void Start () {
	
		TurretLevel = 1;
		MissileLevel = PlayerPrefs.GetInt ("MissileUpgradePayload", 0);
		LaserLevel = PlayerPrefs.GetInt ("LaserUpgradeFireRate", 0);
		EngineLevel = PlayerPrefs.GetInt ("EngineUpgrade", 0);
		ShieldLevel = PlayerPrefs.GetInt ("ShieldUpgradeNumber", 0);


		if (MissileLevel == 5) {
			print (transform.Find("MissileUpgrade").GetComponent<SpriteRenderer>().sprite);
			transform.Find("MissileUpgrade").GetComponent<SpriteRenderer>().sprite = maxMissile;
		} else if(MissileLevel < 5) {
			transform.Find("MissileUpgrade").GetComponent<SpriteRenderer>().sprite = upgradedMissile;
		}

		if (LaserLevel < 1) {
			transform.Find("LaserUpgrade/LaserUpgrade").renderer.enabled = false;
			transform.Find("LaserUpgradeAttachment").renderer.enabled = false;
		}

		if (TurretLevel < 1) {
			transform.Find("TurretUpgrade").renderer.enabled = false;
		}

		if (EngineLevel != 3) {
			transform.Find("EngineUpgrade").renderer.enabled = false;
		}
		
		if (ShieldLevel < 1) {
			transform.Find("ShieldUpgrade").renderer.enabled = false;
		}

	}
	
}
