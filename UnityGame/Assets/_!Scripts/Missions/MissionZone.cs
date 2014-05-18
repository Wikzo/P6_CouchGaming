using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class MissionZone : MissionBase
{
    public GameObject[] ZoneAreas = new GameObject[4];

    private static List<int> ZonesUsed = new List<int>();

    public override bool MissionAccomplished()
    {
        if (!canGo)
            return false;

        if(this.Target.GetComponent<ZoneTrigger>().Accomplished)
        {
        	this._missionIsActive = false;
            this.Target.GetComponent<ZoneTrigger>().RestartZone();
			return true;
        }
		else
			return false;
    }

    public override void TemplateSetUp()
    {
    	base.TemplateSetUp();
    	List<GameObject> chooseTarget = GetComponent<ChooseTargetType>().TargetList;

    	for(int i = 0; i<chooseTarget.Count; i++)
    	{
    		if(chooseTarget[i] != null)
    			chooseTarget[i].SetActive(false);

    	    int zoneTarget = GetUniqueTarget(ZonesUsed, 3, 0, ZoneAreas.Count());

    		chooseTarget[i] = ZoneAreas[zoneTarget];
    		chooseTarget[i].SetActive(true);
    		//chooseTarget[i].GetComponent<ZoneTrigger>().RestartZone();
    	}
    }
}
