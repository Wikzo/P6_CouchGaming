using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

	
	public int JumpForce = 350;
	public int BoostJumpForce = 350;
	public int MaxBoostJumpsAmount = 1;
	public GameObject BoostJumpEffect;

	private GameObject boostJumpEffect;
	private int boostJumpsAmount = 0;
	private bool canJump = true;
	private bool canBoostJump = false;
	private bool addJumpPhysics = false;
	private bool addBoostJumpPhysics = false;
	private Player playerScript;

	private Transform pTran;
	private float groundDetectLength;

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

		groundDetectLength = pTran.localScale.y/2;
	}

	// Update is called once per frame
	public void JumpUpdate () 
	{
		//Rays will be cast on both sides of the player, so edges are also detected
		Vector3 leftPos = pTran.position+Vector3.left*pTran.localScale.x/1.7f; //1.7 puts the ray further out, causing the player to make less mistakes
		Vector3 rightPos = pTran.position+Vector3.right*pTran.localScale.x/1.7f;

		RaycastHit hit;
		if(Physics.Raycast(pTran.position, Vector3.down, out hit, pTran.localScale.y/2) || Physics.Raycast(leftPos, Vector3.down, out hit, pTran.localScale.y/2) || Physics.Raycast(rightPos, Vector3.down, out hit, pTran.localScale.y/2))
		{
			if(hit.collider.gameObject.tag != "NotCollidable")
			{
				CanJump = true;
				boostJumpsAmount = 0;
				rigidbody.velocity = Vector3.zero;
			}
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
			//Jump();
			addJumpPhysics = true;
		}
		else if(CanBoostJump && playerScript.PlayerControllerState.ButtonDownA || CanBoostJump && playerScript.Keyboard && Input.GetKeyDown(KeyCode.Space))
		{
			//BoostJump();
			addBoostJumpPhysics = true;

			boostJumpsAmount++;

			boostJumpEffect = Instantiate(BoostJumpEffect, pTran.position, Quaternion.identity) as GameObject;
			Destroy(boostJumpEffect, 3);
		}

		//Debug.DrawRay(leftPos, Vector3.down);
		//Debug.DrawRay(rightPos, Vector3.down);
	}

	//TODO: Fix so it's the same velocity for each jump
	public void Jump()
	{
		//rigidbody.AddForce(Vector3.up*JumpForce, ForceMode.VelocityChange);
		rigidbody.velocity = new Vector3(0,JumpForce,0);

		DataSaver.Instance.highScores[0].timesJumped++;
	}
	public void BoostJump()
	{
		//rigidbody.AddForce(Vector3.up*BoostJumpForce, ForceMode.VelocityChange);
		rigidbody.velocity = new Vector3(0, BoostJumpForce, 0);

		DataSaver.Instance.highScores[0].timesJumped++;
	}

	void FixedUpdate()
	{
		if(addJumpPhysics)
		{
			Jump();
			addJumpPhysics = false;
		}
		else if(addBoostJumpPhysics)
		{
			BoostJump();
			addBoostJumpPhysics = false;
		}
	}
}
