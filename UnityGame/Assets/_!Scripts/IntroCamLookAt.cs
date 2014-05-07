using UnityEngine;
using System.Collections;

public class IntroCamLookAt : MonoBehaviour
{
    public float lastTimeInput;
    public float Speed = 10;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
                float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(x, y, 0);
        transform.Translate(movement * Time.deltaTime * Speed);

        // check x
        if (transform.localPosition.x <= -5)
            transform.localPosition = new Vector3(-5, transform.localPosition.y, transform.localPosition.z);
        if (transform.localPosition.x >= 5)
            transform.localPosition = new Vector3(5, transform.localPosition.y, transform.localPosition.z);

        // check y
        if (transform.localPosition.y <= -3)
            transform.localPosition = new Vector3(transform.localPosition.x, -3, transform.localPosition.z);
        if (transform.localPosition.y >= 3)
            transform.localPosition = new Vector3(transform.localPosition.x, 3, transform.localPosition.z);


    }
}
