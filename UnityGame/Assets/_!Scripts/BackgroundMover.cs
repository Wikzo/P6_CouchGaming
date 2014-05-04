using UnityEngine;
using System.Collections;

public class BackgroundMover : MonoBehaviour
{
    public float Speed = 3;

    Vector3 JumpTo;

    // Use this for initialization
    private void Start()
    {
        JumpTo = new Vector3(-261.7874f, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * Speed);

        if (transform.position.x >= 134.4113f)
            transform.position = JumpTo;
    }
}
