using UnityEngine;
using System.Collections;

public enum MissionType
{
    PlayerToPlayerMission,
    EnvironmentalMission,
    PropMission,
    MiscMission
}

// interfaces define the contract that a class will adhere to.
// http://stackoverflow.com/questions/2115114/why-cant-c-sharp-interfaces-contain-fields
// https://unity3d.com/learn/tutorials/modules/intermediate/scripting/interfaces
// http://forum.unity3d.com/threads/221532-help-on-C-interface-and-its-implementation

public interface IMission
{
    int Points { get; set; }
    bool MissionIsActive { get; set; }
    GameObject Player { get; set; }
    GameObject Target { get; set; } // TODO: not sure if this should be Player type or GameObject type
    MissionType MissionType { get; set; }

    void InitializeMission(GameObject player, GameObject target, int points);
    bool MissionAccomplished();

}

