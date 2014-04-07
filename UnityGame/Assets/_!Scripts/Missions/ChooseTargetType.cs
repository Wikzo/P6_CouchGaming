using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;


public class ChooseTargetType : MonoBehaviour
{
    protected HowToChooseTarget chooseTarget;
    private MissionBase missionBase;

    public List<GameObject> TargetList;

    private void Awake() // important that this is called in Awake (before Start)!
    {
        missionBase = GetComponent<MissionBase>();
        if (missionBase == null)
            Debug.Log("ERROR - needs to have a mission attached to template game object!");

        this.chooseTarget = missionBase.HowToChooseTarget;

        DecideHowToChooseTarget();
    }

    public void DecideHowToChooseTarget()
    {
        switch (this.chooseTarget)
        {
             case HowToChooseTarget.ChooseTargetBasedOnList:
                ChooseTargetBasedOnListPool();
                break;
            /*case HowToChooseTarget.ChooseTargetBasedOnChildren:
                ChooseTargetBasedOnChildrenPool();
                break;*/
            case HowToChooseTarget.ChooseTargetAmongAllPlayers:
            case HowToChooseTarget.ChooseTargetAmongAllPlayersExceptMe:
                SetTargetsToPlayers();
                break;
            case HowToChooseTarget.Other:
                Debug.Log("ERROR - needs to specify 'Other' choose type");
                break;
        }
    }

    public virtual void ChooseTargetBasedOnListPool()
    {
        if (TargetList.Count <= 0)
            Debug.Log("ERROR - target list is empty for " + this);

        missionBase.TargetPool = TargetList;

    }

    /*public void ChooseTargetBasedOnChildrenPool()
    {
        missionBase.TargetPool = new List<GameObject>();

        Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>(); // Gets Components from Children AND itself!

        foreach (Transform transformObject in allChildren)
        {
            missionBase.TargetPool.Add(transformObject.gameObject);
            //print(transformObject + " was added to" + missionBase.name);
        }
        missionBase.TargetPool.Remove(gameObject); // remove itself

        if (missionBase.TargetPool.Count <= 0)
            Debug.Log("ERROR - mission needs to have some potential targets!");
    }*/

    public void SetTargetsToPlayers()
    {
        this.TargetList = GameManager.Instance.Players;
        missionBase.TargetPool = GameManager.Instance.Players; // all players
    }
}
