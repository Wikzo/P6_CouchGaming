using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class ControllerTesterRumbleVaryingIntensity : ControllerTesterRumble {

	private float intensityTimer;

	private float lowSpeed = 3;
	private float highSpeed = 2;

	private bool intensityTimerInitialized = false;
	
	public ControllerTesterRumbleVaryingIntensity(List<ControllerPlayer> controllerPlayers, ControllerTesterManager controllerManager) : base(controllerPlayers, controllerManager)
	{

	}

	public override void StartRumble(int patternToUse)
	{
		base.StartRumble(patternToUse);
	}
    public override void UpdateRumble()
    {
		switch((int)pattern)
		{
			case 0:
				if(intensityTimerInitialized == false)
				{
					intensityTimer = 0;
					intensityTimerInitialized = true;
				}

				if(intensityTimer <= 1)
				{
					intensityTimer += manager.RumbleTimer/lowSpeed;
					GamePad.SetVibration(PlayerIndex.One, intensityTimer, intensityTimer);
				}
				else
					StopRumble();
				break;
			case 1:
				if(intensityTimerInitialized == false)
				{
					intensityTimer = 0;
					intensityTimerInitialized = true;
				}

				if(intensityTimer <= 1)
				{
					intensityTimer += manager.RumbleTimer/highSpeed;
					GamePad.SetVibration(PlayerIndex.One, intensityTimer, intensityTimer);
				}
				else
					StopRumble();
				break;
			case 2:
				if(intensityTimerInitialized == false)
				{
					intensityTimer = 1;
					intensityTimerInitialized = true;
				}

				if(intensityTimer >= 0.01f)
				{
					intensityTimer -= manager.RumbleTimer/lowSpeed;
					GamePad.SetVibration(PlayerIndex.One, intensityTimer, intensityTimer);
				}
				else
					StopRumble();
				break;
			case 3:
				if(intensityTimerInitialized == false)
				{
					intensityTimer = 1;
					intensityTimerInitialized = true;
				}

				if(intensityTimer >= 0.01f)
				{
					intensityTimer -= manager.RumbleTimer/highSpeed;
					GamePad.SetVibration(PlayerIndex.One, intensityTimer, intensityTimer);
				}
				else
					StopRumble();
				break;
		}
    }
}
