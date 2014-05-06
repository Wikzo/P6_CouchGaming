using UnityEngine;
using System.Collections;

public class CrowdMeter : MonoBehaviour
{
    public Vector3 forw;
    public float time = 5;
    // Update is called once per frame
    private void Update()
    {
        renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, 5, time)); 

        //transform.forward = Vector3.up;
        transform.localEulerAngles = new Vector3(90, 180, 0);


    }
}
