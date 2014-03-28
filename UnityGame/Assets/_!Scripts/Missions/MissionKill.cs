using UnityEngine;
using System.Collections;

public class MissionKill : MissionBase
{
    // specific KillMission fields
    private bool targetWasAliveWhenMissionBegan;

    public override void InitializeMission(GameObject player, GameObject target, MissionBase Template)
    {
        base.InitializeMission(player, target, Template);
        
        if (target != null)
            targetWasAliveWhenMissionBegan = true;
    }
    
    public override bool MissionAccomplished()
    {
        if (!this.targetWasAliveWhenMissionBegan) // no target
            return false;

        if (this.Target == null) // target has died
        {
            this._missionIsActive = false;
            return true;
        }

        return false; // nothing
    }
}
