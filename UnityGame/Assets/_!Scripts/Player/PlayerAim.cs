using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerAim : MonoBehaviour 
{
	
	public KeyCode ShootKey = KeyCode.Tab;

	private bool canAim = false;

	public float ShotForce = 100;

	public float MaxChargeTime = 10;
	public float ShotAmount = 1;
	[HideInInspector]
	public float CurrentShotAmount;

	[HideInInspector]
	public GameObject ProjectileOriginalObject;

    public GameObject ProjectilePrefab;

	private float chargeTimer = 0;

	private bool cancelAim = false;

	private bool addPhysics = false;

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

		CurrentShotAmount = ShotAmount;

		playerScript = GetComponent<Player>();
		playerMove = GetComponent<PlayerMove>();
		playerJump = GetComponent<PlayerJump>();
	}
	
	// Update is called once per frame
	public void AimUpdate ()
	{
		//THIS SHOULD BE DONE SMARTER WITHOUT CHECKING SHOTAMOUNT SO MANY TIMES
		if(playerScript.PlayerControllerState.GetCurrentState().Buttons.X == ButtonState.Pressed && CurrentShotAmount > 0 || playerScript.Keyboard && Input.GetKey(ShootKey) && CurrentShotAmount > 0)
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
				TurnOffAim();
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
					chargeTimer += Time.deltaTime;
					chargeBar.localScale = new Vector3(chargeTimer/MaxChargeTime, chargeBar.localScale.y, chargeBar.localScale.z);

					//Give the position an offset, so the bar is positioned outside of the player and then add its scale to make it charge in one direction.
					chargeBar.position = new Vector3(pTran.position.x-pTran.localScale.x/2+chargeBar.localScale.x, pTran.position.y+pTran.localScale.y/2+chargeBar.localScale.y, pTran.position.z);	
				}			
			}			
		}
		else if(playerScript.PlayerControllerState.ButtonUpX && CurrentShotAmount > 0 && cancelAim == false || playerScript.Keyboard && Input.GetKeyUp(ShootKey) && CurrentShotAmount > 0 && cancelAim == false)
		{
            // original projectile
            ProjectileOriginalObject = Instantiate(ProjectilePrefab, aimTran.position + aimTran.forward * ProjectilePrefab.transform.localScale.x, Quaternion.identity) as GameObject;
            Projectile projectileOriginalScript = ProjectileOriginalObject.GetComponent<Projectile>();
            projectileOriginalScript.Owner = name;
            projectileOriginalScript.PMat = renderer.material;
            ProjectileOriginalObject.transform.right = aimTran.forward;
			addPhysics = true;

            // clone projectile (for screen wrapping)
            if (ProjectileOriginalObject.GetComponent<InstantiateCloneProjectile>() != null)
		    {
                GameObject cloneObject = ProjectileOriginalObject.GetComponent<InstantiateCloneProjectile>().MakeProjectileClone(ProjectileOriginalObject); // used to make screen wrapping clone
                Projectile cloneScript = cloneObject.GetComponent<Projectile>();
                cloneScript.Owner = name;
                cloneScript.PMat = renderer.material;
                cloneObject.transform.right = aimPivotTran.forward;

                projectileOriginalScript.TwinProjectileToDestroy = cloneObject;
                cloneScript.TwinProjectileToDestroy = ProjectileOriginalObject;

		    }

		    //Projectile.rigidbody.velocity = aimTran.forward*ShotForce*chargeTimer;
		    //Projectile.rigidbody.AddForce(aimTran.forward*ShotForce*chargeTimer); REMEMBER TO SET SHOTFORCE TO 200
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

	//Accessed by the Player script
	public void TurnOffAim()
	{
		cancelAim = true;

		aimTran.renderer.enabled = false;
		chargeBar.renderer.enabled = false;
		chargeTimer = 0;
	}

	void FixedUpdate()
	{
		if(addPhysics)
		{
            ProjectileOriginalObject.rigidbody.velocity = aimTran.forward * ShotForce * chargeTimer;
			
			chargeTimer = 0;
			CurrentShotAmount -= 1;

			addPhysics = false;
		}
	}
}


