using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerAim : MonoBehaviour 
{
	public GameObject LaserShotObj;

	public float shotForce = 100;
	public float chargeSpeed = 3;
	public float maxChargeTime = 10;

	private GameObject laserShot;

	private float chargeTimer = 1;

	private Transform pTran;
	private Transform aimPivotTran;
	private Transform aimTran;
	private Transform chargeBar;

	private PlayerMove playerMove;
	private PlayerJump playerJump;

	private ControllerState controllerState;

	// Use this for initialization
	void Start () 
	{
		pTran = transform;
		aimPivotTran = transform.Find("AimPivot");
		aimTran = aimPivotTran.Find("Aim");
		chargeBar = pTran.Find("ChargeBar");

		chargeBar.right = Vector3.right;

		controllerState = GetComponent<ControllerState>();

		playerMove = GetComponent<PlayerMove>();
		playerJump = GetComponent<PlayerJump>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(controllerState.GetCurrentState().Buttons.X == ButtonState.Pressed)
		{
			//FIND A BETTER SOLUTION FOR THIS:
			if(playerMove.MovingLeft && !playerJump.CanJump)
			{
				rigidbody.MovePosition(rigidbody.position + Vector3.left*Time.deltaTime*3);
			}
			else if(playerMove.MovingRight && !playerJump.CanJump)
			{
				rigidbody.MovePosition(rigidbody.position + Vector3.right*Time.deltaTime*3);
			}

			playerMove.CanMove = false;

			aimTran.renderer.enabled = true;
			chargeBar.renderer.enabled = true;

			Vector3 direction = new Vector3(controllerState.GetCurrentState().ThumbSticks.Left.X, controllerState.GetCurrentState().ThumbSticks.Left.Y, 0);
			Quaternion rotation = Quaternion.LookRotation(direction, Vector3.forward);

			if(direction != Vector3.zero)
			{
				aimPivotTran.rotation = rotation;
			}

			if(chargeTimer < maxChargeTime)
			{
				chargeTimer += Time.deltaTime*chargeSpeed;
				chargeBar.localScale = new Vector3(chargeTimer/maxChargeTime, chargeBar.localScale.y, chargeBar.localScale.z);
				chargeBar.position = new Vector3(pTran.position.x-0.5f+chargeBar.localScale.x/2, pTran.position.y+0.6f, pTran.position.z);
			}
		}
		else if(controllerState.ButtonUpX)
		{
			laserShot = Instantiate(LaserShotObj, aimTran.position+aimTran.forward, Quaternion.identity) as GameObject;
			laserShot.GetComponent<Bullet>().Owner = name;
			laserShot.transform.up = aimTran.forward;
			laserShot.rigidbody.AddForce(aimTran.forward*shotForce*chargeTimer);
			chargeTimer = 1;
		}
		else
		{
			aimPivotTran.forward = pTran.forward;
			aimTran.renderer.enabled = false;
			chargeBar.renderer.enabled = false;

			playerMove.CanMove = true;
		}
	}
}
