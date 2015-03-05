using UnityEngine;
using System.Collections;

public class ResizeGame : MonoBehaviour {

	//Stores the boundaries of the game
	Boundaries boundaries;

	// Use this for initialization
	void Start () {
		//Pull the boundaries script from the main camera object and store it
		boundaries = Camera.main.GetComponent<Boundaries>();
	}
	
	// Update is called once per frame
	void Update () {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		
		float worldScreenHeight = Camera.main.orthographicSize * 2;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
		transform.localScale = new Vector3(
			worldScreenHeight / sr.sprite.bounds.size.y * 3,
			worldScreenHeight / sr.sprite.bounds.size.y, 1);
	}
}
