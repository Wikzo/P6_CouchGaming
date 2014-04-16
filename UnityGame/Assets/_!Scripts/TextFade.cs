using UnityEngine;
using System.Collections;

public class TextFade : MonoBehaviour
{

    string TutorialText = "Calibrating controller rumble ...\nPress Start to skip";
    string PracticeText = "PRACTICE MODE\nUse ↑ and ↓\nPress Start to skip";
    private string GetReadyText = "Receiving missions ...\nUse ↑ and ↓";
    private string PauseText = "*Paused*";
    private string ScoreText = "";
    public GUIStyle FadingTextStyle;
    private float FadeTime;

    private Rect TopRect;
    private Rect MidRect;
    private Rect LowerRect;

    private bool shouldFade;

    // Use this for initialization
    private void Start()
    {
        shouldFade = true;
        FadingTextStyle.alignment = TextAnchor.MiddleCenter;
        
        float w = 0.3f; // proportional width (0..1)
        float h = 0.2f; // proportional height (0..1)
        MidRect = new Rect((Screen.width*(1 - w))/2, (Screen.height*(1 - h))/2, Screen.width*w, Screen.height*h);
    }

    // Update is called once per frame
    private void Update()
    {

        if (GameManager.Instance.PlayingState == PlayingState.DisplayingScore)
        {
            shouldFade = false;
            Player[] players = new Player[4];

            for (int i = 0; i < GameManager.Instance.Players.Count; i++)
            {
                players[i] = GameManager.Instance.Players[i].GetComponent<Player>();
            }


            ScoreText = string.Format("{0}: {1} points\n{2}: {3} points\n{4}: {5} points\n{6}: {7} points",
                                      players[0].Name, players[0].Points,
                                      players[1].Name, players[1].Points,
                                      players[2].Name, players[2].Points,
                                      players[3].Name, players[3].Points);
        }
        else
            shouldFade = true;

        if (shouldFade)
        {
            FadeTime = Mathf.PingPong(Time.time, 1);
            FadingTextStyle.normal.textColor = new Color(1, 0, 0, FadeTime);
        }
        else
            FadingTextStyle.normal.textColor = new Color(1, 0, 0, 1);

    }

    private void OnGUI()
    {
        switch (GameManager.Instance.PlayingState)
        {
            case PlayingState.ControllerCalibration:
                GUI.Label(MidRect, TutorialText, FadingTextStyle);
                    break;
            case PlayingState.PraticeMode:
                    GUI.Label(MidRect, PracticeText, FadingTextStyle);
                    break;

            case PlayingState.WaitingForEverbodyToGetReady:
                GUI.Label(MidRect, GetReadyText, FadingTextStyle);
                break;

            case PlayingState.Paused:
                GUI.Label(MidRect, PauseText, FadingTextStyle);
                break;

            case PlayingState.DisplayingScore:
                GUI.Label(MidRect, ScoreText, FadingTextStyle);
                break;

            default:
                break;

        }
    }
}