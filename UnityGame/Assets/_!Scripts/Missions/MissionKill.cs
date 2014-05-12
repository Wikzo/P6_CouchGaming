using UnityEngine;
using System.Collections;

public class MissionKill : MissionBase
{
    // specific KillMission fields
    private bool targetWasAliveWhenMissionBegan;

    public override void InitializeMission(GameObject player, MissionBase Template)
    {
        base.InitializeMission(player, Template);
        
        /*if (target != null)
            targetWasAliveWhenMissionBegan = true;*/
    }
    
    public override bool MissionAccomplished()
    {
        if(this.Player.tag == this.Target.GetComponent<Player>().KilledBy)
        {
           this._missionIsActive = false;
            return true;
        }

        return false; // nothing
        //return true;
    }
}
