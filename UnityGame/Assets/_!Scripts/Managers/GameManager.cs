using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayingState
{
    TalkingBeforeControllerCalibration,
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

    public GameObject PlusObject;
    public GameObject DieParticles;
    public GameObject SpawnParticles;

    private Camera Camera;
    public Camera CameraOrtographic;

    public GameObject Congratulations;
    public GameObject MusicPlayer;
    bool musicIsPlaying = false;

    // Debug stuff
    public bool DebugMode = true;
    public bool UseAnnouncer = true;

    public bool LogForTest = false;

    public int readyCounter = -1;

    [HideInInspector]
    public int GUIRumbleCounter = 1;

    // GUI mission hud stuff
    public GameObject ControllerGUIToRumble;
    public GameObject[] RumbleStepsGUI;
    public GameObject GUIMissionHud;
    private bool RumblePracticeStart = true;
    public TimeBar TimeBar;
    public GameObject GetReady;
    [HideInInspector]
    public bool ReadyNotYetSpawned = false;

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

        Screen.showCursor = false;

        this.PlayingState = PlayingState.TalkingBeforeControllerCalibration;
        ReadyNotYetSpawned = false;

        readyCounter = -1;
    }

    void Start()
    {
        TimeLeft = TimePerRound;

        CurrentRound = NumberOfRoundsPerGame+1;

        TargetChosenSoFar = new List<int>();

        HasPlayedAtLeastOnce = false;

        for (int i = Players.Count - 1; i >= 0; i--)
        {
            GameObject p = Players[i];

            if (!p.GetComponent<Player>().HasBeenChosen)
            {
                Players.Remove(p);
                Destroy(p);
            }


        }


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

        if (UseAnnouncer)
            StartCoroutine(StartCalibrationVoice());
        else
            StartCoroutine(StartRumblePractices());

        if (LogForTest)
        {
            LoggingManager.CreateTextFile();
            string text = "PlayerID, CurrentMissionType, TotalMissionsPlayerHasHadSoFar, ButtonPressType, ThisButtonPressNumber, TimeSinceReceivedMission";
            LoggingManager.AddTextNoTimeStamp(text);
        }


    }

    IEnumerator StartCalibrationVoice()
    {
        this.PlayingState = PlayingState.TalkingBeforeControllerCalibration;

        float length = AudioManager.Instance.CalibrationAudio.length;
        AudioManager.Instance.PlayAnnouncerVoice(AudioManager.Instance.CalibrationAudio);

        yield return new WaitForSeconds(length);
        StartCoroutine(StartRumblePractices());

    }


    /*public void Pause()
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
    }*/

    IEnumerator StartRumblePractices()
    {
        if (RumblePracticeStart) // only if not skipped
            this.PlayingState = PlayingState.ControllerCalibration;

        if (ControllerGUIToRumble != null)
        {
            ControllerGUIToRumble.SetActive(true);
            ControllerGUIToRumble.GetComponent<Animator>().enabled = false;
        }

        MissionBase m;
        for (int i = 0; i < Players.Count; i++)
        {
            if (RumblePracticeStart) // only if not skipped
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
                    {
                        if (ControllerGUIToRumble.GetComponent<Animator>().enabled == false)
                        {
                            AudioManager.Instance.PlayAnnouncerVoice(AudioManager.Instance.VibrationExplain);
                            yield return new WaitForSeconds(AudioManager.Instance.VibrationExplain.length - 2.5f);
                            if (ControllerGUIToRumble.GetComponent<Animator>() != null)
                                ControllerGUIToRumble.GetComponent<Animator>().enabled = true;
                        }

                    }

                    StartCoroutine(RemoveAnimations());
                }
            }
        }

        float waitTime = 0;
        switch (GUIRumbleCounter)
        {
            case 0:
            case 1:
                waitTime = 1;
                break;

            case 2:
                waitTime = 1.8f;
                break;

            case 3:
                waitTime = 2.3f;
                break;

            default:
                waitTime = 2.5f;
                break;

        }
        yield return new WaitForSeconds(waitTime);
        MissionManager.Instance.HideControllerGUIs();

        yield return new WaitForSeconds(0.5f);
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

        yield return new WaitForSeconds(18f);
        Animator hud = GUIMissionHud.GetComponent<Animator>();

        if (ControllerGUIToRumble != null)
            Destroy(ControllerGUIToRumble);
        
        if (hud != null)
            Destroy(hud);
    }

    void Update()
    {
        if (PlayingState == PlayingState.Playing)
        {
            TimeLeft -= Time.deltaTime;

            if (TimeLeft <= 0 && CurrentRoundJustEnded == false)
            {
                CurrentRoundJustEnded = true;
                PlayingState = PlayingState.DisplayingScore;
                MissionManager.Instance.RemoveAllMissions();
                GoKitTweenExtensions.shake(Camera.main.transform, 0.5f, new Vector3(0.8f, 0.8f, 0.8f), GoShakeType.Position);
                
                // TODO: who won?

                int highestScore = -10000;
                int numberOfPeopleWithSameHighScore = 0;

                // check highest score
                TargetIDColorState winningColor = TargetIDColorState.NotAssigned;
                foreach (GameObject g in Players)
                {
                    Player p = g.GetComponent<Player>();
                    if (p.Points >= highestScore)
                    {
                        highestScore = p.Points;
                        winningColor = g.GetComponent<TargetIDColor>().TargetIDColorState;
                    }
                }

                // check if draw
                foreach (GameObject g in Players)
                {
                    Player p = g.GetComponent<Player>();

                    if (p.Points == highestScore)
                    {
                        //Debug.Log(p.ToString() + p.Points);
                        numberOfPeopleWithSameHighScore++;
                    }
                }

                if (numberOfPeopleWithSameHighScore > 1) // draw?
                    winningColor = TargetIDColorState.NotAssigned;

                Destroy(GameObject.Find("Music")); // dont play game music when showing score
                switch(winningColor)
                {
                    case TargetIDColorState.RedOne:
                        AudioManager.Instance.PlayAnnouncerVoice(AudioManager.Instance.WinRed);
                        if (DataSaver.Instance != null)
                        {
                            foreach (var score in DataSaver.Instance.highScores)
                                score.RedWins++;
                        }
                        //Debug.Log("Red won");
                        break;

                    case TargetIDColorState.BlueTwo:
                        AudioManager.Instance.PlayAnnouncerVoice(AudioManager.Instance.WinBlue);
                        if (DataSaver.Instance != null)
                        {
                            foreach (var score in DataSaver.Instance.highScores)
                                score.BlueWins++;
                        }
                        //Debug.Log("blue won");
                        break;

                    case TargetIDColorState.GreenThree:
                        AudioManager.Instance.PlayAnnouncerVoice(AudioManager.Instance.WinGreen);
                        if (DataSaver.Instance != null)
                        {
                            foreach (var score in DataSaver.Instance.highScores)
                                score.GreenWins++;
                        }
                        //Debug.Log("green won");
                        break;

                    case TargetIDColorState.PinkFour:
                        AudioManager.Instance.PlayAnnouncerVoice(AudioManager.Instance.WinPink);
                        if (DataSaver.Instance != null)
                        {
                            foreach (var score in DataSaver.Instance.highScores)
                                score.PinkWins++;
                        }
                        //Debug.Log("pink won");
                        break;

                    case TargetIDColorState.NotAssigned:
                        AudioManager.Instance.PlayAnnouncerVoice(AudioManager.Instance.WinDraw);
                        //Debug.Log("draw won");
                        break;


                }
                Instantiate(Congratulations);
                if (DataSaver.Instance != null)
                    DataSaver.Instance.SaveScoresToDataFile();
            }
        }
        /*if (PlayingState == PlayingState.DisplayingScore)
            Camera.GetComponent<GlitchEffect>().enabled = true;
        else
            Camera.GetComponent<GlitchEffect>().enabled = false;*/

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);

    }

    public void SkipTutorialAndGoToWait() // used to go directly from "tutorial" to wait for ready
    {
        StopCoroutine("StartCalibrationVoice");
        StopCoroutine("StartRumblePractices"); 
        RumblePracticeStart = false;

        //AudioManager.Instance.PlayAnnouncerVoice(AudioManager.Instance.VibrationExplain);

        //yield return new WaitForSeconds(AudioManager.Instance.VibrationExplain.length);



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
        AudioManager.Instance.PlayAnnouncerVoice(AudioManager.Instance.PracticeAudio);

    }

    public void ResetLevel()
    {

        /*if (!musicIsPlaying)
        {
            Instantiate(MusicPlayer);
            musicIsPlaying = true;
        }*/
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
        AudioManager.Instance.PlayAnnouncerVoice(AudioManager.Instance.NewMissionsFirstTime);

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

    /*private void OnGUI()
    {
        
        GUILayout.Label(string.Format("Current state: {0}", PlayingState.ToString()));
        GUILayout.Label(string.Format("Round: {0}", CurrentRound.ToString()));/*

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
         *

    }*/

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
                AudioManager.Instance.StopAllAudio();
                Player playerScript = player.GetComponent<Player>();

                playerScript.PState = PlayerState.Alive;

                playerScript.IsReadyToBegin = false;

                playerScript.ChosenSpawn.SetActive(true);
                playerScript.SpawnZone.SetActive(false);
            }
            CancelInvoke("AllReady");

            //PlayingState = PlayingState.Playing;
            ReadyNotYetSpawned = true;
            AudioManager.Instance.PlayAnnouncerVoice(AudioManager.Instance.ReadyGo);
            Instantiate(GetReady); // ready object sets the state to playing
        }
    }
}
