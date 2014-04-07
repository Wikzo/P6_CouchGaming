using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerMove : MonoBehaviour 
{
	public float GroundMoveSpeed = 3;
	public float AirMoveSpeed = 2;

	[HideInInspector]
	public bool movingLeft = false;
	[HideInInspector]
	public bool movingRight = false;

	private Transform pTran;

	private Player playerScript;
	private PlayerJump playerJump;

	private bool canMove = true;
	private Quaternion startRotation;
	private Vector3 pForwardDir;


	public bool CanMove
	{
		get{return canMove;}
		set{canMove = value;}	
	}
	public bool MovingLeft
	{
		get{return movingLeft;}
		set{movingLeft = value;}	
	}
	public bool MovingRight
	{
		get{return movingRight;}
		set{movingRight = value;}	
	}

	// Use this for initialization
	void Start () 
	{
		pTran = transform;

		playerScript = GetComponent<Player>();
		playerJump = GetComponent<PlayerJump>();

		startRotation = pTran.rotation;

		InvokeRepeating("ResetRotation", 0, 0.3f);
	}
	
	// Update is called once per frame
	public void MoveUpdate () 
	{
		if(CanMove)
		{
			if(playerScript.PlayerControllerState.GetCurrentState().ThumbSticks.Left.X < 0 || playerScript.Keyboard && Input.GetKey(KeyCode.A))
			{
				Move(Vector3.left);
				pTran.forward = Vector3.left;
				pForwardDir = pTran.forward;

				MovingLeft = true;
			}
			else if(playerScript.PlayerControllerState.GetCurrentState().ThumbSticks.Left.X > 0 || playerScript.Keyboard && Input.GetKey(KeyCode.D))
			{
				Move(Vector3.right);
				pTran.forward = Vector3.right;
				pForwardDir = pTran.forward;

				MovingRight = true;
			}
			else
			{
				MovingLeft = false;
				MovingRight = false;
			}
		}
	}

	//THIS SHOULD PROBABLY BE CALLED FROM FIXED UPDATE:
	public void Move(Vector3 direction)
	{
		//Give the player a different movement speed if he is in the air
		if(playerJump.CanJump)
		{
			rigidbody.MovePosition(rigidbody.position + direction*Time.deltaTime*GroundMoveSpeed);
			rigidbody.drag = 0;
		}
		else
		{
			rigidbody.MovePosition(rigidbody.position + direction*Time.deltaTime*AirMoveSpeed);
			rigidbody.drag = 1;
		}
		
		if(GetComponent<Player>().LoFi == false)
			pTran.forward = direction;
	}

	//Correct the rotation if it's skewed
	void ResetRotation()
	{
		if(pTran.rotation != startRotation)
		{
			pTran.rotation = startRotation;
			pTran.forward = pForwardDir;
		}
	}
}
