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
    public HowToChooseTarget ChooseTargetType;
    public Texture2D Texture;
    
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

        IsTemplateMission = false;

        // Use template values
        this.MissionType = Template.MissionType;
        this.Points = Template.Points;
        this.Target = Template.Target;

        // Use specific values
        this.Player = player;

        this._missionIsActive = true;

        //Debug.Log(string.Format("Mission {0} initialized for Player {1} with Target {2}", this, this.Player, this.Target.transform.name));
    }

    void Start()
    {
        if (!IsTemplateMission) // only templates do this
            return;

        switch (this.ChooseTargetType)
        {
            case HowToChooseTarget.ChooseTargetBasedOnChildren:
                ChooseTargetFromPool();
                break;
            case HowToChooseTarget.ChooseTargetAmongAllPlayers:
                SetTargetsToAllPlayers(true);
                break;
            case HowToChooseTarget.ChooseTargetAmongAllPlayersExceptMe:
                SetTargetsToAllPlayers(false);
                break;
            case HowToChooseTarget.Other:
                break;
        }       
    }

    public virtual void ChooseTargetFromPool()
    {
        TargetPool = new List<GameObject>();

        Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform transform in allChildren)
        {
            TargetPool.Add(transform.gameObject);
            print(transform + " was added");
        }

        

        Random.seed = (int)System.DateTime.Now.Ticks;

        if (TargetPool.Count <= 0)
            Debug.Log("ERROR - mission needs to have some potential targets!");

        this.Target = TargetPool[Random.Range(0, TargetPool.Count-1)];
    }

    public void SetTargetsToAllPlayers(bool canChooseMyself)
    {
        TargetPool = new List<GameObject>();

        if (canChooseMyself)
            TargetPool = GameManager.Instance.Players; // all players
        else // everybody except myself
        {
            TargetPool = new List<GameObject>(GameManager.Instance.Players.Count - 1);

            for (int i = 0; i < GameManager.Instance.Players.Count; i++)
            {
                if (GameManager.Instance.Players[i] != this.Player)
                    TargetPool[i] = GameManager.Instance.Players[i];
            }
            
        }
    }

    public abstract bool MissionAccomplished();

    public override string ToString()
    {
        return this.GetType().Name; // returns excact name of C# file (no spaces)
    }
}
