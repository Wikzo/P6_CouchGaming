using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public enum MissionType
{
    PlayerToPlayerMission,
    EnvironmentalMission,
    PropMission,
    MiscMission
}

public enum HowToChooseTarget
{
    ChooseTargetBasedOnList,
    //ChooseTargetBasedOnChildren,
    ChooseTargetAmongAllPlayers,
    ChooseTargetAmongAllPlayersExceptMe,
    Other
}

public abstract class MissionBase : MonoBehaviour
{
    public int Points;
    
    protected bool _missionIsActive; // only this class + derived classes can access this
    public bool MissionIsActive{get { return _missionIsActive; }} // getter property

    [HideInInspector]
    public GameObject Player;

    [HideInInspector]
    public List<GameObject> TargetPool;
    
    //[HideInInspector]
    public GameObject Target;
    
    public MissionType MissionType;
    public HowToChooseTarget HowToChooseTarget;
    //public Texture2D Texture;
    
    [HideInInspector]
    public int MissionRumbleCount;

    [HideInInspector]
    public int TargetRumbleCount;

    //public MissionBase Template;

    // An abstract function has to be overridden while a virtual function may be overridden.

    public virtual void InitializeMission(GameObject player, MissionBase Template)
    {
        if (Template == null)
            Debug.Log("ERROR, Mission Templates have not been assigned for " + player);

        if (Template.TargetPool.Count <= 0)
            Debug.Log("ERROR - Target pool is empty for " + player);

        // Use specific values
        this.Player = player;
        this._missionIsActive = true;
        this.Target = ChooseRandomTarget(Template);

        // Use template values
        this.MissionType = Template.MissionType;
        this.Points = Template.Points;


        //Debug.Log(string.Format("Mission {0} initialized for Player {1} with Target {2}", this, this.Player, this.Target.transform.name));
    }

    public GameObject ChooseRandomTarget(MissionBase Template)
    {
        System.Random random = new System.Random();

        switch (Template.HowToChooseTarget)
        {
            //case HowToChooseTarget.ChooseTargetBasedOnChildren:
            case HowToChooseTarget.ChooseTargetBasedOnList:
            case HowToChooseTarget.ChooseTargetAmongAllPlayers:
                int r = random.Next(0, Template.TargetPool.Count);
                print(r);
                    return Template.TargetPool[r];
                break;

            case HowToChooseTarget.ChooseTargetAmongAllPlayersExceptMe:
                int counter = 0;
                GameObject target = Template.TargetPool[random.Next(0, Template.TargetPool.Count)];

                if (Template.HowToChooseTarget == HowToChooseTarget.ChooseTargetAmongAllPlayersExceptMe) // try to avoid myself
                {
                    while (this.Target == this.Player && counter < 100) // try to pick a new target
                    {
                        target = Template.TargetPool[random.Next(0, Template.TargetPool.Count)];
                        counter++;
                    }
                }
                return target;
                break;
            
            default:
                print("default");
                return null;
                break;

        }
    }

    public abstract bool MissionAccomplished();

    public override string ToString()
    {
        return this.GetType().Name; // returns excact name of C# file (no spaces)
    }
}
