using UnityEngine;
using System.Collections;

public enum MissionType
{
    PlayerToPlayerMission,
    EnvironmentalMission,
    PropMission,
    MiscMission
}
public abstract class MissionBase : MonoBehaviour
{
    public int Points;
    [HideInInspector] public bool MissionIsActive;

    [HideInInspector] public GameObject Player;
    public GameObject Target;
    public MissionType MissionType;

    //public MissionBase Template;

    // An abstract function has to be overridden while a virtual function may be overridden.

    public virtual void InitializeMission(GameObject player, GameObject target, MissionBase Template)
    {
        if (Template == null)
            Debug.Log("ERROR, Mission Templates have not been assigned!");

        // Use template values
        this.MissionType = Template.MissionType;
        this.Points = Template.Points;

        // Use specific values
        this.Player = player;
        this.Target = target;

        this.MissionIsActive = true;

        Debug.Log(string.Format("Mission {0} initialized for Player {1} with Target {2}", this, this.Player, this.Target.transform.name));
    }

    public abstract bool MissionAccomplished();

    public override string ToString()
    {
        return this.GetType().Name; // returns excact name of C# file (no spaces)
    }
}
