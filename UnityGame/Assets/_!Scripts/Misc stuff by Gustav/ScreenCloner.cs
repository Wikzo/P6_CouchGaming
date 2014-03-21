using UnityEngine;
using System.Collections;

public class ScreenCloner : MonoBehaviour
{
    private bool Use2DCollider = true;
    public bool OnlyScreenWrapNoClone = false;
    public Transform Clone;
    private BoxCollider cloneBoxCollider;
    private BoxCollider2D cloneBoxCollider2D;
    private bool standingAtScreenEdgeRightNow;

    private Camera myCam;
    private Vector3 screenToWorldPos;
    private Vector3 screen;
    private Vector3 zeroPosWorldPoint;
    private Vector3 rightSidePosWorldPoint;
    private Vector3 leftSidePosWorldPoint;

    // Use this for initialization
    private void Start()
    {
        myCam = GameObject.Find("Main Camera").camera;
        screen = myCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        zeroPosWorldPoint = myCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        rightSidePosWorldPoint = myCam.ViewportToWorldPoint(new Vector3(1, 0, 0));
        leftSidePosWorldPoint = myCam.ViewportToWorldPoint(new Vector3(0, 1, 0));


        if (myCam == null)
            Debug.Log("Error. Needs to assigne main camera for screen wrapping!");

        if (OnlyScreenWrapNoClone)
            return;

        if (Clone == null)
            Debug.Log("Error. Needs to assigne a clone for screen wrapping!");

        if (Use2DCollider)
            cloneBoxCollider2D = Clone.GetComponent<BoxCollider2D>();
        else
            cloneBoxCollider = Clone.GetComponent<BoxCollider>();

        
    }

    // Update is called once per frame
    private void Update()
    {
        // screen wrapping

        // sides of object (not using origin)
        Vector3 rightSidePosInViewPort = myCam.WorldToViewportPoint(new Vector3(transform.position.x + transform.localScale.x / 2, 0));
        Vector3 leftSidePosInViewPort = myCam.WorldToViewportPoint(new Vector3(transform.position.x - transform.localScale.x / 2, 0));
        Vector3 upperSidePosInViewPort = myCam.WorldToViewportPoint(new Vector3(0, transform.position.y + transform.localScale.y / 2, 0));
        Vector3 bottomSidePosInViewPort = myCam.WorldToViewportPoint(new Vector3(0, transform.position.y - transform.localScale.y / 2, 0));

        // checking screen edges (move original)
        if (leftSidePosInViewPort.x > 1) // check right side
        {
            transform.position = new Vector3(zeroPosWorldPoint.x + transform.localScale.x / 2, transform.position.y, transform.position.z);
        }

        if (rightSidePosInViewPort.x < 0) // check left side
        {
            transform.position = new Vector3(rightSidePosWorldPoint.x - transform.localScale.x / 2, transform.position.y, transform.position.z);
        }

        if (bottomSidePosInViewPort.y > 1) // check up side
        {
            transform.position = new Vector3(transform.position.x, zeroPosWorldPoint.y + transform.localScale.y / 2, transform.position.z);
        }

        if (upperSidePosInViewPort.y < 0) // check bottom side
        {
            transform.position = new Vector3(transform.position.x, leftSidePosWorldPoint.y - transform.localScale.y / 2, transform.position.z);
        }

        // show clone
        if (OnlyScreenWrapNoClone)
            return;

        standingAtScreenEdgeRightNow = false; // default = clone follows

        if (rightSidePosInViewPort.x > 1) // check right side
        {
            standingAtScreenEdgeRightNow = true;
            Clone.transform.position = new Vector3(transform.position.x - screen.x * 2, transform.position.y, transform.position.z);
        }
        else if (leftSidePosInViewPort.x < 0) // check left side
        {
            standingAtScreenEdgeRightNow = true;
            Clone.transform.position = new Vector3(transform.position.x + screen.x * 2, transform.position.y, transform.position.z);
        }

        if (upperSidePosInViewPort.y > 1) // check upper side
        {
            standingAtScreenEdgeRightNow = true;
            Clone.transform.position = new Vector3(transform.position.x, transform.position.y - screen.y * 2 + 2, transform.position.z);
        }
        else if (bottomSidePosInViewPort.y < 0) // check bottom side
        {

            standingAtScreenEdgeRightNow = true;
            Clone.transform.position = new Vector3(transform.position.x, transform.position.y + screen.y * 2 - 2, transform.position.z);
        }

        if (!standingAtScreenEdgeRightNow)
        {
            if (Use2DCollider)
                cloneBoxCollider2D.isTrigger = true;
            else
                cloneBoxCollider.isTrigger = true; // dont collide with yourself
            
            Clone.transform.position = transform.position;
        }
        else
        {
            if (Use2DCollider)
                cloneBoxCollider2D.isTrigger = false;
            else
                cloneBoxCollider.isTrigger = false; // can collide with stuff
        }

    }
}