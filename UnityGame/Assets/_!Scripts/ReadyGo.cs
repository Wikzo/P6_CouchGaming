using UnityEngine;
using System.Collections;

public class ReadyGo : MonoBehaviour {

    float timer = 3;
    GUIText guiText;
    string ready = "Ready ... ";
    string go = "  GO GO GO!!";
    string textToShow;
    bool hasShaken = false;
    float dieTimer = 0;
    float fader = 1;
    void Start()
    {
        guiText = GetComponent<GUIText>();

        transform.position = new Vector3(0.5f, 0.5f, 0);
        guiText.pixelOffset = new Vector2(-236, 0);
    }
	

	// Update is called once per frame
    void Update()
    {

        dieTimer += Time.deltaTime;
        if (dieTimer > 4)
        {

            fader -= Time.deltaTime;

            guiText.color = new Color(guiText.color.r, guiText.color.g, guiText.color.b, fader);

            if (fader <= 0)
            {
                GameManager.Instance.PlayingState = PlayingState.Playing;
                Destroy(guiText);
                Destroy(this);
            }
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            textToShow = ready + timer.ToString("F2");
        }
        else
        {
            textToShow = go;
            timer = 0;
            if (!hasShaken)
            {
                GoKitTweenExtensions.shake(transform, 1.5f, new Vector3(0.2f, 0.2f, 0f), GoShakeType.Position);
                hasShaken = true;
            }
        }

        guiText.text = textToShow;
    }
}
