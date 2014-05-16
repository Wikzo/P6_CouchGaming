using UnityEngine;
using System.Collections;

public class PickUpObject : MonoBehaviour
{
    public bool IsPickedUpRightNow = false;
    public bool CanBeUsedRightNow = true;
    Vector3 PickupOffset = new Vector3(0, 3.89f, 0);
    public GameObject PlayerToFollow;

    Vector3 ScrollScaleOriginal;
    Vector3 ScrollScaleSmall = new Vector3(0.6813736f, 0.7912574f, 0.6813736f);

    Vector3 StartPosition;

    string tagToLookFor = "Player";
    public GameObject RenderObject;

    // idle times (when it has been placed and is currently inactive for some seconds)
    private bool Idle = false;
    private float IdleBlinkRate = 0.5f;
    private float IdleBlinkTimeTotal = 0.5f;

    bool isPuttingOnTerminalRightNow = false;

    bool DroppingRightNow;

    Vector3 terminalPos;

    void Start()
    {
        if (RenderObject == null)
            Debug.Log("ERROR - needs render object (child)");

        ScrollScaleOriginal = RenderObject.transform.localScale;

        StartPosition = transform.position;
    }
  
    //TODO: consider if using OnTrigger instead
    void OnCollisionEnter(Collision col)
    {
        if (!CanBeUsedRightNow) // can't be picked up right now
            return;

        if (IsPickedUpRightNow) // can only be picked up once at a time
            return;

        if (col.gameObject.tag.Contains(tagToLookFor))
        {
            if (col.gameObject.GetComponent<Player>() == null)
                return;

            if (col.gameObject.GetComponent<Player>().PState == PlayerState.Alive) // only works on living players
            {
                IsPickedUpRightNow = true;
                PlayerToFollow = col.gameObject;
                
                gameObject.collider.isTrigger = true;
                
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;

            }
        }
    }

    public void PutIntelOnTerminalScreen(bool doingIt, Vector3 pos)
    {
        if (doingIt)
        {
            isPuttingOnTerminalRightNow = true;
            terminalPos = pos;
        }
        else
            isPuttingOnTerminalRightNow = false;

    }

    void Update()
    {

        if (transform.position.z != StartPosition.z)
            transform.position = new Vector3(transform.position.x, transform.position.y, StartPosition.z);
        // drop if pressing down
        if (PlayerToFollow != null && PlayerToFollow.GetComponent<PlayerAim>().ShootingRightNow)
        {
            StartCoroutine(SetTriggerToFalseForSomeTime());
        }

        // my player just died
        if (PlayerToFollow != null && PlayerToFollow.GetComponent<Player>().PState != PlayerState.Alive)
        {
            PlayerToFollow = null;
            IsPickedUpRightNow = false;
            gameObject.collider.enabled = true;
            gameObject.collider.isTrigger = false;
        }

        // follow player
        if (IsPickedUpRightNow && !DroppingRightNow)
        {
            if (isPuttingOnTerminalRightNow)
            {
                transform.position = terminalPos;
            }
            else
            {
                transform.position = PlayerToFollow.transform.position + PickupOffset;
                RenderObject.transform.localScale = ScrollScaleSmall;
            }

        }
        else if (CanBeUsedRightNow && !DroppingRightNow)// standard
        {
            gameObject.collider.isTrigger = false;
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            RenderObject.transform.localScale = ScrollScaleOriginal;
        }

        /*if (Idle)
        {
            float lerp = Mathf.PingPong(Time.time, IdleBlinkRate) / IdleBlinkRate;
            RenderObject.renderer.material.color = Color.Lerp(Color.white, Color.black, lerp);
        }*/
    }


    IEnumerator SetTriggerToFalseForSomeTime()
    {
        RenderObject.transform.localScale = ScrollScaleOriginal;

        gameObject.collider.enabled = true;
        PlayerToFollow = null;
        IsPickedUpRightNow = false;
        CanBeUsedRightNow = false;
        gameObject.layer = 20; // DontCollideWithPlayer
        DroppingRightNow = true;
        rigidbody.useGravity = true;

        //rigidbody.isKinematic = false;
        //rigidbody.useGravity = true;

        yield return new WaitForSeconds(1.5f);
        DroppingRightNow = false;
        CanBeUsedRightNow = true;
        gameObject.collider.isTrigger = false;
        gameObject.layer = 0; // default
        DroppingRightNow = false;
    }
    public IEnumerator BecomeIdle()
    {
        Idle = true;
        gameObject.collider.isTrigger = true;        
        yield return new WaitForSeconds(IdleBlinkTimeTotal);
        Idle = false;

        CanBeUsedRightNow = true;
        gameObject.collider.isTrigger = false;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        //RenderObject.renderer.material.color = Color.white;
    }

    public void GoToBaseAndStayIdle()
    {
        CanBeUsedRightNow = false;
        
        PlayerToFollow = null;
        IsPickedUpRightNow = false;
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;

        StartCoroutine(Wait());
        


    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        transform.position = StartPosition;
        gameObject.collider.enabled = true;
        gameObject.collider.isTrigger = false;
        StartCoroutine(BecomeIdle());
    }

}
