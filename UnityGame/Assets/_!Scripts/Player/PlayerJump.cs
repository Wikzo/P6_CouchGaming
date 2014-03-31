using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

	private bool canJump = false;
	private ControllerState controllerState;

	public bool CanJump
	{
		get{return canJump;}
		set{canJump = value;}	
	}

	// Use this for initialization
	void Start () 
	{
		controllerState = GetComponent<ControllerState>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(CanJump && controllerState.ButtonDownA)
		{
			rigidbody.AddForce(Vector3.up * 350);
		}
	}

	void OnCollisionStay(Collision collision)
	{
		CanJump = true;
	}
	void OnCollisionExit(Collision collision)
	{
		CanJump = false;
	}
}
