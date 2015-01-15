using UnityEngine;
using System.Collections;

public class ShipMovement : MonoBehaviour {

	public float xPos;
	public float yPos;

	// Use this for initialization
	void Start () {

		xPos = transform.position.x;
		yPos = transform.position.y;

	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown("w") || Input.GetKeyDown ("up"))
		{
			if(yPos < 6)
				yPos++;

		}
		
		if(Input.GetKeyDown("s") || Input.GetKeyDown ("down"))
		{
			if(yPos > -5)
				yPos--;
		}

		if(Input.GetKeyDown("a") || Input.GetKeyDown ("left"))
		{
			if(xPos > -10)
				xPos--;
		}

		if(Input.GetKeyDown("d") || Input.GetKeyDown ("right"))
		{
			if(xPos < 10)
				xPos++;

		}

		transform.position = new Vector3 (xPos, yPos, transform.position.z);
	}
}
