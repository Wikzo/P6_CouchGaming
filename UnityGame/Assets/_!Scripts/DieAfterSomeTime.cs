using UnityEngine;
using System.Collections;

public class DieAfterSomeTime : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        StartCoroutine(DieAfterTenSeconds());
    }
	
	IEnumerator DieAfterTenSeconds()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
