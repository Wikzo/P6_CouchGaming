﻿// Voice Speaker  (c) ZJP
// Windows 32B >> Copy 'Voice_speaker.dll' in windows\system32 folder
// Windows 64B >> Copy 'Voice_speaker.dll' in windows\SysWOW64 folder
// Remember to release "Voice_speaker.dll" with your final project. It will be placed in the same folder as the EXE
// Voice Speaker  (c) ZJP //

// http://forum.unity3d.com/threads/56038-Text-to-Speech-Dll-for-Win32

using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class VoiceSpeaker : MonoBehaviour
{

    [DllImport("Voice_speaker.dll", EntryPoint = "VoiceAvailable")]
    private static extern int VoiceAvailable();

    [DllImport("Voice_speaker.dll", EntryPoint = "InitVoice")]
    private static extern void InitVoice();

    [DllImport("Voice_speaker.dll", EntryPoint = "WaitUntilDone")]
    private static extern int WaitUntilDone(int millisec);

    [DllImport("Voice_speaker.dll", EntryPoint = "FreeVoice")]
    private static extern void FreeVoice();

    [DllImport("Voice_speaker.dll", EntryPoint = "GetVoiceCount")]
    private static extern int GetVoiceCount();

    // Unity V4.x.x

    [DllImport("Voice_speaker.dll", EntryPoint = "GetVoiceName")]
    private static extern IntPtr GetVoiceName(int index);

    //  other Unity version
    // [DllImport ("Voice_speaker.dll", EntryPoint="GetVoiceName")]   private static extern string GetVoiceName(int index);

    [DllImport("Voice_speaker.dll", EntryPoint = "SetVoice")]
    private static extern void SetVoice(int index);

    [DllImport("Voice_speaker.dll", EntryPoint = "Say")]
    private static extern void Say(string ttospeak);

    [DllImport("Voice_speaker.dll", EntryPoint = "SayAndWait")]
    private static extern void SayAndWait(string ttospeak);

    [DllImport("Voice_speaker.dll", EntryPoint = "SpeakToFile")]
    private static extern int SpeakToFile(string filename, string ttospeak);

    [DllImport("Voice_speaker.dll", EntryPoint = "GetVoiceState")]
    private static extern int GetVoiceState();

    [DllImport("Voice_speaker.dll", EntryPoint = "GetVoiceVolume")]
    private static extern int GetVoiceVolume();

    [DllImport("Voice_speaker.dll", EntryPoint = "SetVoiceVolume")]
    private static extern void SetVoiceVolume(int volume);

    [DllImport("Voice_speaker.dll", EntryPoint = "GetVoiceRate")]
    private static extern int GetVoiceRate();

    [DllImport("Voice_speaker.dll", EntryPoint = "SetVoiceRate")]
    private static extern void SetVoiceRate(int rate);

    [DllImport("Voice_speaker.dll", EntryPoint = "PauseVoice")]
    private static extern void PauseVoice();

    [DllImport("Voice_speaker.dll", EntryPoint = "ResumeVoice")]
    private static extern void ResumeVoice();

    public int voice_nb = 0;
    public int VoiceSpeed = 1;
    public int VoiceVolume = 10;
    public int VoiceRate = 1;

    private static VoiceSpeaker _instance;
    public static VoiceSpeaker Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType(typeof(VoiceSpeaker)) as VoiceSpeaker;

            return _instance;
        }
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
    }


    void Start()
    {
        if (VoiceAvailable() > 0)
        {
            InitVoice(); // init the engine
            if (voice_nb > GetVoiceCount()) voice_nb = 0;
            if (voice_nb < 0) voice_nb = 0;

            // Unity V4.x.x *******************************************
            IntPtr pStr = GetVoiceName(voice_nb);
            string str = Marshal.PtrToStringAnsi(pStr);
            Debug.Log("Voice name : " + str); // Voice Name
            // Unity V4.x.x *******************************************

            //Debug.Log ("Voice name : "+GetVoiceName(voice_nb)); // Voice Name other Unity version
            Debug.Log("Number of voice : " + GetVoiceCount()); // Number of voice
            SetVoice(voice_nb); // 0 to voiceCount - 1
            Debug.Log("Voice Rate : " + GetVoiceRate());
            //Debug.Log ("Voice name : "+GetVoiceName(voice_nb));
            //Say("All systems ready. All players: please download your missions. Good luck.");
            //Say("You are a big big big ...");
            //Say("boy");
            //Say("Benjamin B is also a big, big, big, very big");
            //Say("hey ho let's go yeah yeah yeah");
        }
    }

    void OnDisable()
    {
        if (VoiceAvailable() > 0)
        {
            FreeVoice();
        }
    }

    void Update()
    {
        SetVoiceRate(VoiceSpeed);
        SetVoiceVolume(VoiceVolume);
        SetVoiceRate(VoiceRate);

        if (GameManager.Instance.PlayingState == PlayingState.Paused)
            PauseVoice();

    }

    public void Speak(string text)
    {
        Say(text);
    }

    void OnApplicationQuit()
    {
        FreeVoice();
        PauseVoice();

        _instance = null; // release on exit

    }
}