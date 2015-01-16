/* Module      : Boundaries.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file stores the boundaries of the screen based on the
 * 				camera view.
 *
 * Date        : 2015/1/165
 *
 * History:
 * Revision      Date          Changed By
 * --------      ----------    ----------
 * 01.00         2016/1/15    rmsantos
 * 
 * First release.
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
	
		//Top boundary (with some tweaking with the +1)
		top = Camera.main.camera.orthographicSize + 1; 

		//Bottom boundary (with some tweaking with the +1)
		bottom = -Camera.main.camera.orthographicSize + 1;

		//Left boundary
		left = -(Camera.main.orthographicSize * Screen.width / Screen.height);

		//Right boundary
		right = (Camera.main.orthographicSize * Screen.width / Screen.height);
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Update is not used here.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {
	
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
}
