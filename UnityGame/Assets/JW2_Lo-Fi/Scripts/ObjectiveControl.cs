using UnityEngine;
using System.Collections;

public class ObjectiveControl : MonoBehaviour {

	Transform objOne;
	Transform objTwo;
	Transform objThree;
	Transform objFour;

	// Use this for initialization
	void Start () 
	{
		objOne = transform.Find("Objective_P1");
		objTwo = transform.Find("Objective_P2");
		objThree = transform.Find("Objective_P3");
		objFour = transform.Find("Objective_P4");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			objOne.GetComponent<MeshRenderer>().enabled = !objOne.GetComponent<MeshRenderer>().enabled;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			objTwo.GetComponent<MeshRenderer>().enabled = !objTwo.GetComponent<MeshRenderer>().enabled;
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			objThree.GetComponent<MeshRenderer>().enabled = !objThree.GetComponent<MeshRenderer>().enabled;
		}
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			objFour.GetComponent<MeshRenderer>().enabled = !objFour.GetComponent<MeshRenderer>().enabled;
		}
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
