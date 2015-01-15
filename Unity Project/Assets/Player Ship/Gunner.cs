using UnityEngine;
using System.Collections;

public class Gunner : MonoBehaviour {

	public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0))
		{
			print (Input.mousePosition);

			Vector3 pos = GetComponentInParent<Transform>().position;

			float angle = Vector3.Angle(pos, Input.mousePosition);

			GameObject bullet = (GameObject)Instantiate(bulletPrefab,pos,Quaternion.identity);

			bullet.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);

			bullet.transform.rotation = Quaternion.AngleAxis (angle,Vector3.forward);

		}
	
	}
}
