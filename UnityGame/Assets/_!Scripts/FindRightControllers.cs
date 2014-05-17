using UnityEngine;
using System.Collections;

public class FindRightControllers : MonoBehaviour
{
    // Singleton itself
    private static FindRightControllers _instance;

    [HideInInspector]
    public int[] PlayerSlotsToRemember;


    //  public static Instance  
    public static FindRightControllers Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType(typeof(FindRightControllers)) as FindRightControllers;

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
    }
}
