﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class MissionManager : MonoBehaviour
{
    // Singleton itself
    private static MissionManager _instance;
    
    public List<MissionBase> AllAvailableMissionsTotal; // pool of ALL missions available
    public List<MissionBase> FourPotentialMissionsAvailable; // of the total list, four are selected as potential candiates
    public List<MissionBase> AlreadyChosenMissions; // the missions that has already been chosen
    [HideInInspector] public List<MissionBase> InstantiatedMissions; // actual missions on players (to make it easier to see in Inspector)

    public List <GameObject> Players;

    public int ChanceOfGettingUniqueMissions = 5; // higher value = bigger chance of NOT getting same mission 
                                                  //(20-50 seems like a good value if you want to ABSOLUTELY make sure that they won't get same mission!)
                                                  // between 1 and 3 is "so-so"
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
        // set up all the lists
        Players = GameManager.Instance.Players;
        AlreadyChosenMissions = new List<MissionBase>(Players.Count);
        AllAvailableMissionsTotal = new List<MissionBase>(4);
        InstantiatedMissions = new List<MissionBase>(4);
        
        // find all the missions parented to this game object
        MissionBase[] allChildren = GetComponentsInChildren<MissionBase>();
        foreach (MissionBase mission in allChildren)
        {
            AllAvailableMissionsTotal.Add(mission);
        }

        if (AllAvailableMissionsTotal.Count < 4)
            Debug.Log("ERROR - at least 4 missions needs to be assigned to Mission Manager!");

        // choose four missions out of the total amount
        FourPotentialMissionsAvailable = ChooseMissionsFromSet(4, AllAvailableMissionsTotal);
        for (int i = 1; i < 5; i++)
        {
            // set the rumble states for each mission (1, 2, 3, 4)
            FourPotentialMissionsAvailable[i-1].MissionIDRumbleState = i;
        }

        for (int i = 0; i < Players.Count; i++)
        {
            MissionBase c = GetUniqueMission(); // find a "relatively random" mission

            string scriptName = c.ToString(); // get name of mission script so it can be attached to player
            Players[i].AddComponent(scriptName);
            Players[i].GetComponent<MissionBase>().InitializeMission(Players[i], c); // set up various stuff on mission via the template mission
            InstantiatedMissions.Add(Players[i].GetComponent<MissionBase>()); // list of the current missions, easy to see in Inspector
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

    void ShuffleMissions(List<MissionBase> missions) 
    {
        for (int i = 0; i < missions.Count; i++)
        {
            MissionBase temp = missions[i];
            int random = Random.Range(0, missions.Count);
            missions[i] = missions[random];
            missions[random] = temp;
        }
    }

    MissionBase GetUniqueMission() // "relative random" mission
    {
        Random.seed = (int)System.DateTime.Now.Ticks;

        MissionBase m = FourPotentialMissionsAvailable[Random.Range(0, FourPotentialMissionsAvailable.Count)];
        int tries = 0;

        while (AlreadyChosenMissions.Contains(m) && tries < ChanceOfGettingUniqueMissions)
        {
            m = FourPotentialMissionsAvailable[Random.Range(0, FourPotentialMissionsAvailable.Count)];
            tries++;
        }
        
        AlreadyChosenMissions.Add(m);
        return m;

    }

    List<MissionBase> ChooseMissionsFromSet(int howManyToChoose, List<MissionBase> availableMissions) // NOT USED ANYMORE
    {
        Random.seed = (int)System.DateTime.Now.Ticks;

        // Example: have 10 available cards to choose from, but only needs to choose 5
        // The probability of the first item being chosen will be 5 / 10 or 0.5
        // If it's chosen then the probability for the second item will be 4 / 9 or 0.44 (ie, four items still needed, nine left to choose from)
        // However, if the first was not chosen then the probability for the second will be 5 / 9 or 0.56 (ie, five still needed, nine left to choose from)
        // This continues until the set contains the five items required.

        List<MissionBase> result = new List<MissionBase>(howManyToChoose);

        int numToChoose = howManyToChoose;

        for (int numLeft = availableMissions.Count; numLeft > 0; numLeft--)
        {
            // Adding 0.0 is simply to cast the integers to float for the division.
            float probability = numToChoose + 0.0f / numLeft + 0.0f;

            if (probability >= Random.value)
            {
                numToChoose--;
                result.Add(availableMissions[numLeft - 1]);
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

