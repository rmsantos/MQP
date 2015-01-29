/* Module      : Explosion.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the explosion particle system
 *
 * Date        : 2015/1/29
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public int explosionTime;

	int currentTime;

	void Start () {
		currentTime = 0;
		transform.Rotate (new Vector3 (0, 180, 0));
	}

	void FixedUpdate () {

		currentTime++;

		if (explosionTime == currentTime) {
			particleSystem.emissionRate = 0;
		}

		if ((explosionTime + 200) <= currentTime) {
			Destroy (gameObject);
		}
	}
}
