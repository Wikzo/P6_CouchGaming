using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public GameObject TwinProjectileToDestroy;
	public bool IsOriginal = true;

	public float KillVelocity = 1;
	public float DeadlyBlinkRate = 0.1f;
	public float DeadlyTimer = 0.5f;

	public int MaxReflections = 2;
	public bool VelocityReflection = false;
	public bool ForceReflection = false;

	[HideInInspector]
	public Material PMat;

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
		if(!collider.bounds.Intersects(OwnerObject.collider.bounds))
		{	
			outOfBounds = true;

			Physics.IgnoreCollision(collider, OwnerObject.collider, false);
			foreach(Transform child in transform)
				Physics.IgnoreCollision(child.collider, OwnerObject.collider, false);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		lockPos = transform.position;

		if(isDeadly && other.gameObject.GetComponent<PlayerDamage>() && other.gameObject.tag != Owner)
			other.gameObject.GetComponent<PlayerDamage>().CalculateDeath(tag, Owner);
		
		if(other.gameObject.tag == Owner && outOfBounds)
		{
			if(other.gameObject.GetComponent<PlayerAim>())
				DestroyProjectileAndTwin(other.gameObject.GetComponent<PlayerAim>());
		}
		if(!other.gameObject.GetComponent<PlayerDamage>() && other.gameObject.tag != "NotCollidable" && other.gameObject.tag != gameObject.tag)
		{
			rigidbody.velocity = Vector3.zero;
      		rigidbody.angularVelocity = Vector3.zero;
      		transform.position = lockPos;
		}
	}
	//void OnTriggerStay(Collider other)
	//{
	//	if(!other.gameObject.GetComponent<PlayerDamage>() && other.gameObject.tag != "NotCollidable" && other.gameObject.name != gameObject.name)
	//	{
	//		rigidbody.velocity = Vector3.zero;
    //  		rigidbody.angularVelocity = Vector3.zero;
    //  		transform.position = lockPos;
	//	}
	//}

	void OnDestroy()
	{
		if(TwinProjectileToDestroy != null)
			Destroy(TwinProjectileToDestroy);
	}

	void DestroyProjectileAndTwin(PlayerAim playerAim)
	{
		if (IsOriginal)
			playerAim.CurrentShotAmount++;
		
		Destroy(gameObject);
	}
}
