using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Player : MonoBehaviour {

	public int Score;
	public int Id;
	public bool IsAlive;

	//public PlayerIndex ControllerNum;

	// Use this for initialization
	void Start () 
	{
		//Choose a controller matching the ID
		//if(Id == 0)
		//	ControllerNum = PlayerIndex.One;
		//else if(Id == 1)
		//	ControllerNum = PlayerIndex.Two;
		//else if(Id == 2)
		//	ControllerNum = PlayerIndex.Three;
		//else if(Id == 3)
		//	ControllerNum = PlayerIndex.Four;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
