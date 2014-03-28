using UnityEngine;
using System.Collections;

public class MissionPosition : MissionBase
{
    public override bool MissionAccomplished()
    {
        if (this.Target.transform.position.y > 5)
        {
            _missionIsActive = false;
            return true;
        }

        return false;
    }
}
