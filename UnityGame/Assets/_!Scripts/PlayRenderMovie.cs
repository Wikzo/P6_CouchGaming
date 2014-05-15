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
            lastPlayed = -1;
            PickRandomMovie();
        }
    }

    IEnumerator StartNewMovie(float length)
    {
        yield return new WaitForSeconds(length);
        current.Stop();

        PickRandomMovie();
    }

    void PickRandomMovie()
    {
        int play = Random.Range(0, Movies.Count);
        int tries = 0;

        if (play == lastPlayed && tries < 10) // don't repeat same movie
        {
            play = Random.Range(0, Movies.Count);
            tries++;
        }
        lastPlayed = play;

        current = Movies[play];
        renderer.material.mainTexture = current;
        renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0);

        current.Play();

        //Debug.Log("Picked " + current);

        StartCoroutine(StartNewMovie(current.duration));
    }

    private void OnApplicationQuit()
    {
        current.Stop();
    }
}
