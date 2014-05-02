using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public enum ButtonsToPress
{
    A = 0,
    B = 1,
    X = 2,
    Y = 3
}

public class ControllerPlayer
{
    public ControllerPlayer(PlayerIndex index)
    {
        this.Index = index;
        this.state = GamePad.GetState(this.Index);
        this.previousState = GamePad.GetState(this.Index);
    }

    public PlayerIndex Index;
    public GamePadState state;
    public GamePadState previousState;
    public bool HasInputted;

    public void UpdateState()
    {
        this.state = GamePad.GetState(this.Index);
    }

    public bool ButtonPressedRightNow(ButtonsToPress button)
    {
        switch (button)
        {
            case ButtonsToPress.A:
                return this.state.Buttons.A == ButtonState.Pressed && this.previousState.Buttons.A == ButtonState.Released;
                break;

            case ButtonsToPress.B:
                return this.state.Buttons.B == ButtonState.Pressed && this.previousState.Buttons.B == ButtonState.Released;
                break;

            case ButtonsToPress.X:
                return this.state.Buttons.X == ButtonState.Pressed && this.previousState.Buttons.X == ButtonState.Released;
                break;

            case ButtonsToPress.Y:
                return this.state.Buttons.Y == ButtonState.Pressed && this.previousState.Buttons.Y == ButtonState.Released;
                break;

            default:
                return false;
        
        }  
    }
    public bool ButtonReleasedRightNow(ButtonsToPress button)
    {
        switch (button)
        {
            case ButtonsToPress.A:
                return this.state.Buttons.A == ButtonState.Released && this.previousState.Buttons.A == ButtonState.Pressed;
                break;

            case ButtonsToPress.B:
                return this.state.Buttons.B == ButtonState.Released && this.previousState.Buttons.B == ButtonState.Pressed;
                break;

            case ButtonsToPress.X:
                return this.state.Buttons.X == ButtonState.Released && this.previousState.Buttons.X == ButtonState.Pressed;
                break;

            case ButtonsToPress.Y:
                return this.state.Buttons.Y == ButtonState.Released && this.previousState.Buttons.Y == ButtonState.Pressed;
                break;

            default:
                return false;

        }
    }
}

public class ControllerTesterManager : MonoBehaviour
{
    List<ControllerPlayer> ControllerPlayers; // all four players

    public bool RumblingRightNow;
    public ControllerTesterRumble CurrentRumble;
    public float RumbleTimer;
    public float RumbleInterval;
    public bool ReadyToGetInput;
    //public bool ReadyToGetInputPreTime;
    int inputCounter;
    public float InputTime;
    //public float PreTime;
    bool usingHelpPaper;

    private int pattern = 0;
    private string[] patternString = { "A", "B", "X", "Y" , "Random"};

    void FindAllControllers()
    {
        // make controllers ready
        for (int i = 0; i < 4; ++i)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                ControllerPlayer p = new ControllerPlayer(testPlayerIndex);
                ControllerPlayers.Add(p);
            }
        }
    }

    void Start()
    {
        Application.runInBackground = true;

        ControllerPlayers = new List<ControllerPlayer>();
        FindAllControllers();

        RumblingRightNow = false;
        RumbleTimer = 0;
        RumbleInterval = 1;
        ReadyToGetInput = false;
        inputCounter = 0;
        InputTime = 0;
        usingHelpPaper = false;

        LoggingManager.CreateTextFile("ControllerTest_");
        LoggingManager.AddText("\n");
        LoggingManager.AddTextNoTimeStamp("PlayerID, UsingHelpPaper, RumbleType, RumbleVariation, PlayerAnswer, RumbleDuration, ReactionTime, ReactionTimeMinusRumbleDuration, CorrectButtonPress\n\n");
    }

    void OnGUI()
    {
        if (RumblingRightNow || ReadyToGetInput)
        {
            string text = string.Format("Rumbling now ... {0}", CurrentRumble.ToString());

            if (ReadyToGetInput)
            {
                string readyText = string.Format("\nREADY FOR INPUT: {0}", inputCounter);
                text += readyText;
            }
            
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 500, 500), text);

            return;
        }

        // use help paper
        usingHelpPaper = GUI.Toggle(new Rect(800, 300, 200, 30), usingHelpPaper, "Using help paper");

        // choose pattern time interval
        RumbleInterval = GUI.HorizontalSlider(new Rect(800, 400, 100, 30), RumbleInterval, 0.1f, 5f);
        GUI.Label(new Rect(910, 400, 100, 30), RumbleInterval.ToString());
        GUI.Label(new Rect(800, 380, 300, 30), "Time interval (only for *):");


        // choose pattern message (A, B, X, Y)
        pattern = GUI.SelectionGrid(new Rect(800, 0, 300, 200), pattern, patternString, 1);

        GUILayout.BeginArea(new Rect(0, 0, 700, 700));


        // choose pattern type
        if (GUILayout.Button("Static Intensity rumble*"))
        {
            CurrentRumble = new StaticIntensity(ControllerPlayers, this);
                
            if (CurrentRumble != null)
                    CurrentRumble.StartRumble(pattern);
        }

        if (GUILayout.Button("Varying Intensity rumble*")) // Benjamin
        {
            CurrentRumble = new ControllerTesterRumbleVaryingIntensity(ControllerPlayers, this);

            if (CurrentRumble != null)
                CurrentRumble.StartRumble(pattern);
        }

        if (GUILayout.Button("Right/left rumble*"))
        {
            CurrentRumble = new RightLeft(ControllerPlayers, this);

            if (CurrentRumble != null)
                CurrentRumble.StartRumble(pattern);
        }

        if (GUILayout.Button("Morsecode rumble"))
        {
            CurrentRumble = new MorseCode(ControllerPlayers, this);


            if (CurrentRumble != null)
                CurrentRumble.StartRumble(pattern);
        }

        if (GUILayout.Button("Interval rumble"))
        {
            CurrentRumble = new Interval(ControllerPlayers, this);


            if (CurrentRumble != null)
                CurrentRumble.StartRumble(pattern);
        }

        GUILayout.EndArea();

    }

    public void RemoveRumble()
    {
        CurrentRumble = null;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("escape"))
            Application.Quit();

        if (RumblingRightNow)
        {
            RumbleTimer += Time.deltaTime;
            CurrentRumble.UpdateRumble();

        }

        //if (ReadyToGetInputPreTime)
          //  PreTime += Time.deltaTime;

        if (ReadyToGetInput)
            InputTime += Time.deltaTime;
        
        //print(RumbleTimer);

        foreach (ControllerPlayer p in ControllerPlayers)
        {
            p.UpdateState();

            if (ReadyToGetInput)
            {
                if (inputCounter < ControllerPlayers.Count)
                {
                    // pressed
                    if (p.ButtonPressedRightNow(ButtonsToPress.A) == true && !p.HasInputted)
                    {
                        p.HasInputted = true;
                        inputCounter++;

                        float time;
                        time = InputTime;

                        bool correct = CurrentRumble.pattern == ButtonsToPress.A;

                        if (CurrentRumble is RightLeft)
                            if (CurrentRumble.pattern == ButtonsToPress.A || CurrentRumble.pattern == ButtonsToPress.B)
                                correct = true;

                        string test = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", (int)p.Index, usingHelpPaper, CurrentRumble.ToString(), CurrentRumble.pattern, ButtonsToPress.A.ToString(), CurrentRumble.RumbleDuration, time, time - CurrentRumble.RumbleDuration, correct);

                        LoggingManager.AddTextNoTimeStamp(test);
                    }

                    if (p.ButtonPressedRightNow(ButtonsToPress.B) == true && !p.HasInputted)
                    {
                        p.HasInputted = true;
                        inputCounter++;

                        float time;
                        time = InputTime;

                        bool correct = CurrentRumble.pattern == ButtonsToPress.B;

                        if (CurrentRumble is RightLeft)
                            if (CurrentRumble.pattern == ButtonsToPress.B || CurrentRumble.pattern == ButtonsToPress.A)
                                correct = true;

                        string test = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", (int)p.Index, usingHelpPaper, CurrentRumble.ToString(), CurrentRumble.pattern, ButtonsToPress.B.ToString(), CurrentRumble.RumbleDuration, time, time - CurrentRumble.RumbleDuration, correct);
                        LoggingManager.AddTextNoTimeStamp(test);
                    }

                    if (p.ButtonPressedRightNow(ButtonsToPress.X) == true && !p.HasInputted)
                    {
                        p.HasInputted = true;
                        inputCounter++;

                        float time;
                        time = InputTime;

                        bool correct = CurrentRumble.pattern == ButtonsToPress.X;

                        if (CurrentRumble is RightLeft)
                            if (CurrentRumble.pattern == ButtonsToPress.X || CurrentRumble.pattern == ButtonsToPress.Y)
                                correct = true;

                        string test = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", (int)p.Index, usingHelpPaper, CurrentRumble.ToString(), CurrentRumble.pattern, ButtonsToPress.X.ToString(), CurrentRumble.RumbleDuration, time, time - CurrentRumble.RumbleDuration, correct);
                        
                        
                        LoggingManager.AddTextNoTimeStamp(test);
                    }

                    if (p.ButtonPressedRightNow(ButtonsToPress.Y) == true && !p.HasInputted)
                    {
                        p.HasInputted = true;
                        inputCounter++;

                        float time;
                        time = InputTime;

                        bool correct = CurrentRumble.pattern == ButtonsToPress.Y;
                        if (CurrentRumble is RightLeft)
                            if (CurrentRumble.pattern == ButtonsToPress.Y || CurrentRumble.pattern == ButtonsToPress.X)
                                correct = true;

                        string test = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", (int)p.Index, usingHelpPaper, CurrentRumble.ToString(), CurrentRumble.pattern, ButtonsToPress.Y.ToString(), CurrentRumble.RumbleDuration, time, time - CurrentRumble.RumbleDuration, correct);

                        LoggingManager.AddTextNoTimeStamp(test);
                    }
                    
                }
                else
                {
                    ReadyToGetInput = false;
                    //ReadyToGetInputPreTime = false;
                    inputCounter = 0;
                    InputTime = 0;
                    //PreTime = 0;

                    if (!RumblingRightNow)
                        RemoveRumble();
                }
            }

            p.previousState = p.state;

        }

    }

    private void OnApplicationQuit()
    {
        foreach (ControllerPlayer p in ControllerPlayers)
        {
            GamePad.SetVibration(p.Index, 0, 0);
        }
    }
}
