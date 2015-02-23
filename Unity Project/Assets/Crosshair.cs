/* Module      : Crosshair.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the crosshair
 *
 * Date        : 2015/2/23
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Crosshair : MonoBehaviour {
	
	void FixedUpdate () {
		transform.position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
	}

}
