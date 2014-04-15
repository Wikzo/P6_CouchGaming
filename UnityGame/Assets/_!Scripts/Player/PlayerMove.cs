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

	private bool canMove = true;
	private bool isMovingIntoObject = false;

	private Transform pTran;

	private Player playerScript;
	private PlayerJump playerJump;
	
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
	}
	
	// Update is called once per frame
	public void MoveUpdate () 
	{
		if(CanMove)
		{
			if(playerScript.PlayerControllerState.GetCurrentState().ThumbSticks.Left.X < 0 || playerScript.Keyboard && Input.GetKey(KeyCode.A))
			{
				pTran.forward = Vector3.left;
				Move(Vector3.left);
			}
			else if(playerScript.PlayerControllerState.GetCurrentState().ThumbSticks.Left.X > 0 || playerScript.Keyboard && Input.GetKey(KeyCode.D))
			{
				pTran.forward = Vector3.right;
				Move(Vector3.right);
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
		Vector3 upPos = pTran.position+Vector3.up*pTran.localScale.y/2.1f;
		Vector3 downPos = pTran.position+Vector3.down*pTran.localScale.y/2.1f;

		RaycastHit hit;

		//Check if we are walking into something
		if(Physics.Raycast(pTran.position, pTran.forward, out hit, pTran.localScale.x/2) || Physics.Raycast(upPos, pTran.forward, out hit, pTran.localScale.x/2) || Physics.Raycast(downPos, pTran.forward, out hit, pTran.localScale.x/2))
		{
			if(hit.collider.gameObject.tag == "NotCollidable")
				isMovingIntoObject = false;
			else
				isMovingIntoObject = true;
		}
		else if(!Physics.Raycast(pTran.position, pTran.forward, pTran.localScale.x/2) && !Physics.Raycast(upPos, pTran.forward, pTran.localScale.x/2) && !Physics.Raycast(downPos, pTran.forward, pTran.localScale.x/2))
			isMovingIntoObject = false;	

		
		if(isMovingIntoObject == false)
		{
			if(playerJump.CanJump)
			{
				rigidbody.MovePosition(rigidbody.position + direction*Time.deltaTime*GroundMoveSpeed);
				rigidbody.drag = 0;
			}
			else //Give the player a different movement speed if he is in the air
			{
				rigidbody.MovePosition(rigidbody.position + direction*Time.deltaTime*AirMoveSpeed);
				rigidbody.drag = 1;
			}
			if(direction == Vector3.right)
				MovingRight = true;
			else
			{
				MovingLeft = true;
			}
		}
		else
		{
			MovingRight = false;
			MovingLeft = false;
		}
		
		if(GetComponent<Player>().LoFi == false)
			pTran.forward = direction;

		//Debug.DrawRay(upPos, pTran.forward);
		//Debug.DrawRay(downPos, pTran.forward);
	}
}
