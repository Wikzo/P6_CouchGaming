using UnityEngine;
using System.Collections;

public class PickUpObject : MonoBehaviour
{
    private bool isPickedUpRightNow = false;
    private Vector3 Offset = new Vector3(0, 0.9f, 0);
    private GameObject PlayerToFollow;
    
    void OnCollisionEnter(Collision col)
    {
        if (isPickedUpRightNow) // can only be picked up once at a time
            return;

        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<Player>().PState == PlayerState.Alive) // only works on living players
            {
                isPickedUpRightNow = true;
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
            isPickedUpRightNow = false;
        }

        // follow player
        if (isPickedUpRightNow)
            transform.position = PlayerToFollow.transform.position + Offset;
        else // standard
        {
            gameObject.collider.isTrigger = false;
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
    }

}
