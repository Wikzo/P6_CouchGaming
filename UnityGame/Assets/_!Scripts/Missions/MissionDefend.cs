using UnityEngine;
using System.Collections;

public class MissionDefend : MissionBase
{
    private bool targetWasAliveWhenBegan;

    public override bool MissionAccomplished()
    {
        return false;
        if (Target.GetComponent<Player>().PState == PlayerState.Alive)
        {
            _missionIsActive = false;
            return true;
        }

        return false;
    }
}
