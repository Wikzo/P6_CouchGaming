using UnityEngine;
using System.Collections;

public class MissionPosition : MissionBase
{
    public override bool MissionAccomplished()
    {
        if (this.Player.transform.position.y > 5)
        {
            MissionIsActive = false;
            return true;
        }

        return false;
    }
}
