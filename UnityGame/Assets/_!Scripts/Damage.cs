using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void CalculateDeath(string killObject)
	{
		if(killObject == "LaserBullet(Clone)")
		{
			Destroy(gameObject);
		}
	}
}
