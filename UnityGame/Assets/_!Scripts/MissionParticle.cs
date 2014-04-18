using UnityEngine;
using System.Collections;

public class MissionParticle : MonoBehaviour
{
    [HideInInspector]
    public MissionBase PlayerToFollow;

    private Vector3 Offset = new Vector3(0, 1.5f, 0);

    public GameObject CloneToUse;
    bool isOriginal = false;

    // Update is called once per frame
    private void Update()
    {
        if (!isOriginal)
            return;

        transform.position = PlayerToFollow.Player.transform.position + Offset;

        if (PlayerToFollow.HasHeardMissionRumble)
            Destroy(gameObject);
    }

    public void SetUpParticleClone(GameObject clone, GameObject playerToFollow)
    {
        isOriginal = true;

        CloneToUse = clone;

        gameObject.AddComponent<ScreenWrapping>();

        ScreenWrapping screenWrap = gameObject.GetComponent<ScreenWrapping>();

        if (screenWrap == null)
        {
            Debug.Log("New mission text clone not working!");
            return;
        }

        screenWrap.Clone = CloneToUse.transform;
        screenWrap.UseRotation = true;
        screenWrap.UseScale = true;
        screenWrap.RootToDetectScreenEdge = playerToFollow;
        screenWrap.OriginalToFollow = gameObject;
    }

    void OnDestroy()
    {
        Destroy(CloneToUse);
    }
}