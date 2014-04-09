using System.Collections.Generic;
using UnityEngine;

public enum PlayingState
{
    WaitingForEverbodyToGetReady,
    Playing,
    DisplayingScore,
    Paused,
    PraticeMode,
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

    public int NumberOfRoundsPerGame = 5;
    
    // Round stuff
    [HideInInspector]
    public int CurrentRound;
    public bool WaitForReady = true;
    private float TimePerRound = 5;
    private float TimeLeft;
    [HideInInspector]
    public bool CurrentRoundJustEnded;
    [HideInInspector]
    public PlayingState PlayingState;

    // Debug stuff
    public bool DebugMode = true;

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

    }

    void Start()
    {
        CurrentRound = NumberOfRoundsPerGame;
        this.PlayingState = PlayingState.PraticeMode;
    }
    void Update()
    {
        if (PlayingState == PlayingState.Playing)
        {
            TimeLeft -= Time.deltaTime;

            if (TimeLeft <= 0)
            {
                CurrentRoundJustEnded = true;
                PlayingState = PlayingState.DisplayingScore;
            }
        }
    }

    public void ResetLevel()
    {
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
                if (GUI.Button(new Rect(Screen.width/2, Screen.height/2, 200, 200), "START NEXT ROUND"))
                    ResetLevel();
                break;

            case PlayingState.Paused:
                GUILayout.Label("***PAUSED***");
                break;

            case PlayingState.PraticeMode:
                GUILayout.Label("PRACTICE MODE");

                if (GUI.Button(new Rect(Screen.width/2, Screen.height/2, 200, 200), "Click to start game"))
                    ResetLevel();
                break;

            case PlayingState.GameIsOver:
                GUILayout.Label("BUHUUUU GAME IS OVER!! :(");
                if (GUI.Button(new Rect(Screen.width/2, Screen.height/2, 200, 200), "Restart"))
                    ResetWholeGame();
                break;
        }
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

                playerScript.ChosenSpawn.SetActive(true);
                playerScript.SpawnZone.SetActive(false);
                
                playerScript.IsReadyToBegin = false;
            }
            CancelInvoke("AllReady");

            PlayingState = PlayingState.Playing;
        }
    }
}
