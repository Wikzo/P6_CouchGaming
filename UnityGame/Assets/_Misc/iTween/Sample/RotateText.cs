using UnityEngine;
using System.Collections;

public class RotateText : MonoBehaviour {

    public iTween.EaseType EaseType;
    public bool TextShouldFade;
    public bool TextShouldRotate = true;
    TextMesh t;


	// Use this for initialization
	void Start ()
    {
        if (TextShouldRotate)
            iTween.RotateBy(gameObject, iTween.Hash("y", 0.01, "easeType", EaseType, "onComplete", "forward", "loopType", iTween.LoopType.none));

        if (TextShouldFade)
            t = GetComponent<TextMesh>();
	}

    void backward()
    {
        iTween.RotateBy(gameObject, iTween.Hash("y", 0.02, "easeType", EaseType, "onComplete", "forward", "loopType", iTween.LoopType.none));
        
    }

    void forward()
    {
        iTween.RotateBy(gameObject, iTween.Hash("y", -0.02, "easeType", EaseType, "onComplete", "backward", "loopType", iTween.LoopType.none));
        
    }
	
	// Update is called once per frame
    void Update()
    {
        if (TextShouldFade)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, Mathf.PingPong(Time.time, 1));
        }

    }
}
