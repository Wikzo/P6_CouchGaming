using UnityEngine;
using System.Collections;

public enum TargetIDColorState
{
    NotAssigned = 0,
    RedOne = 1, //red
    BlueTwo = 2,
    GreenThree = 3,
    PinkFour = 4 // pink
}

public class TargetIDColor : MonoBehaviour
{
    public TargetIDColorState TargetIDColorState;

    void Start()
    {
        if (TargetIDColorState == TargetIDColorState.NotAssigned)
            Debug.Log("ERROR - needs to be assigned a target id color " + this);

        Player p = gameObject.GetComponent<Player>();
        if (p != null && p.MyColorIDName != TargetIDColorState.ToString())
            Debug.Log("Name and target ID does not match for " + gameObject);
    }
}
