using UnityEngine;
using System.Collections;

public class DPadBlinkWithoutGameManager : MonoBehaviour
{
    public GameObject Blink;

    private Renderer blinkRender;

    private float timer;
    private float blinkInterval = 0.5f;

    // Use this for initialization
    void Start()
    {
        blinkRender = Blink.renderer;
        blinkRender.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > blinkInterval)
        {
            timer = 0;
            blinkRender.enabled = !blinkRender.enabled;
            gameObject.renderer.enabled = !blinkRender.enabled;
        }
    }
}
