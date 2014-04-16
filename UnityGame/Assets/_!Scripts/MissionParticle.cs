using UnityEngine;
using System.Collections;

public class MissionParticle : MonoBehaviour
{
    [HideInInspector]
    public MissionBase PlayerToFollow;

    private Vector3 Offset = new Vector3(0, 1.5f, 0);

    // Update is called once per frame
    private void Update()
    {
        transform.position = PlayerToFollow.Player.transform.position + Offset;

        if (PlayerToFollow.HasHeardMissionRumble)
            Destroy(gameObject);
    }
}