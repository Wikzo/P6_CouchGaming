using UnityEngine;
using System.Collections;

public class KillMission : MonoBehaviour, IMission
{
    // default IMission fields
    public int Points { get; set; }
    public Player Player { get; set; }
    public bool MissionIsActive { get; set; }
    public GameObject Target { get; set; }
    public MissionType MissionType { get; set; }

    // specific KillMission fields
    private bool targetWasAliveWhenMissionBegan;

    // constructor
    public void Start()
    {
        this.Points = 10;
        this.MissionType = MissionType.PlayerToPlayerMission;
    }

    public void InitializeMission(GameObject player, GameObject target)
    {
        this.MissionIsActive = true;

        this.Player = player.GetComponent<Player>();
        this.Target = target;

        //print(this.Target);
        //print("Mission " + this + " initialized for " + this.Player + " with target" + this.Target.ToString());
        Debug.Log(string.Format("Mission {0} initialized for Player {1} with Target {2}", this, this.Player, this.Target.transform.name));

        if (target != null)
            targetWasAliveWhenMissionBegan = true;
    }

    
    public bool MissionAccomplished()
    {
        if (!this.MissionIsActive) // mission not active
            return false;

        if (!this.targetWasAliveWhenMissionBegan) // no target
            return false;

        if (this.Target == null) // target has died
        {
            this.MissionIsActive = false;
            return true;
        }

        return false; // nothing
    }
}
