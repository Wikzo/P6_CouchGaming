using UnityEngine;
using System.Collections;

public class CollisionDetect : MonoBehaviour {

	public bool IsColliding = false;

	void OnTriggerStay(Collider other)
	{
		if(other.collider.gameObject.tag != "NotCollidable")
			IsColliding = true;
        
	}

	void OnTriggerExit(Collider other)
	{
		if(other.collider.gameObject.tag != "NotCollidable")
			IsColliding = false;

	}
}
