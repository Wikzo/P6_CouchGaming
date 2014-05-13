using UnityEngine;
using System.Collections;

public class TextFade : MonoBehaviour
{

    string TutorialText = "Calibrating controllers";
    string PracticeText = "Practice Mode\nPress START to skip";
    private string GetReadyText = "Receiving missions ...\nUse ↑ and ↓\nY to get ready";
    private string GetReadyWithColors = "Use the D-pad ...\nUse       and                        \nPress <color=yellow>Y </color> to get ready";
    private string PauseText = "*Paused*";

    private string[] ScoreText;
    public GUIStyle[] ScoreStylesColor;
    
    public GUIStyle FadingTextStyle;
    private float FadeTime;

    private Rect TopRect;
    private Rect MidRect;
    private Rect LowerRect;

    private bool shouldFade;

    public GameObject ReadyImage;

    float alpha;

    // Use this for initialization
    private void Start()
    {
        ReadyImage.SetActive(false);
        shouldFade = true;
        FadingTextStyle.alignment = TextAnchor.MiddleCenter;
        
        float w = 0.3f; // proportional width (0..1)
        float h = 0.2f; // proportional height (0..1)
        MidRect = new Rect((Screen.width*(1 - w))/2, (Screen.height*(1 - h))/2, Screen.width*w, Screen.height*h);

        ScoreStylesColor = new GUIStyle[4];
        ScoreText = new string[4];
        for (int i = 0; i < ScoreStylesColor.Length; i++)
        {
            ScoreText[i] = "";
            ScoreStylesColor[i] = new GUIStyle();
            ScoreStylesColor[i].font = FadingTextStyle.font;
            ScoreStylesColor[i].fontSize = FadingTextStyle.fontSize;
        }
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

                ScoreText[i] = string.Format("{0} points: {1}", players[i].Name, players[i].Points);

                Color playerColorWithZeroAlpha = players[i].PlayerColor;
                Color colorToUse = new Color(playerColorWithZeroAlpha.r, playerColorWithZeroAlpha.g, playerColorWithZeroAlpha.b, 255);
                ScoreStylesColor[i].normal.textColor = colorToUse;// players[i].PlayerColor;


            }


            /*ScoreText = string.Format("{0}: {1} points\n{2}: {3} points\n{4}: {5} points\n{6}: {7} points",
                                      players[0].Name, players[0].Points,
                                      players[1].Name, players[1].Points,
                                      players[2].Name, players[2].Points,
                                      players[3].Name, players[3].Points);*/
        }
        else
            shouldFade = true;

        if (GameManager.Instance.PlayingState == PlayingState.WaitingForEverbodyToGetReady && !GameManager.Instance.ReadyNotYetSpawned)
            ReadyImage.SetActive(true);
        else
            ReadyImage.SetActive(false);


        if (shouldFade)
        {
            FadeTime = Mathf.PingPong(Time.time, 2f);
            //FadingTextStyle.normal.textColor = new Color(0, 0, 0, FadeTime);
            alpha = FadeTime;
        }
        else
        {
            //FadingTextStyle.normal.textColor = new Color(0, 0, 0, 1);
            alpha = 1;
        }

    }

    public void DrawOutline(Rect pos, string text, GUIStyle style, Color outColor, Color inColor)
    {
        GUIStyle backupStyle = style;
        style.normal.textColor = new Color(outColor.r, outColor.g, outColor.b, alpha);
        pos.x--;
        GUI.Label(pos, text, style);
        pos.x += 2;
        GUI.Label(pos, text, style);
        pos.x--;
        pos.y--;
        GUI.Label(pos, text, style);
        pos.y += 2;
        GUI.Label(pos, text, style);
        pos.y--;
        style.normal.textColor = new Color(inColor.r, inColor.g, inColor.b, alpha);
        GUI.Label(pos, text, style);
        style = backupStyle;

    }

    private void OnGUI()
    {


        switch (GameManager.Instance.PlayingState)
        {
            case PlayingState.TalkingBeforeControllerCalibration:
                    //GUI.Label(MidRect, TutorialText, FadingTextStyle);
                    DrawOutline(MidRect, TutorialText, FadingTextStyle, Color.white, Color.black);
                    break;
            case PlayingState.PraticeMode:
                    //GUI.Label(MidRect, PracticeText, FadingTextStyle);
                    DrawOutline(MidRect, PracticeText, FadingTextStyle, Color.white, Color.black);
                    break;

            /*case PlayingState.WaitingForEverbodyToGetReady:
                if (!GameManager.Instance.ReadyNotYetSpawned)
                    //GUI.Label(MidRect, GetReadyText, FadingTextStyle);
                    DrawOutline(MidRect, GetReadyWithColors, FadingTextStyle, Color.white, Color.black);
                    //GUI.Label(MidRect, "Receiving missions ...\nUse <color=red> ↑ and ↓ </color>\nPress <color=yellow>Y </color> to get ready", FadingTextStyle);
                GUI.DrawTexture(new Rect(Screen.width / 2 - MidRect.x/3, MidRect.y, 100, 100), dPadUp);
                GUI.DrawTexture(new Rect(Screen.width / 2 + MidRect.x/2 - 50, MidRect.y, 100, 100), dPadDown);

                

                break;*/

            case PlayingState.Paused:
                //GUI.Label(MidRect, PauseText, FadingTextStyle);
                DrawOutline(MidRect, PauseText, FadingTextStyle, Color.white, Color.black);
                break;

            case PlayingState.DisplayingScore:
                for (int i = 0; i < 4; i++)
                {
                    // TODO: make GUI resolution independent
                    if (ScoreText[i] != null && ScoreStylesColor[i] != null)
                    {
                        // calculate length of text
                        var textDimensions = GUI.skin.label.CalcSize(new GUIContent(ScoreText[i]));

                        //GUI.Label(new Rect(MidRect.x + Screen.width * (-0.1f), MidRect.y - 200 +  2 + i * 100 + 50, Screen.height, Screen.height), ScoreText[i], ScoreStylesColor[i]);
                        GUI.Label(new Rect(MidRect.x - textDimensions.x / 2, MidRect.y - 200 + 2 + i * 100 + 50, Screen.height, Screen.height), ScoreText[i], ScoreStylesColor[i]);
                    }
                }
                break;

            default:
                break;

        }
    }
}