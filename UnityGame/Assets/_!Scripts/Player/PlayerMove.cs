using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerMove : MonoBehaviour 
{
	private Transform pTran;

	private ControllerState controllerState;

	private bool canMove = true;

	private Quaternion rotRight;
	private Quaternion rotLeft;

	// Use this for initialization
	void Start () 
	{
		pTran = transform;

		controllerState = GetComponent<ControllerState>();

		rotRight = Quaternion.FromToRotation(pTran.forward, Vector3.back);
		rotLeft = Quaternion.FromToRotation(pTran.forward, Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(canMove)
		{
			if(controllerState.GetCurrentState().ThumbSticks.Left.X < 0)
			{
				pTran.Translate(Vector3.left*Time.deltaTime*3, Space.World);
				pTran.forward = Vector3.left;
				//pTran.rotation = Quaternion.Lerp(pTran.rotation, rotLeft, Time.deltaTime*50);
			}
			else if(controllerState.GetCurrentState().ThumbSticks.Left.X > 0)
			{
				pTran.Translate(Vector3.right*Time.deltaTime*3, Space.World);
				pTran.forward = Vector3.right;
				//pTran.rotation = Quaternion.Lerp(pTran.rotation, rotRight, Time.deltaTime*50);
			}
		}
	}
	void CanMove(bool messageBool)
	{
		canMove = messageBool;	
	}
}
