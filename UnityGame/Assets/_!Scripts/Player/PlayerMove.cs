using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerMove : MonoBehaviour 
{
	public int MoveSpeed = 3;

	[HideInInspector]
	public bool movingLeft = false;
	[HideInInspector]
	public bool movingRight = false;

	private Transform pTran;

	private ControllerState controllerState;

	private bool canMove = true;


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

		controllerState = GetComponent<ControllerState>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(CanMove)
		{
			if(controllerState.GetCurrentState().ThumbSticks.Left.X < 0)
			{
				Move(Vector3.left);
				MovingLeft = true;
			}
			else if(controllerState.GetCurrentState().ThumbSticks.Left.X > 0)
			{
				Move(Vector3.right);
				MovingRight = true;
			}
			else
			{
				MovingLeft = false;
				MovingRight = false;
			}
		}
	}

	//MAYBE THIS SHOULD BE CALLED FROM FIXED UPDATE:
	public void Move(Vector3 direction)
	{
		rigidbody.MovePosition(rigidbody.position + direction*Time.deltaTime*MoveSpeed);
		if(GetComponent<Player>().LoFi == false)
			pTran.forward = direction;
	}
}
