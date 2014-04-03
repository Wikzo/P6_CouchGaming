﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MissionManager : MonoBehaviour
{
    // Singleton itself
    private static MissionManager _instance;

    // Fields
    //public MissionBase[] MissionPool;

    
    private List<MissionBase> MissionPoolList;
    
    public List<MissionBase> ChosenMissions; // templates
    [HideInInspector] public List<MissionBase> InstantiatedMissions; // actual missions on players

    public List <GameObject> Players;

    //  public static Instance  
    public static MissionManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType(typeof(MissionManager)) as MissionManager;

            return _instance;
        }
    }

    void OnApplicationQuit()
    {
        _instance = null; // release on exit
    }

    private void Awake()
    {
        // http://clearcutgames.net/home/?p=437
        // First we check if there are any other instances conflicting
        if (_instance != null && _instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        _instance = this;

        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    private void Start()
    {
        Players = GameManager.Instance.Players;
        ChosenMissions = new List<MissionBase>(Players.Count);
        MissionPoolList = new List<MissionBase>(4);
        InstantiatedMissions = new List<MissionBase>(4);
        

        MissionBase[] allChildren = GetComponentsInChildren<MissionBase>();
        foreach (MissionBase mission in allChildren)
        {
            MissionPoolList.Add(mission);
        }
        
        for (int i = 0; i < Players.Count; i++)
        {
            MissionBase c = GetUniqueMission();
            //ChosenMissions[i] = ChooseRandomMission(MissionPoolList);

            string scriptName = c.ToString();
            Players[i].AddComponent(scriptName);
            Players[i].GetComponent<MissionBase>().InitializeMission(Players[i], c);
            InstantiatedMissions.Add(Players[i].GetComponent<MissionBase>());
        }

    }

    void Update()
    {
        foreach (MissionBase m in InstantiatedMissions)
        {
            if (!m.MissionIsActive) // dont look into inactive missions
                return;

            //if (m.MissionAccomplished()) // look if mission has been accomplished
              //  Debug.Log(string.Format("{0} mission accomplished ({1} points)", m.ToString(), m.Points));
        }
    }

    void ShuffleMissions(MissionBase[] missions) // NOT USED ANYMORE
    {
        for (int i = 0; i < missions.Length; i++)
        {
            MissionBase temp = missions[i];
            int random = Random.Range(0, missions.Length);
            missions[i] = missions[random];
            missions[random] = temp;
        }
    }

    MissionBase GetUniqueMission() // "relative random" mission
    {
        Random.seed = (int)System.DateTime.Now.Ticks;

        MissionBase m = MissionPoolList[Random.Range(0, MissionPoolList.Count)];
        int tries = 0;

        while (ChosenMissions.Contains(m) && tries < 1)
        {
            m = MissionPoolList[Random.Range(0, MissionPoolList.Count)];
            tries++;
        }

        ChosenMissions.Add(m);
        return m;

    }

    MissionBase ChooseRandomMission(List<MissionBase> missions) // NOT USED ANYMORE
    {
        
        Random.seed = (int)System.DateTime.Now.Ticks;

        return missions[Random.Range(0, missions.Count)];

        // TODO: implement probability into Missions
        /*float total = 0;

        foreach (GameObject c in cards)
            total += c.probability; // add up all probabilities

        float random = Random.value * total; // Random.value returns value between 0.0 and 1.0

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].probability > random)
                return cards[i];
            else
                random -= cards[i].probability;
        }

        return cards[cards.Length - 1];*/
    }

    MissionBase[] ChooseMissionsFromSet(int howManyToChoose, MissionBase[] availableMissions) // NOT USED ANYMORE
    {
        Random.seed = (int)System.DateTime.Now.Ticks;

        // Example: have 10 available cards to choose from, but only needs to choose 5
        // The probability of the first item being chosen will be 5 / 10 or 0.5
        // If it's chosen then the probability for the second item will be 4 / 9 or 0.44 (ie, four items still needed, nine left to choose from)
        // However, if the first was not chosen then the probability for the second will be 5 / 9 or 0.56 (ie, five still needed, nine left to choose from)
        // This continues until the set contains the five items required.

        MissionBase[] result = new MissionBase[howManyToChoose];

        int numToChoose = howManyToChoose;

        for (int numLeft = availableMissions.Length; numLeft > 0; numLeft--)
        {
            // Adding 0.0 is simply to cast the integers to float for the division.
            float probability = numToChoose + 0.0f / numLeft + 0.0f;

            if (probability >= Random.value)
            {
                numToChoose--;
                result[numToChoose] = availableMissions[numLeft - 1];
            }

            if (numToChoose == 0)
                break;
        }

        // Note that although the selection is random, items in the chosen set will be
        // in the same order they had in the original array.
        // If the items are to be used one at a time in sequence then the ordering can make
        // them partly predictable, so it may be necessary to shuffle the array before use.
        ShuffleMissions(result);

        return result;
    }
}

