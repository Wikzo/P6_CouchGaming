using UnityEngine;
using System.Collections;

public class PlayerDamage : MonoBehaviour {


	private Player playerScript;

	// Use this for initialization
	void Start () 
	{
		playerScript = GetComponent<Player>();	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void CalculateDeath(string killObject, string objectOwner)
	{
		if(playerScript.PState == PlayerState.Alive)
		{	
			if(killObject == "LaserBullet")
			{
				playerScript.KilledBy = objectOwner;
				StartCoroutine(playerScript.Die());
			}
		}
	}
}

