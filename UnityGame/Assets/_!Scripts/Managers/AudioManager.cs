using System.Collections.Generic;
using UnityEngine;

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

    public AudioClip MissionCompletedPink;
    public AudioClip MissionCompletedBlue;
    public AudioClip MissionCompletedGreen;
    public AudioClip MissionCompletedOrange;
    public AudioClip MissionCompletedDefault;

    public AudioClip WinRed;

    float timer;
    bool isPlayingSound;

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
    }

    public void PlaySound(AudioClip a)
    {
        audio.PlayOneShot(a);
    }

    public void StopAllAudio()
    {
        audio.Stop();
    }

    public void PlayAnnouncerVoice(AudioClip audioToPlay)
    {
        if (!GameManager.Instance.UseAnnouncer)
            return;

        // stop and start next audio clip
        audio.Stop();
        audio.PlayOneShot(audioToPlay);
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
