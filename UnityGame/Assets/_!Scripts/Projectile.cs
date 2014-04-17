using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public float KillVelocity = 1;
	public float DeadlyBlinkRate = 0.1f;
	public float DeadlyTimer = 0.5f;

	public int MaxReflections = 2;
	public bool VelocityReflection = false;
	public bool ForceReflection = false;

	[HideInInspector]
	public Material PMat;

	[HideInInspector]
	public GameObject OwnerObject;

	private bool isDeadly = false;
	private bool isHittingPlayer = false;
	private bool outOfBounds = false;

	private int reflectionCount = 0;

	private string owner;

	private Vector3 lockPos;

	public string Owner
	{
		get{return owner;}
		set{owner = value;}
	}

	// Use this for initialization
	void Start () 	
	{
		renderer.material.color = PMat.color;

		Physics.IgnoreCollision(collider, OwnerObject.collider, true);
		foreach(Transform child in transform)
			Physics.IgnoreCollision(child.collider, OwnerObject.collider, true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if(DeadlyTimer > 0)
		//{
		//	DeadlyTimer -= Time.deltaTime;
		//	isDeadly = true;
		//}
		//else
		//{
		//	//Convert to local velocity, so we know which direction the projectile is going in
		//	Vector3 localVelocity = transform.InverseTransformDirection(rigidbody.velocity);
		//	if(localVelocity.x > KillVelocity)
		//	{
		//		isDeadly = true;
		//	}
		//	else
		//	{
		//		isDeadly = false;
		//	}
		//}

		isDeadly = true;

		if(isDeadly)
		{
			float lerp = Mathf.PingPong(Time.time, DeadlyBlinkRate) / DeadlyBlinkRate;
			renderer.material.color = Color.Lerp(PMat.color, Color.red, lerp);
		}
		else
			renderer.material.color = PMat.color;


	}
	void FixedUpdate()
	{
		//Vector3 rightPos = pTran.position+Vector3.right*pTran.localScale.x/1.7f;
		RaycastHit hit;
		if(Physics.Raycast(transform.position-transform.right*transform.localScale.x/2, -transform.right, out hit, transform.localScale.x/2) || Physics.Raycast(transform.position+transform.right*transform.localScale.x/2, transform.right, out hit, transform.localScale.x/2))
		{
			lockPos = transform.position;

			if(isDeadly && hit.collider.gameObject.GetComponent<PlayerDamage>() && hit.collider.gameObject.name != Owner)
				hit.collider.gameObject.GetComponent<PlayerDamage>().CalculateDeath(tag, Owner);
			
			if(hit.collider.gameObject.name == Owner && outOfBounds)
			{
				hit.collider.gameObject.GetComponent<PlayerAim>().CurrentShotAmount++;
					Destroy(gameObject);
			}
			if(!hit.collider.gameObject.GetComponent<PlayerDamage>() && hit.collider.gameObject.tag != "NotCollidable")
			{
				rigidbody.velocity = Vector3.zero;
      			rigidbody.angularVelocity = Vector3.zero;
      			transform.position = lockPos;
			}
		}

		if(!collider.bounds.Intersects(OwnerObject.collider.bounds))
		{	
			outOfBounds = true;

			Physics.IgnoreCollision(collider, OwnerObject.collider, false);
			foreach(Transform child in transform)
				Physics.IgnoreCollision(child.collider, OwnerObject.collider, false);
		}
	}
	//Not working version
	/*void FixedUpdate()
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position+transform.right, transform.right, out hit, transform.localScale.x))
		{
			if(hit.collider.gameObject.GetComponent<PlayerDamage>())
			{
				collider.isTrigger = true;
			}
			else if(hit.collider.gameObject.tag != "NotCollidable")
			{
				collider.isTrigger = false;
			}
		}
		else
			collider.isTrigger = false;
	}


	void OnTriggerEnter(Collider other)
	{
		if(isDeadly && other.gameObject.GetComponent<PlayerDamage>() && other.gameObject.name != Owner)
		{
			other.gameObject.GetComponent<PlayerDamage>().CalculateDeath(tag, Owner);
		}
	}


	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag != "NotCollidable")
		{
			if(collision.gameObject.name == Owner)
			{
				collision.gameObject.GetComponent<PlayerAim>().CurrentShotAmount++;
					Destroy(gameObject);
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
	}*/

	//Working version
	/*void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.tag != "NotCollidable")
		{
			if(isDeadly && collision.gameObject.GetComponent<PlayerDamage>() && collision.gameObject.name != Owner)
			{
				collision.gameObject.GetComponent<PlayerDamage>().CalculateDeath(tag, Owner);
			}
			else if(collision.gameObject.name == Owner)
			{
				collision.gameObject.GetComponent<PlayerAim>().CurrentShotAmount++;
					Destroy(gameObject);
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
	}*/

	//LaserDisk continuing in the determined direction
	/*void OnTriggerStay(Collider other)
	{
		lockPos = transform.position;
		if(isDeadly && other.gameObject.GetComponent<PlayerDamage>() && other.gameObject.name != Owner)
		{
			other.gameObject.GetComponent<PlayerDamage>().CalculateDeath(tag, Owner);
		}
		else if(other.gameObject.name == Owner)
		{
			other.gameObject.GetComponent<PlayerAim>().CurrentShotAmount++;
				Destroy(gameObject);
		}
		else if(!other.gameObject.GetComponent<PlayerDamage>() && other.gameObject.tag != "NotCollidable")
		{
			rigidbody.velocity = Vector3.zero;
      		rigidbody.angularVelocity = Vector3.zero;
      		transform.position = lockPos;
		}
	}*/
}
