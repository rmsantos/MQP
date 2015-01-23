/* Module      : UpdateLevel.cs
 * Author      : Josh Morse
 * Email       : jbmorse@wpi.edu
 * Course      : IMGD MQP
 *
 * Description : This file updates the level in the UI
 *
 * Date        : 2015/1/23
 * 
 *
 * (c) Copyright 2015, Worcester Polytechnic Institute.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateLevel : MonoBehaviour {

	public Text text;

	public void UpdateText(int level) {
		text.text = level.ToString ();
	}
}
