using UnityEngine;
using System.Collections;

public class TimeBar : MonoBehaviour
{

    public Texture2D fillTexture;
    public Texture2D backgroundTexture;
    private Vector2 BarPosition;
    string timerText = "TIME:";

    public float BarHeight = 10;
    private float YOffset = 7.1f;

    private float BarRatio = 0.5f; // cannot be lower than 0!

    private float totalLength;
    private float currentLength;

    void Update()
    {
        totalLength = (GameManager.Instance.TimePerRound / BarRatio) - (BarRatio / (GameManager.Instance.TimePerRound / BarRatio));
        currentLength = (GameManager.Instance.TimeLeft / BarRatio) - (BarRatio / (GameManager.Instance.TimeLeft / BarRatio));
        
        if (BarRatio <= 0)
            Debug.Log("ERROR - time bar ratio cannot be lower than zero!");

        BarPosition = new Vector2(Screen.width/2, Screen.height / YOffset);

        
    }

    private void OnGUI()
    {
        if (GameManager.Instance.PlayingState != PlayingState.Playing)
            return;

        // calculate width of string
        var textDimensions = GUI.skin.label.CalcSize(new GUIContent(timerText));

        // draw string
        GUI.Label(new Rect(BarPosition.x - textDimensions.x / 2, BarPosition.y - 20, 50, 50), timerText);

        // Draw black border
        GUI.DrawTexture(new Rect(BarPosition.x - totalLength/2, BarPosition.y, totalLength, BarHeight), backgroundTexture, ScaleMode.StretchToFill, true, 10.0F);

        // Draw lifebar
        GUI.DrawTexture(new Rect(BarPosition.x - totalLength/2, BarPosition.y, currentLength, BarHeight), fillTexture);
    }
}
