using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MissionIntel : MissionBase
{
    // Intel base = Target
    // IntelToSteal = prop to steal

    public GameObject IntelPropToSteal;
    public float XOffset = 0.1f;
    public float YOffset = 1f;

    private float baseXLeft, baseXRight, intelXLeft, intelXRight;

    public override void InitializeMission(GameObject player, MissionBase Template)
    {
        base.InitializeMission(player, Template);
        this.TargetPool = Template.TargetPool; // needs to know about all potential targets (bases) to see if somebody else won before me
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
        // winning condition: if intel is inside a base that is MINE

        if (IntelPropToSteal == null) // e.g. another player has already solved mission (destroyed intel)
            _missionIsActive = false;
        else
            _missionIsActive = true;

        // bounds intersects looks at if objects touch each other
        if (IntelPropToSteal.renderer.bounds.Intersects(Target.renderer.bounds))
        {
            //Destroy(IntelPropToSteal); // TODO: remove intel from scene so it can't be used for other missions
            //IntelPropToSteal.rigidbody.isKinematic = true;
            _missionIsActive = false;
            return true;
        }

        return false;
    }
}
