using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayRenderMovie : MonoBehaviour {

    public List<MovieTexture> Movies;
    MovieTexture current;
    int lastPlayed;

    public bool PlayMovies = true;

	// Use this for initialization
    void Start()
    {
        if (PlayMovies)
        {
            current = PickRandomMovie();
            renderer.material.mainTexture = current;
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0);

            current.Play();
        }
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!current.isPlaying)
        {
            current = PickRandomMovie();
            renderer.material.mainTexture = current;
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0);
            
            current.Play();
        }
    }

    MovieTexture PickRandomMovie()
    {
        int play = Random.Range(0, Movies.Count);
        int tries = 0;

        if (play == lastPlayed && tries < 10) // don't repeat same movie
        {
            play = Random.Range(0, Movies.Count);
            tries++;
        }
        lastPlayed = play;

        return Movies[play];
        
    }

    private void OnApplicationQuit()
    {
        current.Stop();
    }
}
