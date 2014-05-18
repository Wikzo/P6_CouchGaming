using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;

public static class LoggingManager
{
    private static string fileName = "";
    private static string fileEnding = ".txt";
    public static string format = "HH-mm-ss";    // Use this format
    public static string formatToCreateFileFrom = "dd-MM-yyyy-HH-mm-ss";
    public static string path;



    public static void CreateTextFile()
    {
        fileName = "Log_" + DateTime.Now.ToString(formatToCreateFileFrom);

        path = fileName + fileEnding;

        using (FileStream fs = File.Create(path))
        {
            CreateText(fs, "");
            //Debug.Log("text file created");

        }
    }

    public static void CreateTextFile(string fileName)
    {
        fileName = fileName + DateTime.Now.ToString(formatToCreateFileFrom);

        path = fileName + fileEnding;

        using (FileStream fs = File.Create(path))
        {
            CreateText(fs, "");
            //Debug.Log("text file created");

        }
    }


    private static void CreateText(FileStream fs, string value)
    {
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fs.Write(info, 0, info.Length);
    }

    public static void AddText(string text)
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
        {
            string date = DateTime.Now.ToString(format);

            file.WriteLine("\n" + date + ": " + text);
        }
    }

    public static void AddTextNoTimeStamp(string text)
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
        {
            string date = DateTime.Now.ToString(format);

            file.WriteLine(text);
        }
    }

    public static void AddEndText(string text)
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
        {
            string date = DateTime.Now.ToString(format);

            file.WriteLine("\n" + date + "------------- " + text);
        }
    }
}
