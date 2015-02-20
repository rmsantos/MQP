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

	//The ship that will move on the radar
	public Image radarShip;

	//The end goal of the radar
	public GameObject endPoint;

	//Distance between start and end
	float distance;

	//The length of the current wave
	int waveLength;

	//The planet
	public Image planet;

	//Only get the distance once
	bool gotDistance;

	//The wave order
	int[] waveOrder;
	
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

		//Find the radar power level
		radarPower = PlayerPrefs.GetInt ("RadarPower", 0);

		//Load the correct image sprite
		radarImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/radar/" + radarPower);

		//Calculate the distance between the start and end 
		distance = Vector3.Distance (radarShip.transform.position, endPoint.transform.position);

		//Default length is 0
		waveLength = 0;

		//Set to false
		gotDistance = false;

		//Get the levelhandler
		LevelHandler levelHandler = GameObject.FindGameObjectWithTag ("LevelHandler").GetComponent<LevelHandler> ();

		//Get the wave order
		waveOrder = levelHandler.getWaveOrder ();
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
		//Only do things if the player has power to the radar
		if(radarPower != 0)
		{
			if(waveLength > 0)
			{
				//The new position of the enemy after moving
				Vector3 newPos = new Vector3 (radarShip.transform.position.x + (distance/waveLength)/5, radarShip.transform.position.y, radarShip.transform.position.z);
				
				//Apply the movement
				radarShip.transform.position = newPos;
			}
			else if(waveLength == -1)
			{
				//Only get the distance once
				if(!gotDistance)
				{
					gotDistance = true;
					distance = Vector3.Distance(radarShip.transform.position, planet.transform.position);
				}


				//If the ship has not reached the planet
				if(radarShip.transform.position.x < planet.transform.position.x)
				{

					//The new position of the enemy after moving
					Vector3 newPos = new Vector3 (radarShip.transform.position.x + (distance/50), radarShip.transform.position.y, radarShip.transform.position.z);
					
					//Apply the movement
					radarShip.transform.position = newPos;
				}
			}
		}
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setWaveLength(int newLength)
	 *
	 * Description : Called by instances to set the new wavelength
	 *
	 * Parameters  : int newLength : New wave length to set
	 *
	 * Returns     : Void
	 */
	public void setWaveLength(int newLength)
	{
		waveLength = newLength;
	}
}
