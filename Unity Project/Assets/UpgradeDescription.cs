/* Module      : UpgradeDescription.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the description of the upgrade
 *
 * Date        : 2015/2/6
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeDescription : MonoBehaviour {

	public Text descriptionText;
	public string description;

	void OnMouseEnter () {
		descriptionText.text = description;
	}

}
