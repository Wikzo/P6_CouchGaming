using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayingState
{
    ControllerCalibration,
    PraticeMode,
    WaitingForEverbodyToGetReady,
    Playing,
    DisplayingScore,
    Paused,
    GameIsOver
}

public class GameManager : MonoBehaviour
{
    // Singleton itself
    private static GameManager _instance;

    // Fields
    public List<GameObject> Players;// = new GameObject[4];
    
    [HideInInspector]
    public List<ResetObjectPosition> AllObjectsToReset; // contains all objects in scene that must reset

    public int NumberOfRoundsPerGame = 6;
    
    // Round stuff
    [HideInInspector]
    public int CurrentRound;
    public bool WaitForReady = true;
    public float TimePerRound = 60;
    
    [HideInInspector]
    public float TimeLeft;
    [HideInInspector]
    public bool CurrentRoundJustEnded;
    [HideInInspector]
    public PlayingState PlayingState = PlayingState.ControllerCalibration;
    private PlayingState lastState; // used to remember pause

    [HideInInspector]
    public bool HasPlayedAtLeastOnce;

    // mission stuff
    public List<int> TargetChosenSoFar = new List<int>();
    public GameObject MissionInitializedParticles;

    private Camera Camera;

    // Debug stuff
    public bool DebugMode = true;

    [HideInInspector]
    public int GUIRumbleCounter = 1;

    // GUI mission hud stuff
    public GameObject ControllerGUIToRumble;
    public GameObject[] RumbleStepsGUI;
    public GameObject GUIMissionHud;
    private bool RumblePracticeStart = true;
    public TimeBar TimeBar;

    public Light MainLight;

    //  public static Instance  
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;

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

        this.PlayingState = PlayingState.ControllerCalibration;
    }

    void Start()
    {
        TimeLeft = TimePerRound;

        CurrentRound = NumberOfRoundsPerGame+1;

        TargetChosenSoFar = new List<int>();

        HasPlayedAtLeastOnce = false;

        // TODO: make practice rumble things
        /*foreach (GameObject g in Players)
        {

            MissionBase m = new MissionKill();
            string scriptName = m.ToString();
            g.AddComponent(scriptName);

            g.GetComponent<MissionBase>().PraciceRumbles();
         * 

        }*/



        Camera = GameObject.Find("Main Camera").camera;
        if (Camera == null)
            Debug.Log("ERROR - Game Manager needs a link to the camera!");

        if (MainLight == null)
            Debug.Log("ERROR - assign directional light to TextFade");

        if (TimeBar == null)
            Debug.Log("ERROR - assign timebar to Game Manager");


        MissionManager.Instance.GetNewMissions();

        StartCoroutine(StartRumblePractices());


    }


    public void Pause()
    {
        if (PlayingState != PlayingState.Paused)
        {
            lastState = PlayingState;

            PlayingState = PlayingState.Paused;
            Time.timeScale = 0;
            MainLight.enabled = false;
        }
        else if (PlayingState == PlayingState.Paused)
        {
            Time.timeScale = 1;
            PlayingState = lastState;
            MainLight.enabled = true;

        }
    }

    IEnumerator StartRumblePractices()
    {
        if (ControllerGUIToRumble != null)
        {
            ControllerGUIToRumble.SetActive(true);
            ControllerGUIToRumble.GetComponent<Animator>().enabled = false;
        }

        MissionBase m;
        for (int i = 0; i < Players.Count; i++)
        {
            m = Players[i].GetComponent<MissionBase>();


            if (GUIRumbleCounter < 5)
            {
                //MissionManager.Instance.PracticeControllerRumbleGUI(number-1);

                bool showPunchTween = (i == 0); // only player index 0 makes HUD iTween

                m.StartPracticeRumbleController(GUIRumbleCounter, showPunchTween);
            }
            else
            {
                m.StopPracticeRumbleController();

                if (ControllerGUIToRumble.GetComponent<Animator>() != null)
                    ControllerGUIToRumble.GetComponent<Animator>().enabled = true;

                StartCoroutine(RemoveAnimations());
            }
        }

        yield return new WaitForSeconds(3);
        if (GUIRumbleCounter < 5)
        {
            GUIRumbleCounter++;

            if (RumblePracticeStart) // only repeat if this is true
                StartCoroutine(StartRumblePractices());
                
        }
        else
            SkipTutorialAndGoToWait();
    }

    IEnumerator RemoveAnimations()
    {
        // removes uneccesary animators on controller/mission huds

        yield return new WaitForSeconds(10f);
        Animator hud = GUIMissionHud.GetComponent<Animator>();

        if (ControllerGUIToRumble != null)
            Destroy(ControllerGUIToRumble);
        
        if (hud != null)
            Destroy(hud);
    }

    void Update()
    {
        // turn off when calibrating/tutorial
        if (PlayingState == PlayingState.WaitingForEverbodyToGetReady)
            MainLight.enabled = false;
        else
            MainLight.enabled = true;

        if (PlayingState == PlayingState.Playing)
        {
            TimeLeft -= Time.deltaTime;

            if (TimeLeft <= 0)
            {
                CurrentRoundJustEnded = true;
                PlayingState = PlayingState.DisplayingScore;
            }
        }
        /*if (PlayingState == PlayingState.DisplayingScore)
            Camera.GetComponent<GlitchEffect>().enabled = true;
        else
            Camera.GetComponent<GlitchEffect>().enabled = false;*/

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }

    public void SkipTutorialAndGoToWait() // used to go directly from "tutorial" to wait for ready
    {
        StopCoroutine("StartRumblePractices");
        RumblePracticeStart = false;


        if (ControllerGUIToRumble != null)
            ControllerGUIToRumble.SetActive(false);

        MissionBase m;
        foreach (GameObject p in Players)
        {
            m = p.GetComponent<MissionBase>();
            m.StopPracticeRumbleController();
            m.StopAllRumble();
        }

        StartCoroutine(RemoveAnimations());

        PlayingState = PlayingState.PraticeMode;
    }

    public void ResetLevel()
    {

        PlayingState = PlayingState.Playing;


        if (ControllerGUIToRumble != null)
            Destroy(ControllerGUIToRumble);

        foreach (ResetObjectPosition r in AllObjectsToReset) // reset all objects to initial state
            r.ResetMyPosition();

        TimeLeft = TimePerRound;
        CurrentRoundJustEnded = false;

        CurrentRound--;

        if (CurrentRound <= 0)
        {
            PlayingState = PlayingState.GameIsOver;
            return;
        }

        PlayingState = PlayingState.WaitingForEverbodyToGetReady;

        foreach (GameObject p in Players)
            p.GetComponent<Player>().Reset();

        InvokeRepeating("AllReady", 0, 0.01f);


        MissionManager.Instance.GetNewMissions();

        //GoKitTweenExtensions.shake(Camera.main.transform, 0.5f, new Vector3(0.2f, 0.2f, 0.2f), GoShakeType.Position);

        //if(WaitForReady)
        //{
            
        //}
        //else
          //  PlayingState = PlayingState.Playing;
    }

    private void OnGUI()
    {
        /*
        GUILayout.Label(string.Format("Current state: {0}", PlayingState.ToString()));
        GUILayout.Label(string.Format("Round: {0}", CurrentRound.ToString()));

        switch (PlayingState)
        {
            case PlayingState.WaitingForEverbodyToGetReady:
                break;

            case PlayingState.Playing:
                GUILayout.Label(string.Format("TIME: {0}", TimeLeft.ToString("F2")));
                break;

            case PlayingState.DisplayingScore:
                GUILayout.Label("SCORE TABLES:\nbla\nbla\nbla");
                break;

            case PlayingState.Paused:
                GUILayout.Label("***PAUSED***");
                break;


            case PlayingState.GameIsOver:
                GUILayout.Label("BUHUUUU GAME IS OVER!! :(");
                if (GUI.Button(new Rect(Screen.width/2, Screen.height/2, 200, 200), "Restart"))
                    ResetWholeGame();
                break;
        }
         * */

    }

    public void ResetWholeGame()
    {
        Application.LoadLevel(0);
    }

    void AllReady()
    {
        int playersReady = 0;

        for(int i = 0; i<Players.Count; i++)
        {
            if(Players[i].GetComponent<Player>().IsReadyToBegin)
            {
                playersReady++;
            }
        }

        if(playersReady == Players.Count)
        {
            foreach(GameObject player in Players)
            {
                Player playerScript = player.GetComponent<Player>();

                playerScript.PState = PlayerState.Alive;

                playerScript.IsReadyToBegin = false;

                playerScript.ChosenSpawn.SetActive(true);
                playerScript.SpawnZone.SetActive(false);
            }
            CancelInvoke("AllReady");

            PlayingState = PlayingState.Playing;
        }
    }
}
