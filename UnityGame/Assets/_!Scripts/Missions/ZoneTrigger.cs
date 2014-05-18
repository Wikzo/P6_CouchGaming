using UnityEngine;
using System.Collections;

public class ZoneTrigger : MonoBehaviour {

	[HideInInspector]
	public bool Accomplished = false;
	private bool readyToTrigger = true;

	public float AccomplishTime = 5;
	public float DownSlowFactor = 1;

	private float progressCounter = 0;

	private Transform pTran;
	private Transform background;
	private Transform progressBar;
	private int playersColliding = 0;
	private float progressBarY = 0;

	private bool hasPlayedSound = false;

	// Use this for initialization
	void Start () 
	{
		pTran = transform;
		if(pTran.Find("ZoneProgressBar") != null)
			progressBar = pTran.Find("ZoneProgressBar");
		else
			progressBar = pTran.Find("HologramFront");

		background = pTran.Find("HologramBack");

		audio.clip = AudioManager.Instance.ZoneIncrease;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(progressBar.localScale.z <= background.localScale.z && readyToTrigger == true)
		{
			if(playersColliding > 0)
			{
				progressCounter += Time.deltaTime;

				if(hasPlayedSound == false)
				{
					audio.volume = 0.5f;
					audio.Play();
					hasPlayedSound = true;
				}
			}
		}
		else
			Accomplished = true;

		if(playersColliding == 0 && progressCounter > 0)
		{
			progressCounter -= Time.deltaTime/DownSlowFactor;

			if(hasPlayedSound == false)
			{
				audio.volume = 0.5f;
				audio.Play();
				hasPlayedSound = true;
			}
		}
		else if(playersColliding == 0 && progressCounter < 0)
		{
			audio.Stop();
			hasPlayedSound = false;
		}

		if(readyToTrigger == false && playersColliding == 0)
			readyToTrigger = true;
		audio.pitch = ((progressCounter/AccomplishTime)*2)+1;
		progressBar.localScale = new Vector3(progressBar.localScale.x, 0.75f + (progressCounter/AccomplishTime)/3, progressCounter/AccomplishTime);

		//progressBar.position = new Vector3(progressBar.position.x, pTran.position.y-pTran.localScale.y/2+progressBar.localScale.y, progressBar.position.z);

		// only rumble if playing
        if (GameManager.Instance.PlayingState != PlayingState.Playing)
        {
            audio.Stop();
            return;
        }
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.GetComponent<Player>())
			playersColliding++;
	}

	void OnTriggerExit(Collider collider)
	{
		if(collider.gameObject.GetComponent<Player>())
			playersColliding--;
	}

	public void RestartZone()
	{
		audio.Stop();
		hasPlayedSound = false;
		Accomplished = false;
		readyToTrigger = false;
		progressCounter = 0;
	}
}