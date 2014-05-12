using UnityEngine;
using System.Collections;

public class StuckCollider : MonoBehaviour {

	public bool IsColliding = false;
	private GameObject parent;

	// Use this for initialization
	void Start () 
	{
		parent = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag != "NotCollidable")
			IsColliding = true;
	}
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag != "NotCollidable")
			IsColliding = false;
	}
}
