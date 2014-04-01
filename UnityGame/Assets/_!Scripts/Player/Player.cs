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

	public int Score;
	public int Id;
	public int DeathTime = 5;
	public int RespawnTime = 3;
	public GameObject[] SpawnPoints = new GameObject[4];
	public Material[] Materials = new Material[4];

	[HideInInspector]
	public PlayerState PState;
	[HideInInspector]
	public string KilledBy;
	[HideInInspector]
	public PlayerIndex PlayerController;
	private Transform pTran;
	private GameObject spawnPoint;

	// Use this for initialization
	void Awake () 
	{
		pTran = transform;

		switch(Id)
		{
			case 0:
			PlayerController = PlayerIndex.One;
			spawnPoint = SpawnPoints[0];
			renderer.material = Materials[0];
			break;
			case 1:
			PlayerController = PlayerIndex.Two;
			spawnPoint = SpawnPoints[1];
			renderer.material = Materials[1];
			break;
			case 2:
			PlayerController = PlayerIndex.Three;
			spawnPoint = SpawnPoints[2];
			renderer.material = Materials[2];
			break;
			case 3:
			PlayerController = PlayerIndex.Four;
			spawnPoint = SpawnPoints[3];
			renderer.material = Materials[3];
			break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

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
