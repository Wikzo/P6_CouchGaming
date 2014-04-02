using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public enum PlayerState
{
	Alive,
	Respawning,
	Dead
};

public class Player : MonoBehaviour {

	public bool LoFi = false;

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

	private Transform pTran;
	private GameObject spawnPoint;
	private Material pMat;

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
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(PState == PlayerState.Respawning)
		{
			float lerp = Mathf.PingPong(Time.time, RespawnBlinkRate) / RespawnBlinkRate;
			renderer.material.Lerp(Materials[0], blinkMat, lerp);
		}
		else
			renderer.material = pMat;

	}

	public IEnumerator Die()
	{
		PState = PlayerState.Dead;
		renderer.enabled = false;
		pTran.position = new Vector3(-1000,-1000,-1000);
		yield return new WaitForSeconds(DeathTime);
		StartCoroutine(Respawn());
	}

	public IEnumerator Respawn()
	{
		PState = PlayerState.Respawning;
		renderer.enabled = true;
		pTran.position = spawnPoint.transform.position;
		yield return new WaitForSeconds(RespawnTime);
		PState = PlayerState.Alive;
	}
}
