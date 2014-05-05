﻿using UnityEngine;
using System.Collections;

public class PlayerAnimations : MonoBehaviour {

	private PlayerMove playerMove;
	private PlayerJump playerJump;
	private Animator anim;

	private AnimatorStateInfo currentBaseState;			
	private AnimatorStateInfo layer2CurrentState;	

	private float animSpeed = 1.5f;
	private float runTimer = 0;				

	static int idleState = Animator.StringToHash("Base Layer.Idle");	
	static int runState = Animator.StringToHash("Base Layer.Run");			
	static int jumpState = Animator.StringToHash("Base Layer.Jump");
	static int doubleJumpState = Animator.StringToHash("Base Layer.DoubleJump");
	static int JumpLandState = Animator.StringToHash ("Base Layer.JumpLand");
	static int jumpDownState = Animator.StringToHash("Base Layer.JumpDown");		
	static int fallState = Animator.StringToHash("Base Layer.Fall");
	static int rollState = Animator.StringToHash("Base Layer.Roll");
	static int waveState = Animator.StringToHash("Layer2.Wave");

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
		//anim.speed = animSpeed;								
		//currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	
		//
		//if(anim.layerCount ==2)		
		//	layer2CurrentState = anim.GetCurrentAnimatorStateInfo(1);	
//
//
		//if(playerJump.HasJumped == true)
		//{
		//	anim.SetBool("Jump", true);
//
		//	if(playerJump.HasDoubleJumped == true)
		//		anim.SetBool("DoubleJump", true);
		//}
		//else
		//{
		//	anim.SetBool("Jump", false);
		//	anim.SetBool("DoubleJump", false);
		//}
		//
//
		//if(playerJump.IsLanding == true)
		//{
		//	playerJump.IsLanding = false;
		//	if(playerMove.movingLeft || playerMove.movingRight)
		//	{
		//		anim.CrossFade(runState, 0, 0, Mathf.NegativeInfinity);
		//		anim.SetFloat("Speed", 1f);
		//	}
		//	else
		//	{
		//		anim.CrossFade(JumpLandState, 0, 0, Mathf.NegativeInfinity);
		//		anim.SetFloat("Speed", 0);
		//	}			
		//}
//
		//if(playerMove.movingLeft || playerMove.movingRight)
		//{
		//	anim.SetFloat("Speed", 1f);
		//}
		//else
		//{
		//	anim.SetFloat("Speed", 0);
		//}

		anim.speed = animSpeed;								
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	
		
		if(anim.layerCount ==2)		
			layer2CurrentState = anim.GetCurrentAnimatorStateInfo(1);	


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
			playerJump.IsLanding = false;

			anim.CrossFade(JumpLandState, 0, 0, Mathf.NegativeInfinity);
			
			/*if(playerMove.movingLeft || playerMove.movingRight)
			{
				anim.CrossFade(runState, 0, 0, Mathf.NegativeInfinity);
				anim.SetBool("Run", true);
			}
			else
			{
				anim.CrossFade(JumpLandState, 0, 0, Mathf.NegativeInfinity);
				anim.SetBool("Run", false);
			}*/			
		}
		else
			anim.SetBool("JumpLand", false);

		if(playerMove.movingLeft || playerMove.movingRight)
			anim.SetBool("Run", true);
		else
		{
			runTimer += Time.deltaTime;
			if(runTimer >= 0.25f)
			{
				anim.SetBool("Run", false);
				runTimer = 0;
			}
		}
	}
}
