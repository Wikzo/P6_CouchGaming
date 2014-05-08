using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIPlacement : MonoBehaviour {

    public List<Transform> ObjectsToFollow;
    public List<Texture> Textures;

    public float Scale = 1;

    List<GUITexture> guiTextures;
    List<Vector3> positions;
    List<GameObject> gameObjects;

    Camera mainCam;
    

	void Start ()
    {
        guiTextures = new List<GUITexture>();
        positions = new List<Vector3>();
        gameObjects = new List<GameObject>();
        foreach (Texture t in Textures)
        {
            GameObject gObj = new GameObject("GUIRumbleIndicator");
            gObj.transform.localScale = new Vector3(0, 0, 1);
            gameObjects.Add(gObj);

            GUITexture g = gObj.AddComponent<GUITexture>();
            g.texture = t;

            float width = t.width / Scale;
            float height = t.height / Scale;
            // pixel inset X and Y should be -(width/2) and -(height/2) !!
            g.pixelInset = new Rect(-width / 2, -height / 2, width, height);

            guiTextures.Add(g);
            positions.Add(Vector3.zero);

        }
        mainCam = GameObject.Find("Main Camera").camera;
	}
	
    void Update()
    {

        for (int i = 0; i < guiTextures.Count; i++)
        {
            positions[i] = mainCam.WorldToViewportPoint(ObjectsToFollow[i].position);
            gameObjects[i].transform.position = positions[i];

            float width = guiTextures[i].texture.width / Scale;
            float height = guiTextures[i].texture.height / Scale;
            // pixel inset X and Y should be -(width/2) and -(height/2) !!
            guiTextures[i].pixelInset = new Rect(-width / 2, -height / 2, width, height);
        }

        
    }
}
