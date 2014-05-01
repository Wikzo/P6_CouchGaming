using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public abstract class ControllerTesterRumble
{
    protected List<ControllerPlayer> players;
    protected ControllerTesterManager manager;

    public ButtonsToPress pattern; // A = 0, B = 1, X = 2, Y = 3

    public ControllerTesterRumble(List<ControllerPlayer> controllerPlayers, ControllerTesterManager controllerManager)
    {
        players = controllerPlayers;
        manager = controllerManager;
    }
    public virtual void StartRumble(int patternToUse)
    {
        this.pattern = (ButtonsToPress)patternToUse;
        if (patternToUse == 4) // random
        {
            System.Random random = new System.Random();
            this.pattern = (ButtonsToPress)random.Next(0, 4);
        }

        manager.RumblingRightNow = true;
        manager.ReadyToGetInputPreTime = true;

    }
    public abstract void UpdateRumble();

    void CheckIfActive()
    {
        foreach (ControllerPlayer player in players)
            GamePad.SetVibration(player.Index, 0, 0);
    }
    
    public void StopRumble()
    {
        manager.ReadyToGetInput = true;
        manager.ReadyToGetInputPreTime = true;

        manager.RumblingRightNow = false;
        manager.RumbleTimer = 0;

        foreach (ControllerPlayer player in players)
            GamePad.SetVibration(player.Index, 0, 0);
    }

    public override string ToString()
    {
        return this.GetType().Name;// returns excact name of C# file (no spaces)
    }

}