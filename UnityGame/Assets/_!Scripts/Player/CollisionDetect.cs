using UnityEngine;
using System.Collections;

public class CollisionDetect : MonoBehaviour {

	public bool IsColliding = false;

	private Animator anim;

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other)
	{
		if(other.collider.gameObject.tag != "NotCollidable")
		{
			IsColliding = true;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if(other.collider.gameObject.tag != "NotCollidable")
		{
			IsColliding = false;
		}
	}
}
