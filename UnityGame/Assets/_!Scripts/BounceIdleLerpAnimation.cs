using UnityEngine;
using System.Collections;

public class BounceIdleLerpAnimation : MonoBehaviour
{
    private Vector3 start;
    public float Offset = 0.2f;

    // Use this for initialization
    private void Start()
    {
        start = gameObject.transform.localScale;
    }

    // Update is called once per frame
    private void Update()
    {
        float lerp = Mathf.PingPong(Time.time / 2, Offset);
        Vector3 add = new Vector3(lerp, lerp, lerp);
        transform.localScale = start + add;
    }


}
