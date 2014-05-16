using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public GameObject TwinProjectileToDestroy;
	public bool IsOriginal = true;

	public float KillVelocity = 1;
	public float DeadlyBlinkRate = 0.1f;
	public float DeadlyTimer = 0.5f;
	public float MaxInvisibleTime = 0.1f;

	private float visibleTimer = 0;

	public int MaxReflections = 2;
	public bool VelocityReflection = false;
	public bool ForceReflection = false;

	[HideInInspector]
	public Material PMat;

	public GameObject OwnerObject;

	private StuckDetector stuckDetector;

    public GameObject MinusObject;
    bool canGiveMinusPoint = true;

	private bool isDeadly = false;
	private bool isHittingPlayer = false;
	private bool outOfBounds = false;
	private bool outOfCameraView = false;

	private bool hasPlayedHitSound = false;

	private int reflectionCount = 0;

	private string owner;

	private Vector3 lockPos;

	public string Owner
	{
		get{return owner;}
		set{owner = value;}
	}

	public bool OutOfBounds
	{
		get{return outOfBounds;}
		set{outOfBounds = value;}
	}

	// Use this for initialization
	void Start () 	
	{
		if(IsOriginal)
		{
			audio.loop = true;
			audio.clip = AudioManager.Instance.DiscHover[OwnerObject.GetComponent<Player>().Id];
			audio.Play();
		}

		renderer.material.color = PMat.color;

		if(OwnerObject.transform.Find("ForwardCollider") != null)
			Physics.IgnoreCollision(collider, OwnerObject.transform.Find("ForwardCollider").collider, true);

		Physics.IgnoreCollision(collider, OwnerObject.collider, true); //Make sure that the player's physics are not affected by the collider of the projectile
		gameObject.tag = "NotCollidable"; //Make sure that the player's raycasting in playerMove and playerJump is not affected by the collider of the projectile
		foreach(Transform child in transform)
		{
			if(child.collider != null)
			{
				Physics.IgnoreCollision(collider, child.collider, true);
				Physics.IgnoreCollision(child.collider, OwnerObject.collider, true);
			}

			child.gameObject.tag = "NotCollidable";
		}

		if(transform.Find("StuckDetector") != null)
			stuckDetector = transform.Find("StuckDetector").gameObject.GetComponent<StuckDetector>();
		else
			print(gameObject.name + " needs its StuckDetector. Please add it to the object");
	}
	
	// Update is called once per frame
	void Update () 
	{
		isDeadly = true;

		if(isDeadly)
		{
			float lerp = Mathf.PingPong(Time.time, DeadlyBlinkRate) / DeadlyBlinkRate;
			renderer.material.color = Color.Lerp(PMat.color, Color.white, lerp);
		}
		else
			renderer.material.color = PMat.color;

		if(renderer.isVisible == false)
		{
			visibleTimer += Time.deltaTime;
			if(visibleTimer >= MaxInvisibleTime)
			{
				outOfCameraView = true;
			}
		}
		else
			visibleTimer = 0;

		if(outOfCameraView)
		{
			print(gameObject.name + " came out of the camera's view and has now been destroyed");
			DestroyProjectileAndTwin(OwnerObject.GetComponent<PlayerAim>());
		}
		else if(stuckDetector.StuckInObject)
		{
			print(gameObject.name + " is stuck inside an object and has now been destroyed");
			DestroyProjectileAndTwin(OwnerObject.GetComponent<PlayerAim>());
		}
	}

	void FixedUpdate()
	{
		if(!collider.bounds.Intersects(OwnerObject.collider.bounds))
		{	
			OutOfBounds = true;

			Physics.IgnoreCollision(collider, OwnerObject.collider, false);
			gameObject.tag = "Projectile";
			foreach(Transform child in transform)
			{
				if(child.gameObject.name != "StuckDetector")
				{
					Physics.IgnoreCollision(collider, child.collider, false);
					Physics.IgnoreCollision(child.collider, OwnerObject.collider, false);
					child.gameObject.tag = "Untagged";
				}
			}			
		}
	}

    IEnumerator MinusPointCoolDown() // used so won't give multiple minus points (e.g. hitting original AND clone should not give 2 minus points)
    {
        canGiveMinusPoint = false;
        yield return new WaitForSeconds(1f);
        canGiveMinusPoint = true;
    }

	void OnTriggerEnter(Collider other)
	{
		lockPos = transform.position;

<<<<<<< HEAD
        if (isDeadly && other.gameObject.GetComponent<PlayerDamage>() && other.gameObject.tag != Owner)
        {
            MissionBase m = OwnerObject.GetComponent<MissionBase>();
            PlayerDamage playerDamage = other.gameObject.GetComponent<PlayerDamage>();


            // penalty for shooting random people
            // either (have Mission Kill but didn't hit target) OR (don't have Mission Kill)
            if ((m.GetType() == typeof(MissionKill) == true && !other.name.Contains(m.Target.name)) || (m.GetType() == typeof(MissionKill) == false))
            {
                // minus one point
                if (canGiveMinusPoint && playerDamage.playerScript.PState == PlayerState.Alive)
                {
                    StartCoroutine(MinusPointCoolDown());
                    OwnerObject.GetComponent<Player>().Points--;
                    Instantiate(MinusObject, OwnerObject.transform.position + new Vector3(1.5f, 2f, 0), Quaternion.identity);

                    // doors going up/down
                    MissionManager.Instance.DoorGoUpDown();
                }
            }


            //other.gameObject.GetComponent<PlayerDamage>().CalculateDeath(Owner);
            playerDamage.CalculateDeath(Owner);
        }
=======
		if(isDeadly && other.gameObject.GetComponent<PlayerDamage>() && other.gameObject.tag != Owner)
		{
			other.gameObject.GetComponent<PlayerDamage>().CalculateDeath(Owner);
		}
>>>>>>> 50ac3e6785c5d188b0a6ef8975a8c9d58107e5d7
		
		if(other.gameObject.tag == Owner && OutOfBounds)
		{
            if (other.gameObject.GetComponent<PlayerAim>())
            {
                if (!IsOriginal)
                    return;

                DestroyProjectileAndTwin(other.gameObject.GetComponent<PlayerAim>());
            }
		}
		if(!other.gameObject.GetComponent<PlayerDamage>() && other.gameObject.tag != "NotCollidable" && other.gameObject.tag != gameObject.tag)
		{

			rigidbody.velocity = Vector3.zero;
      		rigidbody.angularVelocity = Vector3.zero;
      		transform.position = lockPos;
      		

      		if(IsOriginal && hasPlayedHitSound == false)
      		{
      			
      			print("stuff");
      			hasPlayedHitSound = true;
      			PlayHitSound();
      		}
		}
	}

	void OnDestroy()
	{
		if(TwinProjectileToDestroy != null)
			Destroy(TwinProjectileToDestroy);
	}

	void DestroyProjectileAndTwin(PlayerAim playerAim)
	{
		if (IsOriginal)
			playerAim.CurrentShotAmount++;
		
		Destroy(gameObject);
	}

	void PlayHitSound()
	{
		audio.Stop();
      	audio.loop = false;
      	audio.clip = AudioManager.Instance.DiscHit;
		audio.Play();
      	//audio.clip = AudioManager.Instance.DiscHit;
      	//audio.Play();
	}
}
