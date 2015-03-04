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

public class FadingScript : MonoBehaviour {

	float alpha;
	public float startingAlpha;
	public float changeRate;
	public int counter;

	public Image[] images;
	public Text[] texts;

	bool fading;

	// Use this for initialization
	void Start () {

		alpha = startingAlpha;
		fading = false;

	}
	
	// Update is called once per frame
	void Update () {

		if (counter > 0) {
			counter--;
			alpha += changeRate;
			Color originalColour;
			for (int i = 0; i < images.Length; i++) {
				originalColour = images[i].color;
				images[i].color = new Color(originalColour.r, originalColour.g, originalColour.b, alpha);
			}
			for (int i = 0; i < texts.Length; i++) {
				originalColour = texts[i].color;
				texts[i].color = new Color(originalColour.r, originalColour.g, originalColour.b, alpha);
			}
		}

		if (fading) {

			alpha -= changeRate;
			Color originalColour;
			for (int i = 0; i < images.Length; i++) {
				originalColour = images[i].color;
				images[i].color = new Color(originalColour.r, originalColour.g, originalColour.b, alpha);
			}
			for (int i = 0; i < texts.Length; i++) {
				originalColour = texts[i].color;
				texts[i].color = new Color(originalColour.r, originalColour.g, originalColour.b, alpha);
			}
			if (alpha <= 0) {
				gameObject.active = false;
			}

		}
	
	}

	public void FadeOut() {

		if (!texts[1].text.Equals("")) {
			fading = true;
		}

	}
}
