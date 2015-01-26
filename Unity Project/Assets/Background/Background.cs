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

	public Sprite[] backgrounds;
	public float speed;

	//Randomizer script
	public GameObject randomizer;
	Randomizer random;
	static bool started;
	static bool stopped;
	Vector3 initialPosition;

	// Use this for initialization
	void Start() {

		//Get the randomizer script
		random = (Randomizer)randomizer.GetComponent("Randomizer");

		started = false;

		initialPosition = transform.position;
	}

	void FixedUpdate() {

		if (!started) {
			GetComponent<SpriteRenderer>().sprite = backgrounds[random.GetRandom(backgrounds.GetLength(0))];
			transform.position = initialPosition;
			started = true;
		}

		if (!stopped) {
			transform.position = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);
		} 

	}

	public void ChangeBackground() {
		started = false;
		stopped = false;
	}

	public void StopBackground() {
		stopped = true;
	}

	public void StartBackground() {
		stopped = false;	
	}
	
}
