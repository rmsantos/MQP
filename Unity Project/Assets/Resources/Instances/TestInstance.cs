/* Module      : ExampleInstance.cs
 * Author      : Joshua Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file holds all the variables and methods that all instances share.
 *
 * Date        : 2015/2/17
 *
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

public class TestInstance : AbstractInstance {
	
	void Start () {
		Initialize();
	}
	
	void FixedUpdate () {
		DefaultUpdate();
	}
	
}
