using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
//You must include these namespaces
//to use BinaryFormatter
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Random = UnityEngine.Random;

public class DataSaver : MonoBehaviour
{
    //High score entry
    [System.Serializable]
    public class ScoreEntry
    {
        public int RedWins = 0;
        public int BlueWins = 0;
        public int GreenWins = 0;
        public int PinkWins = 0;
    }

    [HideInInspector]
    public List<ScoreEntry> highScores = new List<ScoreEntry>();

    private static DataSaver _instance;
    bool fileLoadedCorrectly;

    string path = "";


    //  public static Instance  
    public static DataSaver Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType(typeof(DataSaver)) as DataSaver;

            return _instance;
        }
    }

    void OnApplicationQuit()
    {
        SaveScoresToDataFile();
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
    }

    public void SaveScoresToDataFile()
    {
        //Get a binary formatter
        var b = new BinaryFormatter();
        //Create a file
        var f = File.Create(path);
        //Save the scores
        b.Serialize(f, highScores);
        f.Close();

        //print("saved via data file");
    }

    void Start()
    {

    #if UNITY_EDITOR
        path = "_SaveData/highscores.dat";
    #endif

    #if UNITY_STANDALONE_WIN
        path = "highscores.dat";
    #endif

        //print(path);
        LoadDataFile();
        //highScores[0].timesStarted++;

        //PlayerPrefs.DeleteAll();
    }

    void LoadDataFile()
    {
        //print("file loaded");
        //If not blank then load it
        if (!File.Exists(path))
        {
            var file = File.Create(path);

            Debug.Log("File created at " + path);

            highScores.Add(new ScoreEntry
            {
                RedWins = 1,
                BlueWins = 1,
                GreenWins = 1,
                PinkWins = 1
            });

            file.Close();
        }

        else
        {
            //Binary formatter for loading back
            var b = new BinaryFormatter();
            //Get the file
            var f = File.Open(path, FileMode.Open);
            //Load back the scores
            highScores = (List<ScoreEntry>)b.Deserialize(f);
            f.Close();
        }

        fileLoadedCorrectly = true;

    }

    void Update()
    {
        if (!fileLoadedCorrectly)
            return;

        /*foreach (var score in highScores)
        {
            Debug.Log("red: " + score.RedWins);
            Debug.Log("blue: " + score.BlueWins);
            Debug.Log("green: " + score.GreenWins);
            Debug.Log("pink: " + score.PinkWins);
        }*/

        /*if (Input.GetKeyDown(KeyCode.H))
        {
            print("highscore added");

            highScores.Add(new ScoreEntry
            {
                RedWins = 25, BlueWins = 2, GreenWins = 10, PinkWins = 1
            });
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            print("file loaded");
            LoadDataFile();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            print("file cleared");
            highScores.Clear();
            if (File.Exists(path))
                File.Delete(path);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            print("file saved");
            SaveScoresToDataFile();
        }*/

    }



    /*void OnGUI()
    {
        foreach (var score in highScores)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 100, 100), string.Format("Red wins: {0}\nBluewins: {1}\nGreen wins: {2}\nPink wins: {4}", score.RedWins.ToString(), score.BlueWins.ToString(), score.GreenWins.ToString(), score.PinkWins.ToString()));
        }

    }*/

}