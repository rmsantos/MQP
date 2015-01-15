using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	bool set;
	float distance;

	// Use this for initialization
	void Start () {
	
		set = false;
		distance = 0;

	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetMouseButtonDown(0))
		{
			distance = Vector3.Distance (GameObject.FindGameObjectWithTag("Ship").transform.position,Input.mousePosition);
			set = true;
		}

		if(set)
		{
			transform.position = new Vector3(transform.position.x+(1/distance),transform.position.y + (1/distance),transform.position.z);
		}

	}
}
