using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerMove : MonoBehaviour 
{
	[HideInInspector]
	public bool MovingLeft = false;
	public bool MovingRight = false;

	private Transform pTran;

	private ControllerState controllerState;

	private bool canMove = true;

	private Quaternion rotRight;
	private Quaternion rotLeft;

	public bool CanMove
	{
		get{return canMove;}
		set{canMove = value;}	
	}

	// Use this for initialization
	void Start () 
	{
		pTran = transform;

		controllerState = GetComponent<ControllerState>();

		rotRight = Quaternion.FromToRotation(pTran.forward, Vector3.back);
		rotLeft = Quaternion.FromToRotation(pTran.forward, Vector3.forward);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(CanMove)
		{
			if(controllerState.GetCurrentState().ThumbSticks.Left.X < 0)
			{
				//pTran.Translate(Vector3.left*Time.deltaTime*3, Space.World);
				rigidbody.MovePosition(rigidbody.position + Vector3.left*Time.deltaTime*3);
				pTran.forward = Vector3.left;
				MovingLeft = true;
				//pTran.rotation = Quaternion.Lerp(pTran.rotation, rotLeft, Time.deltaTime*50);
			}
			else if(controllerState.GetCurrentState().ThumbSticks.Left.X > 0)
			{
				//pTran.Translate(Vector3.right*Time.deltaTime*3, Space.World);
				rigidbody.MovePosition(rigidbody.position + Vector3.right*Time.deltaTime*3);
				pTran.forward = Vector3.right;
				MovingRight = true;
				//pTran.rotation = Quaternion.Lerp(pTran.rotation, rotRight, Time.deltaTime*50);
			}
			else
			{
				MovingLeft = false;
				MovingRight = false;
			}
		}
	}
}
