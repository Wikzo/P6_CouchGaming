using UnityEngine;
using System.Collections;

public class MinusFade : MonoBehaviour
{
    TextMesh text;

    float time = 1.2f;

    // Use this for initialization
    void Start()
    {
        //TODO: play audio
        text = GetComponent<TextMesh>();

        iTween.PunchPosition(gameObject, new Vector3(0.8f, 0.8f, 0f), 1f);

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        while (time > 0)
        {
            time -= Time.deltaTime / 2;

            text.color = new Color(text.color.r, text.color.g, text.color.b, time);

            yield return null;
        }
        Destroy(gameObject);
    }
}
