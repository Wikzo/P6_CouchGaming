using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class ControllerTesterRumbleVaryingIntensity : ControllerTesterRumble {

	private float intensityTimer;

	private float increasingLowTarget = 0.5f;
	private float increasingHighTarget = 1f;
	private float decreasingLowTarget = 0f;
	private float decreasingHighTarget = 0.5f;

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
				lerpValue = (1-normalizedTimer) * 0 + normalizedTimer * increasingLowTarget;
				if(lerpValue < increasingLowTarget)
				{
					foreach (ControllerPlayer player in players)
						GamePad.SetVibration(player.Index, lerpValue, lerpValue);
				}
				else
					StopRumble();
		
				break;
			case 1:
				lerpValue = (1-normalizedTimer) * 0.5f + normalizedTimer * increasingHighTarget;
				if(lerpValue < increasingHighTarget)
				{
					foreach (ControllerPlayer player in players)
						GamePad.SetVibration(player.Index, lerpValue, lerpValue);
				}
				else
					StopRumble();
		
				break;
			case 2:
				lerpValue = (1-normalizedTimer) * 0.5f + normalizedTimer * decreasingLowTarget;
				if(lerpValue > decreasingLowTarget)
				{
					foreach (ControllerPlayer player in players)
						GamePad.SetVibration(player.Index, lerpValue, lerpValue);
				}
				else
					StopRumble();

				break;
			case 3:
				lerpValue = (1-normalizedTimer) * 1f + normalizedTimer * decreasingHighTarget;
				if(lerpValue > decreasingHighTarget)
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
