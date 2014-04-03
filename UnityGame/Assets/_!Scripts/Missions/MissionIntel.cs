using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MissionIntel : MissionBase
{
    // Intel base = Target
    // IntelToSteal = prop to steal
    public GameObject IntelPropToSteal;

    public override void InitializeMission(GameObject player, MissionBase Template)
    {
        base.InitializeMission(player, Template);
        SetTargetBaseAndIntel(Template);
    }

    void SetTargetBaseAndIntel(MissionBase template)
    {
        if (template is MissionIntel)
        {
            MissionIntel intel = (MissionIntel)template;
            this.IntelPropToSteal = intel.IntelPropToSteal;
            //Debug.Log("Successfully casted from MissionBase to MissionIntel");
        }
        else
            Debug.Log("ERROR - could not cast from MissionBase to MissionIntel!");
    }

    public override bool MissionAccomplished()
    {
        _missionIsActive = false;

        return false;
        if (IntelPropToSteal.transform.position.x > Target.transform.position.x)
        {
            _missionIsActive = false;
            return true;
        }

        return false;
    }
}
