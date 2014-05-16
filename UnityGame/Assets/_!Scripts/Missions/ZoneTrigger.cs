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

	// Use this for initialization
	void Start () 
	{
		pTran = transform;
		if(pTran.Find("ZoneProgressBar") != null)
			progressBar = pTran.Find("ZoneProgressBar");
		else
			progressBar = pTran.Find("HologramFront");

		background = pTran.Find("HologramBack");
	}
	
	// Update is called once per frame
	void Update () 
	{
        // only rumble if playing
        if (GameManager.Instance.PlayingState != PlayingState.Playing)
            return;

		if(progressBar.localScale.z <= background.localScale.z && readyToTrigger == true)
		{
			if(playersColliding > 0)
				progressCounter += Time.deltaTime;
		}
		else
			Accomplished = true;

		if(playersColliding == 0 && progressCounter > 0)
				progressCounter -= Time.deltaTime/DownSlowFactor;

		if(readyToTrigger == false && playersColliding == 0)
			readyToTrigger = true;

		progressBar.localScale = new Vector3(progressBar.localScale.x, progressBar.localScale.y, progressCounter/AccomplishTime);
		//progressBar.position = new Vector3(progressBar.position.x, pTran.position.y-pTran.localScale.y/2+progressBar.localScale.y, progressBar.position.z);
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
		Accomplished = false;
		readyToTrigger = false;
		progressCounter = 0;

        // TODO: for BENJAMIN:
        // this reset function does not work! I call it from MissionZone.cs, but it needs to reset the progressBar scale things
	}
}