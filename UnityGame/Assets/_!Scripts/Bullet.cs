using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	private string owner;

	public string Owner
	{
		get{return owner;}
		set{owner = value;}
	}


	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.GetComponent<PlayerDamage>())
		{
			collider.gameObject.GetComponent<PlayerDamage>().CalculateDeath(tag, owner);
		}
		if(collider.gameObject.tag != "NotCollidable")
		{
			Destroy(gameObject);
		}
	}
}
