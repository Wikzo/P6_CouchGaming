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
        ControllerPlayers = new List<ControllerPlayer>();
        FindAllControllers();

        RumblingRightNow = false;
        RumbleTimer = 0;
    }

    void OnGUI()
    {
        if (RumblingRightNow)
        {
            string text = string.Format("Rumbling now ... {0}", CurrentRumble.ToString());
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 500, 500), text);

            return;
        }

        pattern = GUI.SelectionGrid(new Rect(600, 0, 300, 200), pattern, patternString, 1);

        GUILayout.BeginArea(new Rect(0, 0, 500, 500));

        if (GUILayout.Button("Static Intensity rumble"))
        {
            CurrentRumble = new StaticIntensity(ControllerPlayers, this);
                
            if (CurrentRumble != null)
                    CurrentRumble.StartRumble(pattern);
        }

        if (GUILayout.Button("Varying Intensity rumble")) // Benjamin
        {
            CurrentRumble = new ControllerTesterRumbleVaryingIntensity(ControllerPlayers, this);

            if (CurrentRumble != null)
                CurrentRumble.StartRumble(pattern);
        }

        if (GUILayout.Button("Right/left rumble"))
        {
            if (CurrentRumble != null)
                CurrentRumble.StartRumble(pattern);
        }

        if (GUILayout.Button("Morsecode rumble"))
        {
            if (CurrentRumble != null)
                CurrentRumble.StartRumble(pattern);
        }

        if (GUILayout.Button("Interval rumble"))
        {
            if (CurrentRumble != null)
                CurrentRumble.StartRumble(pattern);
        }

        GUILayout.EndArea();

    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentRumble != null)
            CurrentRumble.UpdateRumble();

        foreach (ControllerPlayer p in ControllerPlayers)
        {
            p.UpdateState();

            // pressed
            if (p.ButtonPressedRightNow(ButtonsToPress.A) == true)
                print("A is down");

            if (p.ButtonPressedRightNow(ButtonsToPress.B) == true)
                print("B is down");

            if (p.ButtonPressedRightNow(ButtonsToPress.X) == true)
                print("X is down");

            if (p.ButtonPressedRightNow(ButtonsToPress.Y) == true)
                print("Y is down");

            p.previousState = p.state;

        }
        //this.previousState = this.state;

    }

    private void OnApplicationQuit()
    {
        foreach (ControllerPlayer p in ControllerPlayers)
        {
            GamePad.SetVibration(p.Index, 0, 0);
        }
    }
}
