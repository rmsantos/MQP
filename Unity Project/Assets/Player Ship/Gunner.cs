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
using UnityEngine.UI;
using System.Collections;

/* -- DATA STRUCTURES ---------------------------------------------------- */
//None

public class Gunner : MonoBehaviour {

	/* -- GLOBAL VARIABLES --------------------------------------------------- */

	//The transform of the turret
	public Transform blasterTurret;

	//The transform of the missile shooter
	public Transform missileTurret;

	//The transform of the laser shooter
	public Transform laserTurret;

	//The tip of the turret
	public Transform turretEnd;

	//The prefab object for the blaster
	public GameObject blasterPrefab;

	//The prefab object for the missile
	public GameObject missilePrefab;

	//The prefab object for the laser
	public GameObject laserPrefab;

	//Displays the number of missiles available
	public Text missileCount;

	//Is the player ready to shoot the blaser?
	bool readyBlaster;

	//Is the player ready to shoot a missile?
	bool readyMissile;

	//Is the player ready to shoot a laser?
	bool readyLaser;

	//Player is going to shoot the blaster
	bool shootingBlaster;

	//Player is going to shoot a missile
	bool shootingMissile;

	//Player is going to shoot a laser
	bool shootingLaser;

	//Counter for reloading the blaster
	int blasterShootTimer;

	//Time before the player can shoot the blaster again 
	int blasterReloadTime;

	//Time before the player can shoot lasers again 
	int laserReloadTime;

	//Counter for reloading missiles
	int missileShootTimer;

	//Counter for reloading lasers
	int laserShootTimer;
	
	//Time before the player can shoot missiles again 
	int missileReloadTime;

	//The damage the blaster will deal
	int blasterDamage;

	//The damage missiles will deal
	int missileDamage;

	//The damage lasers will deal
	int laserDamage;

	//The power levels for each weapon
	int blasterPower;
	int missilePower;
	int laserPower;

	//Randomizer script
	public GameObject pauseObject;
	PauseController pauseMenu;

	//Get the images for the power levels
	public Image laserImage;
	public Image blasterImage;
	public Image missileImage;

	//Number of missiles and crystals the player has
	int missiles;

	//Get the portrait controller to play audio clips
	PortraitController portraitController;

	//Flag for playing the missile audio clip
	bool missileCheck;

	//The audiohandler
	AudioHandler audioHandler;

	//The score handler
	static ScoreHandler scoreHandler;

	//Blaster recharge bar
	public Image blasterReloadImage;

	//Missile reload bar
	public Image missileReloadImage;

	//Sprite for the tri-shot
	public Sprite triShot;

	//The turret object for the tri-shot
	public GameObject laserTurretObject;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the fire rate variables
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Search for the audioHandler
		audioHandler = GameObject.FindGameObjectWithTag ("AudioHandler").GetComponent<AudioHandler> ();

		//Set the shoot timer to its reload time
		blasterShootTimer = blasterReloadTime;
		missileShootTimer = missileReloadTime;
		laserShootTimer = laserReloadTime;

		//Load the pause controller object 
		pauseMenu = (PauseController)pauseObject.GetComponent("PauseController");

		//Pull the values from player prefs
		blasterPower = PlayerPrefs.GetInt ("BlasterPower", 0);
		missilePower = PlayerPrefs.GetInt ("MissilePower", 0);
		laserPower = PlayerPrefs.GetInt ("LaserPower", 0);

		//Set all weapons based on power levels
		setBlaster (blasterPower);
		setLaser (laserPower);
		setMissile (missilePower);

		//Load the image sprites
		laserImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/laser power/" + laserPower);
		blasterImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/blaster power/" + blasterPower);
		missileImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/missile power/" + missilePower);

		//Load the players missiles
		missiles = PlayerPrefs.GetInt ("Missiles", 0);
		missileCount.text = missiles.ToString();

		//Find the portrait controller script
		portraitController = GameObject.FindGameObjectWithTag ("Portrait").GetComponent<PortraitController>();

		//The audio file should not defaultly play
		missileCheck = false;

		//Search for the ScoreHandler object for tracking crystals
		scoreHandler = GameObject.FindGameObjectWithTag("ScoreHandler").GetComponent<ScoreHandler>();

		//If the player upgrade to tri-shot, then display the new sprite
		if(laserPower == 3)
			laserTurretObject.GetComponent<SpriteRenderer>().sprite = triShot;

	}
	

	/* ----------------------------------------------------------------------- */
	/* Function    : FixedUpdate()
	 *
	 * Description : Spawns a bullet object, rotates it towards the mouse click location.
	 * 				Also sends the mouse click location to the bullet object.
	 * 				Also deals with firing rates and reloading time.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void FixedUpdate () {

		//Play the missile clip only when the missiles go under 5 for the first time.
		//Dont play it again until it goes over 5 then back under
		if(!missileCheck && missiles < 2)
		{
			missileCheck = true;
			portraitController.playMissilesLow();
		}
		else if(missiles >= 2)
		{
			missileCheck = false;
		}
			
			
		//If the player is "reloading" the blaster, don't decrement the timer
		if (!readyBlaster) {
			
			//Decrements the shoot timer
			blasterShootTimer--;

			//Update the blaster recharge bar
			DisplayBlaster();

			//If the shoot timer has reached 0, reset it and flag that the player can shoot
			if (blasterShootTimer <= 0) {
				
				readyBlaster = true;
				blasterShootTimer = blasterReloadTime;
			}
		}

		//If the player is "reloading" a missile, don't decrement the timer
		if (!readyMissile) {
			
			//Decrements the shoot timer
			missileShootTimer--;

			//Update the missile recharge bar
			DisplayMissile();

			//If the shoot timer has reached 0, reset it and flag that the player can shoot
			if (missileShootTimer <= 0) {
				
				readyMissile = true;
				missileShootTimer = missileReloadTime;
			}
		}

		//If the player is "reloading" a laser, don't decrement the timer
		if (!readyLaser) {
			
			//Decrements the shoot timer
			laserShootTimer--;
			
			//If the shoot timer has reached 0, reset it and flag that the player can shoot
			if (laserShootTimer <= 0) {
				
				readyLaser = true;
				laserShootTimer = laserReloadTime;
			}
		}

		//Read the mouse location in pixels
		Vector3 mousePos = Input.mousePosition;
		
		//Set the z offset since the camera is at -10z
		mousePos.z = 10;
		
		//Store the mouse's position in world coordinates
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (mousePos);

		//Store the direction of the player in respect to the bullet
		Vector3 turretDirection = mouseWorldPos-blasterTurret.position;
	
		//Rotate the turret towards the mouse
		blasterTurret.rotation = Quaternion.LookRotation(turretDirection, blasterTurret.up);
		laserTurret.rotation = Quaternion.LookRotation(turretDirection, laserTurret.up);

		//For some reason the turrets tend to rotate into oblivion if we don't reset their angle to 0 or 180
		//Determine which angle the turret rotations are closets to
		if(Mathf.Abs (blasterTurret.eulerAngles.z) < (Mathf.Abs(blasterTurret.eulerAngles.z- 180)))
		{
			//If closest to 0, then reset the z rotation to 0
			blasterTurret.rotation = Quaternion.Euler(blasterTurret.eulerAngles.x,blasterTurret.eulerAngles.y,0);
			laserTurret.rotation = Quaternion.Euler(laserTurret.eulerAngles.x,laserTurret.eulerAngles.y,0);
		}
		else
		{
			//Else reset the z rotation to 180
			blasterTurret.rotation = Quaternion.Euler(blasterTurret.eulerAngles.x,blasterTurret.eulerAngles.y,180);
			laserTurret.rotation = Quaternion.Euler(laserTurret.eulerAngles.x,laserTurret.eulerAngles.y,180);
		}

		//If the user clicked the left mouse button
		if (shootingLaser && readyLaser) {

			//Flag that player has just shot
			readyLaser = false;

			//Instantiate a laser with laserPrefab at the players turret
			GameObject laser = (GameObject)Instantiate(laserPrefab,turretEnd.position, Quaternion.identity);
						
			//Store the direction of the player in respect to the laser
			Vector3 direction = mouseWorldPos-laser.transform.position;

			//Rotate the laster towards the player
			laser.transform.rotation = Quaternion.LookRotation(direction);
			
			//Rotate the laser along the y so that it faces the mouse point
			laser.transform.Rotate(0,90,0);

			//Send the damage the laser will deal to the enemy
			laser.GetComponent<Laser>().setDamage(laserDamage);

			//If power level 3, then enable tri-shot
			if(PlayerPrefs.GetInt("LaserPower",0) == 3)
			{
				//Play the burst laser sound effect
				audioHandler.playBurstLaser();

				//SECOND LASER
				//Instantiate a laser with laserPrefab at the players turret
				GameObject laser2 = (GameObject)Instantiate(laserPrefab,turretEnd.position, Quaternion.identity);
				
				//Store the direction of the player in respect to the laser
				Vector3 direction2 = mouseWorldPos-laser2.transform.position;
				
				//Rotate the laser towards the player
				laser2.transform.rotation = Quaternion.LookRotation(direction2);
				
				//Rotate the laser along the y so that it faces the mouse point
				laser2.transform.Rotate(0,90,5);
				
				//Send the damage the laser will deal to the enemy
				laser2.GetComponent<Laser>().setDamage(laserDamage);


				//THIRD LASER
				//Instantiate a laser with bulletPrefab at the players turret
				GameObject laser3 = (GameObject)Instantiate(laserPrefab,turretEnd.position, Quaternion.identity);
				
				//Store the direction of the player in respect to the laser
				Vector3 direction3 = mouseWorldPos-laser3.transform.position;
				
				//Rotate the laser towards the player
				laser3.transform.rotation = Quaternion.LookRotation(direction3);
				
				//Rotate the laser along the y so that it faces the mouse point
				laser3.transform.Rotate(0,90,-5);
				
				//Send the damage the laser will deal to the enemy
				laser3.GetComponent<Laser>().setDamage(laserDamage);
			}
			else
				//Play the laser sound effect
				audioHandler.playLaser();
		
		}

		//If the user clicked the right mouse button
		if (shootingMissile) {

			//Update the missile recharge bar
			missileReloadImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/MissileReload/1");

			//Subtract the number of missiles
			missiles--;
			missileCount.text = missiles.ToString();

			//Store that pref
			PlayerPrefs.SetInt("Missiles",missiles);

			//Flag that player has just shot
			readyMissile = false;
			shootingMissile = false;
			
			//Instantiate a bullet with bulletPrefab at the players current location
			GameObject missile = (GameObject)Instantiate(missilePrefab,missileTurret.position,Quaternion.identity);
			
			//Store the direction of the player in respect to the bullet
			Vector3 direction = mouseWorldPos-missile.transform.position;
			
			//Rotate the bullet towards the player
			missile.transform.rotation = Quaternion.LookRotation(direction);

			//Send the damage the bullet will deal to the bullet
			missile.GetComponent<Missile>().setDamage(missileDamage);
			
		}

		//If the usesr has clicked the middle mouse button
		if(shootingBlaster)
		{

			//Update the blaster recharge bar
			blasterReloadImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/MissileReload/1");

			//Play the sound effect for the blaster
			audioHandler.playBlaster();

			//Create the blaster object and store it
			GameObject blaster = (GameObject)Instantiate(blasterPrefab,blasterTurret.position,Quaternion.identity);

			//Transfer the damage to the blaster object
			blaster.GetComponent<Blaster>().setDamage(blasterDamage);

			//Flag that the blaster was shot
			readyBlaster = false;
			shootingBlaster = false;
		}
	
	}

	void Update() {

		//If the game is paused, don't do anything
		if (!pauseMenu.IsPaused()) 
		{
			//If the player tries to shoot the laser and can
			if(Input.GetMouseButtonDown(0)) {

				//Flag the shoot
				shootingLaser = true;

			}
			else if(Input.GetMouseButtonUp(0)) {
				
				//Flag the shoot
				shootingLaser = false;

			}

			//If the player tries to shoot a missile and can
			if(readyMissile && Input.GetKeyDown(KeyCode.Space) && missiles > 0) {

				//if power is supplied to the missiles
				if(missilePower > 0)
				{
					//Flag the shoot
					shootingMissile= true;
				}
				else
				{
					//Else play the dialogue that tells the player they are without power to it
					portraitController.playGunnerNoPower();
				}

			}

			//If the player tries to shoot the blaster and can
			if(readyBlaster && Input.GetMouseButtonDown(1)) {

				//If the player supplied power to the blaster
				if(blasterPower > 0)
				{
					//Flag the shoot
					shootingBlaster= true;
				}
				//If the blaster has no power AND has been upgraded to appear
				else if(PlayerPrefs.GetInt("BlasterUpgradeFireRate",0) != 0)
				{
					//Play the no power audio clip
					portraitController.playGunnerNoPower();
				}
				
			}
		}

	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setLaser(int level)
	 *
	 * Description : Used to set the appropriate values based on laser power level
	 *
	 * Parameters  : int level : The laser power level
	 *
	 * Returns     : Void
	 */
	void setLaser(int level)
	{
		//Set the reload and damage stats based on power level
		switch (level)
		{
			case 0:
				laserDamage = 1;
				laserReloadTime = 22;
				break;
			case 1:
				laserDamage = 1;
				laserReloadTime = 18;
				break;
			case 2:
				laserDamage = 1;
				laserReloadTime = 14;
				break;
			case 3:
				laserDamage = 1;
				laserReloadTime = 22;
				break;
		}

		//If the player has bought the laserspeed upgrade
		//Then reduce reload time
		laserReloadTime -= PlayerPrefs.GetInt("LaserUpgradeSpeed")*2;

		//Add upgrade damage
		laserDamage += PlayerPrefs.GetInt ("LaserUpgradeDamage");
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : setBlaster(int level)
	 *
	 * Description : Used to set the appropriate values based on blaster power level
	 *
	 * Parameters  : int level : The blaster power level
	 *
	 * Returns     : Void
	 */
	void setBlaster(int level)
	{
		//Set the reload and damage stats based on power level
		switch (level)
		{
			case 0:
				blasterDamage = 0;
				blasterReloadTime = 99999;
				break;
			case 1:
				blasterDamage = 2;
				blasterReloadTime = 360;
				break;
			case 2:
				blasterDamage = 4;
				blasterReloadTime = 300;
				break;
			case 3:
				blasterDamage = 6;
				blasterReloadTime = 240;
				break;
			case 4:
				blasterDamage = 8;
				blasterReloadTime = 180;
				break;
		}

		//Subtract reload time based on blaster upgrade
		blasterReloadTime -= PlayerPrefs.GetInt ("BlasterUpgradeFireRate", 0) * 20;

		//If the player has bought the laser damage upgrade
		//Then increase damage
		blasterDamage += PlayerPrefs.GetInt("BlasterUpgradeDamage",0) * 4;
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : setMissile(int level)
	 *
	 * Description : Used to set the appropriate values based on missile power level
	 *
	 * Parameters  : int level : The missile power level
	 *
	 * Returns     : Void
	 */
	void setMissile(int level)
	{
		//Set the reload and damage stats based on power level
		switch (level)
		{
		case 0:
			missileDamage = 0;
			missileReloadTime = 99999;
			break;
		case 1:
			missileDamage = 4;
			missileReloadTime = 360;
			break;
		case 2:
			missileDamage = 8;
			missileReloadTime = 300;
			break;
		case 3:
			missileDamage = 12;
			missileReloadTime = 240;
			break;
		case 4:
			missileDamage = 16;
			missileReloadTime = 180;
			break;
		}

		//Subtract reload time based on missile upgrade
		missileReloadTime -= PlayerPrefs.GetInt ("MissileUpgradeLoader", 0) * 20;
		
		//If the player has bought the missile payload upgrade
		//Then increase damage
		missileDamage += PlayerPrefs.GetInt("MissileUpgradePayload",0) * 10;
	}

	void DisplayMissile() 
	{
		//Use an algorithm to map the images to the players missile reload time
		int imageValue = missileReloadTime - missileShootTimer;
		int missileMax = missileReloadTime;
		int missileMin = 1;
		int imageMax = 11;
		int imageMin = 1;
		
		//Figure out which images go with each missile value
		int imageNumber = missileMin + (imageValue-missileMin)*(imageMax-imageMin)/(missileMax-missileMin);
		
		//Only show 0 if the player has no health left
		if(missileShootTimer <= 0)
			imageNumber = 0;
		
		//Load the appropriate sprite
		missileReloadImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/MissileReload/" + imageNumber);
	}

	void DisplayBlaster() 
	{
		//Use an algorithm to map the images to the players missile reload time
		int imageValue = blasterReloadTime - blasterShootTimer;
		int blasterMax = blasterReloadTime;
		int blasterMin = 1;
		int imageMax = 11;
		int imageMin = 1;
		
		//Figure out which images go with each missile value
		int imageNumber = blasterMin + (imageValue-blasterMin)*(imageMax-imageMin)/(blasterMax-blasterMin);
		
		//Only show 0 if the player has no health left
		if(blasterShootTimer <= 0)
			imageNumber = 0;
		
		//Load the appropriate sprite
		blasterReloadImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/MissileReload/" + imageNumber);
	}

}
