using UnityEngine;
using System.Collections;

public class PlayerDamage : MonoBehaviour
{

    public bool IsClone;
    public GameObject OriginalPlayer;

	//public AudioClip missionSound;
	private Player playerScript;

	//Lo-Fi
	private Transform shield;
	private bool shieldEnabled = false;

	// Use this for initialization
	void Start ()
	{
	    if (IsClone)
	    {
            if (OriginalPlayer == null)
                Debug.Log("ERROR - clone needs to have assigned link to original player");
	        
            playerScript = OriginalPlayer.GetComponent<Player>();
	    }
	    else
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
		/*if(playerScript.LoFi)
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
		}*/
	}

	public void CalculateDeath(string killObjectTag, string objectOwner)
	{
		if(playerScript.PState == PlayerState.Alive)
		{	
			if(killObjectTag == "Projectile" && shieldEnabled == false)
			{
				playerScript.KilledBy = objectOwner;
				StartCoroutine(playerScript.Die());
			}
		}
	}
}

