using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;


public class StaticIntensity : ControllerTesterRumble
{
    public StaticIntensity(List<ControllerPlayer> controllerPlayers, ControllerTesterManager controllerManager) : base(controllerPlayers, controllerManager) {}

    public override void StartRumble(int patternToUse)
    {
        base.StartRumble(patternToUse);
    }

    public override void UpdateRumble()
    {
        switch ((int)pattern)
        {
            case 0:
                foreach (ControllerPlayer player in players)
                    GamePad.SetVibration(player.Index, 0.2f, 0.2f);
                break;

            case 1:
                foreach (ControllerPlayer player in players)
                    GamePad.SetVibration(player.Index, 0.4f, 0.4f);
                break;

            case 2:
                foreach (ControllerPlayer player in players)
                    GamePad.SetVibration(player.Index, 0.6f, 0.6f);
                break;

            case 3:
                foreach (ControllerPlayer player in players)
                    GamePad.SetVibration(player.Index, 0.8f, 0.8f);
                break;

        }

        if (manager.RumbleTimer > RumbleDuration)
        {
            StopRumble();
        }
    }
}