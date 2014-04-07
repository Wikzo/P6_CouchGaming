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

	private float chargeTimer = 0;

	private bool cancelAim = false;

	private Transform pTran;
	private Transform aimPivotTran;
	private Transform aimTran;
	private Transform chargeBar;

	private Player playerScript;
	private PlayerMove playerMove;
	private PlayerJump playerJump;

	// Use this for initialization
	void Start () 
	{
		pTran = transform;
		aimPivotTran = transform.Find("AimPivot");
		aimTran = aimPivotTran.Find("Aim");
		chargeBar = pTran.Find("ChargeBar");

		chargeBar.right = Vector3.right;

		playerScript = GetComponent<Player>();
		playerMove = GetComponent<PlayerMove>();
		playerJump = GetComponent<PlayerJump>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (GameManager.Instance.PlayingState == PlayingState.Paused) // round is paused
            return;

		//THIS SHOULD BE DONE SMARTER WITHOUT CHECKING SHOTAMOUNT SO MANY TIMES
		if(playerScript.PlayerControllerState.GetCurrentState().Buttons.X == ButtonState.Pressed && ShotAmount > 0 || playerScript.Keyboard && Input.GetKey(ShootKey) && ShotAmount > 0)
		{
			playerMove.CanMove = false;

			//Give the player momentum in the air, but remove it as soon as he hits the ground (CanJump)
			if(playerMove.MovingLeft)
			{
				if(playerJump.CanJump)
					playerMove.MovingLeft = false;
				else
					playerMove.Move(Vector3.left);
			}
			else if(playerMove.MovingRight)
			{
				if(playerJump.CanJump)
					playerMove.MovingRight = false;
				else
					playerMove.Move(Vector3.right);
			}

			Vector3 direction;

			if(playerScript.Keyboard && Input.GetKey(ShootKey))
				direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
			else
				direction = new Vector3(playerScript.PlayerControllerState.GetCurrentState().ThumbSticks.Left.X, playerScript.PlayerControllerState.GetCurrentState().ThumbSticks.Left.Y, 0);

			//Face the direction that the player is aiming in
			if(direction.x < 0)
			{
				pTran.forward = Vector3.left;
			}
			else if(direction.x > 0)
			{
				pTran.forward = Vector3.right;
			}

			//Cancel the aim if the player is aiming downwards
			if(direction.y == -1 && playerJump.CanJump)
			{
				cancelAim = true;

				aimTran.renderer.enabled = false;
				chargeBar.renderer.enabled = false;

				chargeTimer = 0;
			}
			else
			{
				cancelAim = false;

				aimTran.renderer.enabled = true;
				chargeBar.renderer.enabled = true;

				//Only rotate the aim if the player is aiming someplace 
				if(direction != Vector3.zero)
				{
					Quaternion rotation = Quaternion.LookRotation(direction, Vector3.forward);
					aimPivotTran.rotation = rotation;
				}

				//Scale the charge bar with a timer
				if(chargeTimer < MaxChargeTime)
				{
					chargeTimer += Time.deltaTime*ChargeSpeed;
					chargeBar.localScale = new Vector3(chargeTimer/MaxChargeTime, chargeBar.localScale.y, chargeBar.localScale.z);
					chargeBar.position = new Vector3(pTran.position.x-0.5f+chargeBar.localScale.x/2, pTran.position.y+0.6f, pTran.position.z);
				}
			}			
		}
		else if(playerScript.PlayerControllerState.ButtonUpX && ShotAmount > 0 && cancelAim == false || playerScript.Keyboard && Input.GetKeyUp(ShootKey) && ShotAmount > 0 && cancelAim == false)
		{
			projectile = Instantiate(ProjectileObj, aimTran.position+aimTran.forward*ShotOffset, Quaternion.identity) as GameObject;
			projectile.GetComponent<Projectile>().Owner = name;
			projectile.transform.right = aimTran.forward;
			projectile.rigidbody.velocity = aimTran.forward*ShotForce*chargeTimer;
			//projectile.rigidbody.AddForce(aimTran.forward*ShotForce*chargeTimer); REMEMBER TO SET SHOTFORCE TO 200
			projectile.renderer.material = renderer.material;
			
			chargeTimer = 1;

			ShotAmount -= 1;
		}
		else
		{
			aimPivotTran.forward = pTran.forward;
			aimTran.renderer.enabled = false;
			chargeBar.renderer.enabled = false;

			playerMove.CanMove = true;
			cancelAim = false;
		}
	}
}
