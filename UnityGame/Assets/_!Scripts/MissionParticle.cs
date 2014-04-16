using UnityEngine;
using System.Collections;

public class MissionParticle : MonoBehaviour
{
    private float timer;
    private float DieTime = 5f;

    [HideInInspector]
    public GameObject PlayerToFollow;

    private Vector3 Offset = new Vector3(0, 1.5f, 0);

    // Update is called once per frame
    private void Update()
    {
        transform.position = PlayerToFollow.transform.position + Offset;

        timer += Time.deltaTime;

        if (timer > DieTime)
            Destroy(gameObject);
    }
}