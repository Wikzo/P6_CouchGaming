using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

	public int JumpForce = 350;

	private bool canJump = true;
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
	void Update () 
	{
		if(CanJump && controllerState.ButtonDownA)
		{
			rigidbody.AddForce(Vector3.up*JumpForce);
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
