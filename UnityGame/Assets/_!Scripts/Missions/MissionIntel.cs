using UnityEngine;
using System.Collections;

public class MissionIntel : MissionBase
{

    public GameObject Base;

    public override bool MissionAccomplished()
    {

        if (Target.transform.position.x > base.transform.position.x)
            return true;

        return false;
    }
}
