﻿using UnityEngine;
using System.Collections;

public class MissionCompletedText : MonoBehaviour
{

    GUIText text;
    Vector3 lerp;
    Vector3 end = new Vector3(60, 60, 60);
    float time = 0;

    Vector3 destStart = new Vector3(-1.5f, 0.5f, 0);
    Vector3 destMiddle = new Vector3(0.5f, 0.5f, 0);
    Vector3 destOut = new Vector3(1.5f, 0.5f, 0);

    public iTween.EaseType EaseType;

    [HideInInspector]
    public Color ColorToUse;

    [HideInInspector]
    public AudioClip SoundToPlay;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<GUIText>();
        transform.position = destStart;

        text.color = ColorToUse;

        //MoveIn();

        AudioManager.Instance.PlaySound(SoundToPlay, this);

    }

    void OnEnable()
    {
        AudioManager.OnAudio += MoveIn;
    }

    void OnDisable()
    {
        AudioManager.OnAudio -= MoveIn;
    }


    public void MoveIn()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", destMiddle, "oncomplete", "MoveOut", "easeType", iTween.EaseType.easeOutBack));

        AudioManager.OnAudio -= MoveIn;

        //AudioManager.Instance.StopAllAudio();
        
    }

    void MoveOut()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", destOut, "oncomplete", "DestroyMe", "easeType", iTween.EaseType.easeOutBounce, "delay", 1.8));
    }

    IEnumerator DestroyMeAfterSomeTime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void DestroyMe()
    {
        StartCoroutine(DestroyMeAfterSomeTime());
    }
}
