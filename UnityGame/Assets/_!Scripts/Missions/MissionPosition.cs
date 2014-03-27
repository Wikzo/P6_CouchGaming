using UnityEngine;
using System.Collections;

public class MissionPosition : MissionBase
{

    public override void InitializeMission(GameObject player, GameObject target)
    {
        base.InitializeMission(player, target);

        this.Points = Random.Range(0, 42);
        print(this.Points);
    } 
    public override bool MissionAccomplished()
    {
        if (this.Player.transform.position.y > 5)
        {
            MissionIsActive = false;
            return true;
        }

        return false;
    }
}
