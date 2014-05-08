using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharPlaneMenuNav : MonoBehaviour
{
    public List<GameObject> ReadyObjects;
    public List<GameObject> NotReadyObjects;
    public GameObject AButton;

    void Start()
    {
        SetReadyState(false);
    }

    public void CanUseAButtonRightNow(bool can)
    {
        if (can)
            AButton.SetActive(true);
        else
            AButton.SetActive(false);

    }

    public void SetReadyState(bool ready)
    {
        if (ready == true)
        {
            foreach (GameObject g in NotReadyObjects)
            {
                g.SetActive(true);
            }

            foreach (GameObject g in ReadyObjects)
            {
                g.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject g in NotReadyObjects)
            {
                g.SetActive(false);
            }

            foreach (GameObject g in ReadyObjects)
            {
                g.SetActive(true);
            }
        }
    }
}