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
	private bool shootingRightNow = false;
    public bool ShootingRightNow
    {
        get { return shootingRightNow; }
    }

	private bool addPhysics = false;

	public Material aimMat;
	public Material aimCrossesMat;

	private Transform pTran;
	private Transform aimPivotTran;
	private Transform aimTran;
	private Transform chargeBar;

	private Vector3 chargeBarStartPos;
	private bool hasPlayedChargeSound = false;

	[HideInInspector]
	public Player playerScript;
	private PlayerMove playerMove;
	private PlayerJump playerJump;

	// Use this for initialization
	void Start () 
	{
		pTran = transform;
		aimPivotTran = transform.Find("AimPivot");
		aimTran = aimPivotTran.Find("Aim");
		chargeBar = aimTran.Find("ChargeBar");

		chargeBar.forward = Vector3.forward;
		

		CurrentShotAmount = ShotAmount;

		playerScript = GetComponent<Player>();
		playerMove = GetComponent<PlayerMove>();
		playerJump = GetComponent<PlayerJump>();
	}
	
	// Update is called once per frame
	public void AimUpdate ()
	{
        if (playerScript.PlayerControllerState.GetCurrentState().Buttons.X == ButtonState.Pressed)
            shootingRightNow = true;
        else
            shootingRightNow = false;


		//THIS SHOULD BE DONE SMARTER WITHOUT CHECKING SHOTAMOUNT SO MANY TIMES
		if(playerScript.PlayerControllerState.GetCurrentState().Buttons.X == ButtonState.Pressed || playerScript.Keyboard && Input.GetKey(ShootKey) && CurrentShotAmount > 0)
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

            // only pressing down, not aiming
            //if (direction.y == -1)
              //  pressingDownButNotAiming = true;

			//Cancel the aim if the player is aiming downwards
			if(direction.y == -1 && playerJump.CanJump)
			{
				TurnOffAim();
			}
			else
			{
				cancelAim = false;

				Quaternion aimRotation;

				//Only rotate the aim if the player is aiming someplace. Or else just use the forward rotation of the player
				if(direction != Vector3.zero)
					aimRotation = Quaternion.LookRotation(direction, Vector3.forward);
				else
					aimRotation = Quaternion.LookRotation(pTran.forward, Vector3.forward);

				aimPivotTran.rotation = aimRotation;

				aimTran.renderer.enabled = true;	

				if(CurrentShotAmount > 0)
				{
					//Scale the charge bar with a timer
					if(chargeTimer < MaxChargeTime)
					{
						if(hasPlayedChargeSound == false)
						{
							hasPlayedChargeSound = true;
							audio.loop = true;
							audio.clip = AudioManager.Instance.DiscCharge[playerScript.Id];
							audio.Play();
						}
						audio.pitch = chargeTimer;

						chargeTimer += Time.deltaTime;
						chargeBar.localScale = new Vector3(chargeTimer/MaxChargeTime, 1, aimTran.localScale.y*3); //Y is and Z, and Z is Y
	
						//Give the position an offset, so the bar is positioned outside of the player and then add its scale to make it charge in one direction.
						//chargeBar.localPosition = new Vector3(0, 0, chargeBarStartPos.z-chargeBar.localScale.x/2);
						chargeBar.localPosition = new Vector3(-1f, 0, -0.5f+chargeBar.localScale.x/2); //X is Z, and Z is X

					}

					if(aimCrossesMat != null)
					{
						Destroy(aimTran.renderer.material);
						aimTran.renderer.material = aimMat;
						chargeBar.renderer.enabled = true;
					}
				}
				else
				{
					if(aimMat != null)
					{
						Destroy(aimTran.renderer.material);
						aimTran.renderer.material = aimCrossesMat;
						chargeBar.renderer.enabled = false;
					}
				}
			}		
		}
		else if(playerScript.PlayerControllerState.ButtonUpX && CurrentShotAmount > 0 && cancelAim == false || playerScript.Keyboard && Input.GetKeyUp(ShootKey) && CurrentShotAmount > 0 && cancelAim == false)
		{
            audio.Stop();
            audio.loop = false;
            audio.pitch = 1;
			audio.clip = AudioManager.Instance.DiscShot;
			audio.Play();

            hasPlayedChargeSound = false;
            // original projectile
            ProjectileOriginalObject = Instantiate(ProjectilePrefab, pTran.position+Vector3.up*2f, Quaternion.identity) as GameObject;
            Projectile projectileOriginalScript = ProjectileOriginalObject.GetComponent<Projectile>();
            projectileOriginalScript.Owner = gameObject.tag;
            projectileOriginalScript.OwnerObject = gameObject;

            if(playerScript.BodyRenderer != null)
            	projectileOriginalScript.PMat = playerScript.BodyRenderer.material;
            else
            	projectileOriginalScript.PMat = renderer.material;

            ProjectileOriginalObject.transform.right = aimTran.forward;
			addPhysics = true;

			//Projectile.rigidbody.velocity = aimTran.forward*ShotForce*chargeTimer;
		    //Projectile.rigidbody.AddForce(aimTran.forward*ShotForce*chargeTimer); REMEMBER TO SET SHOTFORCE TO 200

            // clone projectile (for screen wrapping)
            if (ProjectileOriginalObject.GetComponent<InstantiateCloneProjectile>() != null)
		    {
                GameObject cloneObject = ProjectileOriginalObject.GetComponent<InstantiateCloneProjectile>().MakeProjectileClone(ProjectileOriginalObject); // used to make screen wrapping clone
                Projectile cloneScript = cloneObject.GetComponent<Projectile>();
                cloneScript.Owner = gameObject.tag;
                cloneScript.OwnerObject = gameObject;

                if(playerScript.BodyRenderer != null)
            		cloneScript.PMat = playerScript.BodyRenderer.material;
           		else
            		cloneScript.PMat = renderer.material;

                cloneObject.transform.right = aimPivotTran.forward;
                cloneScript.IsOriginal = false;

                projectileOriginalScript.TwinProjectileToDestroy = cloneObject;
                cloneScript.TwinProjectileToDestroy = ProjectileOriginalObject;
		    }
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

		audio.Stop();
        hasPlayedChargeSound = false;
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