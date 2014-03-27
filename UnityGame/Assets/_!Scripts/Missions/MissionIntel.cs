using UnityEngine;
using System.Collections;

public class MissionIntel : MissionBase
{

    public GameObject TargetBase;

    public override void InitializeMission(GameObject player, GameObject target, MissionBase Template)
    {
        base.InitializeMission(player, target, Template);
        SetTargetBase(Template as MissionIntel);
    }

    void SetTargetBase(MissionIntel mission)
    {
        this.TargetBase = mission.TargetBase;
    }

    public override bool MissionAccomplished()
    {
        if (Target.transform.position.x > TargetBase.transform.position.x)
            return true;

        return false;
    }
}
