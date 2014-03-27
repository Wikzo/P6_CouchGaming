using UnityEngine;
using System.Collections;

public class PositionMision : MonoBehaviour, IMission
{

    public int Points { get; set; }
    public GameObject Player { get; set; }
    public bool MissionIsActive { get; set; }
    public GameObject Target { get; set; }
    public MissionType MissionType { get; set; }
    public void InitializeMission(GameObject player, GameObject target, int points)
    {
        this.Points = points;
        this.Player = player;
        this.Target = target;

        this.MissionIsActive = true;
        this.MissionType = MissionType.PlayerToPlayerMission;

        Debug.Log(string.Format("Mission {0} initialized for Player {1} with Target {2}", this, this.Player, this.Target.transform.name));
    }

    public bool MissionAccomplished()
    {
        if (this.Player.transform.position.y > 5)
        {
            MissionIsActive = false;
            return true;
        }

        return false;
    }
}
