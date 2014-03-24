using System.Collections;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class PulseRumble_LoFi : MonoBehaviour
{
     private bool playerIndexSet = false;
    private PlayerIndex playerIndex;
    private GamePadState state;
    private GamePadState prevState;

    private bool buttonLBDown;
    private bool buttonRBDown;


    private float missionTimeCounter;
    private int missionRumbleCounter;
    private bool missionFirstTimeDisplaying;
    private int missionNumber = 0;

    private float targetTimeCounter;
    private int targetRumbleCounter;
    private bool targetFirstTimeDisplaying;
    private int targetNumber = 0;

    private bool isDisplayingMissionOrTargetRumbleRightNow;


    void Awake() // Awake is called before Start
    {
        // reset all vibrations before game begins
        //for (int i = 0; i < 4; ++i)
        //{
        //    PlayerIndex testIndex = (PlayerIndex)i;
        //    if (GamePad.GetState(testIndex).IsConnected)
        //    {
        //        GamePad.SetVibration(testIndex, 0, 0);
        //    }
        //}
        playerIndex = PlayerIndex.One;
    }

    private void Start()
    {
        missionFirstTimeDisplaying = true;
        targetFirstTimeDisplaying = true;
        isDisplayingMissionOrTargetRumbleRightNow = false;

        GoKitTweenExtensions.shake(Camera.main.transform, 0.5f, new Vector3(1, 1, 1), GoShakeType.Position);
    }

    


    // Update is called once per frame
    private void Update()
    {
        // Find a PlayerIndex, for a single player game
        //if (!playerIndexSet || !prevState.IsConnected)
        //{
        //    for (int i = 0; i < 4; ++i)
        //    {
        //        PlayerIndex testPlayerIndex = (PlayerIndex)i;
        //        GamePadState testState = GamePad.GetState(testPlayerIndex);
        //        if (testState.IsConnected)
        //        {
        //            Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
        //            playerIndex = testPlayerIndex;
        //            playerIndexSet = true;
        //        }
        //    }
        //}


        state = GamePad.GetState(playerIndex);

        buttonLBDown = (state.Buttons.LeftShoulder == ButtonState.Pressed && prevState.Buttons.LeftShoulder != ButtonState.Pressed);
        buttonRBDown = (state.Buttons.RightShoulder == ButtonState.Pressed && prevState.Buttons.RightShoulder != ButtonState.Pressed);


        if (buttonLBDown && !isDisplayingMissionOrTargetRumbleRightNow)
            PickMissionRumble();

        if (buttonRBDown && !isDisplayingMissionOrTargetRumbleRightNow)
            PickTargetRumble();

        if (missionRumbleCounter > 0)
            MissionRumbler(0.2f);

        if (targetRumbleCounter > 0)
            TargetRumbler(0.2f);

        if (state.Buttons.Y == ButtonState.Pressed)
            Application.LoadLevel(0);


        prevState = state;
    }

    void PickMissionRumble()
    {
        if (isDisplayingMissionOrTargetRumbleRightNow)
            return;
        else
            isDisplayingMissionOrTargetRumbleRightNow = true;

        if (missionFirstTimeDisplaying)
        {
            Random.seed = (int)System.DateTime.Now.Ticks;

            missionNumber = Random.Range(1, 5);
            missionRumbleCounter = missionNumber;
            //print("Mission number: " + missionRumbleCounter);
            missionFirstTimeDisplaying = false;
        }
        else
            missionRumbleCounter = missionNumber;

    }

    private void MissionRumbler(float interval)
    {
        //transform.guiText.text = "Mission number: " + missionNumber;
        if (missionTimeCounter < interval)
        {
            missionTimeCounter += Time.deltaTime;
            GamePad.SetVibration(playerIndex, 1, 1);
        }
        else if (missionTimeCounter < interval * 3)
        {
            missionTimeCounter += Time.deltaTime;
            GamePad.SetVibration(playerIndex, 0, 0);
        }
        else
        {
            //transform.guiText.text = "";

            missionTimeCounter = 0;
            missionRumbleCounter--;
            isDisplayingMissionOrTargetRumbleRightNow = false;

        }
    }

    void PickTargetRumble()
    {
        if (isDisplayingMissionOrTargetRumbleRightNow)
            return;
        else
            isDisplayingMissionOrTargetRumbleRightNow = true;

        if (targetFirstTimeDisplaying)
        {
            Random.seed = (int)System.DateTime.Now.Ticks;

            targetNumber = Random.Range(1, 5);
            targetRumbleCounter = targetNumber;
            //print("Target number: " + targetRumbleCounter);
            targetFirstTimeDisplaying = false;
        }
        else
            targetRumbleCounter = targetNumber;

    }

    

    private void TargetRumbler(float interval)
    {
        //transform.guiText.text = "Target number: " + targetNumber;

        if (targetTimeCounter < interval)
        {
            targetTimeCounter += Time.deltaTime;
            GamePad.SetVibration(playerIndex, 1, 1);
        }
        else if (targetTimeCounter < interval * 3)
        {
            targetTimeCounter += Time.deltaTime;
            GamePad.SetVibration(playerIndex, 0, 0);
        }
        else
        {
            //transform.guiText.text = "";

            targetTimeCounter = 0;
            targetRumbleCounter--;
            isDisplayingMissionOrTargetRumbleRightNow = false;

        }
    }

    private void OnApplicationQuit()
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }
}
