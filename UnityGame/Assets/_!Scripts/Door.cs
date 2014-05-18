using UnityEngine;
using System.Collections;

public enum DoorLocation
{
    Upper,
    Lower
}

public class Door : MonoBehaviour
{
    private Vector3 doorOpenScale;
    private Vector3 doorOpenPos;
    private Vector3 doorCloseScale;

    public DoorLocation DoorLocation;
    public float ClosedScaleY = 7;

    public float smoothValue = 2f;

    bool goingUp, goingDown;


    // Use this for initialization
    void Start()
    {
        doorOpenPos = transform.position;
        doorOpenScale = transform.localScale;
        doorCloseScale = new Vector3(transform.localScale.x, ClosedScaleY, transform.localScale.z);

        goingUp = false;
        goingDown = false;
    }

    void OnEnable()
    {
        if (this.DoorLocation == DoorLocation.Upper)
            MissionManager.OnMissionCompletedDoorsUpper += DoorGoDown;
        else if (this.DoorLocation == DoorLocation.Lower)
            MissionManager.OnMissionCompletedDoorsLower += DoorGoDown;

    }

    void OnDisable()
    {
        if (this.DoorLocation == DoorLocation.Upper)
            MissionManager.OnMissionCompletedDoorsUpper -= DoorGoDown;
        else if (this.DoorLocation == DoorLocation.Lower)
            MissionManager.OnMissionCompletedDoorsLower -= DoorGoDown;

        if (this.DoorLocation == DoorLocation.Upper)
            MissionManager.OnMissionCompletedDoorsUpper -= DoorGoUp;
        else if (this.DoorLocation == DoorLocation.Lower)
            MissionManager.OnMissionCompletedDoorsLower -= DoorGoUp;
    }

    // ondisable

    void DoorGoUp()
    {
        if (this.DoorLocation == DoorLocation.Upper)
            MissionManager.OnMissionCompletedDoorsUpper -= DoorGoUp;
        else if (this.DoorLocation == DoorLocation.Lower)
            MissionManager.OnMissionCompletedDoorsLower -= DoorGoUp;

        if (this.DoorLocation == DoorLocation.Upper)
            MissionManager.OnMissionCompletedDoorsUpper += DoorGoDown;
        else if (this.DoorLocation == DoorLocation.Lower)
            MissionManager.OnMissionCompletedDoorsLower += DoorGoDown;

        StartCoroutine(OpenDoor());
    }

    void DoorGoDown()
    {
        if (this.DoorLocation == DoorLocation.Upper)
            MissionManager.OnMissionCompletedDoorsUpper -= DoorGoDown;
        else if (this.DoorLocation == DoorLocation.Lower)
            MissionManager.OnMissionCompletedDoorsLower -= DoorGoDown;

        if (this.DoorLocation == DoorLocation.Upper)
            MissionManager.OnMissionCompletedDoorsUpper += DoorGoUp;
        else if (this.DoorLocation == DoorLocation.Lower)
            MissionManager.OnMissionCompletedDoorsLower += DoorGoUp;

        StartCoroutine(CloseDoor());
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !goingDown && !goingDown)
            StartCoroutine(CloseDoor());

        if (Input.GetKeyDown(KeyCode.I) && !goingDown && !goingDown)
            StartCoroutine(OpenDoor());

    }*/

    IEnumerator CloseDoor()
    {
        renderer.enabled = true;

        //print("door closing");

        /*while (going == true)
            yield return null;*/

        if (!goingDown && !goingUp)
        {
            while (transform.localScale.y < doorCloseScale.y - 0.1f)
            {
                goingDown = true;

                Vector3 lerp = Vector3.Lerp(transform.localScale, doorCloseScale, smoothValue * Time.deltaTime);

                transform.localScale = new Vector3(transform.localScale.x, lerp.y, transform.localScale.z);
                transform.position = new Vector3(transform.position.x, doorOpenPos.y - transform.localScale.y / 2, transform.position.z);

                yield return null;
            }

            goingDown = false;
        }
    }

    IEnumerator OpenDoor()
    {
        renderer.enabled = true;
        //print("door opening");

        /*while (going == true)
            yield return null;*/

        if (!goingDown && !goingUp)
        {

            while (transform.localScale.y > doorOpenScale.y + 0.1f)
            {
                goingUp = true;
                Vector3 lerp = Vector3.Lerp(transform.localScale, doorOpenScale, smoothValue * Time.deltaTime);

                transform.localScale = new Vector3(transform.localScale.x, lerp.y, transform.localScale.z);
                transform.position = new Vector3(transform.position.x, doorOpenPos.y - transform.localScale.y / 2, transform.position.z);

                yield return null;
            }

            goingUp = false;
            renderer.enabled = false;
        }
    }
}
