/* Module      : ScoreHandler.cs
 * Author      : Josh Morse
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the score and money handling of the game.
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

public class ScoreHandler : MonoBehaviour {
	
	public static long score;
	public static long money;
	public static long crystals;

	static UpdateMoney updateMoney;
	static UpdateCrystal updateCrystal;
	static UpdateScore updateScore;

	/* ----------------------------------------------------------------------- */
	/* Function    : Start()
	 *
	 * Description : Stores the X and Y positions of the ship in global variables.
	 * 				Initializes acceleration to 0, the held flags to false, and the
	 * 				boundaries to their vector locations.
	 *
	 * Parameters  : None
	 *
	 * Returns     : Void
	 */
	void Start () {

		//Pull the values from player prefs
		score = PlayerPrefs.GetInt ("Score", 99999);
		money = PlayerPrefs.GetInt ("Money", 99999);
		crystals = PlayerPrefs.GetInt ("Crystals", 99999);

		//Search for the ScoreHandler object for tracking score
		updateMoney = GameObject.Find("/UI/MoneyText").GetComponent<UpdateMoney>(); 
		updateCrystal = GameObject.Find("/UI/CrystalText").GetComponent<UpdateCrystal>(); 
		updateScore = GameObject.Find("/UI/ScoreText").GetComponent<UpdateScore>(); 

		updateScore.UpdateText (score);
		updateMoney.UpdateText (money);
		updateCrystal.UpdateText (crystals);
	}

	public long UpdateScore(long amount) {
		score += amount;

		updateScore.UpdateText (score);

		return score;
	}

	public long UpdateMoney(long amount) {
		money += amount;

		updateMoney.UpdateText (money);

		return money;
	}

	public long UpdateCrystals(long amount) {
		crystals += amount;
		
		updateCrystal.UpdateText (crystals);
		
		return crystals;
	}

	public long GetScore() {
		return score;
	}

	public long GetMoney() {
		return money;
	}

	public long GetCrystals() {
		return crystals;
	}

	public void ResetScore() {
		score = 0;

		updateScore.UpdateText (score);
	}

	public void ResetMoney() {
		money = 0;

		updateMoney.UpdateText (money);
	}

	public void ResetCrystals() {
		crystals = 0;
		
		updateCrystal.UpdateText (crystals);
	}


}
