using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Aim : MonoBehaviour 
{

	private GamePadState currentState;

	private Transform armsPivot;

	private Transform pTran;

	private bool canAim = false;

	// Use this for initialization
	void Start () 
	{
		armsPivot = transform.Find("AimPivot");
		pTran = transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentState = GamePad.GetState(PlayerIndex.One);

		if(currentState.Buttons.X == ButtonState.Pressed)
		{
			SendMessage("CanMove", false);
			SendMessage("CanAim", true);
		}
		else
		{
			SendMessage("CanMove", true);
			SendMessage("CanAim", false);
		}

		if(canAim)
		{
			//float xInput = currentState.ThumbSticks.Left.X;
			//float yInput = currentState.ThumbSticks.Left.Y;
//
			//Vector3 targetRotation = new Vector3(xInput*360, yInput*360, armsPivot.rotation.eulerAngles.z);
			//armsPivot.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), .1f);

			armsPivot.rotation = Quaternion.AngleAxis(currentState.ThumbSticks.Left.X*360, -Vector3.forward);
			//print(currentState.ThumbSticks.Left.X);
		}
		else
		{
			armsPivot.right = pTran.forward;
		}
	}

	void CanAim(bool messageBool)
	{
		canAim = messageBool;
	}
}
