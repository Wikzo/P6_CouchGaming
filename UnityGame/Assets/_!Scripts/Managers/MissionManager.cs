using System;
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
    private List<MissionBase> AlreadyChosenMissions; // the missions that has already been chosen- used to try to get a "relative random" set
    public List<MissionBase> InstantiatedMissions; // actual missions on players (to make it easier to see in Inspector)

    public List <GameObject> Players;

    public List<TextMesh> MissionTexts; 
    public List<GameObject> MissionIcons;
    public List<GameObject> TargetHuds;

    public GameObject RingPrefabForIntelMission;

    public GameObject MissionIsCompletedPrefab;

    public int ChanceOfGettingUniqueMissions = 5; // higher value = bigger chance of NOT getting same mission 
                                                  //(20-50 seems like a good value if you want to ABSOLUTELY make sure that they won't get same mission!)
                                                  // between 1 and 3 is "so-so"


    private bool firstTime;

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

        if (RingPrefabForIntelMission == null)
            Debug.Log("ERROR - MissionManager needs to have RingPrefabForIntelMission assigned!");

        if (MissionIsCompletedPrefab == null)
            Debug.Log("ERROR - MissionIsCompletedPrefab not assigned to GameManager");

    }

    

    public void GetNewMissions()
    {
        // set up all the lists
        Players = GameManager.Instance.Players;
        AlreadyChosenMissions = new List<MissionBase>(Players.Count);
        AllAvailableMissionsTotal = new List<MissionBase>(4);
        InstantiatedMissions = new List<MissionBase>(4);

        if (MissionTexts.Count != 4)
            Debug.Log("ERROR - Mission Manager needs to have 4 links to TextMesh!");

        // find all the missions parented to this game object
        MissionBase[] allChildren = GetComponentsInChildren<MissionBase>();
        foreach (MissionBase mission in allChildren)
        {
            AllAvailableMissionsTotal.Add(mission);
        }

        if (AllAvailableMissionsTotal.Count < 4)
            Debug.Log("ERROR - at least 4 missions needs to be assigned to Mission Manager!");

        if (MissionTexts.Count != 4)
            Debug.Log("ERROR - 4 missions texts needs to be assigned to Mission Manager!");
        if (MissionIcons.Count != 4)
            Debug.Log("ERROR - 4 missions icons needs to be assigned to Mission Manager!");


        // choose four missions out of the total amount
        FourPotentialMissionsAvailable = ChooseMissionsFromSet(4, AllAvailableMissionsTotal);
        ShuffleMissions(FourPotentialMissionsAvailable);

        // set up template stuff that is only called once per mission
        foreach (MissionBase m in FourPotentialMissionsAvailable)
            m.TemplateSetUp();

        // set the rumble states for each mission (1, 2, 3, 4)
        for (int i = 1; i < 5; i++)
        {
            FourPotentialMissionsAvailable[i - 1].MissionIDRumble = i;
        }

        // destroy all missions on players
        // NOTE: players should only have 1 active mission at the same time!
        for (int i = 0; i < Players.Count; i++)
        {
            //Players[i].GetComponent<Player>().RemoveAllMissionsOnMe(); // does not work
            MissionBase m = Players[i].GetComponent<MissionBase>();
            //DestroyImmediate(m);

            if (m != null)
            {
                InstantiatedMissions.Remove(m);
                m.DestroyMission();
            }
        }

        // assign new missions
        for (int i = 0; i < Players.Count; i++)
        {
            MissionBase c = GetUniqueMission(); // find a "relatively random" mission

            string scriptName = c.ToString(); // get name of mission script so it can be attached to player
            Players[i].AddComponent(scriptName);
            Players[i].GetComponent<MissionBase>().InitializeMission(Players[i], c); // set up various stuff on mission via the template mission
            //Players[i].name = Players[i].GetComponent<Player>().Name + "_" + scriptName;
            InstantiatedMissions.Add(Players[i].GetComponent<MissionBase>()); // list of the current missions, easy to see in Inspector
        }

        // set the GUI things
        SetTextAndIcons();

    }

    public void GetNewMissionToSinglePlayer(GameObject Player)
    {
        // destroy old
        MissionBase oldMission = Player.GetComponent<MissionBase>();
        if (oldMission != null)
        {
            InstantiatedMissions.Remove(oldMission);
            oldMission.DestroyMission();
        }


        // get new mission
        MissionBase newMission = GetUniqueMission();
        string scriptName = newMission.ToString();
        Player.AddComponent(scriptName);
        Player.GetComponent<MissionBase>().InitializeMission(Player, newMission);
        InstantiatedMissions.Add(Player.GetComponent<MissionBase>());
    }

    public void RemoveAllMissions()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            //Players[i].GetComponent<Player>().RemoveAllMissionsOnMe(); // does not work
            MissionBase m = Players[i].GetComponent<MissionBase>();
            //DestroyImmediate(m);

            if (m != null)
            {
                InstantiatedMissions.Remove(m);
                m.DestroyMission();
            }
        }
    }

    void SetTextAndIcons()
    {
        for (int i = 0; i < 4; i++)
        {
            Destroy(MissionIcons[i].renderer.material); // to save up space

            MissionTexts[i].text = FourPotentialMissionsAvailable[i].MissionDescription;
            MissionIcons[i].renderer.material = FourPotentialMissionsAvailable[i].MissionMaterial; // TODO: use one parent object with text AND icon instead
        }
    }

    // missions
    public void PracticeMissionHUDRumble(int number)
    {
        if (MissionManager.Instance.MissionIcons[number] != null)
            iTween.PunchScale(MissionManager.Instance.MissionIcons[number], new Vector3(4, 4, 0), 0.5f);
    }

    // targets
    public void PracticeTargetHUDRumble(int number)
    {
            if (MissionManager.Instance.TargetHuds[number] != null)
        iTween.PunchScale(MissionManager.Instance.TargetHuds[number], new Vector3(0.05f, 0.05f, 0), 0.5f);
    }

    // controller
    public void PracticeControllerRumbleGUI(int number)
    {
        if (GameManager.Instance.ControllerGUIToRumble != null)
            iTween.PunchScale(GameManager.Instance.ControllerGUIToRumble, new Vector3(2, 2, 0), 0.5f);

        if (number < 4)
        {
            // only do this initially, then stop doing it
            if (GameManager.Instance.ControllerGUIToRumble != null)
                GameManager.Instance.RumbleStepsGUI[number].transform.renderer.enabled = true;
        }
    }

    IEnumerator PunchIcons(int howMany)
    {
        if (howMany+1 > 0)
        {
            print("hey" + howMany);
            if (MissionIcons[howMany] != null)
                iTween.PunchScale(MissionIcons[howMany], new Vector3(5, 5), 1f);
            yield return new WaitForSeconds(0f);
            StartCoroutine(PunchIcons(howMany - 1));
        }

    }

    void Update()
    {
        if (GameManager.Instance.PlayingState != PlayingState.Playing) // only check if game is playing
            return;

        for (int i = 4 - 1; i >= 0; i--)
        {
            MissionBase m = InstantiatedMissions[i];
            if (m != null)
            {
                if (!m.MissionIsActive) // dont look into inactive missions
                    return;

                if (m.MissionAccomplished()) // look if mission has been accomplished
                {
                    GoKitTweenExtensions.shake(Camera.main.transform, 0.5f, new Vector3(0.2f, 0.2f, 0.2f), GoShakeType.Position);

                    GameObject g = (GameObject)Instantiate(MissionIsCompletedPrefab);
                    MissionCompletedText t = g.GetComponent<MissionCompletedText>();
                    t.SoundToPlay = m.ChooseCompletedSound();
                    t.ColorToUse = new Color(m.PlayerScript.PlayerColor.r, m.PlayerScript.PlayerColor.g, m.PlayerScript.PlayerColor.b, 255);

                    //Debug.Log(string.Format("{0} mission accomplished ({1} points)", m.ToString(), m.Points));
                    m.GivePointsToPlayer();
                }
            }
        }
        /*foreach (MissionBase m in InstantiatedMissions)
        {
            if (!m.MissionIsActive) // dont look into inactive missions
                return;

            if (m.MissionAccomplished()) // look if mission has been accomplished
            {
                GoKitTweenExtensions.shake(Camera.main.transform, 0.5f, new Vector3(0.2f, 0.2f, 0.2f), GoShakeType.Position);
                audio.PlayOneShot(AudioManager.Instance.MissionAccomplishedSound);
                Debug.Log(string.Format("{0} mission accomplished ({1} points)", m.ToString(), m.Points));
                m.GivePointsToPlayer();
            }
        }*/
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

