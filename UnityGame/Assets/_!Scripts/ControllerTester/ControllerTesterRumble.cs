using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public abstract class ControllerTesterRumble
{
    protected List<ControllerPlayer> players;
    protected ControllerTesterManager manager;

    protected int pattern; // A = 0, B = 1, X = 2, Y = 3

    public ControllerTesterRumble(List<ControllerPlayer> controllerPlayers, ControllerTesterManager controllerManager)
    {
        players = controllerPlayers;
        manager = controllerManager;
    }
    public virtual void StartRumble(int patternToUse)
    {
        manager.RumblingRightNow = true;

        this.pattern = patternToUse;
        if (patternToUse == 4) // random
        {
            System.Random random = new System.Random();
            this.pattern = random.Next(0, 4);
        }
    }
    public abstract void UpdateRumble();
    
    public void StopRumble()
    {
        foreach (ControllerPlayer player in players)
            GamePad.SetVibration(player.Index, 0, 0);
    }

}