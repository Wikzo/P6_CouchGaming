using UnityEngine;
using System.Collections;

public class MissionZone : MissionBase
{
    public override bool MissionAccomplished()
    {
        return false;
    }

    public override void TemplateSetUp()
    {
    	base.TemplateSetUp();

    	//GetComponent<ZoneSpawner>().CanSpawn = true;
    }
}
