using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{
    Vector3 dest;
    public bool GoRight = true;

	void Start(){
        //iTween.MoveTo(gameObject, dest, 2f);

        if (GoRight)
            dest = new Vector3(transform.position.x + 0.0689f, transform.position.y, transform.position.z);
        else
            dest = new Vector3(transform.position.x - 0.0689f, transform.position.y, transform.position.z);


		iTween.MoveTo(gameObject, iTween.Hash("position", dest, "easeType", iTween.EaseType.linear, "loopType", "pingPong"));
	}
}

