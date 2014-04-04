using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public int MaxReflections = 2;
	public bool VelocityReflection = false;
	public bool ForceReflection = false;

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
		if(collision.gameObject.tag != "NotCollidable")
		{
			//VELOCITY REFLECTION:
			if(VelocityReflection)
			{
				if(reflectionCount < MaxReflections)
				{
					Vector3 oldVelocity = rigidbody.velocity;
      				ContactPoint contact = collision.contacts[0];
      				Vector3 reflectedVelocity = Vector3.Reflect(oldVelocity, contact.normal);    
      				rigidbody.velocity = reflectedVelocity;
      				Quaternion rotation = Quaternion.FromToRotation(oldVelocity, reflectedVelocity);
      				transform.rotation = rotation * transform.rotation;
      			}
      			else
      			{
      				rigidbody.velocity = Vector3.zero;
      				rigidbody.angularVelocity = Vector3.zero;
      			}
      			reflectionCount++;
      			//rigidbody.useGravity = true;
      		}
      		
      		//FORCE REFLECTION:
      		else if(ForceReflection)
      		{
      			if(reflectionCount < MaxReflections)
      			{
      				ContactPoint contact = collision.contacts[0];
      				Vector3 reflectedForce = Vector3.Reflect(transform.right, contact.normal);
      				rigidbody.AddForce(reflectedForce*300);
      				Quaternion rotation = Quaternion.FromToRotation(transform.forward, reflectedForce);
      				transform.rotation = rotation * transform.rotation;
      			}
      			else
      			{
      				rigidbody.velocity = Vector3.zero;
      				rigidbody.angularVelocity = Vector3.zero;
      			}
      			reflectionCount++;
      		}
      		else
      		{
      			rigidbody.velocity = Vector3.zero;
      			rigidbody.angularVelocity = Vector3.zero;
      		}
      		//rigidbody.useGravity = true;
		}	
	}
}
