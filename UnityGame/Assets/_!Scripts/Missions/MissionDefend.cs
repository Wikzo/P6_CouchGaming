using UnityEngine;
using System.Collections;

public class MissionDefend : MissionBase
{
    private bool targetWasAliveWhenBegan;

    public override bool MissionAccomplished()
    {
        if (GameManager.Instance.CurrentRoundJustEnded)
        {
            if (Target.GetComponent<Player>().PState == PlayerState.Alive)
            {
                _missionIsActive = false;
                return true;
            }
        }

        return false;
    }
}
