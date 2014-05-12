using UnityEngine;
using System.Collections;

public class MissionDefend : MissionBase
{
    private bool targetWasAliveWhenBegan;

    private float deadlineTime;
    private float maxIntervalForDeadline = 30;
    private float minIntervalForDeadline = 10;

    private bool targetHasNotDiedYet;

    public override void InitializeMission(GameObject player, MissionBase Template)
    {
        base.InitializeMission(player, Template);

        if (GameManager.Instance.PlayingState == PlayingState.ControllerCalibration || GameManager.Instance.PlayingState == PlayingState.PraticeMode) // only do this in play mode, not practice
            return;

        int tries = 0;

        deadlineTime = Random.Range(GameManager.Instance.TimeLeft - maxIntervalForDeadline,
                                    GameManager.Instance.TimeLeft - minIntervalForDeadline);

        if (deadlineTime <= 0 && tries < 10)
        {
            deadlineTime = Random.Range(GameManager.Instance.TimeLeft - minIntervalForDeadline,
                                    GameManager.Instance.TimeLeft - maxIntervalForDeadline);
            tries++;
            
        }

        if (deadlineTime <= 0) // get new mission
        {
            //Debug.Log(this.Player + " Not enough time for defend mission. Getting new mission instead...");
            MissionManager.Instance.GetNewMissionToSinglePlayer(this.Player);
        }
        else // add mission timer mark
        {
            GameManager.Instance.TimeBar.TimeMarks.Add(deadlineTime);
            StuffToShowInGUI = "Deadline: " + deadlineTime.ToString();
            targetHasNotDiedYet = true;
        }

    }

    public override void TemplateSetUp()
    {
        base.TemplateSetUp();
    }

    public override void DestroyMission()
    {
        GameManager.Instance.TimeBar.TimeMarks.Remove(deadlineTime);
        base.DestroyMission();
    }

    public override bool MissionAccomplished()
    {
        // check if player is still alive - if dead: mission is done
        if (Target.GetComponent<Player>().PState == PlayerState.Dead)
        {
            targetHasNotDiedYet = true;
            return false;
        }

        // reached deadline
        if (GameManager.Instance.TimeLeft <= deadlineTime)
        {
            if (targetHasNotDiedYet)
            {
                this._missionIsActive = false;
                return true;
            }
            else
            {
                this._missionIsActive = false;
                MissionManager.Instance.GetNewMissionToSinglePlayer(this.Player);
                return false;
            }
        }

        return false;
    }
}
