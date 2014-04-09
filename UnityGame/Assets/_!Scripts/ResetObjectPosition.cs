using UnityEngine;
using System.Collections;

public class ResetObjectPosition : MonoBehaviour
{
    private Vector3 originalPos;
    private Quaternion originalRotation;

    // Use this for initialization
    private void Start()
    {
        originalPos = gameObject.transform.position;
        originalRotation = gameObject.transform.rotation;

        GameManager.Instance.AllObjectsToReset.Add(this);
    }

    public void ResetMyPosition()
    {
        transform.position = originalPos;
        transform.rotation = originalRotation;

        if (rigidbody != null) // reset rigid body
        {
            if (rigidbody.isKinematic)
                return;

            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }

    
}
