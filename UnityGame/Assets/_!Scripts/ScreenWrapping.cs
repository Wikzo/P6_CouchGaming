using UnityEngine;
using System.Collections;

public class ScreenWrapping : MonoBehaviour
{
    public bool OnlyScreenWrapNoClone = false;

    public GameObject RootToDetectScreenEdge;
    public GameObject OriginalToFollow;
    public Transform Clone;

    private Transform RootToDetectScreenEdgeTransform;
    private Transform OriginalToFollowTransform;

    private BoxCollider cloneBoxCollider;
    private Rigidbody cloneRigidbody;
    private bool standingAtScreenEdgeRightNow;

    private Camera myCam;
    private Vector3 screenToWorldPos;
    private Vector3 screen;
    private Vector3 zeroPosWorldPoint;
    private Vector3 rightSidePosWorldPoint;
    private Vector3 leftSidePosWorldPoint;

    private SkinnedMeshRenderer originalRenderer;
    private SkinnedMeshRenderer cloneRenderer;
    private SkinnedMeshRenderer[] cloneRenderers;

    public bool UseRotation;
    public bool UseScale;
    public bool IsAlwaysTrigger;
    private bool UseColor = true;

    // Use this for initialization
    private void Start()
    {
        myCam = GameObject.Find("Main Camera").camera;
        screen = myCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        zeroPosWorldPoint = myCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        rightSidePosWorldPoint = myCam.ViewportToWorldPoint(new Vector3(1, 0, 0));
        leftSidePosWorldPoint = myCam.ViewportToWorldPoint(new Vector3(0, 1, 0));

        if (RootToDetectScreenEdge == null)
            Debug.Log("Error. Needs to assigne RootToDetectScreenEdge (used for screen detecting)");

        if (OriginalToFollow == null)
            Debug.Log("Error. Needs to assigne OriginalToFollow to follow");

        if (OnlyScreenWrapNoClone)
        {
            RootToDetectScreenEdgeTransform = gameObject.transform;
            OriginalToFollowTransform = gameObject.transform;

        }
        else
        {
            RootToDetectScreenEdgeTransform = RootToDetectScreenEdge.transform;
            OriginalToFollowTransform = OriginalToFollow.transform;
        }

        if (myCam == null)
            Debug.Log("Error. Needs to assigne main camera for screen wrapping!");

        if (OnlyScreenWrapNoClone)
            return;

        if (Clone == null)
            Debug.Log("Error. Needs to assigne a clone for screen wrapping!");

        if (Clone.GetComponent<BoxCollider>() != null)
            cloneBoxCollider = Clone.GetComponent<BoxCollider>();
        //else
          //  Debug.Log("Has no trigger attached");

        if (Clone.GetComponent<Rigidbody>() != null)
           cloneRigidbody = Clone.GetComponent<Rigidbody>();
        //else
          //  Debug.Log("Has no rigidbody attached");

        if(OriginalToFollow.GetComponent<Player>() != null)
        {
            if(OriginalToFollow.GetComponent<Player>().PlayerBodyRenderer != null)
                originalRenderer = OriginalToFollow.GetComponent<Player>().PlayerBodyRenderer;
        }

        if(Clone != null)
        {
            if(Clone.GetComponentsInChildren<SkinnedMeshRenderer>() != null)
            {
                cloneRenderers = Clone.GetComponentsInChildren<SkinnedMeshRenderer>();
            
                foreach(SkinnedMeshRenderer rend in cloneRenderers)
                {
                    if(rend.gameObject.name == "Body")
                    {
                        cloneRenderer = rend;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // screen wrapping

        // sides of object (not using origin)
        Vector3 rightSidePosInViewPort = myCam.WorldToViewportPoint(new Vector3(RootToDetectScreenEdgeTransform.position.x + RootToDetectScreenEdgeTransform.localScale.x / 2, 0));
        Vector3 leftSidePosInViewPort = myCam.WorldToViewportPoint(new Vector3(RootToDetectScreenEdgeTransform.position.x - RootToDetectScreenEdgeTransform.localScale.x / 2, 0));
        Vector3 upperSidePosInViewPort = myCam.WorldToViewportPoint(new Vector3(0, RootToDetectScreenEdgeTransform.position.y + RootToDetectScreenEdgeTransform.localScale.y / 2, 0));
        Vector3 bottomSidePosInViewPort = myCam.WorldToViewportPoint(new Vector3(0, RootToDetectScreenEdgeTransform.position.y - RootToDetectScreenEdgeTransform.localScale.y / 2, 0));

        // checking screen edges (move original)
        if (leftSidePosInViewPort.x > 1) // check right side
        {
            RootToDetectScreenEdgeTransform.position = new Vector3(zeroPosWorldPoint.x + RootToDetectScreenEdgeTransform.localScale.x / 2, RootToDetectScreenEdgeTransform.position.y, RootToDetectScreenEdgeTransform.position.z);
        }

        if (rightSidePosInViewPort.x < 0) // check left side
        {
            RootToDetectScreenEdgeTransform.position = new Vector3(rightSidePosWorldPoint.x - RootToDetectScreenEdgeTransform.localScale.x / 2, RootToDetectScreenEdgeTransform.position.y, RootToDetectScreenEdgeTransform.position.z);
        }

        if (bottomSidePosInViewPort.y > 1) // check up side
        {
            RootToDetectScreenEdgeTransform.position = new Vector3(RootToDetectScreenEdgeTransform.position.x, zeroPosWorldPoint.y + RootToDetectScreenEdgeTransform.localScale.y / 2, RootToDetectScreenEdgeTransform.position.z);
        }

        if (upperSidePosInViewPort.y < 0) // check bottom side
        {
            RootToDetectScreenEdgeTransform.position = new Vector3(RootToDetectScreenEdgeTransform.position.x, leftSidePosWorldPoint.y - RootToDetectScreenEdgeTransform.localScale.y / 2, RootToDetectScreenEdgeTransform.position.z);
        }

        if(Clone != null)
        {
            if(cloneRenderer != null && originalRenderer != null)
            {
                cloneRenderer.enabled = originalRenderer.enabled;
            }
            else if(Clone.renderer != null && OriginalToFollow.renderer != null)
                Clone.renderer.enabled = OriginalToFollow.renderer.enabled;
        }

        // show clone
        if (OnlyScreenWrapNoClone)
        {
            if (Clone != null)
            {
                Clone.transform.position = OriginalToFollowTransform.position;

                if (cloneBoxCollider != null)
                    cloneBoxCollider.isTrigger = true; // dont collide with yourself

                Clone.gameObject.SetActive(false);
            }
            return;
        }

        standingAtScreenEdgeRightNow = false; // default = clone follows

        if (rightSidePosInViewPort.x > 1) // check right side
        {
            standingAtScreenEdgeRightNow = true;
            Clone.transform.position = new Vector3(OriginalToFollowTransform.position.x - screen.x * 2, OriginalToFollowTransform.position.y, OriginalToFollowTransform.position.z);
        }
        else if (leftSidePosInViewPort.x < 0) // check left side
        {
            standingAtScreenEdgeRightNow = true;
            Clone.transform.position = new Vector3(OriginalToFollowTransform.position.x + screen.x * 2, OriginalToFollowTransform.position.y, OriginalToFollowTransform.position.z);
        }

        if (upperSidePosInViewPort.y > 1) // check upper side
        {
            standingAtScreenEdgeRightNow = true;
            Clone.transform.position = new Vector3(OriginalToFollowTransform.position.x, OriginalToFollowTransform.position.y - screen.y * 2 + 2, OriginalToFollowTransform.position.z);
        }
        else if (bottomSidePosInViewPort.y < 0) // check bottom side
        {

            standingAtScreenEdgeRightNow = true;
            Clone.transform.position = new Vector3(OriginalToFollowTransform.position.x, OriginalToFollowTransform.position.y + screen.y * 2 - 2, OriginalToFollowTransform.position.z);
        }

        if (UseScale)
            Clone.transform.localScale = OriginalToFollowTransform.localScale;

        if (UseRotation)
            Clone.transform.rotation = OriginalToFollowTransform.rotation;

        if (UseColor)
            if(cloneRenderer != null && originalRenderer != null)
                cloneRenderer.material.color = originalRenderer.material.color; 
            //Clone.transform.renderer.material.color = OriginalToFollow.renderer.material.color;


        if (cloneBoxCollider != null && IsAlwaysTrigger)
            cloneBoxCollider.isTrigger = true;

        if (!standingAtScreenEdgeRightNow)
        {
            if (cloneBoxCollider != null)
            {
                cloneBoxCollider.isTrigger = true; // dont collide with yourself
            }

            Clone.transform.position = OriginalToFollowTransform.position;
        }
        else
        {
            if (cloneBoxCollider != null && !IsAlwaysTrigger)
                cloneBoxCollider.isTrigger = false; // can collide with stuff
        }

    }
}