using UnityEngine;
using System.Collections;

public enum TargetIDColorState
{
    NotAssigned = 0,
    RedOne = 1,
    GreenTwo = 2,
    BlueThree = 3,
    OrangeFour = 4
}

public class TargetIDColor : MonoBehaviour
{
    public TargetIDColorState TargetIDColorState;

    void Start()
    {
        if (TargetIDColorState == TargetIDColorState.NotAssigned)
            Debug.Log("ERROR - needs to be assigned a target id color " + this);
    }
}
