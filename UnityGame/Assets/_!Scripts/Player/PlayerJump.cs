using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

	public GameObject SoundObject;
	private GameObject soundObject;
	
	public int JumpForce = 350;
	public int BoostJumpForce = 350;
	public int MaxBoostJumpsAmount = 1;
	public GameObject BoostJumpEffect;
	private GameObject boostJumpEffect;
	private int boostJumpsAmount = 0;
	private bool canJump = true;
	private bool canBoostJump = false;
	private bool hasJumped = false;
	private bool hasDoubleJumped = false;
	private bool readyToLand = false;
	private bool isLanding = false;
	private bool addJumpPhysics = false;
	private bool addBoostJumpPhysics = false;
	private Player playerScript;

	private Transform pTran;
	private float groundDetectLength;

	private CollisionDetect downCollider;

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
	public bool HasJumped
	{
		get{return hasJumped;}
		set{hasJumped = value;}	
	}
	public bool HasDoubleJumped
	{
		get{return hasDoubleJumped;}
		set{hasDoubleJumped = value;}	
	}
	public bool IsLanding
	{
		get{return isLanding;}
		set{isLanding = value;}
	}

	// Use this for initialization
	void Start () 
	{
		pTran = transform;

		playerScript = GetComponent<Player>();

		groundDetectLength = pTran.localScale.y/2;

		if(transform.Find("ForwardCollider").GetComponent<CollisionDetect>() != null)
			downCollider = transform.Find("DownCollider").GetComponent<CollisionDetect>();
		else
			print("DownCollider is needed on " + name);
	}

	// Update is called once per frame
	public void JumpUpdate () 
	{
		//Rays will be cast on both sides of the player, so edges are also detected
		/*
		Vector3 leftPos = pTran.position+Vector3.left*pTran.localScale.x/2f; //1.7 puts the ray further out, causing the player to make less mistakes
		Vector3 rightPos = pTran.position+Vector3.right*pTran.localScale.x/2f;

		RaycastHit hit;
		if(Physics.Raycast(pTran.position, Vector3.down, out hit, pTran.localScale.y/2) || Physics.Raycast(leftPos, Vector3.down, out hit, pTran.localScale.y/2) || Physics.Raycast(rightPos, Vector3.down, out hit, pTran.localScale.y/2))
		{
			bool ownProjInBounds = false;

			if(hit.collider.gameObject.tag != "NotCollidable")
			{
				if(readyToLand == true)
				{
					IsLanding = true;
					readyToLand = false;
				}

				CanJump = true;
				HasJumped = false;
				HasDoubleJumped = false;

				boostJumpsAmount = 0;
				rigidbody.velocity = Vector3.zero;
			}
		}*/
		if(downCollider.IsColliding)
		{
			bool ownProjInBounds = false;

			if(readyToLand == true)
			{
				IsLanding = true;
				readyToLand = false;
			}

			CanJump = true;
			HasJumped = false;
			HasDoubleJumped = false;

			boostJumpsAmount = 0;
			rigidbody.velocity = Vector3.zero;
		}
		else //We are in the air
		{
			CanJump = false;
			if(boostJumpsAmount < MaxBoostJumpsAmount)
				CanBoostJump = true;
			else
				CanBoostJump = false;

			if(HasJumped == true)
				readyToLand = true;
		}

		if(CanJump && playerScript.PlayerControllerState.ButtonDownA || CanJump && playerScript.Keyboard && Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
			
			//addJumpPhysics = true;
		}
		else if(CanBoostJump && playerScript.PlayerControllerState.ButtonDownA || CanBoostJump && playerScript.Keyboard && Input.GetKeyDown(KeyCode.Space))
		{
			BoostJump();
			
			//addBoostJumpPhysics = true;
		}
		
		/*
		if(downCollider.IsColliding == true)
		{
			bool ownProjInBounds = false;

			CanJump = true;
			boostJumpsAmount = 0;
			rigidbody.velocity = Vector3.zero;
		}
		else //We are in the air
		{
			CanJump = false;
			if(boostJumpsAmount < MaxBoostJumpsAmount)
				CanBoostJump = true;
			else
				CanBoostJump = false;
		}
		*/
		//Debug.DrawRay(leftPos, Vector3.down);
		//Debug.DrawRay(rightPos, Vector3.down);
	}

	//TODO: Fix so it's the same velocity for each jump
	public void Jump()
	{
		HasJumped = true;		//Used for animation

		//rigidbody.AddForce(Vector3.up*JumpForce, ForceMode.VelocityChange);
		rigidbody.velocity = new Vector3(0,JumpForce,0);

		DataSaver.Instance.highScores[0].timesJumped++;

		soundObject = Instantiate(SoundObject, pTran.position, Quaternion.identity) as GameObject;
		if(soundObject.audio != null)
		{
			soundObject.audio.clip = AudioManager.Instance.Jump[playerScript.Id];
			soundObject.audio.Play();
			soundObject.audio.volume = 0.3f;
			soundObject.audio.pitch = 1;
			Destroy(soundObject, AudioManager.Instance.Jump[playerScript.Id].length);
		}

	}
	public void BoostJump()
	{
		HasDoubleJumped = true;		//Used for animation

		boostJumpsAmount++;
		
		boostJumpEffect = Instantiate(BoostJumpEffect, pTran.position, Quaternion.identity) as GameObject;
		Destroy(boostJumpEffect, 3);

		//rigidbody.AddForce(Vector3.up*BoostJumpForce, ForceMode.VelocityChange);
		rigidbody.velocity = new Vector3(0, BoostJumpForce, 0);

		DataSaver.Instance.highScores[0].timesJumped++;

		soundObject = Instantiate(SoundObject, pTran.position, Quaternion.identity) as GameObject;
		if(soundObject.audio != null)
		{
			soundObject.audio.clip = AudioManager.Instance.Jump[playerScript.Id];
			soundObject.audio.Play();
			soundObject.audio.volume = 0.3f;
			soundObject.audio.pitch = 1.5f;
			Destroy(soundObject, AudioManager.Instance.Jump[playerScript.Id].length);
		}
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
