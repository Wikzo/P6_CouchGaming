using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;


public class RightLeft : ControllerTesterRumble
{

    // X = left
    // B = right
    //  A = 0, B = 1, X = 2, Y = 3

    public RightLeft(List<ControllerPlayer> controllerPlayers, ControllerTesterManager controllerManager) : base(controllerPlayers, controllerManager) { }

    public override void StartRumble(int patternToUse)
    {
        base.StartRumble(patternToUse);

        // we only use two patterns, so convert if neccessary
        if (pattern == ButtonsToPress.A) // 0
            pattern = ButtonsToPress.X; // 2
        
        if (pattern == ButtonsToPress.Y) // 3
            pattern = ButtonsToPress.B; // 1
    }

    public override void UpdateRumble()
    {
        switch ((int)pattern)
        {
            case 0: // A (left)
            case 2: // X (left)
                foreach (ControllerPlayer player in players)
                    GamePad.SetVibration(player.Index, 0, 1f);
                break;

            case 1: // B (right)
            case 3: // Y (right)
                foreach (ControllerPlayer player in players)
                    GamePad.SetVibration(player.Index, 0f, 1f);
                break;
        }

        if (manager.RumbleTimer > manager.RumbleInterval)
        {
            StopRumble();
        }
    }
}
