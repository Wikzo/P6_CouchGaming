using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using XInputDotNetPure;


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
    private float intervals;
    public int Points;
    
    protected bool _missionIsActive; // only this class + derived classes can access this
    public bool MissionIsActive{get { return _missionIsActive; }} // getter property

    [HideInInspector]
    public GameObject Player;
    protected Player PlayerScript;

    //[HideInInspector]
    public List<GameObject> TargetPool;
    
    //[HideInInspector]
    public GameObject Target;
    
    public MissionType MissionType;
    public HowToChooseTarget HowToChooseTarget;
    public Material MissionMaterial;

    public int MissionIDRumble; // what mission (1 to 4; Left Bumper rumble)
    public TargetIDColorState TargetIDColorState; // target color (Right Bumper rumble)

    public static List<int> TargetNumbersUsed = new List<int>();

    private int ChanceOfGettingUniqueTarget = 2; // higher value = bigger chance of NOT getting same player target

    // rumble fields
    protected float missionTimeCounter;
    protected bool isDisplayingMissionOrTargetRumbleRightNow;
    protected int missionRumbleCounter;
    protected int targetRumbleCounter;
    protected float targetTimeCounter;

    protected bool isInstanceMission = false;

    public string MissionDescription = "MISSION";

    public virtual void TemplateSetUp() { }

    // An abstract function has to be overridden while a virtual function may be overridden.
    public virtual void InitializeMission(GameObject player, MissionBase Template)
    {
        if (Template == null)
            Debug.Log("ERROR, Mission Templates have not been assigned for " + player);

        if (Template.TargetPool.Count <= 0)
            Debug.Log("ERROR - Target pool is empty for " + player);

        // Use specific values
        this.Player = player;
        this.PlayerScript = player.GetComponent<Player>();

        if (PlayerScript == null)
            Debug.Log("ERROR - mission doesn't have link to player! " + this);
        
        this._missionIsActive = true;

        this.HowToChooseTarget = Template.HowToChooseTarget;

        this.Target = ChooseRandomTarget(Template);

        // Rumble IDs
        if (Target.GetComponent<TargetIDColor>() == null)
            Debug.Log("ERROR  - target for " + gameObject + " doesn't have a TargetIDColor!");
        this.TargetIDColorState = Target.GetComponent<TargetIDColor>().TargetIDColorState;

        // Use template values
        this.MissionType = Template.MissionType;
        this.Points = Template.Points;
        this.MissionIDRumble = Template.MissionIDRumble;
        this.isInstanceMission = true;


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


    public void PickMissionRumble() // check if ready to display mission
    {
        if (isDisplayingMissionOrTargetRumbleRightNow)
            return;
        else
            isDisplayingMissionOrTargetRumbleRightNow = true;

        missionRumbleCounter = this.MissionIDRumble;

    }

    public void MissionRumbler(float interval) // display mission (rumble)
    {
        //print("rumble " + missionRumbleCounter + " " + this);
        if (missionTimeCounter < interval)
        {
            missionTimeCounter += Time.deltaTime;
            GamePad.SetVibration(PlayerScript.PlayerController, 1, 1);
        }
        else if (missionTimeCounter < interval * 3)
        {
            missionTimeCounter += Time.deltaTime;
            GamePad.SetVibration(PlayerScript.PlayerController, 0, 0);
        }
        else
        {
            missionTimeCounter = 0;
            missionRumbleCounter--;
            isDisplayingMissionOrTargetRumbleRightNow = false;

        }
    }

    public void PickTargetRumble() // check if ready to display target
    {
        if (isDisplayingMissionOrTargetRumbleRightNow)
            return;
        else
            isDisplayingMissionOrTargetRumbleRightNow = true;

        targetRumbleCounter = (int)this.TargetIDColorState;

    }

    public void TargetRumbler(float interval) // display target (rumble)
    {
        if (targetTimeCounter < interval)
        {
            targetTimeCounter += Time.deltaTime;
            GamePad.SetVibration(PlayerScript.PlayerController, 1, 1);
        }
        else if (targetTimeCounter < interval * 3)
        {
            targetTimeCounter += Time.deltaTime;
            GamePad.SetVibration(PlayerScript.PlayerController, 0, 0);
        }
        else
        {
            targetTimeCounter = 0;
            targetRumbleCounter--;
            isDisplayingMissionOrTargetRumbleRightNow = false;

        }
    }

    public void Update()
    {
        if (!isInstanceMission) // don't rumble for template missions
            return;

        if (GameManager.Instance.PlayingState == PlayingState.Paused) // round is paused
            return;

        if (PlayerScript.PlayerControllerState.ButtonDownLeftShoulder && !isDisplayingMissionOrTargetRumbleRightNow)
            PickMissionRumble();

        if (PlayerScript.PlayerControllerState.ButtonDownRightShoulder && !isDisplayingMissionOrTargetRumbleRightNow)
            PickTargetRumble();

        if (missionRumbleCounter > 0)
            MissionRumbler(0.2f);

        if (targetRumbleCounter > 0)
            TargetRumbler(0.2f);
    }

    public abstract bool MissionAccomplished();

    public bool MissionIsDone()
    {
        _missionIsActive = false;
        return true;
    }

    public override string ToString()
    {
        return this.GetType().Name; // returns excact name of C# file (no spaces)
    }
}
