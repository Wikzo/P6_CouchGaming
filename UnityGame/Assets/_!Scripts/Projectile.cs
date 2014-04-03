using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public int MaxReflections = 2;

	private int reflectionCount = 0;

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

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.GetComponent<PlayerDamage>())
		{
			if(collision.gameObject.name == Owner)
			{
				collision.gameObject.GetComponent<PlayerAim>().ShotAmount += 1;
				Destroy(gameObject);
			}
			else
				collision.gameObject.GetComponent<PlayerDamage>().CalculateDeath(tag, Owner);
		}
		else if(collision.gameObject.tag != "NotCollidable")
		{
			//VELOCITY REFLECTION:
			//if(reflectionCount < MaxReflections)
			//{
			//	Vector3 oldVelocity = rigidbody.velocity;
      		//	ContactPoint contact = collision.contacts[0];
      		//	Vector3 reflectedVelocity = Vector3.Reflect(oldVelocity, contact.normal);    
      		//	rigidbody.velocity = reflectedVelocity;
      		//	Quaternion rotation = Quaternion.FromToRotation(oldVelocity, reflectedVelocity);
      		//	transform.rotation = rotation * transform.rotation;
      		//}
      		//else
      		//{
      		//	rigidbody.velocity = Vector3.zero;
      		//	rigidbody.angularVelocity = Vector3.zero;
      		//}
      		//reflectionCount++;
      		//rigidbody.useGravity = true;
      		
      		//FORCE REFLECTION:
      		//if(reflectionCount < MaxReflections)
      		//{
      		//	ContactPoint contact = collision.contacts[0];
      		//	Vector3 reflectedForce = Vector3.Reflect(transform.right, contact.normal);
      		//	rigidbody.AddForce(reflectedForce*300);
      		//	Quaternion rotation = Quaternion.FromToRotation(transform.forward, reflectedForce);
      		//	transform.rotation = rotation * transform.rotation;
      		//}
      		//else
      		//{
      		//	rigidbody.velocity = Vector3.zero;
      		//	rigidbody.angularVelocity = Vector3.zero;
      		//}

      		//reflectionCount++;
      		////rigidbody.useGravity = true;

			
		}	
	}
}
