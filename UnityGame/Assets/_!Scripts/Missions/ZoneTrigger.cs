using UnityEngine;
using System.Collections;

public class ZoneTrigger : MonoBehaviour {

	[HideInInspector]
	public bool Accomplished = false;

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

		if(progressBar.localScale.z <= background.localScale.z)
		{
			if(playersColliding > 0)
				progressCounter += Time.deltaTime;
				
			else if(progressCounter > 0)
				progressCounter -= Time.deltaTime/DownSlowFactor;

			progressBar.localScale = new Vector3(progressBar.localScale.x, progressBar.localScale.y, progressCounter/AccomplishTime);
			//progressBar.position = new Vector3(progressBar.position.x, pTran.position.y-pTran.localScale.y/2+progressBar.localScale.y, progressBar.position.z);
		}
		else
			Accomplished = true;
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
		progressCounter = 0;
	}
}