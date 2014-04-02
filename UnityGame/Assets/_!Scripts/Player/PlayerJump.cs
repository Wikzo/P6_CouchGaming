using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

	public int JumpForce = 350;
	public float GroundDetectLength = 0.5f;

	private bool canJump = true;
	private ControllerState controllerState;

	private Transform pTran;

	public bool CanJump
	{
		get{return canJump;}
		set{canJump = value;}	
	}

	// Use this for initialization
	void Start () 
	{
		controllerState = GetComponent<ControllerState>();
		pTran = transform;
	}

	// Update is called once per frame
	void Update () 
	{
		//Rays will be cast on both sides of the player, so edges are also detected
		Vector3 leftPos = pTran.position+Vector3.left*0.5f;
		Vector3 rightPos = pTran.position+Vector3.right*0.5f;

		if(Physics.Raycast(pTran.position, Vector3.down, 0.5f) || Physics.Raycast(leftPos, Vector3.down, 0.5f) || Physics.Raycast(rightPos, Vector3.down, 0.5f))
		{
			CanJump = true;
		}
		else
		{
			CanJump = false;
		}
	}

	void FixedUpdate()
	{
		if(CanJump && controllerState.ButtonDownA)
		{
			rigidbody.AddForce(Vector3.up*JumpForce);
		}
	}
}
