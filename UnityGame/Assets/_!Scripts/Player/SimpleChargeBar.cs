using UnityEngine;
using System.Collections;

public class SimpleChargeBar : MonoBehaviour {

	public float MaxChargeTime = 10;
	private float chargeTimer = 0;

	private Vector3 startPos;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(chargeTimer < MaxChargeTime)
		{
			chargeTimer += Time.deltaTime;
	
			transform.localScale = new Vector3(chargeTimer/MaxChargeTime, 1, 1);
			transform.position = new Vector3(startPos.x+transform.localScale.x/2, startPos.y, startPos.z);
		}	
	}
}
