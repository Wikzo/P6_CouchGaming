using UnityEngine;

public class Singleton : MonoBehaviour
{

    // http://www.indiedb.com/games/coco-blast/tutorials/delegates-events-and-singletons-with-unity3d-c
    // http://clearcutgames.net/home/?p=437
    // https://www.youtube.com/watch?v=VnbfEyL85kE

    // Fields
    private string _myName;
    public string MyName
    {
        get { return _myName; }
    }

    // Singleton itself
    private static Singleton _instance;

    //  Instance  
    public static Singleton Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType(typeof(Singleton)) as Singleton;

            return _instance;
        }
    }

    // Constructor
    private Singleton() { }

    void OnApplicationQuit()
    {
        _instance = null; // release on exit
    }

    // Do something here, make sure this is public so we can access it through our Instance.   
    public void DoSomething() { }
}

public class SomeOtherClass
{
}