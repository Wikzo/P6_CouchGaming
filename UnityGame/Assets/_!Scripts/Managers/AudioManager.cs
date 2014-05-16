using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // Singleton itself
    private static AudioManager _instance;

    // Missions audio
    public AudioClip MissionAccomplishedSound;
    public AudioClip IntelKeyboardPressingSound;
    public AudioClip CalibrationAudio;
    public AudioClip VibrationExplain;
    public AudioClip PracticeAudio;
    public AudioClip NewMissionsFirstTime;
    public AudioClip ReadyGo;

    public AudioClip[] ReadySounds;
    public AudioClip Unready;

    public AudioClip MissionCompletedRed;
    public AudioClip MissionCompletedBlue;
    public AudioClip MissionCompletedGreen;
    public AudioClip MissionCompletedPink;
    public AudioClip MissionCompletedDefault; 

    public AudioClip WinRed;

    public AudioClip[] DiscHover = new AudioClip[4];

    public AudioClip[] DiscCharge = new AudioClip[4];
    public AudioClip DiscShot;
    public AudioClip DiscHit;

    float timer;
    bool isPlayingSound;

    public GameObject MusicPlayer;
    AudioLowPassFilter lowPass;
    bool lowPassIsMuted;

    public delegate void AudioAction();
    public static event AudioAction OnAudio;

    //  public static Instance  
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType(typeof(AudioManager)) as AudioManager;

            return _instance;
        }
    }

    void OnApplicationQuit()
    {
        _instance = null; // release on exit
    }

    private void Awake()
    {
        // http://clearcutgames.net/home/?p=437
        // First we check if there are any other instances conflicting
        if (_instance != null && _instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        _instance = this;

        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        DontDestroyOnLoad(gameObject);

        timer = 0;
        isPlayingSound = false;

        lowPass = MusicPlayer.GetComponent<AudioLowPassFilter>();
        if (lowPass == null)
            Debug.Log("ERROR - music player needs to have low pass filter!");

        lowPass.enabled = false;
        lowPassIsMuted = false;
    }


    IEnumerator AudioTimerCountdown(float audioLength)
    {
        isPlayingSound = true;
        yield return new WaitForSeconds(audioLength);
        isPlayingSound = false;
        
    }

    IEnumerator PlayAudioWithDelay(AudioClip a, float delayLength)
    {
        StartCoroutine(AudioTimerCountdown(a.length));
        yield return new WaitForSeconds(delayLength);
        
        if (delayLength > 0)
            audio.Stop();

        StartCoroutine(EnableLowPassFilter(a.length));
        audio.PlayOneShot(a);

    }

    IEnumerator PlayAudioWithDelay(AudioClip a, float delayLength, MissionCompletedText m)
    {
        StartCoroutine(AudioTimerCountdown(a.length));
        yield return new WaitForSeconds(delayLength);

        if (OnAudio != null)
            OnAudio(); // make MissionCompletedText call

        audio.PlayOneShot(a);
        StartCoroutine(EnableLowPassFilter(a.length));


    }

    public void PlaySound(AudioClip a)
    {
        audio.PlayOneShot(a);
    }

    public void PlaySound(AudioClip a, MissionCompletedText m)
    {
        if (!isPlayingSound)
            StartCoroutine(PlayAudioWithDelay(a, 0, m));
        else
            StartCoroutine(PlayAudioWithDelay(a, 1.5f, m));

    }

    public bool AudioIsPlaying()
    {
        return audio.isPlaying;
    }


    public void StopAllAudio()
    {
        audio.Stop();
    }

    IEnumerator EnableLowPassFilter(float time)
    {
        if (!lowPassIsMuted)
        {
            lowPass.enabled = true;
            lowPassIsMuted = true;

            yield return new WaitForSeconds(time - 0.5f);
            
            lowPass.enabled = false;
            lowPassIsMuted = false;
        }
    }

    public void PlayAnnouncerVoice(AudioClip audioToPlay)
    {
        if (!GameManager.Instance.UseAnnouncer)
            return;

        // stop and start next audio clip
        audio.Stop();
        audio.PlayOneShot(audioToPlay);

        StartCoroutine(EnableLowPassFilter(audioToPlay.length));
        return;

        // no overlapping
        /*if (!isPlayingSound)
        {
            timer = audioToPlay.length;
            isPlayingSound = true;
            audio.PlayOneShot(audioToPlay);
            
            audio.Stop();
            audio.PlayOneShot(audioToPlay);


        }

        if (isPlayingSound)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                isPlayingSound = false;
            }
        }*/

    }

}
