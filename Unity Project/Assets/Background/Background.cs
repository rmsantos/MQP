/* Module      : Background.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the Background
 *
 * Date        : 2015/1/20
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	public Texture[] backgrounds;
	public float speed;

	//Randomizer script
	public GameObject randomizer;
	Randomizer random;
	bool started;

	// Use this for initialization
	void Start() {

		//Get the randomizer script
		random = (Randomizer)randomizer.GetComponent("Randomizer");

		started = false;

	}

	void Update() {

		if (started) {
			transform.position = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);
		} 
		else {
			ChangeBackground();
			started = true;
		}

	}

	void ChangeBackground() {

		renderer.material.mainTexture = backgrounds[random.GetRandom(backgrounds.GetLength(0))];

	}


}
