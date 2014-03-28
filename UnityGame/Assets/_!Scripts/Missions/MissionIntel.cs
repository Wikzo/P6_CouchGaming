using UnityEngine;
using System.Collections;

public class MissionIntel : MissionBase
{

    public GameObject[] IntelBasePool;
    public GameObject[] IntelPropPool;

    public GameObject IntelBase;
    public GameObject IntelProp;

    public override void InitializeMission(GameObject player, GameObject target, MissionBase Template)
    {
        base.InitializeMission(player, target, Template);
        SetTargetBaseAndIntel(Template as MissionIntel);
    }

    void SetTargetBaseAndIntel(MissionIntel template)
    {
        this.IntelBase = template.IntelBase;
        this.IntelProp = template.IntelProp;
    }

    void Awake()
    {
        IntelBase = ChooseRandom(IntelBasePool);
        IntelProp = ChooseRandom(IntelPropPool);
    }

    GameObject ChooseRandom(GameObject[] g)
    {
        Random.seed = (int)System.DateTime.Now.Ticks;

        return g[Random.Range(0, g.Length)];
    }

    public override bool MissionAccomplished()
    {
        if (IntelProp.transform.position.x > IntelBase.transform.position.x)
        {
            _missionIsActive = false;
            return true;
        }

        return false;
    }
}
