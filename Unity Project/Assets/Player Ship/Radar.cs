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

	//Radar images
	public Image normal1;
	public Image normal2;
	public Image normal3;
	public Image heavy;
	public Image asteroid;

	//Array for normal images
	Image[] normalImages;

	//Counter for normal image array
	int imageCount;

	//Final image order
	Image[] imageOrder;

	//Initial setup
	bool init;

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

		//Flag for first frame setup
		init = false;

		//Make all images invisible to start
		normal1.enabled = false;
		normal2.enabled = false;
		normal3.enabled = false;
		heavy.enabled = false;
		asteroid.enabled = false;
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
		//If first time setup annd radar power level 2
		if(!init && radarPower == 2)
		{
			//Enable all images
			normal1.enabled = true;
			normal2.enabled = true;
			normal3.enabled = true;
			heavy.enabled = true;
			asteroid.enabled = true;

			//Get the levelhandler
			LevelHandler levelHandler = GameObject.FindGameObjectWithTag ("LevelHandler").GetComponent<LevelHandler> ();

			//Get the wave order
			waveOrder = levelHandler.getWaveOrder ();

			//Initialize imageOrder
			imageOrder = new Image[5];
			
			//Store the normal wave images
			normalImages = new Image[] {normal1, normal2, normal3};
			
			//Initialize imageCount
			imageCount = 0;
			
			//Place the radar images accordingly on the radar
			for(int x = 0; x<waveOrder.Length; x++)
			{
				//0 is for normal wave
				if(waveOrder[x] == 0)
				{
					imageOrder[x] = normalImages[imageCount];
					imageCount++;
				}
				//1 is for asteroid wave
				else if(waveOrder[x] == 1)
					imageOrder[x] = asteroid;
				//2  is for heavy wave
				else if(waveOrder[x] == 2)
					imageOrder[x] = heavy;

				//Set the position on the radar
				imageOrder[x].transform.position = new Vector3(imageOrder[x].transform.position.x + ((x) * distance / 5) + distance/10, imageOrder[x].transform.position.y,
				                                               imageOrder[x].transform.position.z);
			}

			//Flag the first time setup over
			init = true;
		}
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
