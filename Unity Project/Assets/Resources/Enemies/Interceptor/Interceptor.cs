using UnityEngine;
using System.Collections;

public class Interceptor : MonoBehaviour, BasicEnemy {

	// Use this for initialization
	void Start () {

		gameObject.transform.Rotate(90, 180, 0);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void takeDamage(int damage)
	{
		print ("Interceptor!");
	}

	public int getCollisionDamage()
	{
		return 0;
	}

}
