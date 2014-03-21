using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Mover : MonoBehaviour
{

    private GamePadState state;
    private GamePadState prevState;
    private Vector3 speed = new Vector3(10, 10, 0);
    private Vector3 movement;
    private float inputX, inputY;

    public bool isGrounded;

    void Start()
    {
        state = GamePad.GetState(PlayerIndex.One);
        prevState = state;

    }

    // Update is called once per frame
    private void Update()
    {

        state = GamePad.GetState(PlayerIndex.One);

        inputX = state.ThumbSticks.Left.X;
        inputY = state.ThumbSticks.Left.Y;

        movement = new Vector3(speed.x * inputX, speed.y * inputY, 0);

        //transform.Translate(movement * Time.deltaTime);

        if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
        {
            prevState = state;
            rigidbody2D.AddForce(Vector2.up * 500);
        }
        

        // OLD STUFF
        /*
        // my position in world space
        Vector3 rightX = new Vector3(transform.position.x + transform.localScale.x / 2, 0, 0); // x origin is in center
        Vector3 leftX = new Vector3(transform.position.x, 0, 0); // x origin is in center
        Vector3 topY = new Vector3(transform.position.y + transform.localScale.y / 2, 0, 0); // for some reason, origin is at bottom Y
        Vector3 bottomY = new Vector3(transform.position.y - transform.localScale.y / 2, 0, 0); // for some reason, origin is at bottom Y



        //Vector3 myPosInViewPort = myCam.WorldToViewportPoint(transform.position);
        //print(string.Format("rightX: {0}; screenwidth: {1}", rightX.x, Screen.width));

        /*
        // my position in view space
        rightX = myCam.WorldToViewportPoint(rightX);
        leftX = myCam.WorldToViewportPoint(leftX);
        topY = myCam.WorldToViewportPoint(topY);
        bottomY = myCam.WorldToViewportPoint(bottomY);

        // screen edges in view space
        Vector3 leftButtomEdge = myCam.ViewportToWorldPoint(new Vector3(0, 0, myCam.nearClipPlane));
        Vector3 rightTopEdge = myCam.ViewportToWorldPoint(new Vector3(1, 1, myCam.nearClipPlane));

        //transform.position = new Vector3(rightTopEdge.x - transform.localScale.x / 2, rightTopEdge.y - transform.localScale.y / 2, 0);

        // move original
        if (leftX.x < 0) // left side check
        {
            transform.position = new Vector3(rightTopEdge.x - transform.localScale.x / 2, transform.position.y, 0);

        }

        print(bottomY.x);

        if (rightX.x > 1) // right side check
        {
            /*Vector3 myPosInViewPortCoord = myCam.WorldToViewportPoint(transform.position);

            Vector3 viewPortCoord = new Vector3(leftButtomEdge.x, myPosInViewPortCoord.y, myPosInViewPortCoord.z);
            Vector3 worldCoord = myCam.ViewportToWorldPoint(viewPortCoord);

            //transform.position = worldCoord;
            print("right side check");

            transform.position = new Vector3(leftButtomEdge.x + transform.localScale.x / 2, transform.position.y, 0);

        }

        if (topY.x > 1) // top side check
        {
            transform.position = new Vector3(transform.position.x, leftButtomEdge.y + transform.localScale.y / 2, 0);

        }

        if (bottomY.x < 0) // top side check
        {
            transform.position = new Vector3(transform.position.x, rightTopEdge.y - transform.localScale.y / 2, 0);

        }

        Clone.transform.position = new Vector3(transform.position.x, myCam.ViewportToWorldPoint(bottomY).x,
                                              transform.position.z);
         * 


*/

        prevState = state;

    }

    private bool CheckIfGrounded()
    {

        // dont work, since it hits myself or my clone ...

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 0.1f);

        print(hit.transform.name);
        if (hit != null && hit.transform.name != "Player1_clone" && hit.transform.name != transform.name)
            return true;
        else
            return false;

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // doesnt work, since I collide with my clone
        if (col.gameObject.name == "floor")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name == "floor")
        {
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {

        

        Vector3 v3 = rigidbody2D.velocity;
        v3 = new Vector3(movement.x, v3.y, v3.z);
        rigidbody2D.velocity = v3;

        
    }
}