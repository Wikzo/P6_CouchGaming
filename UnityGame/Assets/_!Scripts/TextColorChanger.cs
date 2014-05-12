﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextColorChanger : MonoBehaviour
{
    public List<Material> MaterialsToUse;
    Color[] colors;

    TextMesh myText;
    // Use this for initialization
    void Start()
    {
        colors = new Color[MaterialsToUse.Count];
        myText = GetComponent<TextMesh>();

        for (int i = 0; i < MaterialsToUse.Count; i++)
            colors[i] = MaterialsToUse[i].color;

        StartCoroutine(ChooseColor());

    }

    IEnumerator ChooseColor()
    {
        myText.color = colors[Random.Range(0, colors.Length)];

        float waitTime = Random.Range(0.2f, 1.5f);
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(ChooseColor());
    }
}
