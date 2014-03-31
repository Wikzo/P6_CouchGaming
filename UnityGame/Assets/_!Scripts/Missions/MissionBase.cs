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
    ChooseTargetBasedOnChildren,
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

    public bool IsTemplateMission; //TODO: target system does not work on children


    //public MissionBase Template;

    // An abstract function has to be overridden while a virtual function may be overridden.

    public virtual void InitializeMission(GameObject player, GameObject target, MissionBase Template)
    {
        if (Template == null)
            Debug.Log("ERROR, Mission Templates have not been assigned!");

        IsTemplateMission = false; // this function is not called on template missions, anyway

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
        if (Template.Target == null)
            return null;

        System.Random random = new System.Random();

        switch (Template.HowToChooseTarget)
        {
                case HowToChooseTarget.ChooseTargetBasedOnChildren:
                case HowToChooseTarget.ChooseTargetAmongAllPlayers:
                    return Template.TargetPool[random.Next(0, Template.TargetPool.Count)];
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
