using System.Collections.Generic;
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

	public Material[] Materials = new Material[4];

	[HideInInspector]
	public GameObject SpawnZone;

	[HideInInspector]
	public GameObject ChosenSpawn;

	public GameObject SpawnPoints;
	private GameObject[] spawnPoints;

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
			pMat = Materials[0];
			break;
			case 1:
			PlayerController = PlayerIndex.Two;
			pMat = Materials[1];
			break;
			case 2:
			PlayerController = PlayerIndex.Three;
			pMat = Materials[2];
			break;
			case 3:
			PlayerController = PlayerIndex.Four;
			pMat = Materials[3];
			break;
		}
		renderer.material = pMat;

		spawnPoints = new GameObject[SpawnPoints.transform.GetChildCount()];

		for(int i = 0; i<spawnPoints.Length; i++)
		{
			spawnPoints[i] = SpawnPoints.transform.GetChild(i).gameObject;
		}

		PlayerControllerState = GetComponent<ControllerState>();
		playerAim = GetComponent<PlayerAim>();
		playerMove = GetComponent<PlayerMove>();
		playerJump = GetComponent<PlayerJump>();

		SpawnZone = transform.Find("SpawnZone").gameObject;

        if (GetComponent<TargetIDColor>() == null)
            Debug.Log("ERROR - player needs to have TargetIDColor component " + gameObject);
	    
        Name = gameObject.name;
	    Points = 0;

        SpawnZone.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
        // reset whole game - DEBUG
        if (GameManager.Instance.DebugMode && PlayerControllerState.ButtonDownBack)
           GameManager.Instance.ResetWholeGame();


        // hide player
	    if (GameManager.Instance.PlayingState == PlayingState.DisplayingScore)
	        Hide();

        // tell game manager that I am ready
        if (GameManager.Instance.PlayingState == PlayingState.WaitingForEverbodyToGetReady)
            PlayerReady();

        // playing loop
	    if (GameManager.Instance.PlayingState == PlayingState.Playing || GameManager.Instance.PlayingState == PlayingState.PraticeMode)
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
	    else if(GameManager.Instance.PlayingState == PlayingState.WaitingForEverbodyToGetReady)
	    {
            // can move/aim even while waiting ... MAYBE?
	    	//playerMove.MoveUpdate();
	    }

        if (GameManager.Instance.PlayingState == PlayingState.PraticeMode)
        {
            if (PlayerControllerState.ButtonDownStart)
                GameManager.Instance.ResetLevel();
        }

	    if (PState != PlayerState.Alive && GameManager.Instance.PlayingState == PlayingState.Playing)
	    {
	        float lerp = Mathf.PingPong(Time.time, RespawnBlinkRate)/RespawnBlinkRate;
	        renderer.material.color = Color.Lerp(pMat.color, Color.white, lerp);
	    }
	    else
	    {
            //Destroy(renderer.material); // just to be sure no memory garbage
	        renderer.material.color = pMat.color;
	    }
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
		Respawn();
	}

    void Hide()
    {
        renderer.enabled = false;

        pTran.position = new Vector3(-1000, -1000, -1000);

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    public void Reset()
    {
        KilledBy = "";
        IsReadyToBegin = false;

        renderer.enabled = true;

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        playerAim.CurrentShotAmount = playerAim.ShotAmount;
        if(playerAim.Projectile != null)
        	Destroy(playerAim.Projectile);

        ChooseSpawnPoint();

        // TODO: unsure if this sometimes doesn't get called due to State check
       //if(GameManager.Instance.PlayingState == PlayingState.WaitingForEverbodyToGetReady)
       //{
       		PState = PlayerState.Reset;
       		//InvokeRepeating("PlayerReady", 0, 0.01f);
       //}

        
        /*else
       {
       		PState = PlayerState.Alive;
       		SpawnZone.SetActive(false);
       }*/
    }


    public void OnGUI()
    {
        if (GameManager.Instance.PlayingState == PlayingState.WaitingForEverbodyToGetReady)
        {
            string text = "Is ready: " + IsReadyToBegin.ToString();
            var point = Camera.main.WorldToScreenPoint(transform.position - new Vector3(2,0, 0));
            GUI.Label(new Rect(point.x, Screen.currentResolution.height - point.y - 200, 200, 200), text);
        }

    }
	public void Respawn()
	{
		PState = PlayerState.Respawning;
		KilledBy = "";
		
		renderer.enabled = true;
		//pTran.position = spawnPoint;

		ChooseSpawnPoint();

		InvokeRepeating("CheckMovement", 0, 0.01f);
	}

	void PlayerReady()
	{
		if (PlayerControllerState.ButtonDownY || Keyboard && Input.GetKeyDown(KeyCode.Q))
		{
		    IsReadyToBegin = !IsReadyToBegin;
		    //CancelInvoke("PlayerReady");

		    //This is set in AllReady in GameManager instead, to make sure that everyone is done spawning:
		    //ChosenSpawn.SetActive(true);
		}
	}

	void CheckMovement()
	{
		if(PlayerControllerState.GetCurrentState().ThumbSticks.Left.X != 0 || PlayerControllerState.GetCurrentState().ThumbSticks.Left.Y != 0 || PlayerControllerState.ButtonDownA || Keyboard && Input.anyKey)
		{
			PState = PlayerState.Alive;
			
			ChosenSpawn.SetActive(true);
			SpawnZone.SetActive(false);

			CancelInvoke("CheckMovement");
		}
	}

	void ChooseSpawnPoint()
	{
	    if (GameManager.Instance.PlayingState != PlayingState.PraticeMode)
	        SpawnZone.SetActive(true);

	    List<GameObject> spawnsToChoose = new List<GameObject>();

		//Finds all active spawnpoints and stores them
        for(int i = 0; i<spawnPoints.Length; i++)
        {
        	if(spawnPoints[i].activeInHierarchy)
        	{
        		spawnsToChoose.Add(spawnPoints[i]);
        	}
        }
        //Picks a random active spawnpoint and places the player there
        if(spawnsToChoose.Count != 0)
        {
        	ChosenSpawn = spawnsToChoose[Random.Range(0,spawnsToChoose.Count-1)];
        	pTran.position = ChosenSpawn.transform.position;
        }

        ChosenSpawn.SetActive(false);
	}
}
