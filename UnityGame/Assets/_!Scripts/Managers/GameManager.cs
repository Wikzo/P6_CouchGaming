using System.Collections.Generic;
using UnityEngine;

public enum PlayingState
{
    Playing,
    Paused,
    Pratice
}

public class GameManager : MonoBehaviour
{
    // Singleton itself
    private static GameManager _instance;

    // Fields
    public List<GameObject> Players;// = new GameObject[4];

    public int NumberOfRoundsPerGame = 5;
    
    // Round stuff
    [HideInInspector]
    public int CurrentRound;
    private float TimePerRound = 5;
    private float TimeLeft;
    [HideInInspector]
    public bool CurrentRoundJustEnded;
    [HideInInspector]
    public PlayingState PlayingState = PlayingState.Playing;

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

        GoKitTweenExtensions.shake(Camera.main.transform, 0.5f, new Vector3(0.2f, 0.2f, 0.2f), GoShakeType.Position);
    }

    void Start()
    {
        CurrentRound = NumberOfRoundsPerGame;
        TimeLeft = TimePerRound;
        CurrentRoundJustEnded = false;
    }
    void Update()
    {
        TimeLeft -= Time.deltaTime;

        if (TimeLeft <= 0)
        {
            CurrentRoundJustEnded = true;
            Time.timeScale = 0;
            PlayingState = PlayingState.Paused;
        }
    }

    void OnGUI()
    {
        GUILayout.Label(string.Format("TIME: {0}", TimeLeft.ToString("F2")));
        GUILayout.Label(string.Format("ROUND: {0}", CurrentRound.ToString()));

        if (CurrentRoundJustEnded)
        {
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 200, 200), "START NEXT ROUND"))
            {
                CurrentRoundJustEnded = false;
                CurrentRound--;
                TimeLeft = TimePerRound;
                PlayingState = PlayingState.Playing;
                Time.timeScale = 1;

            }
        }

        
    }
}
