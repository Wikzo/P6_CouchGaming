using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

	
	public int JumpForce = 350;
	public int BoostJumpForce = 350;
	public int MaxBoostJumpsAmount = 1;
	public GameObject BoostJumpEffect;
	public float GroundDetectLength = 0.5f;
	public float GroundDetectOffset = 0.7f;

	private GameObject boostJumpEffect;
	private int boostJumpsAmount = 0;
	private bool canJump = true;
	private bool canBoostJump = false;
	private Player playerScript;

	private Transform pTran;

	public bool CanJump
	{
		get{return canJump;}
		set{canJump = value;}	
	}
	public bool CanBoostJump
	{
		get{return canBoostJump;}
		set{canBoostJump = value;}	
	}

	// Use this for initialization
	void Start () 
	{
		pTran = transform;

		playerScript = GetComponent<Player>();
	}

	// Update is called once per frame
	public void JumpUpdate () 
	{
		//Rays will be cast on both sides of the player, so edges are also detected
		Vector3 leftPos = pTran.position+Vector3.left*GroundDetectOffset;
		Vector3 rightPos = pTran.position+Vector3.right*GroundDetectOffset;

		if(Physics.Raycast(pTran.position, Vector3.down, GroundDetectLength) || Physics.Raycast(leftPos, Vector3.down, GroundDetectLength) || Physics.Raycast(rightPos, Vector3.down, GroundDetectLength))
		{
			CanJump = true;
			boostJumpsAmount = 0;
		}
		else //We are in the air
		{
			CanJump = false;
			if(boostJumpsAmount < MaxBoostJumpsAmount)
				CanBoostJump = true;
			else
				CanBoostJump = false;
		}

		if(CanJump && playerScript.PlayerControllerState.ButtonDownA || CanJump && playerScript.Keyboard && Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}
		else if(CanBoostJump && playerScript.PlayerControllerState.ButtonDownA || CanBoostJump && playerScript.Keyboard && Input.GetKeyDown(KeyCode.Space))
		{
			BoostJump();
			boostJumpsAmount++;

			boostJumpEffect = Instantiate(BoostJumpEffect, pTran.position, Quaternion.identity) as GameObject;
			Destroy(boostJumpEffect, 3);
		}

		//Debug.DrawRay(leftPos, Vector3.down);
		//Debug.DrawRay(rightPos, Vector3.down);
	}

	public void Jump()
	{
		//THIS SHOULD PROBABLY BE CALLED FROM FIXED UPDATE:
		rigidbody.AddForce(Vector3.up*JumpForce);

		DataSaver.Instance.highScores[0].timesJumped++;
	}
	public void BoostJump()
	{
		//THIS SHOULD PROBABLY BE CALLED FROM FIXED UPDATE:
		rigidbody.AddForce(Vector3.up*BoostJumpForce);

		DataSaver.Instance.highScores[0].timesJumped++;
	}
}
