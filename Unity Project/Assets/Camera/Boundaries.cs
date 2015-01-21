/* Module      : Boundaries.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file stores the boundaries of the screen based on the
 * 				camera view.
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


public class Boundaries : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//Variables to store the boundary distances from the origin
	float top;
	float bottom;
	float left;
	float right;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Stores the boundaries based on the camera's vision. This allows
	 * 				for dynamic allocation of the boundaries.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {
	
		//Top boundary
		top = Camera.main.camera.orthographicSize; 

		//Bottom boundary
		bottom = -Camera.main.camera.orthographicSize;

		//Left boundary
		left = -(Camera.main.orthographicSize * Screen.width / Screen.height);

		//Right boundary
		right = (Camera.main.orthographicSize * Screen.width / Screen.height);
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : getLeft()
	 *
	 * Description : Returns the distance to the left wall boundary.
	 *
	 * Parameters  : None
	 *
	 * Returns     : float : Distance to left wall boundary
	 */
	public float getLeft()
	{
		return left;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : getRight()
	 *
	 * Description : Returns the distance to the right wall boundary.
	 *
	 * Parameters  : None
	 *
	 * Returns     : float : Distance to right wall boundary
	 */
	public float getRight()
	{
		return right;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : getTop()
	 *
	 * Description : Returns the distance to the top wall boundary.
	 *
	 * Parameters  : None
	 *
	 * Returns     : float : Distance to top wall boundary
	 */
	public float getTop()
	{
		return top;
	}


	/* ----------------------------------------------------------------------- */
	/* Function    : getBottom()
	 *
	 * Description : Returns the distance to the bottom wall boundary.
	 *
	 * Parameters  : None
	 *
	 * Returns     : float : Distance to bottom wall boundary
	 */
	public float getBottom()
	{
		return bottom;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : inBoundaries(Vector3 position, float modifier)
	 *
	 * Description : Returns true if the position is in the game boundaries
	 *
	 * Parameters  : Vector3 position : The position being tested
	 * 				float modifier : Multiplies the boundaries to tweaking
	 *
	 * Returns     : bool : True if the position is inside the boundaries, false otherwise
	 */
	public bool inBoundaries(Vector3 position, float modifier)
	{
		return (position.y > bottom * modifier && position.y < top * modifier && position.x > left * modifier && position.x < right * modifier);
	}
}
