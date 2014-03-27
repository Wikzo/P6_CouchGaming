﻿using UnityEngine;
using System.Collections;

public class KillMission : MonoBehaviour, IMission
{
    // default IMission fields
    public int Points { get; set; }
    public GameObject Player { get; set; }
    public bool MissionIsActive { get; set; }
    public GameObject Target { get; set; }
    public MissionType MissionType { get; set; }

    // specific KillMission fields
    private bool targetWasAliveWhenMissionBegan;

    // constructor
    public void Start()
    {
        
    }

    public void InitializeMission(GameObject player, GameObject target, int points)
    {
        this.Points = points;
        this.Player = player;
        this.Target = target;

        this.MissionIsActive = true;
        this.MissionType = MissionType.PlayerToPlayerMission;


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
