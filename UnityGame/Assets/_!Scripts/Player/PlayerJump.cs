﻿using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

	public int JumpForce = 350;
	public float GroundDetectLength = 0.5f;
	public float GroundDetectOffset = 0.7f;

	private bool canJump = true;
	private Player playerScript;

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

		playerScript = GetComponent<Player>();
	}

	// Update is called once per frame
	void Update () 
	{
		//Rays will be cast on both sides of the player, so edges are also detected
		Vector3 leftPos = pTran.position+Vector3.left*GroundDetectOffset;
		Vector3 rightPos = pTran.position+Vector3.right*GroundDetectOffset;

		if(Physics.Raycast(pTran.position, Vector3.down, GroundDetectLength) || Physics.Raycast(leftPos, Vector3.down, GroundDetectLength) || Physics.Raycast(rightPos, Vector3.down, GroundDetectLength))
		{
			CanJump = true;
		}
		else
		{
			CanJump = false;
		}

		//Debug.DrawRay(leftPos, Vector3.down);
		//Debug.DrawRay(rightPos, Vector3.down);
	}

	void FixedUpdate()
	{
		if(CanJump && controllerState.ButtonDownA || CanJump && playerScript.Keyboard && Input.GetKeyDown(KeyCode.Space))
		{
			rigidbody.AddForce(Vector3.up*JumpForce);

		    DataSaver.Instance.highScores[0].timesJumped++;
		}
	}
}
