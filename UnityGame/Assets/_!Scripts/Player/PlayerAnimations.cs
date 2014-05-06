using UnityEngine;
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
		CurrentBaseState = anim.GetCurrentAnimatorStateInfo(0);	
		
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
		}
		else
			anim.SetBool("JumpLand", false);

		if(playerMove.movingLeft || playerMove.movingRight)
			anim.SetBool("Run", true);
		else
		{
			runTimer += Time.deltaTime;
			//if(runTimer >= 0.1f)
			//{
				anim.SetBool("Run", false);
				runTimer = 0;
			//}
		}
	}
}
