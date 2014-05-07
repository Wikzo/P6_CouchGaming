using UnityEngine;
using System.Collections;

public class RunAnimDummy : MonoBehaviour {

    public bool Selected;
    bool isRunning;

	// Use this for initialization
    void Start()
    {
        /*Animator anim = GetComponent<Animator>();
        anim.SetBool("Run", true);*/

        //iTween.RotateBy(gameObject, iTween.Hash("y", 1, "easeType", "easeInOutBack", "loopType", iTween.LoopType.loop));
        //iTween.RotateAdd(gameObject, new Vector3(0, 360, 0), 5);
    }

    void Update()
    {
        transform.Rotate(Vector3.up, 40 * Time.deltaTime);

        if (Selected)
        {
            if (!isRunning)
            {
                Animator anim = GetComponent<Animator>();
                anim.SetBool("Run", true);

                isRunning = true;
            }
        }
        else
        {
            Animator anim = GetComponent<Animator>();
            anim.SetBool("Run", false);

            isRunning = false;
        }


    }
}
