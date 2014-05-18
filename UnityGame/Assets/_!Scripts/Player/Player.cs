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

    public Color PlayerColor;
    //static int readyCounter = -1;

	[HideInInspector]
	public bool IsReadyToBegin = false;

	public GameObject RendererObject;
	[HideInInspector]
	public SkinnedMeshRenderer BodyRenderer;
	[HideInInspector]
	public MeshRenderer[] HelmetRenderers;
	private Color[] originalHelmetColors;

    Vector3 particleOffset = new Vector3(0, 2f, 0);

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
	GameObject[] spawnPoints;
    List<GameObject> spawnsToChoose;

	[HideInInspector]
	public PlayerState PState;
	public string KilledBy;

    [HideInInspector]
    public int TotalMissionPlayerHasCompleted;
	

	public PlayerIndex PlayerController;
	public ControllerState PlayerControllerState;

	public float RespawnIdleTime = 3;
	private float respawnIdleTimer;
	
	private PlayerAim playerAim;

	private PlayerMove playerMove;
	private PlayerJump playerJump;

	public CollisionDetect ForwardCollider;

	private Transform pTran;

	private Material pMat;

    [HideInInspector]
    public string Name;

    public string MyColorIDName;
    private int resetCounter;

    [HideInInspector]
    public bool HasBeenChosen = false;

    // Use this for initialization
	void Awake () 
	{

        if (FindRightControllers.Instance != null)
        {
            if (FindRightControllers.Instance.PlayerSlotsToRemember[Id] != -10)
            {
                PlayerController = (PlayerIndex)FindRightControllers.Instance.PlayerSlotsToRemember[Id];
                HasBeenChosen = true;
            }
        }
        else if (FindRightControllers.Instance != null)
            HasBeenChosen = false;
        else if (FindRightControllers.Instance == null)
            HasBeenChosen = true;

		pTran = transform;

		if(RendererObject != null)
		{
			if(RendererObject.GetComponent<SkinnedMeshRenderer>() != null)
        		BodyRenderer = RendererObject.GetComponent<SkinnedMeshRenderer>();
        	if(RendererObject.GetComponentsInChildren<MeshRenderer>() != null)
        	{
        		HelmetRenderers = RendererObject.GetComponentsInChildren<MeshRenderer>();
        		originalHelmetColors = new Color[HelmetRenderers.Length];
        		for(int i=0; i<originalHelmetColors.Length; i++)
        		{
                    // removed by Gustav
        			//originalHelmetColors[i] = HelmetRenderers[i].material.color;
        		}
        	}
        }

		switch(Id)
		{
			case 0:
			if (FindRightControllers.Instance == null)
                PlayerController = PlayerIndex.One;
			pMat = Materials[0];
			break;
			case 1:
            if (FindRightControllers.Instance == null)
                PlayerController = PlayerIndex.Two;
			pMat = Materials[1];
			break;
			case 2:
            if (FindRightControllers.Instance == null)
                PlayerController = PlayerIndex.Three;
			pMat = Materials[2];
			break;
			case 3:
            if (FindRightControllers.Instance == null)
    			PlayerController = PlayerIndex.Four;
			pMat = Materials[3];
			break;
		}
		if(BodyRenderer != null)
		{
			BodyRenderer.material.color = pMat.color;
			//BodyRenderer.materials[1].color = pMat.color;
			PlayerColor = pMat.color;
		}
		else
		{
			renderer.material = pMat;
			PlayerColor = pMat.color;
		}

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
		SpawnZone.SetActive(false);

        if (GetComponent<TargetIDColor>() == null)
            Debug.Log("ERROR - player needs to have TargetIDColor component " + gameObject);
	    
        Name = gameObject.name;
	    Points = 0;

        TotalMissionPlayerHasCompleted = 0;
	}

    void Start()
    {
    	if(transform.Find("ForwardCollider").GetComponent<CollisionDetect>() != null)
			ForwardCollider = transform.Find("ForwardCollider").GetComponent<CollisionDetect>();
		else
			print("ForwardCollider is needed on " + name);

    	respawnIdleTimer = RespawnIdleTime;
        resetCounter = 0;
        Reset();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    // reset whole game - DEBUG
        if (GameManager.Instance.DebugMode && PlayerControllerState.ButtonDownBack)
           GameManager.Instance.ResetWholeGame();


        // hide player
	    if (GameManager.Instance.PlayingState == PlayingState.DisplayingScore)
	    {
	       Hide();
	       //Activate the spawn point to make sure that everyone will be able to spawn.
	       ChosenSpawn.SetActive(true);
	    }

        // tell game manager that I am ready
        if (GameManager.Instance.PlayingState == PlayingState.WaitingForEverbodyToGetReady)
            PlayerReady();

        // playing loop
	    if (GameManager.Instance.PlayingState == PlayingState.Playing || GameManager.Instance.PlayingState == PlayingState.PraticeMode)
	    {
	        playerAim.AimUpdate();
	        playerMove.MoveUpdate();
	        playerJump.JumpUpdate();

            if (GameManager.Instance.DebugMode && GameManager.Instance.PlayingState == PlayingState.Playing)
	        {
	            // kill myself - DEBUG
	            if (PlayerControllerState.ButtonDownY)
	                StartCoroutine(Die());
	        }
	    }

        // go from practice mode to game, or from show score to game
	    if (PlayerControllerState.ButtonDownStart)
	    {
            switch (GameManager.Instance.PlayingState)
            {
                case PlayingState.TalkingBeforeControllerCalibration:
                case PlayingState.ControllerCalibration:
                    GameManager.Instance.SkipTutorialAndGoToWait();
                    GameManager.Instance.ResetLevel();
                    break;

                case PlayingState.PraticeMode:
	                GameManager.Instance.ResetLevel();
                    break;

                case PlayingState.DisplayingScore:
                    Application.LoadLevel(0);
                    break;

                /*default:
                    GameManager.Instance.Pause();
                    break;*/
            }
	    }


	    if (PState != PlayerState.Alive && GameManager.Instance.PlayingState == PlayingState.Playing)
	    {
	      	float lerp = Mathf.PingPong(Time.time, RespawnBlinkRate)/RespawnBlinkRate;
	      	
	
	      	if(BodyRenderer != null)
	      	{
					BodyRenderer.material.color = Color.Lerp(pMat.color, Color.white, lerp);
					BodyRenderer.materials[1].color = Color.Lerp(pMat.color, Color.white, lerp);
	      	}
			else
				renderer.material.color = Color.Lerp(pMat.color, Color.white, lerp);

			if(HelmetRenderers[0] != null)
			{
				for(int i=0; i<HelmetRenderers.Length; i++)
				{
                    // removed by Gustav
					//HelmetRenderers[i].material.color = Color.Lerp(originalHelmetColors[i], Color.white, lerp);
				}
			}

	      respawnIdleTimer -= Time.deltaTime;

	      //Make sure the player can't aim
	    	playerAim.TurnOffAim();

	    }
	    else
	    {
	       	if(BodyRenderer != null)
	      	{
				BodyRenderer.material.color = pMat.color;
				BodyRenderer.materials[1].color = pMat.color;
	      	}
			else
				renderer.material.color = pMat.color;

			if(HelmetRenderers[0] != null)
			{
				for(int i=0; i<HelmetRenderers.Length; i++)
				{
                    // removed by Gustav
					//HelmetRenderers[i].material.color = originalHelmetColors[i];
				}
			}
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
        GameObject particle = (GameObject)Instantiate(GameManager.Instance.DieParticles, transform.position + particleOffset, Quaternion.identity);
        Color c = new Color(pMat.color.r, pMat.color.g, pMat.color.b, 1);
        particle.renderer.material.color = c;

		PState = PlayerState.Dead;
		
		EnableRenderers(false);
		rigidbody.useGravity = false;
		//if(BodyRenderer != null)
		//	BodyRenderer.enabled = false;
		//else
		//	renderer.enabled = false;

		pTran.position = new Vector3(-1000,-1000,-1000);

		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		playerAim.CurrentShotAmount = playerAim.ShotAmount;
        if (playerAim.ProjectileOriginalObject != null)
            Destroy(playerAim.ProjectileOriginalObject);

		yield return new WaitForSeconds(DeathTime);
		rigidbody.useGravity = true;
		Respawn();
	}

    void Hide()
    {
        EnableRenderers(false);
        //if(BodyRenderer != null)
        //	BodyRenderer.enabled = false;
		//else
		//	renderer.enabled = false;

        pTran.position = new Vector3(-1000, -1000, -1000);

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    public void Reset()
    {

        KilledBy = "";
        IsReadyToBegin = false;


        GameObject particle = (GameObject)Instantiate(GameManager.Instance.SpawnParticles, transform.position + particleOffset, Quaternion.identity);
        Color c = new Color(pMat.color.r, pMat.color.g, pMat.color.b, 1);
        particle.renderer.material.color = c;
        EnableRenderers(true);
        //if(BodyRenderer != null)
        //	BodyRenderer.enabled = true;
		//else
		//	renderer.enabled = true;


        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        playerAim.CurrentShotAmount = playerAim.ShotAmount;
        if (playerAim.ProjectileOriginalObject != null)
            Destroy(playerAim.ProjectileOriginalObject);


        if (resetCounter != 1) // 
        {
            ChooseSpawnPoint();
        }
        else if (resetCounter == 1) // unique case between PRACTICE and WAIT mode --> don't find new spawn point, just go to existing one
            pTran.position = ChosenSpawn.transform.position;

        // Gustav: unsure if this should be PracticeMode or ControllerCalibration mode
	    if (GameManager.Instance.PlayingState != PlayingState.TalkingBeforeControllerCalibration)
	    {
	        SpawnZone.SetActive(true);
	    }

        CancelInvoke("CheckMovement");

        // TODO: unsure if this sometimes doesn't get called due to State check
       	PState = PlayerState.Reset;

        resetCounter++;

    }


    public void OnGUI()
    {
        // display controller slot
            /*int controller = (int)PlayerController + 1;
            string text = "P" + controller;

            var point = GameManager.Instance.CameraOrtographic.WorldToScreenPoint(transform.position);

            GUI.Label(new Rect(point.x, point.y, 50, 50), text);*/

        // display ready text
        /*if (GameManager.Instance.PlayingState == PlayingState.WaitingForEverbodyToGetReady)
        {

            // TODO: fix offset
            string ready = (IsReadyToBegin == true) ? "Ready" : "Not Ready";
            string text = "Press Y to toggle ready\n" + ready;

            var point = Camera.main.WorldToScreenPoint(transform.position - new Vector3(-2,3, 0));

            int xOffset = 0;
            int yOffset = 0;

            if (point.x > Screen.width - 160)
                xOffset = -160;

            if (point.y > Screen.height - 50)
                yOffset = 50;


            GUI.Label(new Rect(point.x + xOffset, Screen.currentResolution.height - point.y - 200 + yOffset, 200, 200), text);
        }*/

    }
	public void Respawn()
	{
		PState = PlayerState.Respawning;

		KilledBy = "";

        

		EnableRenderers(true);
		
		//if(BodyRenderer != null)
		//	BodyRenderer.enabled = true;
		//else
		//	renderer.enabled = true;
		
		//pTran.position = spawnPoint;

		ChooseSpawnPoint();

		// Gustav: unsure if this should be PracticeMode or ControllerCalibration mode
	    if (GameManager.Instance.PlayingState != PlayingState.ControllerCalibration)
	    {
	        SpawnZone.SetActive(true);
	    }

        GameObject particle = (GameObject)Instantiate(GameManager.Instance.SpawnParticles, transform.position + particleOffset, Quaternion.identity);
        Color c = new Color(pMat.color.r, pMat.color.g, pMat.color.b, 1);
        particle.renderer.material.color = c;

		InvokeRepeating("CheckMovement", 0, 0.01f);
	}

    void PlayerReady()
    {

        MeshRenderer[] renders = SpawnZone.GetComponentsInChildren<MeshRenderer>();


        if (PlayerControllerState.ButtonDownY || Input.GetKeyDown(KeyCode.Y))
        {
            IsReadyToBegin = !IsReadyToBegin;

            Color c = Color.white;

            if (IsReadyToBegin) // use player color + alpha
            {
                c = new Color(pMat.color.r, pMat.color.g, pMat.color.b, 255);
                
                GameManager.Instance.readyCounter++;

                if (GameManager.Instance.readyCounter < 4)
                    AudioManager.Instance.PlaySound(AudioManager.Instance.ReadySounds[GameManager.Instance.readyCounter]);

            }
            else
            {
                GameManager.Instance.readyCounter--;
            }

            foreach (MeshRenderer r in renders)
            {
                r.material.color = c;
            }
        }

        playerAim.aimTran.renderer.enabled = false;
        playerAim.chargeBar.renderer.enabled = false;
        playerAim.aimCross.renderer.enabled = false;
    }
        

	void CheckMovement()
	{
		if(PlayerControllerState.GetCurrentState().ThumbSticks.Left.X != 0
		|| PlayerControllerState.GetCurrentState().ThumbSticks.Left.Y != 0
		|| PlayerControllerState.ButtonDownA
		|| respawnIdleTimer <= 0)
		{
			PState = PlayerState.Alive;
			
			ChosenSpawn.SetActive(true);
			SpawnZone.SetActive(false);

			CancelInvoke("CheckMovement");
			respawnIdleTimer = RespawnIdleTime;
		}
	}

	void ChooseSpawnPoint()
	{
	    spawnsToChoose = new List<GameObject>(); // copy of original

		//Finds all active spawnpoints and stores them
        for(int i = 0; i<spawnPoints.Length; i++)
        {
        	if(spawnPoints[i].activeInHierarchy)
        	{
        		spawnsToChoose.Add(spawnPoints[i]);
        	}
        }

        // shuffle potential spawn points
        for (int i = 0; i < spawnsToChoose.Count; i++)
	    {
            GameObject temp = spawnsToChoose[i];
            GameObject random = spawnsToChoose[Random.Range(0, spawnsToChoose.Count)];

            spawnsToChoose[i] = random;
	    }
	    //Picks a random active spawnpoint and places the player there
        if(spawnsToChoose.Count != 0)
        {
        	ChosenSpawn = spawnsToChoose[Random.Range(0,spawnsToChoose.Count-1)];
        	pTran.position = ChosenSpawn.transform.position;
        	pTran.forward = ChosenSpawn.transform.forward;
        }

        ChosenSpawn.SetActive(false);
	}

	void EnableRenderers(bool on)
	{
		if(BodyRenderer != null)
			BodyRenderer.enabled = on;
		else
			renderer.enabled = on;

		if(HelmetRenderers[0] != null)
		{
			for(int i=0; i<HelmetRenderers.Length; i++)
			{
				HelmetRenderers[i].enabled = on;
			}
		}
	}

	void FaceMiddle()
	{

	}
}
