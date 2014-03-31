using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class ChooseIntelTargetType : MonoBehaviour
{
    private HowToChooseTarget chooseTarget;
    public MissionIntel missionIntel;

    void Start()
    {

        missionIntel = GetComponent<MissionIntel>();
        if (missionIntel == null)
            Debug.Log("ERROR - needs to have a mission attached to template game object!");

        this.chooseTarget = missionIntel.HowToChooseTarget;

        ChooseTargetBasedOnChildrenPool(); // does not (yet) care about enumerator types

        Random.seed = (int)System.DateTime.Now.Ticks;

        // random stuff
        int[] randoms = new int[100];

        for (int i = 0; i < 100; i++)
        {
            randoms[i] = Random.Range(0, 5);


        }

        

        for (int i = 0; i < 100; i++)
        {
            int temp = randoms[i];
            int randomIndex = Random.Range(0, 100);
            randoms[i] = randoms[randomIndex];
            randoms[randomIndex] = temp;
        }

        /*for (int i = 0; i < 100; i++)
        {
            print(randoms[i]);
        }*/

        /*for (int i = 0; i < 100; i++)
        {
            print(randoms[i]);
        }*/
    }


    public void ChooseTargetBasedOnChildrenPool()
    {
        missionIntel.IntelPropPool = new List<GameObject>();
        missionIntel.IntelBasePool = new List<GameObject>();

        Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>(); // Gets Components from Children AND itself!

        foreach (Transform transformObject in allChildren)
        {
            if (transformObject.tag == "IntelProp")
            missionIntel.IntelPropPool.Add(transformObject.gameObject);
            else if (transformObject.tag == "IntelBase")
                missionIntel.IntelBasePool.Add(transformObject.gameObject);
        }

        Random.seed = (int) System.DateTime.Now.Ticks;

        if (missionIntel.IntelPropPool.Count <= 0 || missionIntel.IntelBasePool.Count <= 0)
            Debug.Log("ERROR - mission needs to have some potential targets!");

        missionIntel.IntelProp = missionIntel.IntelPropPool[Random.Range(0, missionIntel.IntelPropPool.Count)];
        missionIntel.IntelBase = missionIntel.IntelBasePool[Random.Range(0, missionIntel.IntelBasePool.Count)];
    }
}
