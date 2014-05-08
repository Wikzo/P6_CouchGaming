using UnityEngine;
using System.Collections;

public class WelcomeIsPlaying : MonoBehaviour
{
    public GameObject LowPass;
    AudioLowPassFilter lp;

    float up;

    void Start()
    {
        lp = LowPass.GetComponent<AudioLowPassFilter>();
        up = 0;
    }


    // Update is called once per frame
    void Update()
    {
        if (audio.isPlaying)
            lp.cutoffFrequency = 250;
        else
        {
            up += Time.deltaTime * 70;

            // done?
            if (lp.cutoffFrequency > 13000)
            {
                up = 22000;
                lp.cutoffFrequency = up;
                lp.enabled = false;
                Destroy(lp);
                Destroy(gameObject);
            }

            lp.cutoffFrequency += up;
        }
    }
}
