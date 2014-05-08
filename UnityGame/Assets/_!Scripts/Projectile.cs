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

	public bool OutOfBounds
	{
		get{return outOfBounds;}
		set{outOfBounds = value;}
	}

	// Use this for initialization
	void Start () 	
	{
		renderer.material.color = PMat.color;

		if(OwnerObject.transform.Find("ForwardCollider") != null)
			Physics.IgnoreCollision(collider, OwnerObject.transform.Find("ForwardCollider").collider, true);

		Physics.IgnoreCollision(collider, OwnerObject.collider, true); //Make sure that the player's physics are not affected by the collider of the projectile
		gameObject.tag = "NotCollidable"; //Make sure that the player's raycasting in playerMove and playerJump is not affected by the collider of the projectile
		foreach(Transform child in transform)
		{
			Physics.IgnoreCollision(collider, child.collider, true);
			Physics.IgnoreCollision(child.collider, OwnerObject.collider, true);

			child.gameObject.tag = "NotCollidable";
		}
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
			OutOfBounds = true;

			Physics.IgnoreCollision(collider, OwnerObject.collider, false);
			gameObject.tag = "Projectile";
			foreach(Transform child in transform)
			{
				Physics.IgnoreCollision(collider, child.collider, false);
				Physics.IgnoreCollision(child.collider, OwnerObject.collider, false);

				child.gameObject.tag = "Untagged";
			}			
		}
	}

	void OnTriggerEnter(Collider other)
	{
		lockPos = transform.position;

		if(isDeadly && other.gameObject.GetComponent<PlayerDamage>() && other.gameObject.tag != Owner)
			other.gameObject.GetComponent<PlayerDamage>().CalculateDeath(Owner);
		
		if(other.gameObject.tag == Owner && OutOfBounds)
		{
            if (other.gameObject.GetComponent<PlayerAim>())
            {
                if (!IsOriginal)
                    return;

                DestroyProjectileAndTwin(other.gameObject.GetComponent<PlayerAim>());
            }
		}
		if(!other.gameObject.GetComponent<PlayerDamage>() && other.gameObject.tag != "NotCollidable" && other.gameObject.tag != gameObject.tag)
		{
			rigidbody.velocity = Vector3.zero;
      		rigidbody.angularVelocity = Vector3.zero;
      		transform.position = lockPos;
		}
	}

	//void OnTriggerExit(Collider other)
	//{
	//	if(other.gameObject.tag == Owner)
	//	{
	//		outOfBounds = true;
//
	//		Physics.IgnoreCollision(collider, OwnerObject.collider, false);
	//		foreach(Transform child in transform)
	//		{
	//			Physics.IgnoreCollision(child.collider, OwnerObject.collider, false);
	//		}
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
