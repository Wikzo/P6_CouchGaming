using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerAim : MonoBehaviour 
{
	private Transform pTran;
	private Transform aimTran;

	private bool canAim = false;

	private PlayerIndex controllerNum;

	private ControllerState controllerState;

	// Use this for initialization
	void Start () 
	{
		pTran = transform;
		aimTran = transform.Find("AimPivot");

		controllerState = GetComponent<ControllerState>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(controllerState.GetCurrentState().Buttons.X == ButtonState.Pressed)
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
			Vector3 direction = new Vector3(controllerState.GetCurrentState().ThumbSticks.Left.X, controllerState.GetCurrentState().ThumbSticks.Left.Y, 0);
			Quaternion rotation = Quaternion.LookRotation(direction, Vector3.forward);

			if(direction != Vector3.zero)
			{
				aimTran.transform.rotation = rotation;
			}
		}
		else
		{
			aimTran.forward = pTran.forward;
		}
	}

	void CanAim(bool messageBool)
	{
		canAim = messageBool;
	}
}
