using UnityEngine;
using System.Collections;

public class TextFade : MonoBehaviour
{

    string PracticeText = "PRACTICE MODE";
    private string GetReadyText = "Press Y to get ready";
    private string PauseText = "*Paused*";
    private string ScoreText = "";
    public GUIStyle TextStyle;
    private float FadeTime;
    private Rect rect;

    private bool shouldFade;

    public Light MainLight;

    // Use this for initialization
    private void Start()
    {
        shouldFade = true;
        TextStyle.alignment = TextAnchor.MiddleCenter;
        
        float w = 0.3f; // proportional width (0..1)
        float h = 0.2f; // proportional height (0..1)
        rect = new Rect((Screen.width*(1 - w))/2, (Screen.height*(1 - h))/2, Screen.width*w, Screen.height*h);

        if (MainLight == null)
            Debug.Log("ERROR - assign directional light to TextFade");
    }

    // Update is called once per frame
    private void Update()
    {
        // turn off light when showing gui text
        if (GameManager.Instance.PlayingState != PlayingState.Playing && GameManager.Instance.PlayingState != PlayingState.PraticeMode)
            MainLight.enabled = false;
        else
            MainLight.enabled = true;

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
            TextStyle.normal.textColor = new Color(1, 0, 0, FadeTime);
        }
        else
            TextStyle.normal.textColor = new Color(1, 0, 0, 1);

    }

    private void OnGUI()
    {
        switch (GameManager.Instance.PlayingState)
        {
            case PlayingState.PraticeMode:
                    GUI.Label(rect, PracticeText, TextStyle);
                    break;

            case PlayingState.WaitingForEverbodyToGetReady:
                GUI.Label(rect, GetReadyText, TextStyle);
                break;

            case PlayingState.Paused:
                GUI.Label(rect, PauseText, TextStyle);
                break;

            case PlayingState.DisplayingScore:
                GUI.Label(rect, ScoreText, TextStyle);
                break;

            default:
                break;

        }
    }
}