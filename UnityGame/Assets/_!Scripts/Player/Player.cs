using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public enum PlayerState
{
	Reset,
	Alive,
	Respawning,
	Dead
};


[RequireComponent(typeof(TargetIDColor))]
public class Player : MonoBehaviour
{

    public int Points;

	public bool LoFi = false;
	public bool Keyboard = false;

	[HideInInspector]
	public bool IsReadyToBegin = false;

	public int Score;
	public int Id;
	public int DeathTime = 5;
	public float RespawnTime = 3;
	public float RespawnBlinkRate = 0.1f;
	public GameObject[] SpawnPoints = new GameObject[4];
	public Material[] Materials = new Material[4];

	[HideInInspector]
	public GameObject spawnZone;

	[HideInInspector]
	public PlayerState PState;
	public string KilledBy;
	[HideInInspector]
	public PlayerIndex PlayerController;
	public ControllerState PlayerControllerState;

	private PlayerAim playerAim;
	private PlayerMove playerMove;
	private PlayerJump playerJump;

	private Transform pTran;
	private GameObject spawnPoint;

	private Material pMat;

    [HideInInspector]
    public string Name;

	// Use this for initialization
	void Awake () 
	{
		pTran = transform;

		switch(Id)
		{
			case 0:
			PlayerController = PlayerIndex.One;
			spawnPoint = SpawnPoints[0];
			pMat = Materials[0];
			break;
			case 1:
			PlayerController = PlayerIndex.Two;
			spawnPoint = SpawnPoints[1];
			pMat = Materials[1];
			break;
			case 2:
			PlayerController = PlayerIndex.Three;
			spawnPoint = SpawnPoints[2];
			pMat = Materials[2];
			break;
			case 3:
			PlayerController = PlayerIndex.Four;
			spawnPoint = SpawnPoints[3];
			pMat = Materials[3];
			break;
		}
		renderer.material = pMat;

		PlayerControllerState = GetComponent<ControllerState>();
		playerAim = GetComponent<PlayerAim>();
		playerMove = GetComponent<PlayerMove>();
		playerJump = GetComponent<PlayerJump>();

		spawnZone = spawnPoint.transform.Find("SpawnZone").gameObject;

        if (GetComponent<TargetIDColor>() == null)
            Debug.Log("ERROR - player needs to have TargetIDColor component " + gameObject);
	    
        Name = gameObject.name;
	    Points = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
	    if (GameManager.Instance.PlayingState == PlayingState.Playing)
	    {
	        playerAim.AimUpdate();
	        playerMove.MoveUpdate();
	        playerJump.JumpUpdate();

	        if (GameManager.Instance.DebugMode)
	        {
	            // kill myself - DEBUG
	            if (PlayerControllerState.ButtonDownY)
	                StartCoroutine(Die());
	        }
	    }
	    else if(GameManager.Instance.PlayingState == PlayingState.Reset)
	    {
	    	playerMove.MoveUpdate();
	    }

	    if(PState != PlayerState.Alive)
		{
			float lerp = Mathf.PingPong(Time.time, RespawnBlinkRate) / RespawnBlinkRate;
			renderer.material.color = Color.Lerp(pMat.color, Color.white, lerp);
		}
		else
			renderer.material = pMat;
	}

    public void RemoveAllMissionsOnMeDontUseThis()
    {
        // Removes all previous missions from player

        // DOES NOT WORK WITH ARRAY!

        MissionBase[] missions = gameObject.GetComponents<MissionBase>();
        foreach (MissionBase m in missions)
            DestroyImmediate(m);
    }

	public IEnumerator Die()
	{
		PState = PlayerState.Dead;

		renderer.enabled = false;
		pTran.position = new Vector3(-1000,-1000,-1000);

		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		yield return new WaitForSeconds(DeathTime);
		StartCoroutine(Respawn());
	}

    public void Reset()
    {
        KilledBy = "";
        IsReadyToBegin = false;

        renderer.enabled = true;
        pTran.position = spawnPoint.transform.position;

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        playerAim.CurrentShotAmount = playerAim.ShotAmount;
        if(playerAim.Projectile != null)
        	Destroy(playerAim.Projectile);

       if(GameManager.Instance.WaitForReady)
       {
       		PState = PlayerState.Reset;
       		spawnZone.SetActive(true);
       		InvokeRepeating("PlayerReady", 0, 0.01f);
       }
       else
       {
       		PState = PlayerState.Alive;
       		spawnZone.SetActive(false);
       }
    }

	public IEnumerator Respawn()
	{
		PState = PlayerState.Respawning;
		KilledBy = "";
		
		renderer.enabled = true;
		pTran.position = spawnPoint.transform.position;

		spawnZone.SetActive(true);
		InvokeRepeating("CheckMovement", 0, 0.01f);

		yield return null;
	}

	void PlayerReady()
	{
		if(PlayerControllerState.ButtonDownY || Keyboard && Input.GetKeyDown(KeyCode.Q))
		{
			IsReadyToBegin = true;
			CancelInvoke("PlayerReady");
		}
	}

	void CheckMovement()
	{
		if(PlayerControllerState.GetCurrentState().ThumbSticks.Left.X != 0 || PlayerControllerState.GetCurrentState().ThumbSticks.Left.Y != 0 || PlayerControllerState.ButtonDownA || Keyboard && Input.anyKey)
		{
			PState = PlayerState.Alive;
			spawnZone.SetActive(false);

			CancelInvoke("CheckMovement");
		}
	}
}
