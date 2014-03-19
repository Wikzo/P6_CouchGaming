using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using Random = UnityEngine.Random;

// Required in C#

public class Player
{
    // fields
    public PlayerIndex index;
    public GamePadState state;
    public GamePadState prevState;
    public bool RumbleNow;
    private float timer;
    private float whenToRumbleNext = 10;
    private float currentRumbleTimer;
    private float rumbleDuration = 1;
    private float rumblePowerL = 1;
    private float rumblePowerR = 1;
    private bool hasWrittenRumbleLog;

    public bool ButtonDown;
    private bool ButtonUp;

    // constructor
    public Player(int index)
    {
        switch (index)
        {
            case 1:
                this.index = PlayerIndex.One;
                break;
            case 2:
                this.index = PlayerIndex.Two;
                break;
            case 3:
                this.index = PlayerIndex.Three;
                break;
            case 4:
                this.index = PlayerIndex.Four;
                break;
            default:
                Debug.Log(string.Format("ERROR! {0}'s gamepad index has not been assigned!", index));
                this.index = PlayerIndex.One;
                break;
        }

        this.state = GamePad.GetState(this.index);
        this.prevState = GamePad.GetState(this.index);

        if (this.state.IsConnected)
        {
            Debug.Log(string.Format("GamePad found P{0}", (int)this.index));
            LoggingManager.AddText(this + " was added.");
            this.StartRumbleCountdown();
        }

    }

    public bool ControllerIsConnected()
    {
        return this.state.IsConnected;
    }

    // to string
    public override string ToString()
    {
        //return (String.Format("({0},{1})", x, y));
        return (String.Format("P{0}", (int)index));
    }

    public void UpdatePlayer()
    {
        this.state = GamePad.GetState(this.index);

        // does not work??
        if (this.state.IsConnected && !this.prevState.IsConnected)
            LoggingManager.AddText(this + " was added.");

        // button to use
        ButtonDown = (state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B != ButtonState.Pressed);
        ButtonUp = (state.Buttons.B != ButtonState.Pressed && prevState.Buttons.B == ButtonState.Pressed);

    }

    // rumble interval
    public void StartRumbleCountdown()
    {
        whenToRumbleNext = Random.Range(5, 90); // rumble after 5 sec to 90 sec
        rumbleDuration = Random.Range(0.1f, 2.5f);
        rumblePowerL = Random.Range(0.1f, 1f);
        rumblePowerR = Random.Range(0.1f, 1f);

        timer = 0;
        currentRumbleTimer = 0;
        hasWrittenRumbleLog = false;

        //Debug.Log(string.Format("{0} will rumble in {1} seconds", this, whenToRumbleNext));
    }

    public void Rumble()
    {
        timer += Time.deltaTime;

        if (timer >= whenToRumbleNext) // get ready to rumble!
        {
            if (currentRumbleTimer < rumbleDuration) // rumble duration
            {
                if (!hasWrittenRumbleLog)
                {
                    hasWrittenRumbleLog = true;
                    LoggingManager.AddText(string.Format("{0} rumbles for {1} s with Power = ({2}; {3})", this, rumbleDuration, rumblePowerL, rumblePowerR));
                }
                currentRumbleTimer += Time.deltaTime;
                GamePad.SetVibration(this.index, rumblePowerL, rumblePowerR); // start rumbling
                //Debug.Log("Rumbling for " + (rumbleDuration-currentRumbleTimer) + " seconds");
            }
            else
            {
                GamePad.SetVibration(this.index, 0, 0); // stop rumbling
                StartRumbleCountdown(); // get ready for next random rumble
            }
        }
    }


}


public class RandomRumble : MonoBehaviour
{
    private List<Player> Players;
    private GameObject textObject;
    private string controllers;

    private void Start()
    {

        Application.runInBackground = true;

        textObject = GameObject.Find("textObject");
        LoggingManager.CreateTextFile();

        Players = new List<Player>(4);

        for (int i = 1; i < 5; i++)
        {
            Player p = new Player(i);
            p.state = GamePad.GetState(p.index);

            Players.Add(p);

            Debug.Log(p + " is connected: " + p.ControllerIsConnected().ToString());
        }
    }
    // Update is called once per frame
    private void Update()
    {
        // just to test if controllers work
        /*
        for (int i = 0; i < 4; i++)
        {
            print(i + " is connected: " + GamePad.GetState((PlayerIndex)i).IsConnected);
            if (GamePad.GetState((PlayerIndex)i).IsConnected)
            {
                GamePad.SetVibration((PlayerIndex)i, 1, 1);
                Debug.Log(i + " is vibrating");
            }
        }*/
        // check for new controllers OLD
        /*for (int i = 0; i < 4; i++)
        {
            if (!Players[i].ControllerIsConnected() && GamePad.GetState((PlayerIndex)i).IsConnected)
            {
                Player p = new Player(i);
                p.state = GamePad.GetState(p.index);

                Players[i] = p;

            }
        }*/

        // check current ontrollers
        controllers = "";
        foreach (Player p in Players)
        {
            p.UpdatePlayer();

            if (p.state.IsConnected)
            {
                p.Rumble();

                if (p.ButtonDown)
                    LoggingManager.AddText(string.Format("***{0} pressed***", p));

                controllers += p + " ";
            }
        }

        textObject.guiText.text = controllers;        

        foreach (Player p in Players)
        {
            if (p.state.IsConnected)
                p.prevState = p.state;
        }

    }

    private void OnApplicationQuit()
    {
        foreach (Player p in Players)
        {
            if (p.state.IsConnected)
                GamePad.SetVibration(p.index, 0, 0);
        }
    }
}