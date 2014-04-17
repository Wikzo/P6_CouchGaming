using UnityEngine;
using System.Collections;

public class PrintPosition : MonoBehaviour
{
    public bool Local;
    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Local)
            print(transform.localPosition.x.ToString("F2"));
        else
            print(transform.position.x.ToString("F2"));
    }
}