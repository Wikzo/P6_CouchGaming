using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class ChooseIntelTargetType : ChooseTargetType
{
    private MissionIntel missionIntel;

    public GameObject IntelToSteal;

    void Awake()
    {
        missionIntel = GetComponent<MissionIntel>();
        if (missionIntel == null)
            Debug.Log("ERROR - needs to have a mission attached to template game object!");

        if (IntelToSteal == null)
            Debug.Log("ERROR - needs to assign IntelPropToSteal for " + this);

        this.chooseTarget = missionIntel.HowToChooseTarget;

        DecideHowToChooseTarget();
    }

    public override void ChooseTargetBasedOnListPool()
    {
        missionIntel.TargetPool = TargetList;
        missionIntel.IntelPropToSteal = IntelToSteal;

    }
}
