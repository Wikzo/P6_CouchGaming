using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class ControllerTesterRumbleVaryingIntensity : ControllerTesterRumble {

	private float intensityTimer;

	private float upLowTarget = 0.5f;
	private float upHighTarget = 1f;
	private float downLowTarget = 0f;
	private float downHighTarget = 0.5f;

	private float lerpValue = 0;
	private float normalizedTimer = 0;
	
	public ControllerTesterRumbleVaryingIntensity(List<ControllerPlayer> controllerPlayers, ControllerTesterManager controllerManager) : base(controllerPlayers, controllerManager)
	{

	}

	public override void StartRumble(int patternToUse)
	{
		base.StartRumble(patternToUse);
	}
    public override void UpdateRumble()
    {
		normalizedTimer = manager.RumbleTimer/manager.RumbleInterval;
		switch((int)pattern)
		{
			//Output = (1 - t) * Input1 + t * Input2
			case 0:
				lerpValue = (1-normalizedTimer) * 0 + normalizedTimer * upLowTarget;
				if(lerpValue < upLowTarget)
				{
					foreach (ControllerPlayer player in players)
						GamePad.SetVibration(player.Index, lerpValue, lerpValue);
				}
				else
					StopRumble();
		
				break;
			case 1:
				lerpValue = (1-normalizedTimer) * 0 + normalizedTimer * upHighTarget;
				if(lerpValue < upHighTarget)
				{
					foreach (ControllerPlayer player in players)
						GamePad.SetVibration(player.Index, lerpValue, lerpValue);
				}
				else
					StopRumble();
		
				break;
			case 2:
				lerpValue = (1-normalizedTimer) * 1 + normalizedTimer * downLowTarget;
				if(lerpValue > downLowTarget)
				{
					foreach (ControllerPlayer player in players)
						GamePad.SetVibration(player.Index, lerpValue, lerpValue);
				}
				else
					StopRumble();

				break;
			case 3:
				lerpValue = (1-normalizedTimer) * 1 + normalizedTimer * downHighTarget;
				if(lerpValue > downHighTarget)
				{
					foreach (ControllerPlayer player in players)
						GamePad.SetVibration(player.Index, lerpValue, lerpValue);
				}
				else
					StopRumble();

				break;
		}
    }
}
