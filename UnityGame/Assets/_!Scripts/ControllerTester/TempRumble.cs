using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class TempRumble : MonoBehaviour {

	private int pattern = 1;
	private float intensityTimer;

	private float lowSpeed = 3;
	private float highSpeed = 2;

	private bool intensityTimerInitialized = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(pattern)
		{
			case 0:
				if(intensityTimerInitialized == false)
				{
					intensityTimer = 0;
					intensityTimerInitialized = true;
				}

				if(intensityTimer <= 1)
					intensityTimer += Time.deltaTime/lowSpeed;
				break;
			case 1:
				if(intensityTimerInitialized == false)
				{
					intensityTimer = 0;
					intensityTimerInitialized = true;
				}

				if(intensityTimer <= 1)
					intensityTimer += Time.deltaTime/highSpeed;
				break;
			case 2:
				if(intensityTimerInitialized == false)
				{
					intensityTimer = 1;
					intensityTimerInitialized = true;
				}

				if(intensityTimer >= 0.01f)
					intensityTimer -= Time.deltaTime/lowSpeed;
				break;
			case 3:
				if(intensityTimerInitialized == false)
				{
					intensityTimer = 1;
					intensityTimerInitialized = true;
				}

				if(intensityTimer >= 0.01f)
					intensityTimer -= Time.deltaTime/highSpeed;
				break;
		}
		GamePad.SetVibration(PlayerIndex.One, intensityTimer, intensityTimer);
	}

	void OnApplicationQuit()
	{
		GamePad.SetVibration(PlayerIndex.One, 0, 0);
	}
}
