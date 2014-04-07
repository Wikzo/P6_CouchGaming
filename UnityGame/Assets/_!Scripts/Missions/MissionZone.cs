using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MissionZone : MissionBase
{
    public GameObject[] ZoneAreas = new GameObject[4];

    public override bool MissionAccomplished()
    {
        if(this.Target.GetComponent<ZoneTrigger>().Accomplished)
        {
        	this._missionIsActive = false;
			return true;
        }
		else
			return false;
    }

    public override void TemplateSetUp()
    {
    	base.TemplateSetUp();

    	GameObject chosenZone = ZoneAreas[Random.Range(0,3)];
    	chosenZone.active = true;
    	GetComponent<ChooseTargetType>().TargetList[0] = chosenZone;
    }
}
