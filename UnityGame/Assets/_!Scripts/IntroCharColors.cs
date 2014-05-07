using UnityEngine;
using System.Collections;

public class IntroCharColors : MonoBehaviour {

    public int Id;
    public Material[] Materials = new Material[4];
    private Material pMat;

    public GameObject RendererObject;
    [HideInInspector]
    public SkinnedMeshRenderer BodyRenderer;
    [HideInInspector]
    public MeshRenderer[] HelmetRenderers;
    private Color[] originalHelmetColors;

    void Start()
    {
        if (RendererObject != null)
        {
            if (RendererObject.GetComponent<SkinnedMeshRenderer>() != null)
                BodyRenderer = RendererObject.GetComponent<SkinnedMeshRenderer>();
            if (RendererObject.GetComponentsInChildren<MeshRenderer>() != null)
            {
                HelmetRenderers = RendererObject.GetComponentsInChildren<MeshRenderer>();
                originalHelmetColors = new Color[HelmetRenderers.Length];
                for (int i = 0; i < originalHelmetColors.Length; i++)
                {
                    originalHelmetColors[i] = HelmetRenderers[i].material.color;
                }
            }
        }

        switch (Id)
        {
            case 0:
                //PlayerController = PlayerIndex.One;
                pMat = Materials[0];
                break;
            case 1:
                //PlayerController = PlayerIndex.Two;
                pMat = Materials[1];
                break;
            case 2:
                //PlayerController = PlayerIndex.Three;
                pMat = Materials[2];
                break;
            case 3:
                //PlayerController = PlayerIndex.Four;
                pMat = Materials[3];
                break;
        }
        if (BodyRenderer != null)
        {
            BodyRenderer.material.color = pMat.color;
            BodyRenderer.materials[1].color = pMat.color;

        }
    }
}
