/* Module      : UpgradeMenu.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the behavior of the upgrade menu
 *
 * Date        : 2015/1/30
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

public class UpgradeMenu : MonoBehaviour {

	//Upgrade management variables
	delegate void Upgrade();

	Upgrade[] upgradeFunction;

	public int[] engine1Cost;
	public int[] blaster1Cost;
	public int[] blaster2Cost;
	public int[] shields1Cost;
	public int[] shields2Cost;
	public int[] shields3Cost;
	public int[] power1Cost;
	public int[] missiles1Cost;
	public int[] missiles2Cost;
	public int[] cargo1Cost;
	public int[] cargo2Cost;
	public int[] cargo3Cost;
	public int[] hull1Cost;
	public int[] hull2Cost;
	public int[] lasers1Cost;
	public int[] lasers2Cost;
	public int[] lasers3Cost;

	public Image healthImage;

	public Button[] selectButtons;

	int[][] costs;

	int[] upgradeLevel;

	string[] upgradePrefs = {"EngineUpgrade", 
		"BlasterUpgradeFireRate", "BlasterUpgradeDamage", 
		"ShieldUpgradeNumber", "ShieldUpgradeRecharge", "ShieldUpgradeHardened",
		"PowerUpgrade", 
		"MissileUpgradeLoader", "MissileUpgradePayload",  
		"CargoUpgradeMissiles", "CargoUpgradeCrystals", "CargoUpgradeCredits",
		"HullUpgradeReinforced", "HullUpgradeAsteroidResistance", 
		"LaserUpgradeSpeed", "LaserUpgradeDamage", "LaserUpgradeBurst"};
	
	public string[] descriptions;
	char[][] descriptionChars;

	//Flags on whether to start the game
	bool startGame;
	
	//The buttons available to press
	public Button startButton;
	public Button purchaseButton;
	public Button repairButton;

	//The dynamic text found on the screen
	public Text moneyText;
	public Text crystalText;
	public Text missileText;
	public Text descriptionText;
	public Text scoreText;
	public Text levelText;
	public Text[] upgradeCosts;

	//Player variables that are displayed
	int money;
	int missiles;
	int crystals;
	int score;
	int playerHealth;

	//The actively selected upgrade (very important)
	int selected;

	//The counter to keep track of the typing
	int typeCounter;

	//flag if the first time with this text
	bool done;

	//Purcahse animation
	public Image purchaseAnimation;

	//Animation count
	int animationCount;

	//Flag if the animation is done
	bool animationDone;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Initializes the start and quit bools
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Start false as default
		done = false;
		animationDone = false;

		//Initialize selected to -1 to flag no action
		selected = -1;

		//Initialize the type counter
		typeCounter = 0;

		//Initialize the animation count
		animationCount = 0;

		//Initialize the char array
		descriptionChars = new char[17][];

		//Cast each description to a char array
		for(int x = 0; x < 17; x++)
			descriptionChars[x] = descriptions[x].ToCharArray();

		//Populate the upgrade functions
		upgradeFunction = new Upgrade[] {UpgradeEngine1,
										   UpgradeBlaster1, UpgradeBlaster2,
										   UpgradeShields1, UpgradeShields2, UpgradeShields3,
										   UpgradePower1, 
										   UpgradeMissiles1, UpgradeMissiles2,
										   UpgradeCargo1, UpgradeCargo2, UpgradeCargo3,
										   UpgradeHull1, UpgradeHull2,
										   UpgradeLasers1, UpgradeLasers2, UpgradeLasers3};

		//Populate the cost arrays
		costs = new int[][] {engine1Cost, 
							blaster1Cost, blaster2Cost, 
							shields1Cost, shields2Cost, shields3Cost, 
							power1Cost,
							missiles1Cost, missiles2Cost, 
							cargo1Cost, cargo2Cost, cargo3Cost, 
							hull1Cost, hull2Cost, 
							lasers1Cost, lasers2Cost, lasers3Cost};

		//Initialize the player values from playerprefs
		money = PlayerPrefs.GetInt ("Money", 0);
		missiles = PlayerPrefs.GetInt ("Missiles", 0);
		crystals = PlayerPrefs.GetInt ("Crystals", 0);
		score = PlayerPrefs.GetInt ("Score", 0);
		playerHealth = PlayerPrefs.GetInt ("Health", 0);

		//Display the player values
		moneyText.text = money.ToString();
		scoreText.text = score.ToString();
		missileText.text = missiles.ToString();
		crystalText.text = crystals.ToString();
		DisplayHealth ();
		
		//Setup and retreive the previously bought upgrade levels
		upgradeLevel = new int[upgradePrefs.Length];
		for (int i = 0; i < upgradePrefs.Length; i++) {
			upgradeLevel[i] = PlayerPrefs.GetInt (upgradePrefs[i], 0);
			if (costs[i].GetLength(0) > upgradeLevel[i]) {
				upgradeCosts[i].text = costs[i][upgradeLevel[i]].ToString();
			}
			else {
				upgradeCosts[i].text = "MAX";
			}
		}

		//Initialize start state to false
		startGame = false;

		//Selected should be set to a value that isn't an upgrade value, so -1
		selected = -1;
		purchaseButton.interactable = false;

		//Check for special case button disables
		if (upgradeLevel[3] < 1) {
			selectButtons[4].interactable = false;
			selectButtons[5].interactable = false;
		}
		if (upgradeLevel[1] < 1) {
			selectButtons[2].interactable = false;
		}

		print (PlayerPrefs.GetFloat ("VictoryLocation", 0));

		//Play the victory music where it left off on the game screen
		Camera.main.audio.time = PlayerPrefs.GetFloat ("VictoryLocation", 0);
		Camera.main.audio.Play();
	}
	
	/* ----------------------------------------------------------------------- */
	/* Function    : Update()
	 *
	 * Description : Starts the game when the button is pressed
	 * 				and the sound clip stops playing.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Update () {

		//If the user clicked start and the audio file is done
		if(startGame && !startButton.audio.isPlaying)
		{
			PlayerPrefs.SetInt("Money", money);
			PlayerPrefs.SetInt("Missiles", missiles);
			PlayerPrefs.SetInt("Crystals", crystals);
			PlayerPrefs.SetInt("Score", score);
			PlayerPrefs.SetInt("Health", playerHealth);
			//Load the main game
			Application.LoadLevel (4);
		}

		//If the player has something selected and the text is not done writing
		if(selected > -1 && !done)
		{
			//Write the text one letter at a time
			descriptionText.text += descriptionChars[selected][typeCounter];

			//Increase the index
			typeCounter++;

			//If hitting the end
			if(typeCounter == descriptions[selected].Length)
			{
				//Flag the end
				done = true;

				//And reset the index
				typeCounter = 0;
			}
		}

		//If the player has clicked a button
		if(selected > -1 && !animationDone)
		{
			//Load the new sprite in the animation
			//Load the sprite
			purchaseAnimation.GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> ("UI Sprites/Purchase/purchase_" + animationCount);

			//Increment the counter
			animationCount++;

			//If hitting the end of the animation
			if(animationCount == 10)
			{
				//Flag the end
				animationDone = true;

				//And reset the count
				animationCount = 0;

				//Load the default image
				purchaseAnimation.GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> ("UI Sprites/Purchase/purchase_0");
			}
		}

		//If the player health is full, then grey out the repair hull button
		if(PlayerPrefs.GetInt("Health",0) == 100)
			repairButton.interactable = false;
		else
			repairButton.interactable = true;
	}

	/* ----------------------------------------------------------------------- */
	/* Function    : setStart()
	 *
	 * Description : Sets the start bool to true
	 *
	 * Parameters  : bool start : Start the game?
	 *
	 * Returns     : Void
	 */
	public void setStart(bool start)
	{
		startGame = start;
	}

	public void Purchase() {

		if (costs[selected][upgradeLevel[selected]] <= money) {

			//Do general "every upgrade" purchasing logic
			money -= costs[selected][upgradeLevel[selected]];
			moneyText.text = money.ToString();
			upgradeLevel[selected] = upgradeLevel[selected] + 1;
			PlayerPrefs.SetInt(upgradePrefs[selected], upgradeLevel[selected]);
			Select(selected);
			descriptionText.text = "Upgrade purchased!";

			//Flag not to add text
			typeCounter = 0;
			done = true;
				
			//Do the specific upgrade logic
			upgradeFunction[selected]();

		}
		else {
			//Flag not to add text
			typeCounter = 0;
			done = true;

			descriptionText.text = "Not enough money!";
		}

	}

	public void Select(int select) {

		//Set selected variable to appropriate value
		selected = select;

		//Flag that theres text to be written and reset the count
		done = false;
		typeCounter = 0;

		//Also reset the animation count
		animationDone = false;
		animationCount = 0;

		//Clear the description box
		descriptionText.text = "";

		//Display the appropriate cost 
		if (costs[selected].GetLength(0) > upgradeLevel[selected]) {
			upgradeCosts[selected].text = costs[selected][upgradeLevel[selected]].ToString();
			purchaseButton.interactable = true;
		}
		else {
			upgradeCosts[selected].text = "MAX";
			purchaseButton.interactable = false;
		}

		//Set descriptive texts
		levelText.text = "lvl: " + upgradeLevel[selected].ToString();

	}

	public void UpgradeEngine1() {

	}

	public void UpgradeBlaster1() {
		selectButtons[2].interactable = true;
	}

	public void UpgradeBlaster2() {

	}

	public void UpgradeShields1() {
		selectButtons[4].interactable = true;
		selectButtons[5].interactable = true;
	}

	public void UpgradeShields2() {

	}

	public void UpgradeShields3() {

	}

	public void UpgradePower1() {

	}

	public void UpgradeMissiles1() {

	}

	public void UpgradeMissiles2() {

	}

	public void UpgradeCargo1() {

	}

	public void UpgradeCargo2() {

	}

	public void UpgradeCargo3() {

	}

	public void UpgradeHull1() {

	}

	public void UpgradeHull2() {

	}

	public void UpgradeLasers1() {

	}

	public void UpgradeLasers2() {

	}

	public void UpgradeLasers3() {

	}

	//Used to purchase crystal
	public void PurchaseCrystal() {

		if (money >= 4 && crystals < 5 + (PlayerPrefs.GetInt("CargoUpgradeCrystals", 0) * 5)) {
			crystals++;
			money -= 4;
			moneyText.text = money.ToString();
			crystalText.text = crystals.ToString();
		}

	}

	//Used to purchase missile
	public void PurchaseMissile() {

		if (money >= 1 && missiles < 5 + (PlayerPrefs.GetInt("CargoUpgradeMissiles", 0) * 5)) {
			missiles++;
			money -= 1;
			moneyText.text = money.ToString ();
			missileText.text = missiles.ToString ();
		}

	}

	//Used to sell missile
	public void SellMissile() {

		if (missiles >= 1) {
			missiles--;
			money += 1;
			moneyText.text = money.ToString();
			missileText.text = missiles.ToString();
		}

	}

	//Used to sell crystal
	public void SellCrystal() {

		if (crystals >= 1) {
			crystals--;
			money += 4;
			moneyText.text = money.ToString();
			crystalText.text = crystals.ToString();
		}

	}

	//Used to increase score
	public void PurchaseScore() {

		if (money >= 25) {
			score += 100;
			money -= 25;
			moneyText.text = money.ToString();
			scoreText.text = score.ToString();
		}
		
	}

	//Used to repair hull
	public void RepairHull() {

		if (money >= 5 && playerHealth < 100) {

			money -= 5;
			playerHealth += 25;
			if (playerHealth > 100) {
				playerHealth = 100;
			}
			moneyText.text = money.ToString();
			DisplayHealth();

		}

	}

	void DisplayHealth() 
	{
		//Use an algorithm to map the images to the players health
		int imageValue = playerHealth;
		int healthMax = 100;
		int healthMin = 1;
		int imageMax = 23;
		int imageMin = 1;
		
		//Figure out which images go with each health value
		int imageNumber = healthMin + (imageValue-healthMin)*(imageMax-imageMin)/(healthMax-healthMin);
		
		//Only show 0 if the player has no health left
		if(playerHealth <= 0)
			imageNumber = 0;
		
		//Load the appropriate sprite
		healthImage.sprite = Resources.Load<UnityEngine.Sprite> ("UI Sprites/Health/" + imageNumber);
	}

	public int getSelected()
	{
		return selected;
	}

}
