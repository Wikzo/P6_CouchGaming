using UnityEngine;
using System.Collections;

public class DPadBlinkGUI : MonoBehaviour
{

    public GameObject Blink;

    private Renderer blinkRender;

    private bool showBlink;

    private float timer;
    private float blinkInterval = 0.5f;

    void Start()
    {

        if (Blink == null)
            Debug.Log("ERROR - Dpad blink not assigned");

        blinkRender = Blink.renderer;
        showBlink = false;
        blinkRender.enabled = false;

    }
	
	// Update is called once per frame
    private void Update()
    {
        // show blink between normal and pressed-down
        if (GameManager.Instance.PlayingState == PlayingState.PraticeMode || GameManager.Instance.PlayingState == PlayingState.WaitingForEverbodyToGetReady)
            showBlink = true;
        else if (GameManager.Instance.PlayingState != PlayingState.GettingTutorial) // just show pressed-down
        {
            showBlink = false;
            blinkRender.enabled = true;
        }
        

        if (showBlink)
        {
            timer += Time.deltaTime;

            if (timer > blinkInterval)
            {
                timer = 0;
                blinkRender.enabled = !blinkRender.enabled;
            }
        }


    }
}
