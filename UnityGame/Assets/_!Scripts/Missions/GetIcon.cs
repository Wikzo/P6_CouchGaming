using UnityEngine;
using System.Collections;

public class GetIcon : MonoBehaviour
{

    public GameObject Icon;

    // Use this for initialization
    private void Awake()
    {

        if (Icon == null)
            Debug.Log("ERROR - mission icon not assigned! " + this);
    }
}
