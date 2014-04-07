using System.Collections.Generic;
using System.Linq;
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
    	List<GameObject> chooseTarget = GetComponent<ChooseTargetType>().TargetList;

    	for(int i = 0; i<chooseTarget.Count; i++)
    	{
    		if(chooseTarget[i] != null)
    			chooseTarget[i].SetActive(false);

    		chooseTarget[i] = ZoneAreas[Random.Range(0,3)];
    		chooseTarget[i].SetActive(true);
    	}
    }
}
