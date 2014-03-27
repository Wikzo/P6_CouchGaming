using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerMove_LoFi : MonoBehaviour 
{
	public PlayerIndex pIndex;

	private GamePadState currentState;

	private Transform pTran;

	private Quaternion rotRight;
	private Quaternion rotLeft;

	// Use this for initialization
	void Start () 
	{
		pTran = transform;

		rotRight = Quaternion.FromToRotation(pTran.forward, Vector3.back);
		rotLeft = Quaternion.FromToRotation(pTran.forward, Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentState = GamePad.GetState(pIndex);

		if(currentState.ThumbSticks.Left.X < 0)
		{
			pTran.Translate(Vector3.left*Time.deltaTime*3, Space.World);
			//pTran.rotation = Quaternion.Lerp(pTran.rotation, rotLeft, Time.deltaTime*50);
		}
		else if(currentState.ThumbSticks.Left.X > 0)
		{
			pTran.Translate(Vector3.right*Time.deltaTime*3, Space.World);
			//pTran.rotation = Quaternion.Lerp(pTran.rotation, rotRight, Time.deltaTime*50);
		}

		if(currentState.Triggers.Right == 1)
		{
			//pTran.Translate(Vector3.up*Time.deltaTime*6, Space.World);
			rigidbody.AddForce(Vector2.up * 50);
		}
	}
}
