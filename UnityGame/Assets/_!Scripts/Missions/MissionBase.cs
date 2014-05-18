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
    public bool MissionIsActive { get { return _missionIsActive; } } // getter property

    [HideInInspector]
    public GameObject Player;

    [HideInInspector]
    public Player PlayerScript;

    //[HideInInspector]
    public List<GameObject> TargetPool;

    //[HideInInspector]
    public GameObject Target;

    public MissionType MissionType;
    public HowToChooseTarget HowToChooseTarget;
    public Material MissionMaterial;

    public int MissionIDRumble; // what mission (1 to 4; Left Bumper rumble)
    private TargetIDColorState TargetIDColorState; // target color (Right Bumper rumble)

    public static List<int> TargetNumbersUsed = new List<int>();

    private int ChanceOfGettingUniqueTarget = 2; // higher value = bigger chance of NOT getting same player target

    // rumble fields
    protected float missionTimeCounter;
    protected bool isDisplayingMissionOrTargetRumbleRightNow;
    protected int missionRumbleCounter;
    protected int targetRumbleCounter;
    protected float targetTimeCounter;
    public bool RumblePractice;
    public bool ShowMissionGUI;
    private bool PunchTweenHUDInControllerTutorial = false; // used so only one player makes the tween
    private bool PracticeHUDRumble = false;

    protected bool canGo; // for delay getting mission

    [HideInInspector]
    public bool HasHeardMissionRumble;

    // for logging to test
    protected int ThisButtonPressNumberUp;
    protected int ThisButtonPressNumberDown;
    protected float TimeSinceReceivedMission;


    protected bool isInstanceMission = false;

    public string MissionDescription = "MISSION";

    public bool DebugShowCurrentMission = false;

    protected string StuffToShowInGUI = "";

    public virtual void TemplateSetUp() { }

    public AudioClip ChooseCompletedSound()
    {
        TargetIDColorState t = Player.GetComponent<TargetIDColor>().TargetIDColorState;

        if (t == null)
            return AudioManager.Instance.MissionCompletedDefault;

        switch (t)
        {
            case TargetIDColorState.RedOne:
                return AudioManager.Instance.MissionCompletedRed;
                break;
            case TargetIDColorState.BlueTwo:
                return AudioManager.Instance.MissionCompletedBlue;
                break;

            case TargetIDColorState.GreenThree:
                return AudioManager.Instance.MissionCompletedGreen;
                break;

            case TargetIDColorState.PinkFour:
                return AudioManager.Instance.MissionCompletedPink;

                break;

            default:
                return AudioManager.Instance.MissionCompletedDefault;
                break;
        }
    }

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

        StopAllRumble();

        if (GameManager.Instance.PlayingState != PlayingState.ControllerCalibration && GameManager.Instance.PlayingState != PlayingState.TalkingBeforeControllerCalibration)
        {
            GameObject Particles = (GameObject)Instantiate(GameManager.Instance.MissionInitializedParticles);
            Particles.GetComponent<MissionParticle>().PlayerToFollow = this;
            GameObject ParticlesClone = (GameObject)Instantiate(GameManager.Instance.MissionInitializedParticles);
            Particles.GetComponent<MissionParticle>().SetUpParticleClone(ParticlesClone, this.Player);
        }
        HasHeardMissionRumble = false;

        //Debug.Log(string.Format("Mission {0} initialized for Player {1} with Target {2}", this, this.Player, this.Target.transform.name));

        ThisButtonPressNumberUp = 0;
        ThisButtonPressNumberDown = 0;
        TimeSinceReceivedMission = 0;

        StartCoroutine(WaitForMissionToGetActive());
    }

    public IEnumerator WaitForMissionToGetActive() // wait 1 second (just to be sure you don't beat defend mission multip
    {
        this.canGo = false;
        yield return new WaitForSeconds(1f);
        this.canGo = true;
    }

    public virtual void UpdateSpecificMissionStuff() {}

    public GameObject ChooseRandomTarget(MissionBase template)
    {
        System.Random random = new System.Random();

        switch (this.HowToChooseTarget)
        {
            //case HowToChooseTarget.ChooseTargetBasedOnChildren:
            case HowToChooseTarget.ChooseTargetBasedOnList:
            case HowToChooseTarget.ChooseTargetAmongAllPlayers:
                int randomAmongAll = GetUniqueTarget(TargetNumbersUsed, ChanceOfGettingUniqueTarget, 0, template.TargetPool.Count);
                //print(r);
                return template.TargetPool[randomAmongAll];
                break;

            case HowToChooseTarget.ChooseTargetAmongAllPlayersExceptMe:
                int counter = 0;
                int randomExceptMyself = GetUniqueTarget(TargetNumbersUsed, ChanceOfGettingUniqueTarget, 0, template.TargetPool.Count);

                GameObject tempTarget = template.TargetPool[randomExceptMyself];
                if (this.HowToChooseTarget == HowToChooseTarget.ChooseTargetAmongAllPlayersExceptMe) // try to avoid choosing myself
                {
                    while (tempTarget == this.Player && counter < 100) // try to pick a new target
                    {
                        randomExceptMyself = GetUniqueTarget(TargetNumbersUsed, ChanceOfGettingUniqueTarget, 0, template.TargetPool.Count);
                        tempTarget = template.TargetPool[randomExceptMyself];
                        counter++;
                    }

                    /*if (tempTarget == this.Player)
                        print("got myself");*/

                }
                return template.TargetPool[randomExceptMyself];
                break;

            default:
                //print("default");
                return null;
                break;

        }
    }

    public int GetUniqueTarget(List<int> NumbersUsed, int chance, int min, int max) // "relative random" target
    {
        // from http://forum.unity3d.com/threads/60033-CODE-Unique-random-numbers

        System.Random rnd = new System.Random();
        List<int> l = new List<int>();

        int lfsr = (rnd.Next() % max) + 1;

        for (int i = 0; i < max; i++)
        {

            do
            {
                lfsr ^= (lfsr << 9);
                lfsr &= 31;

                lfsr ^= (lfsr >> 2);
                lfsr &= 31;

                lfsr ^= (lfsr << 3);
                lfsr &= 31;



            } while (lfsr > max);

            //Debug.Log(lfsr.ToString() + ", ");
            l.Add(lfsr-1);
        }

        int number = l[Random.Range(0, l.Count)];

        //print("I CHOSE: " + number);

        return number;

    }


    public void StartPracticeRumbleController(int rumbleNumber, bool shouldShowPunchTween)
    {
        missionRumbleCounter = rumbleNumber;

        if (shouldShowPunchTween)
            PunchTweenHUDInControllerTutorial = true;

        RumblePractice = true;
        ShowMissionGUI = false;
    }

    public void StopPracticeRumbleController()
    {
        PunchTweenHUDInControllerTutorial = false;
        RumblePractice = false;

        missionRumbleCounter = 0;
        targetRumbleCounter = 0;
        isDisplayingMissionOrTargetRumbleRightNow = false;

        PracticeHUDRumble = false;

        StartPracticeRumbleHUD();
    }

    private void StartPracticeRumbleHUD()
    {
        PracticeHUDRumble = true;
        ShowMissionGUI = true;
    }

    private void StopPracticeRumbleHUD()
    {
        PracticeHUDRumble = false;
        ShowMissionGUI = false;

        missionRumbleCounter = 0;
        targetRumbleCounter = 0;
    }

    public void StopAllRumble()
    {
        missionTimeCounter = 0;
        targetTimeCounter = 0;

        missionRumbleCounter = 0;
        targetRumbleCounter = 0;

        GamePad.SetVibration(PlayerScript.PlayerController, 0f, 0f);


    }

    public void WriteLogForTest(bool up)
    {
        string text = "";

        if (up)
            text = string.Format("{0}, {1}, {2}, {3}, {4}, {5}", this.PlayerScript.Id+1, this.ToString(), this.PlayerScript.TotalMissionPlayerHasCompleted, "UP", ThisButtonPressNumberUp, TimeSinceReceivedMission);
        else if (!up)
            text = string.Format("{0}, {1}, {2}, {3}, {4}, {5}", this.PlayerScript.Id + 1, this.ToString(), this.PlayerScript.TotalMissionPlayerHasCompleted, "DOWN", ThisButtonPressNumberDown, TimeSinceReceivedMission);

        LoggingManager.AddTextNoTimeStamp(text);
    }

    public void PickMissionRumble() // check if ready to display mission
    {
        ThisButtonPressNumberUp++;
        WriteLogForTest(true);

        if (isDisplayingMissionOrTargetRumbleRightNow)
            return;
        else
            isDisplayingMissionOrTargetRumbleRightNow = true;

        missionRumbleCounter = this.MissionIDRumble;


    }

    public void MissionRumbler(float interval) // display mission (rumble)
    {
        //print("rumble " + missionRumbleCounter + " " + this);
        HasHeardMissionRumble = true;  // TODO: sometimes doesn't work??
        if (missionTimeCounter < interval)
        {
            missionTimeCounter += Time.deltaTime;

            GamePad.SetVibration(PlayerScript.PlayerController, 0.8f, 0.8f);

            if (GameManager.Instance.PlayingState == PlayingState.ControllerCalibration || GameManager.Instance.PlayingState == PlayingState.PraticeMode)
            {
                if (PunchTweenHUDInControllerTutorial)
                    MissionManager.Instance.PracticeControllerRumbleGUI(GameManager.Instance.GUIRumbleCounter - 1);
                else if (PracticeHUDRumble)
                    MissionManager.Instance.PracticeMissionHUDRumble(this.MissionIDRumble - 1);
            }
            
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
        ThisButtonPressNumberDown++;
        WriteLogForTest(false);

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
            GamePad.SetVibration(PlayerScript.PlayerController, 0.8f, 0.8f);

            if (GameManager.Instance.PlayingState == PlayingState.ControllerCalibration | GameManager.Instance.PlayingState == PlayingState.PraticeMode)
            {
                if (PracticeHUDRumble)
                    MissionManager.Instance.PracticeTargetHUDRumble((int)this.TargetIDColorState - 1);
            }
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

        if (!_missionIsActive)
            return;


        TimeSinceReceivedMission += Time.deltaTime;

        UpdateSpecificMissionStuff();

        // TODO: only rumble in PRACTICE MODE, GET READY MODE or PLAYING MODE

        if (!RumblePractice)
        {
            if (PlayerScript.PlayerControllerState.ButtonDownDPadUp && !isDisplayingMissionOrTargetRumbleRightNow)
                PickMissionRumble();

            if (PlayerScript.PlayerControllerState.ButtonDownDPadDown && !isDisplayingMissionOrTargetRumbleRightNow)
                PickTargetRumble();
        }

        if (missionRumbleCounter > 0)
            MissionRumbler(0.2f);

        if (targetRumbleCounter > 0)
            TargetRumbler(0.2f);
    }

    public void GivePointsToPlayer()
    {
        this.PlayerScript.TotalMissionPlayerHasCompleted++;

        Instantiate(GameManager.Instance.PlusObject, this.Player.transform.position + new Vector3(1.5f, 2f, 0), Quaternion.identity);

        HasHeardMissionRumble = true;
        this.PlayerScript.Points += this.Points;

        MissionManager.Instance.GetNewMissionToSinglePlayer(this.Player);
        //StartCoroutine(DelayGiveNewMission());
    }

    IEnumerator DelayGiveNewMission()
    {
        yield return new WaitForSeconds(1f);
        MissionManager.Instance.GetNewMissionToSinglePlayer(this.Player);
    }

    public virtual void DestroyMission()
    {
        HasHeardMissionRumble = true;
        DestroyImmediate(this);
    }

    public void OnGUI()
    {
        //if (isInstanceMission && GameManager.Instance.PlayingState == PlayingState.PraticeMode && ShowMissionGUI)
        if (isInstanceMission && GameManager.Instance.DebugMode)
        {

            // TODO: fix offset

            string text = this.ToString() + " - " + this.TargetIDColorState;
            text += "\nIs Active: " + this._missionIsActive;
            text += "\nPoints: " + this.PlayerScript.Points;
            text += "\n" + StuffToShowInGUI;
            var point = Camera.main.WorldToScreenPoint(transform.position);

            int xOffset = 0;
            int yOffset = 0;

            if (point.x > Screen.width - 150)
                xOffset = -150;

            if (point.y > Screen.height - 50)
                yOffset = 110;

            GUI.Label(new Rect(point.x + xOffset, Screen.currentResolution.height - point.y - 200 + yOffset, 200, 200), text);
        }
    }

    public abstract bool MissionAccomplished();

    public override string ToString()
    {
        return this.GetType().Name; // returns excact name of C# file (no spaces)
    }
}