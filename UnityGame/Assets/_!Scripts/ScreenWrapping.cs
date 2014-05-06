using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenWrapping : MonoBehaviour
{
    public bool OnlyScreenWrapNoClone = false;

    public Transform Body;
    public Transform Helmet;

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

    public GameObject BoostJumpEffect;
    private GameObject boostJumpEffect;
    private bool startBoostEffect = false;

    private GameObject cloneBody;

    private PlayerAnimations originalAnimations;
    private Animator anim;

    public bool UseRotation;
    public bool UseScale;
    public bool UseAnimations;
    public bool IsAlwaysTrigger;
    private bool UseColor = true;

    // Use this for initialization
    private void Start()
    {   
        myCam = GameObject.Find("MainCamera_ortographic").camera;
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

        if(Clone != null && UseAnimations)
        {
            cloneBody = Clone.transform.Find("Armature").gameObject;
            cloneBody.SetActive(false);
        }

        if(OriginalToFollow.GetComponent<PlayerAnimations>() != null)
            originalAnimations = OriginalToFollow.GetComponent<PlayerAnimations>();

        if(GetComponent<Animator>() != null && UseAnimations == true)
        {
            anim = Clone.GetComponent<Animator>();
            anim.speed = 1.5f;
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
            if(cloneBody != null)
                cloneBody.SetActive(true);
        }
        else if (leftSidePosInViewPort.x < 0) // check left side
        {
            standingAtScreenEdgeRightNow = true;
            Clone.transform.position = new Vector3(OriginalToFollowTransform.position.x + screen.x * 2, OriginalToFollowTransform.position.y, OriginalToFollowTransform.position.z);
            if(cloneBody != null)
                cloneBody.SetActive(true);
        }
        else
        {
            if(cloneBody != null)
                cloneBody.SetActive(false);
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

        //if (UseColor)
        //    if(cloneBodyRenderer != null && originalRenderer != null)
        //        cloneBodyRenderer.material.color = originalRenderer.material.color; 

        if (UseAnimations)
        {            
            //TODO: Do this for all animations
            if(originalAnimations.CurrentBaseState.nameHash == PlayerAnimations.runState)    
                anim.SetBool("Run", true);
            else   
                anim.SetBool("Run", false);
            if(originalAnimations.CurrentBaseState.nameHash == PlayerAnimations.jumpState)    
                anim.SetBool("Jump", true);
            else   
                anim.SetBool("Jump", false);
            if(originalAnimations.CurrentBaseState.nameHash == PlayerAnimations.doubleJumpState)
            {    
                anim.SetBool("DoubleJump", true);

                if(startBoostEffect == false)
                {
                    boostJumpEffect = Instantiate(BoostJumpEffect, Clone.position, Quaternion.identity) as GameObject;
                    Destroy(boostJumpEffect, 3);

                    startBoostEffect = true;
                }
            }
            else
            {   
                anim.SetBool("DoubleJump", false);
                startBoostEffect = false;
            }
            if(originalAnimations.CurrentBaseState.nameHash == PlayerAnimations.JumpLandState)    
                anim.CrossFade(PlayerAnimations.JumpLandState, 0, 0, Mathf.NegativeInfinity);    


            Clone.transform.rotation = OriginalToFollowTransform.rotation;
        }


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