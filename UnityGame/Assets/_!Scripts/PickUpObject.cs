using UnityEngine;
using System.Collections;

public class PickUpObject : MonoBehaviour
{
    public bool IsPickedUpRightNow = false;
    public bool CanBeUsedRightNow = true;
    public Vector3 Offset = new Vector3(0, 0.9f, 0);
    public GameObject PlayerToFollow;

    Vector3 StartPosition;

    string tagToLookFor = "Player";
    public GameObject RenderObject;

    // idle times (when it has been placed and is currently inactive for some seconds)
    private bool Idle = false;
    private float IdleBlinkRate = 0.5f;
    private float IdleBlinkTimeTotal = 3f;

    void Start()
    {
        if (RenderObject == null)
            Debug.Log("ERROR - needs render object (child)");

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

    void Update()
    {
        // my player just died
        if (PlayerToFollow != null && PlayerToFollow.GetComponent<Player>().PState != PlayerState.Alive)
        {
            PlayerToFollow = null;
            IsPickedUpRightNow = false;
        }

        // follow player
        if (IsPickedUpRightNow)
            transform.position = PlayerToFollow.transform.position + Offset;
        else if (CanBeUsedRightNow)// standard
        {
            gameObject.collider.isTrigger = false;
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }

        if (Idle)
        {
            float lerp = Mathf.PingPong(Time.time, IdleBlinkRate) / IdleBlinkRate;
            RenderObject.renderer.material.color = Color.Lerp(Color.white, Color.black, lerp);
        }
    }

    public IEnumerator BecomeIdle()
    {
        Idle = true;
        yield return new WaitForSeconds(IdleBlinkTimeTotal);
        Idle = false;

        CanBeUsedRightNow = true;
        gameObject.collider.isTrigger = false;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        RenderObject.renderer.material.color = Color.white;
    }

    public void GoToBaseAndStayIdle()
    {
        CanBeUsedRightNow = false;
        gameObject.collider.isTrigger = true;
        PlayerToFollow = null;
        IsPickedUpRightNow = false;
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;

        transform.position = StartPosition;
        StartCoroutine(BecomeIdle());


    }

}
