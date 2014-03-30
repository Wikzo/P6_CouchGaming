using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerAim : MonoBehaviour 
{
	public GameObject LaserShotObj;
	private GameObject laserShot;

	private float chargeTimer = 1;

	private Transform pTran;
	private Transform aimPivotTran;
	private Transform aimTran;
	private Transform chargeBar;

	private bool canAim = false;

	private ControllerState controllerState;

	// Use this for initialization
	void Start () 
	{
		pTran = transform;
		aimPivotTran = transform.Find("AimPivot");
		aimTran = aimPivotTran.Find("Aim");
		chargeBar = pTran.Find("ChargeBar");

		controllerState = GetComponent<ControllerState>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(controllerState.GetCurrentState().Buttons.X == ButtonState.Pressed)
		{
			SendMessage("CanAim", true);
			SendMessage("CanMove", false);
			SendMessage("CanJump", false);

			aimTran.renderer.enabled = true;
			chargeBar.renderer.enabled = true;
			chargeBar.right = Vector3.right;

			Vector3 direction = new Vector3(controllerState.GetCurrentState().ThumbSticks.Left.X, controllerState.GetCurrentState().ThumbSticks.Left.Y, 0);
			Quaternion rotation = Quaternion.LookRotation(direction, Vector3.forward);

			if(direction != Vector3.zero)
			{
				aimPivotTran.rotation = rotation;
			}

			if(chargeTimer < 10)
			{
				chargeTimer += Time.deltaTime*3;
				chargeBar.localScale = new Vector3(chargeTimer/10, chargeBar.localScale.y, chargeBar.localScale.z);
				chargeBar.position = new Vector3(pTran.position.x-0.5f+chargeBar.localScale.x/2, pTran.position.y+0.6f, pTran.position.z);
			}
		}
		else if(controllerState.ButtonUpX)
		{
			laserShot = Instantiate(LaserShotObj, aimTran.position+aimTran.forward*1.2f, Quaternion.identity) as GameObject;
			laserShot.transform.up = aimTran.forward;
			laserShot.transform.localScale = new Vector3(0.02f * chargeTimer, 0.02f * chargeTimer, 0.02f * chargeTimer);
			laserShot.rigidbody.AddForce(aimTran.forward * 100 * chargeTimer);
			chargeTimer = 1;
		}
		else
		{
			aimPivotTran.forward = pTran.forward;
			aimTran.renderer.enabled = false;
			chargeBar.renderer.enabled = false;

			SendMessage("CanAim", false);
			SendMessage("CanMove", true);
		}
	}

	void CanAim(bool messageBool)
	{
		canAim = messageBool;
	}
}
