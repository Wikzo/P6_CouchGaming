using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public float DeadlyTime = 1;
	public float DeadlyBlinkRate = 0.1f;

	public int MaxReflections = 2;
	public bool VelocityReflection = false;
	public bool ForceReflection = false;

	[HideInInspector]
	public Material PMat;

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
		if(DeadlyTime > 0)
		{
			DeadlyTime -= Time.deltaTime;

			float lerp = Mathf.PingPong(Time.time, DeadlyBlinkRate) / DeadlyBlinkRate;
			renderer.material.color = Color.Lerp(PMat.color, Color.red, lerp);
		}
		else
		{
			renderer.material.color = PMat.color;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag != "NotCollidable")
		{
			if(collision.gameObject.GetComponent<PlayerDamage>())
			{
				if(DeadlyTime > 0)
				{
					collision.gameObject.GetComponent<PlayerDamage>().CalculateDeath(tag, Owner);
				}
				else if(collision.gameObject.name == Owner)
				{
					collision.gameObject.GetComponent<PlayerAim>().CurrentShotAmount++;
					Destroy(gameObject);
				}
			}

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
      		
		}	
	}
}
