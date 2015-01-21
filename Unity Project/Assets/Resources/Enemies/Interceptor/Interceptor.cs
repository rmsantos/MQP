using UnityEngine;
using System.Collections;

public class Interceptor : BasicEnemy {

	// Use this for initialization
	void Start () {

		gameObject.transform.Rotate(90, 180, 0);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public override void takeDamage(int damage)
	{
		print ("Interceptor!");
	}

	public override int getCollisionDamage()
	{
		return 0;
	}

}
