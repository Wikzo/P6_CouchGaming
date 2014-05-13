using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private Vector3 doorOpenScale;
    private Vector3 doorOpenPos;
    private Vector3 doorCloseScale;

    public float ClosedScaleY = 7;

    public float smoothValue = 2f;


    private bool doorOpen;

    bool going;

    // Use this for initialization
    void Start()
    {
        doorOpenPos = transform.position;
        doorOpenScale = transform.localScale;
        doorCloseScale = new Vector3(transform.localScale.x, ClosedScaleY, transform.localScale.z);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && !going)
            StartCoroutine(CloseDoor());

        if (Input.GetKeyDown(KeyCode.U) && !going)
            StartCoroutine(OpenDoor());

    }

    IEnumerator CloseDoor()
    {
        while (transform.localScale.y < doorCloseScale.y-0.1f)
        {
            going = true;

            Vector3 lerp = Vector3.Lerp(transform.localScale, doorCloseScale, smoothValue  * Time.deltaTime);

            transform.localScale = new Vector3(transform.localScale.x, lerp.y, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, doorOpenPos.y - transform.localScale.y / 2, transform.position.z);

            yield return null;
        }

        going = false;
    }

    IEnumerator OpenDoor()
    {
        while (transform.localScale.y > doorOpenScale.y+0.1f)
        {
            going = true;
            Vector3 lerp = Vector3.Lerp(transform.localScale, doorOpenScale, smoothValue * Time.deltaTime);

            transform.localScale = new Vector3(transform.localScale.x, lerp.y, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, doorOpenPos.y - transform.localScale.y / 2, transform.position.z);

            yield return null;
        }

        going = false;
    }
}
