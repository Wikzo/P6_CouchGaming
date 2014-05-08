using UnityEngine;
using System.Collections;

public class CollisionDetect : MonoBehaviour {

	public bool IsColliding = false;
	private Collider other;
	private bool collidingWithProjectile = false;

	void OnTriggerStay(Collider other)
	{
		if(other.collider.gameObject.tag != "NotCollidable")
			IsColliding = true;

		//Make an extra check for the projectile, as it can be deleted, if it is our own, making it impossible to detect an OnTriggerExit.
		//Two checks here are needed to both check if we are hitting the laser itself and the plates below and above.
		if(other.gameObject.GetComponent<Projectile>() || other.gameObject.transform.parent != null && other.gameObject.transform.parent.GetComponent<Projectile>())
		{
			collidingWithProjectile = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.collider.gameObject.tag != "NotCollidable")
			IsColliding = false;

		//Make an extra check for the projectile, as it can be deleted, if it is our own, making it impossible to detect an OnTriggerExit.
		//Two checks here are needed to both check if we are hitting the laser itself and the plates below and above.
		if(other.gameObject.GetComponent<Projectile>() || other.gameObject.transform.parent != null && other.gameObject.transform.parent.GetComponent<Projectile>())
		{
			collidingWithProjectile = false;
		}
	}

	void Start()
	{


	}

	void Update()
	{
		if(collidingWithProjectile == true && !other)
		{
			IsColliding = false;
			collidingWithProjectile = false;
		}
	}
}
