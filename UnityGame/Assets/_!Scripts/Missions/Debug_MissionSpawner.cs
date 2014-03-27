using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Debug_MissionSpawner : MonoBehaviour
{
    public GameObject Mission, Player, Target;
    public GameObject[] MissionObjects;

    private List<IMission> Missions;

    private List<KillMission> kill; 

    // Use this for initialization
    private void Start()
    {
        Missions = new List<IMission>();

        foreach (GameObject g in MissionObjects)
        {
            print("h");
            // soft cast - no exceptions, only null
            IMission mission = g.GetComponent(typeof(IMission)) as IMission;

            if (mission == null)
                Debug.Log(string.Format("Didn't find {0}", mission.ToString()));

            mission.InitializeMission(Player, Target, 10);
            Missions.Add(mission);
        }
        // soft cast - no exceptions, only null
        //killMision = Mission.GetComponent(typeof(IMission)) as IMission;
        //Missions.Add(killMision);
        
        //killMision.InitializeMission(Player, Target, 10);

        //CheckIfMissionsAreValid();

    }

    void CheckIfMissionsAreValid()
    {
        foreach (IMission mission in Missions)
        {
            if (mission == null)
                Debug.Log(string.Format("Didn't find {0}", mission.ToString()));
        }
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (IMission mission in Missions)
        {
            if (mission.MissionAccomplished())
                Debug.Log(string.Format("{0} mission accomplished ({1} points)", mission.ToString(), mission.Points));
        }
    }
}
