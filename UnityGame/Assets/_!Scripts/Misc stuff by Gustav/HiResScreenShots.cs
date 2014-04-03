using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;

// http://pacoup.com/2011/06/12/list-of-true-169-resolutions/

public class HiResScreenShots : MonoBehaviour
{
    public int resWidth = 512;
    public int resHeight = 288;

    private List<byte[]> data = new List<byte[]>(); // list of byte arrays

    private int counter;

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    private void Start()
    {
        StartCoroutine(TakeSnapShot());
    }

    // OR go in LateUpdate()
    private IEnumerator TakeSnapShot()
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();

        data.Add(bytes);

        if (data.Count > 100)
        {
            data.RemoveAt(0);
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(TakeSnapShot());
    }

    void LateUpdatesss()
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();

        data.Add(bytes);

        if (data.Count > 20)
        {
            data.RemoveAt(0);
        }

    }

    private void OnApplicationQuit()
    {
        double i = 0;
        foreach (var b in data)
        {
            i++;
            string path = "Screenshots/" + i + "_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

            FileStream fs = new FileStream(path, FileMode.CreateNew);

            BinaryWriter w = new BinaryWriter(fs);

            w.Write(b);
            w.Close();
            fs.Close();
        }
    }
}