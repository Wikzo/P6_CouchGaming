using UnityEngine;
using System.Collections;

public class KillMission : MissionBase
{
    // specific KillMission fields
    private bool targetWasAliveWhenMissionBegan;

    public override void InitializeMission(GameObject player, GameObject target, int points)
    {
        base.InitializeMission(player, target, points);
        
        if (target != null)
            targetWasAliveWhenMissionBegan = true;
    }
    
    public override bool MissionAccomplished()
    {
        if (!this.MissionIsActive) // mission not active
            return false;

        if (!this.targetWasAliveWhenMissionBegan) // no target
            return false;

        if (this.Target == null) // target has died
        {
            this.MissionIsActive = false;
            return true;
        }

        return false; // nothing
    }
}
