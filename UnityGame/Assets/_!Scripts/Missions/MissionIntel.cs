using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MissionIntel : MissionBase
{

    public List<GameObject> IntelBasePool;
    public List<GameObject> IntelPropPool;

    public GameObject IntelBase;
    public GameObject IntelProp;

    public override void InitializeMission(GameObject player, GameObject target, MissionBase Template)
    {
        base.InitializeMission(player, target, Template);
        this.Target = null;
        SetTargetBaseAndIntel(Template);
    }

    void SetTargetBaseAndIntel(MissionBase template)
    {
        if (template is MissionIntel)
        {
            MissionIntel intel = (MissionIntel)template;
            this.IntelBase = intel.IntelBase;
            this.IntelProp = intel.IntelProp;
            Debug.Log("Successfully casted from MissionBase to MissionIntel");
        }
        else
            Debug.Log("ERROR - could not cast from MissionBase to MissionIntel!");
    }

    public override bool MissionAccomplished()
    {
        _missionIsActive = false;

        return false;
        if (IntelProp.transform.position.x > IntelBase.transform.position.x)
        {
            _missionIsActive = false;
            return true;
        }

        return false;
    }
}
