using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class Interval : ControllerTesterRumble
{
    public Interval(List<ControllerPlayer> controllerPlayers, ControllerTesterManager controllerManager) : base(controllerPlayers, controllerManager) {}

    int targetRumbleCounter;
    float targetTimeCounter = 0;
    float intervalTime = 0.2f;

    public override void StartRumble(int patternToUse)
    {
        base.StartRumble(patternToUse);

        targetRumbleCounter = (int)pattern + 1; // between 1 and 4

        RumbleDuration = (intervalTime + intervalTime * 3) * targetRumbleCounter;

    }

    public override void UpdateRumble()
    {
        if (targetRumbleCounter > 0)
            TargetRumbler(intervalTime);
        else
            StopRumble();

    }

    public void TargetRumbler(float interval) // display target (rumble)
    {
        if (targetTimeCounter < interval)
        {
            targetTimeCounter += Time.deltaTime;
            foreach (ControllerPlayer player in players)
                GamePad.SetVibration(player.Index, 0.8f, 0.8f);
        }
        else if (targetTimeCounter < interval * 3)
        {
            targetTimeCounter += Time.deltaTime;
            foreach (ControllerPlayer player in players)
                GamePad.SetVibration(player.Index, 0, 0);
        }
        else
        {
            targetTimeCounter = 0;
            targetRumbleCounter--;
        }
    }

}
