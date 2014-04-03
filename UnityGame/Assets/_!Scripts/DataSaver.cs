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
        //Players name
        public string name;
        //Score
        public float score;

        public int timesStarted;

        public int timesJumped;
    }

    private static DataSaver _instance;


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

    [HideInInspector]
    public List<ScoreEntry> highScores = new List<ScoreEntry>();

    void SaveScoresToDataFile()
    {
        //Get a binary formatter
        var b = new BinaryFormatter();
        //Create a file
        var f = File.Create("_SaveData/highscores.dat");
        //Save the scores
        b.Serialize(f, highScores);
        f.Close();

        //print("saved via data file");
    }

    void Start()
    {
        LoadDataFile();
        //highScores[0].timesStarted++;

        //PlayerPrefs.DeleteAll();
    }

    void LoadDataFile()
    {
        //print("file loaded");
        //If not blank then load it
        if (File.Exists("_SaveData/highscores.dat"))
        {
            //Binary formatter for loading back
            var b = new BinaryFormatter();
            //Get the file
            var f = File.Open("_SaveData/highscores.dat", FileMode.Open);
            //Load back the scores
            highScores = (List<ScoreEntry>)b.Deserialize(f);
            f.Close();

        }
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.H))
        {
            print("highscore added");

            highScores.Add(new ScoreEntry
            {
                name = "myName " + DateTime.Now,
                score = 42,
            });
        }*/

        /*if (Input.GetKeyDown(KeyCode.L))
            LoadDataFile();*/

        /*if (Input.GetKeyDown(KeyCode.C))
        {
            print("file deleted");
            highScores.Clear();
            if (File.Exists(Application.persistentDataPath + "/highscores.dat"))
                File.Delete(Application.persistentDataPath + "/highscores.dat");
        }*/

        /*if (Input.GetKeyDown(KeyCode.S))
            SaveScoresToDataFile();*/

    }



    void OnGUI()
    {
        foreach (var score in highScores)
        {
            GUILayout.Label(string.Format("NUMBER OF JUMPS: {0}", score.timesJumped));
        }

    }

}