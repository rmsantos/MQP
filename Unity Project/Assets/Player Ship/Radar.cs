/* Module      : Radar.cs
 * Author      : Ryan Santos
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls for the radar
 *
 * Date        : 2015/2/4
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

/* -- INCLUDE FILES ------------------------------------------------------ */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Radar : MonoBehaviour {
	
	//The radar image for showing power levels
	public Image radarImage;

	//The radar power level
	int radarPower;
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Loads the radar levels
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Shield power starts at 0
		radarPower = PlayerPrefs.GetInt ("RadarPower", 0);
		
		//Load the correct image sprite
		radarImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/radar/" + radarPower);

	}
	
	
	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Moves the ship on the radar
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate()
	{		

	}
}
