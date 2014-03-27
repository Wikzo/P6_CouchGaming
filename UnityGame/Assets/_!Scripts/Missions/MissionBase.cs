using UnityEngine;
using System.Collections;


public enum MissionTypeB
{
    PlayerToPlayerMission,
    EnvironmentalMission,
    PropMission,
    MiscMission
}
public abstract class MissionBase : MonoBehaviour
{
    public int Points;
    public bool MissionIsActive;
    public GameObject Player;
    public GameObject Target;
    public MissionTypeB MissionType;

    // An abstract function has to be overridden while a virtual function may be overridden.

    public virtual void InitializeMission(GameObject player, GameObject target, int points)
    {
        this.Points = points;
        this.Player = player;
        this.Target = target;

        this.MissionIsActive = true;
        this.MissionType = MissionTypeB.PlayerToPlayerMission;

        Debug.Log(string.Format("Mission {0} initialized for Player {1} with Target {2}", this, this.Player, this.Target.transform.name));
    }

    public abstract bool MissionAccomplished();
}
