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
    public int Points = 10;
    [HideInInspector] public bool MissionIsActive;

    [HideInInspector] public GameObject Player;
    [HideInInspector] public GameObject Target;
    public MissionType MissionType;

    public string text;
    public string name;

    // An abstract function has to be overridden while a virtual function may be overridden.

    public virtual void InitializeMission(GameObject player, GameObject target)
    {
        this.Player = player;
        this.Target = target;

        this.MissionIsActive = true;
        this.MissionType = MissionType.PlayerToPlayerMission;

        player.GetComponent<Player>().MyMission = this;
        player.GetComponent<Player>().TestIvoke(this);

        Debug.Log(string.Format("Mission {0} initialized for Player {1} with Target {2}", this, this.Player, this.Target.transform.name));
    }

    public abstract bool MissionAccomplished();

    /*public override string ToString()
    {
        return this.n
    }*/
}
