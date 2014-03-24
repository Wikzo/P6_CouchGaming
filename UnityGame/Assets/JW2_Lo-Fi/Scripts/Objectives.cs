using UnityEngine;
using System.Collections;

public class Objectives : MonoBehaviour 
{

	public string eye;

	private bool onObj = false;
	private bool otherOnObj = false;

	private MeshRenderer meshR;

	// Use this for initialization
	void Start () 
	{
		meshR = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == eye)
		{
			onObj = true;
			if(otherOnObj == false)
			{
				meshR.enabled = true;
			}
		}
		if(other.gameObject.name != eye && other.gameObject.name != "Player")
		{
			otherOnObj = true;
			meshR.enabled = false;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.name == eye)
		{
			onObj = false;
			meshR.enabled = false;
		}
		if(other.gameObject.name != eye && other.gameObject.name != "Player")
		{
			otherOnObj = false;
			if(onObj)
			{
				meshR.enabled = true;
			}
		}
	}
}
