using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public enum PlayerState
{
	Alive,
	Respawning,
	Dead
};


[RequireComponent(typeof(TargetIDColor))]
public class Player : MonoBehaviour {

	public bool LoFi = false;
	public bool Keyboard = false;

	public int Score;
	public int Id;
	public int DeathTime = 5;
	public float RespawnTime = 3;
	public float RespawnBlinkRate = 0.1f;
	public GameObject[] SpawnPoints = new GameObject[4];
	public Material[] Materials = new Material[4];
	public Material blinkMat;

	[HideInInspector]
	public PlayerState PState;
	[HideInInspector]
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

        if (GetComponent<TargetIDColor>() == null)
            Debug.Log("ERROR - player needs to have TargetIDColor component " + gameObject);
	    
        Name = gameObject.name;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(GameManager.Instance.PlayingState == PlayingState.Playing)
		{
			playerAim.AimUpdate();
			playerMove.MoveUpdate();
			playerJump.JumpUpdate();
		}

		if(PState == PlayerState.Respawning)
		{
			float lerp = Mathf.PingPong(Time.time, RespawnBlinkRate) / RespawnBlinkRate;
			renderer.material.Lerp(pMat, blinkMat, lerp);
		}
		else
			renderer.material = pMat;

	}

    public void RemoveAllMissionsOnMe()
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

		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		transform.rotation = Quaternion.identity;

		renderer.enabled = false;
		pTran.position = new Vector3(-1000,-1000,-1000);
		yield return new WaitForSeconds(DeathTime);
		StartCoroutine(Respawn());
	}

	public IEnumerator Respawn()
	{
		KilledBy = "";
		PState = PlayerState.Respawning;
		renderer.enabled = true;
		pTran.position = spawnPoint.transform.position;
		yield return new WaitForSeconds(RespawnTime);
		PState = PlayerState.Alive;
	}
}
