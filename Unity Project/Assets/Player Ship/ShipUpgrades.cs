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
	
	void Start () {
	
		MissileLevel = PlayerPrefs.GetInt ("Missiles", 0);
		ShieldLevel = PlayerPrefs.GetInt ("Shields", 0);
		LaserLevel = PlayerPrefs.GetInt ("Laser", 0);
		TurretLevel = 1;

		if (MissileLevel < 1) {
			transform.Find("MissileUpgrade").renderer.enabled = false;
		}
		if (ShieldLevel < 1) {
			transform.Find("ShieldUpgrade").renderer.enabled = false;
		}
		if (LaserLevel < 1) {
			transform.Find("LaserUpgrade").renderer.enabled = false;
			transform.Find("LaserUpgradeAttachment").renderer.enabled = false;
		}
		if (TurretLevel < 1) {
			transform.Find("TurretUpgrade").renderer.enabled = false;
		}

	}
	
}
