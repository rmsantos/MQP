/* Module      : HitFader.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the fader when an object gets hurt
 *
 * Date        : 2015/2/13
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitFader : MonoBehaviour {

	Color startingColor;
	Color currentColor;

	bool fading;
	bool normal;

	// Use this for initialization
	void Start () {

		startingColor = renderer.material.color;
		currentColor = startingColor;
		fading = false;
		normal = true;

	}
	
	// Update is called once per frame
	void FixedUpdate() {

		if (fading) {
			if (currentColor.r >= 10) {
				fading = false;
			}
			else {
				currentColor = new Color(currentColor.r + 1, currentColor.g, currentColor.b, currentColor.a);
				renderer.material.color = currentColor;
			}
		}
		else if (!normal) {
			if (currentColor.r <= 2) {
				normal = true;
				renderer.material.color = startingColor;
				currentColor = startingColor;
			}
			else {
				currentColor = new Color(currentColor.r - 1, currentColor.g, currentColor.b, currentColor.a);
				renderer.material.color = currentColor;
			}
		}
	
	}

	public void BeenHit() {

		fading = true;
		normal = false;

	}

}
