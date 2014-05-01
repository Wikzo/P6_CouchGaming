using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;


public abstract class ControllerTesterRumble
{
    List<PlayerIndex> playerIndex;

    public ControllerTesterRumble(List<PlayerIndex> playerIndexes)
    {
        playerIndex = playerIndexes;
    }
    public abstract void StartRumble(int pattern);
    public abstract void UpdateRumble();
    
    public void StopRumble()
    {
        foreach(PlayerIndex player in playerIndex)
            GamePad.SetVibration(player, 0, 0);
    }

}