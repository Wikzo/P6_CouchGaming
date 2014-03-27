using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

	private bool canJump = false;
	private ControllerState controllerState;

	// Use this for initialization
	void Start () 
	{
		controllerState = GetComponent<ControllerState>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(canJump && controllerState.ButtonDownA())
		{
			rigidbody.AddForce(Vector3.up * 350);
		}
		controllerState.PreviousState = controllerState.GetCurrentState();
	}

	void OnCollisionStay(Collision collision)
	{
		CanJump(true);
	}
	void OnCollisionExit(Collision collision)
	{
		CanJump(false);
	}

	void CanJump(bool messageBool)
	{
		canJump = messageBool;	
	}
}
