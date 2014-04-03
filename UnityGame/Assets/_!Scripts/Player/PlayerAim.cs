using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerAim : MonoBehaviour 
{
	public GameObject ProjectileObj;
	public KeyCode ShootKey = KeyCode.Tab;

	public float ShotOffset = 1.5f;
	public float ShotForce = 100;
	public float ChargeSpeed = 3;
	public float MaxChargeTime = 10;
	public float ShotAmount = 1;

	private GameObject projectile;

	private float chargeTimer = 1;

	private Transform pTran;
	private Transform aimPivotTran;
	private Transform aimTran;
	private Transform chargeBar;

	private Player playerScript;
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

		playerScript = GetComponent<Player>();
		playerMove = GetComponent<PlayerMove>();
		playerJump = GetComponent<PlayerJump>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//THIS SHOULD BE DONE SMARTER WITHOUT CHECKING SHOTAMOUNT SO MANY TIMES
		if(controllerState.GetCurrentState().Buttons.X == ButtonState.Pressed && ShotAmount > 0 || playerScript.Keyboard && Input.GetKey(ShootKey) && ShotAmount > 0)
		{
			if(playerMove.MovingLeft && !playerJump.CanJump)
				playerMove.Move(Vector3.left);
			else if(playerMove.MovingRight && !playerJump.CanJump)
				playerMove.Move(Vector3.right);

			playerMove.CanMove = false;

			aimTran.renderer.enabled = true;
			chargeBar.renderer.enabled = true;

			Vector3 direction;

			if(playerScript.Keyboard && Input.GetKey(ShootKey))
				direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
			else
				direction = new Vector3(controllerState.GetCurrentState().ThumbSticks.Left.X, controllerState.GetCurrentState().ThumbSticks.Left.Y, 0);


			Quaternion rotation = Quaternion.LookRotation(direction, Vector3.forward);

			if(direction != Vector3.zero)
				aimPivotTran.rotation = rotation;

			if(chargeTimer < MaxChargeTime)
			{
				chargeTimer += Time.deltaTime*ChargeSpeed;
				chargeBar.localScale = new Vector3(chargeTimer/MaxChargeTime, chargeBar.localScale.y, chargeBar.localScale.z);
				chargeBar.position = new Vector3(pTran.position.x-0.5f+chargeBar.localScale.x/2, pTran.position.y+0.6f, pTran.position.z);
			}
		}
		else if(controllerState.ButtonUpX && ShotAmount > 0 || playerScript.Keyboard && Input.GetKeyUp(ShootKey) && ShotAmount > 0)
		{
			projectile = Instantiate(ProjectileObj, aimTran.position+aimTran.forward*ShotOffset, Quaternion.identity) as GameObject;
			projectile.GetComponent<Projectile>().Owner = name;
			projectile.transform.right = aimTran.forward;
			projectile.rigidbody.velocity = aimTran.forward*ShotForce*chargeTimer;
			//projectile.rigidbody.AddForce(aimTran.forward*ShotForce*chargeTimer); REMEMBER TO SET SHOTFORCE TO 200
			
			chargeTimer = 1;

			ShotAmount -= 1;
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
