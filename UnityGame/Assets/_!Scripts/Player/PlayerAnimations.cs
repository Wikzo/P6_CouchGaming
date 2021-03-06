﻿using UnityEngine;
using System.Collections;

public class PlayerAnimations : MonoBehaviour {

	private PlayerMove playerMove;
	private PlayerJump playerJump;
	private Animator anim;

	[HideInInspector]
	public AnimatorStateInfo CurrentBaseState;			
	private AnimatorStateInfo layer2CurrentState;	

	private float animSpeed = 1.5f;
	private float runTimer = 0;				

	public static int idleState = Animator.StringToHash("Base Layer.Idle");	
	public static int runState = Animator.StringToHash("Base Layer.Run");			
	public static int jumpState = Animator.StringToHash("Base Layer.Jump");
	public static int doubleJumpState = Animator.StringToHash("Base Layer.DoubleJump");
	public static int JumpLandState = Animator.StringToHash ("Base Layer.JumpLand");
	public static int jumpDownState = Animator.StringToHash("Base Layer.JumpDown");		
	public static int fallState = Animator.StringToHash("Base Layer.Fall");
	public static int rollState = Animator.StringToHash("Base Layer.Roll");
	public static int waveState = Animator.StringToHash("Layer2.Wave");

	// Use this for initialization
	void Start ()
	{
		playerMove = GetComponent<PlayerMove>();
		playerJump = GetComponent<PlayerJump>();

		anim = GetComponent<Animator>();

		if(anim.layerCount ==2)
			anim.SetLayerWeight(1, 1);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		anim.speed = animSpeed;								
		CurrentBaseState = anim.GetCurrentAnimatorStateInfo(0);	

		if(playerJump.HasJumped == true)
		{
			anim.SetBool("Jump", true);

			if(playerJump.HasDoubleJumped == true)
				anim.SetBool("DoubleJump", true);
		}
		else
		{
			anim.SetBool("Jump", false);
			anim.SetBool("DoubleJump", false);
		}
		

		if(playerJump.IsLanding == true)
		{
			anim.SetBool("JumpLand", true);

			anim.SetBool("Fall", false);
			
			playerJump.IsLanding = false;

			anim.CrossFade(JumpLandState, 0, 0, Mathf.NegativeInfinity);		
		}
		else
		{
			anim.SetBool("JumpLand", false);
		}

		if(playerMove.movingLeft || playerMove.movingRight)
			anim.SetBool("Run", true);
		else
		{
			runTimer += Time.deltaTime;
			if(runTimer >= 0.1f)
			{
				anim.SetBool("Run", false);
				runTimer = 0;
			}
		}

		if(playerJump.CanJump == false && playerJump.HasJumped == false && playerJump.HasDoubleJumped == false && rigidbody.velocity.y != 0)
			anim.SetBool("Fall", true);
		else
			anim.SetBool("Fall", false);

		if(rigidbody.velocity.y == 0)
			anim.SetBool("Fall", false);
	}
}
