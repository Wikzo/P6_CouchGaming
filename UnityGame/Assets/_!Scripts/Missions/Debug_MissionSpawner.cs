using UnityEngine;
using System.Collections;

public class Debug_MissionSpawner : MonoBehaviour
{
    public GameObject Mission, Player, Target;
    public IMission killMision;
    // Use this for initialization
    private void Start()
    {
        killMision = Mission.GetComponent(typeof(IMission)) as IMission;
        killMision.InitializeMission(Player, Target);

    }

    // Update is called once per frame
    private void Update()
    {
        if (killMision.MissionAccomplished())
            Debug.Log("Mission done! You just got " + killMision.Points + " points");
    }
}
