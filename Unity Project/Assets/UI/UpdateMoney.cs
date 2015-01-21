/* Module      : UpdateMoney.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file updates the money in the UI
 *
 * Date        : 2015/1/21
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateMoney : MonoBehaviour {

	public Text text;

	public void UpdateText(long money) {
		text.text = money.ToString ();
	}
}
