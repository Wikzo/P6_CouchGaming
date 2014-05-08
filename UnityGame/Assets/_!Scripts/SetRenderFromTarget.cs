using UnityEngine;
using System.Collections;

public class SetRenderFromTarget : MonoBehaviour
{
    public Renderer Target;


    // Update is called once per frame
    void Update()
    {
        gameObject.renderer.enabled = Target.enabled;
    }
}
