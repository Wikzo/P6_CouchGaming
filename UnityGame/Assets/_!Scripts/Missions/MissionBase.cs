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

public enum MissionIDRumbleState
{
    NotAssigned,
    One,
    Two,
    Three,
    Four
}

public abstract class MissionBase : MonoBehaviour
{
    public int Points;
    
    protected bool _missionIsActive; // only this class + derived classes can access this
    public bool MissionIsActive{get { return _missionIsActive; }} // getter property

    [HideInInspector]
    public GameObject Player;

    //[HideInInspector]
    public List<GameObject> TargetPool;
    
    //[HideInInspector]
    public GameObject Target;
    
    public MissionType MissionType;
    public HowToChooseTarget HowToChooseTarget;
    //public Texture2D Texture;

    public int MissionIDRumbleState; // what mission (1 to 4; Left Bumper rumble)
    public TargetIDColorState TargetIDColorState; // target color (Right Bumper rumble)

    public static List<int> TargetNumbersUsed = new List<int>();

    private int ChanceOfGettingUniqueTarget = 2; // higher value = bigger chance of NOT getting same player target

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

        this.HowToChooseTarget = Template.HowToChooseTarget;

        this.Target = ChooseRandomTarget(Template);

        // Rumble IDs
        //this.TargetIDColorState = Target.GetComponent<TargetIDColor>().TargetIDColorState;

        // Use template values
        this.MissionType = Template.MissionType;
        this.Points = Template.Points;
        this.MissionIDRumbleState = Template.MissionIDRumbleState;


        //Debug.Log(string.Format("Mission {0} initialized for Player {1} with Target {2}", this, this.Player, this.Target.transform.name));
    }

    public GameObject ChooseRandomTarget(MissionBase template)
    {
        System.Random random = new System.Random();

        switch (this.HowToChooseTarget)
        {
            //case HowToChooseTarget.ChooseTargetBasedOnChildren:
            case HowToChooseTarget.ChooseTargetBasedOnList:
            case HowToChooseTarget.ChooseTargetAmongAllPlayers:
                int randomAmongAll = GetUniqueTarget(0, template.TargetPool.Count);
                //print(r);
                return template.TargetPool[randomAmongAll];
                break;

            case HowToChooseTarget.ChooseTargetAmongAllPlayersExceptMe:
                int counter = 0;
                int randomExceptMyself = GetUniqueTarget(0, template.TargetPool.Count);

                GameObject tempTarget = template.TargetPool[randomExceptMyself];
                if (this.HowToChooseTarget == HowToChooseTarget.ChooseTargetAmongAllPlayersExceptMe) // try to avoid choosing myself
                {
                    while (tempTarget == this.Player && counter < 100) // try to pick a new target
                    {
                        randomExceptMyself = GetUniqueTarget(0, template.TargetPool.Count);
                        tempTarget = template.TargetPool[randomExceptMyself];
                        counter++;
                    }
                }
                return template.TargetPool[randomExceptMyself];
                break;
            
            default:
                print("default");
                return null;
                break;

        }
    }

    int GetUniqueTarget(int min, int max) // "relative random" target
    {
        Random.seed = (int)System.DateTime.Now.Ticks;

        int number = Random.Range(min, max);
        int tries = 0;

        while (TargetNumbersUsed.Contains(number) && tries < ChanceOfGettingUniqueTarget)
        {
            number = Random.Range(min, max);
            tries++;
        }

        TargetNumbersUsed.Add(number);
        return number;

    }

    public abstract bool MissionAccomplished();

    public override string ToString()
    {
        return this.GetType().Name; // returns excact name of C# file (no spaces)
    }
}
