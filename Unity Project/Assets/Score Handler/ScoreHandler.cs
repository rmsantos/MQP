/* Module      : ScoreHandler.cs
 * Author      : Josh Morse
 * Email       : rmsantos@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file controls the movement of the player ship.
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

	public UpdateMoney updateMoney;

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

		score = 0;
		money = 0;

		//Search for the ScoreHandler object for tracking score
		updateMoney = GameObject.FindGameObjectWithTag ("MoneyText").GetComponent<UpdateMoney>(); 
	}

	public long UpdateScore(long amount) {
		score += amount;
		print ("Score: " + score);
		return score;
	}

	public long UpdateMoney(long amount) {
		money += amount;

		updateMoney.UpdateText (money);

		return money;
	}

	public long GetScore() {
		return score;
	}

	public long GetMoney() {
		return money;
	}

	public void ResetScore() {
		score = 0;
	}

	public void ResetMoney() {
		money = 0;

		updateMoney.UpdateText (money);

	}

}
