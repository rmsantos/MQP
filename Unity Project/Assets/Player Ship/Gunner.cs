/* Module      : Gunner.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls input from the player who is currently the gunner.
 *
 * Date        : 2015/1/16
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */
using UnityEngine;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Gunner : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//The prefab object for the bullet
	public GameObject bulletPrefab;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Not used here.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {
	
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Spawns a bullet object, rotates it towards the mouse click location.
	 * 				Also sends the mouse click location to the bullet object.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {

		//If the user clicked the left mouse button
		if(Input.GetMouseButtonDown(0))
		{

			/* -- LOCAL VARIABLES  --------------------------------------------------- */

			//Instantiate a bullet with bulletPrefab at the players current location
			GameObject bullet = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);

			//Read the mouse location in pixels
			Vector3 mousePos = Input.mousePosition;

			//Set the z offset since the camera is at -10z
			mousePos.z = 10;

			//Store the mouse's position in world coordinates
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (mousePos);

			//Store the direction of the mouse click
			Vector3 direction = mouseWorldPos-bullet.transform.position;

			//Look at the mouse click 
			bullet.transform.LookAt(direction,Vector3.up);

			//Rotate 90 degrees along the x so that the cylinder is facing the target (like a bullet)
			bullet.transform.Rotate(90,0,0);

			//Send the mouse position in world space to the bullet
			bullet.GetComponent<Bullet>().setMousePosition(mouseWorldPos);
		
		}
	
	}
}
