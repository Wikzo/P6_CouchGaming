using UnityEngine;
using System.Collections;

public class StuckDetector : MonoBehaviour {

	public bool StuckInObject = false;
	private StuckCollider[] stuckColliders;

	// Use this for initialization
	void Start () 
	{
		stuckColliders = GetComponentsInChildren<StuckCollider>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(stuckColliders[0].IsColliding && stuckColliders[1].IsColliding && stuckColliders[2].IsColliding)
			StuckInObject = true;
	}
}
