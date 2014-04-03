using UnityEngine;
using System.Collections;

public enum TargetIDColorState
{
    NotAssigned,
    Red,
    Green,
    Blue,
    Orange
}

public class TargetIDColor : MonoBehaviour
{
    public TargetIDColorState TargetIDColorState;
}
