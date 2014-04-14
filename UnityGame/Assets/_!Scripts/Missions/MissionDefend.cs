using UnityEngine;
using System.Collections;

public class MissionDefend : MissionBase
{
    private bool targetWasAliveWhenBegan;

    public override void TemplateSetUp()
    {
        base.TemplateSetUp();
    }

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

        if (Target.GetComponent<Player>().PState == PlayerState.Dead)
            _missionIsActive = false;

        return false;
    }
}
