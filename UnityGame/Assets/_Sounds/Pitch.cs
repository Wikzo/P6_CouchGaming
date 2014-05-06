using UnityEngine;
using System.Collections;

public class Pitch : MonoBehaviour {
    public float note;

    void Update()
    {
        

        audio.pitch = Mathf.Pow(2, note / 12);
    }

    void OnGUI()
    {
        note = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(20, 20, 180, 20), note, -12, 12));
    }
}
