using UnityEngine;
using System.Collections;

public class PlayerDamage : MonoBehaviour {

	public AudioClip missionSound;
	private Player playerScript;

	//Lo-Fi
	private Transform shield;
	private bool shieldEnabled = false;

	// Use this for initialization
	void Start () 
	{
		playerScript = GetComponent<Player>();	

		//LO-FI
		if(playerScript.LoFi && playerScript.Id == 0)
		{
			shield = transform.Find("Shield");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//LO-FI
		if(playerScript.LoFi)
		{
			if(Input.GetKeyDown(KeyCode.D))
			{		
				shield.GetComponent<MeshRenderer>().enabled = !shield.GetComponent<MeshRenderer>().enabled;
				shieldEnabled = !shieldEnabled;
			}
			if(Input.GetKeyDown(KeyCode.S))
			{
				audio.clip = missionSound;
				audio.Play();
			}
		}
	}

	public void CalculateDeath(string killObject, string objectOwner)
	{
		if(playerScript.PState == PlayerState.Alive)
		{	
			if(killObject == "Projectile" && shieldEnabled == false)
			{
				playerScript.KilledBy = objectOwner;
				StartCoroutine(playerScript.Die());
			}
		}
	}
}

