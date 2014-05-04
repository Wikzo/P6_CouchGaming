using UnityEngine;
using System.Collections;

public class NewMissionTextShower : MonoBehaviour
{
    // rotation
    public float direction = 1;
    float counter;
    public float RotationAmount = 0.61f;
    bool Rotate = false;

    // scale
    Vector3 start;
    public float ScaleAmount = 0.13f;


    // Use this for initialization
    void Start()
    {
        counter = 0;
        start = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        float lerp = Mathf.PingPong(Time.time / 2, ScaleAmount);
        Vector3 add = new Vector3(lerp, lerp, lerp);
        transform.localScale = start + add;

        if (Rotate)
        {
            counter += Time.deltaTime;

            if (counter >= RotationAmount)
            {
                direction *= -1;
                counter = 0;
            }

            transform.Rotate(Vector3.forward * direction);
        }

    }
}
